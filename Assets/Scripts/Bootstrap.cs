using System.Linq;
using CarPool.Level;
using CarPool.States;
using CarPool.Tools;
using Client;
using Ji2.CommonCore;
using Ji2.CommonCore.SaveDataContainer;
using Ji2.Context;
using Ji2Core.Core;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using Ji2Core.Core.Tools;
using Ji2Core.Core.UserInput;
using UnityEngine;

namespace CarPool
{
    class Bootstrap : BootstrapBase
    {
        [SerializeField] private UpdateService updateService;
        [SerializeField] private DragInput dragInput;
        [SerializeField] private ScreenNavigator screenNavigator;
        [SerializeField] private LevelDatabase levelDatabase;
        [SerializeField] private Config config;

        private readonly Context _context = Context.GetOrCreateInstance();
        
        protected override void Start()
        {
            DontDestroyOnLoad(this);
            
            _context.Register(updateService);
            _context.Register(dragInput);
            _context.Register(levelDatabase);
            _context.Register(config);

            ISaveDataContainer saveDataContainer = InstallSaveDataContainer();
            InstallMoneyContainer(saveDataContainer);
            InstallLevelProgressService(saveDataContainer);
            InstallScreenNavigator();
            CameraProvider cameraProvider = InstallCameraProvider();
            InstallSceneLoader();
            InstallTimeScaler();
            InstallPositionedDragInput(dragInput, cameraProvider);
            
            StateMachine stateMachine = InstallStateMachine();
            
            stateMachine.Load();
            stateMachine.Enter<InitialState>();
        }

        private void InstallSceneLoader()
        {
            _context.Register(new SceneLoader(updateService));
        }

        private ISaveDataContainer InstallSaveDataContainer()
        {
            var saveDataContainer = new PlayerPrefsSaveDataContainer();
            _context.Register<ISaveDataContainer>(saveDataContainer);

            return saveDataContainer;
        }

        private void InstallScreenNavigator()
        {
            screenNavigator.Bootstrap();
            _context.Register(screenNavigator);
        }

        private void InstallLevelProgressService(ISaveDataContainer saveDataContainer)
        {
            _context.Register(new LevelsLoopProgress(
                saveDataContainer,
                levelDatabase.Levels.Select(levelData => levelData.name).ToArray()
            ));
        }

        private void InstallTimeScaler()
        {
            _context.Register(new TimeScaler());
        }

        private CameraProvider InstallCameraProvider()
        {
            CameraProvider cameraProvider = new CameraProvider();
            _context.Register(cameraProvider);

            return cameraProvider;
        }

        private void InstallPositionedDragInput(DragInput dragInput, CameraProvider cameraProvider)
        {
            _context.Register(new PositionedDragInput(dragInput, cameraProvider));
        }

        private StateMachine InstallStateMachine()
        {
            StateMachine stateMachine = new StateMachine(new StateFactory(_context));
            
            _context.Register(stateMachine);
            return stateMachine;
        }

        private void InstallMoneyContainer(ISaveDataContainer saveDataContainer)
        {
            _context.Register(new MoneyDataContainer(saveDataContainer));
        }
    }
}
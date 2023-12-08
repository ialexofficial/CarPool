using System.Linq;
using CarPool.Level;
using CarPool.States.Tutorial;
using CarPool.States.Tutorial.Steps;
using CarPool.States;
using CarPool.Tools;
using CarPool.UI.Views;
using Client;
using Ji2.CommonCore;
using Ji2.CommonCore.SaveDataContainer;
using Ji2.Context;
using Ji2.Presenters.Tutorial;
using Ji2Core.Core;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using Ji2Core.Core.Tools;
using Ji2Core.Core.UserInput;
using UnityEngine;

namespace CarPool
{
    sealed class Bootstrap : BootstrapBase
    {
        [SerializeField] private UpdateService updateService;
        [SerializeField] private DragInput dragInput;
        [SerializeField] private ScreenNavigator screenNavigator;
        [SerializeField] private LevelDatabase levelDatabase;
        [SerializeField] private Config config;
        [SerializeField] private TutorialPointer tutorialPointer;

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
            levelDatabase.Bootstrap();
            InstallLevelProgressService(saveDataContainer);
            InstallScreenNavigator();
            CameraProvider cameraProvider = InstallCameraProvider();
            InstallSceneLoader();
            InstallTimeScaler();
            InstallTrackingCamera(cameraProvider);
            InstallPositionedDragInput(dragInput, cameraProvider);
            
            StateMachine stateMachine = InstallStateMachine();
            InstallTutorialService(saveDataContainer, stateMachine);
            
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
                levelDatabase.GetLevelsOrder()
            ));
        }

        private void InstallTimeScaler()
        {
            _context.Register(new TimeScaler());
        }

        private void InstallTrackingCamera(CameraProvider cameraProvider)
        {
            _context.Register(new TrackingCamera(cameraProvider, updateService));
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

        private void InstallTutorialService(ISaveDataContainer saveDataContainer, StateMachine stateMachine)
        {
            var tutorialStepFactory = new TutorialStepFactory(_context,
                stateMachine,
                tutorialPointer
            );
            
            _context.Register(new TutorialService(saveDataContainer, new []
                {
                    tutorialStepFactory.Create<InitialTutorialStep>(),
                    tutorialStepFactory.Create<MultipleCarTutorialStep>()
                }
            ));
        }

        private void InstallMoneyContainer(ISaveDataContainer saveDataContainer)
        {
            _context.Register(new MoneyDataContainer(saveDataContainer));
        }
    }
}
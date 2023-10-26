using System.Linq;
using CarPool.Entities.Views;
using CarPool.Entities.Views.Cars;
using CarPool.Level;
using CarPool.Level.Presenters;
using CarPool.Tools;
using Client;
using Cysharp.Threading.Tasks;
using Ji2.Context;
using Ji2Core.Core;
using Ji2Core.Core.ScreenNavigation;
using Ji2Core.Core.States;
using Ji2Core.Core.UserInput;
using UI.Screens;
using Ji2.CommonCore;
using Ji2.CommonCore.SaveDataContainer;
using ClearLevelData = CarPool.Level.LevelData;

namespace CarPool.States
{
    public class LevelLoadingState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly Context _context;
        private readonly ScreenNavigator _screenNavigator;
        private readonly LevelsLoopProgress _levelProgressService;
        private readonly SceneLoader _sceneLoader;
        private readonly LevelDatabase _levelDatabase;
        private readonly ISaveDataContainer _saveDataContainer;

        public LevelLoadingState(Context context, StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _context = context;

            _screenNavigator = _context.GetService<ScreenNavigator>();
            _saveDataContainer = _context.SaveDataContainer;
            _levelProgressService = _context.GetService<LevelsLoopProgress>();
            _levelDatabase = _context.GetService<LevelDatabase>();
            _sceneLoader = _context.GetService<SceneLoader>();
        }

        public async UniTask Enter()
        {
            await _screenNavigator.PushScreen<LoadingScreen>();

            LevelData rawData = _levelProgressService.GetNextLevelData();
            ClearLevelData levelData = _levelDatabase.Levels.First(data => data.name == rawData.name);
            await _sceneLoader.LoadScene(levelData.name);

            LevelPresenter levelPresenter = new LevelPresenter(
                _context.GetService<Config>(),
                _context.GetService<TrackingCamera>(),
                levelData,
                _levelProgressService,
                _context.GetService<SpawnPoint[]>(),
                _context.GetService<DragInput>(),
                _context.GetService<PositionedDragInput>(),
                _context.GetService<UpdateService>(),
                _context.GetService<Finish>(),
                _context.GetService<Coin[]>(),
                _context.GetService<MoneyDataContainer>(),
                _context.GetService<StaticCar[]>()
            );
            levelPresenter.BuildModels();
            
            _stateMachine.Enter<GameState, GamePayload>(new GamePayload(levelPresenter));
        }

        public async UniTask Exit()
        {
        }
    }
}
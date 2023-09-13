using System;
using CarPool.Entities.Views;
using CarPool.Entities.Views.Cars;
using CarPool.Level.Models;
using CarPool.Level.Views;
using CarPool.Tools;
using CarPool.UI.Views;
using Client;
using Ji2.CommonCore;
using Ji2Core.Core.Tools;
using Ji2Core.Core.UserInput;

namespace CarPool.Level.Presenters
{
    public class LevelPresenter
    {
        private readonly Config _config;
        private readonly LevelData _levelData;
        private readonly DragInput _dragInput;
        private readonly UpdateService _updateService;
        private readonly Finish _finish;
        private readonly Coin[] _coins;
        private readonly MoneyDataContainer _moneyDataContainer;
        private readonly LevelsLoopProgress _levelProgress;
        private readonly StaticCar[] _staticCars;
        private readonly PositionedDragInput _positionedDragInput;
        private readonly SpawnPoint[] _spawnPoints;

        private LevelModel _levelModel;
        private MoneyModel _moneyModel;
        private LevelView _levelView;

        public event Action OnLevelEnd;

        public LevelPresenter(
            Config config,
            LevelData levelData,
            LevelsLoopProgress levelProgress,
            SpawnPoint[] spawnPoints,
            DragInput dragInput,
            PositionedDragInput positionedDragInput,
            UpdateService updateService,
            Finish finish,
            Coin[] coins,
            MoneyDataContainer moneyDataContainer,
            StaticCar[] staticCars
        )
        {
            _config = config;
            _levelData = levelData;
            _spawnPoints = spawnPoints;
            _dragInput = dragInput;
            _positionedDragInput = positionedDragInput;
            _updateService = updateService;
            _finish = finish;
            _coins = coins;
            _moneyDataContainer = moneyDataContainer;
         
            _levelProgress = levelProgress;
            _staticCars = staticCars;
        }

        public void BuildModels()
        {
            _levelModel = new LevelModel(
                _config,
                _levelData,
                _levelProgress,
                _spawnPoints,
                _dragInput,
                _positionedDragInput,
                _updateService,
                _finish,
                _staticCars
            );
            _moneyModel = new MoneyModel(_moneyDataContainer, _coins);
            
            _levelModel.Build();
            _levelModel.OnWin += _moneyModel.Save;
        }

        public void BuildView(GameScreen gameScreen, TimeScaler timeScaler)
        {
            _levelView = new LevelView(
                this, 
                gameScreen, 
                timeScaler,
                _levelProgress.GetNextLevelData().uniqueLevelNumber + 1
            );

            _levelModel.OnWin += _levelView.OnWon;
            _levelModel.OnLose += _levelView.OnLost;
            _levelModel.OnSwipeCountChange += _levelView.OnSwipeCountChanged;
            _moneyModel.OnMoneyAmountChange += _levelView.OnMoneyAmountChanged;
            
            _levelView.OnMoneyAmountChanged(_moneyDataContainer.MoneyAmount);
            _levelView.OnSwipeCountChanged(_levelData.SwipeCount);
        }

        public void ResetSaves()
        {
            _moneyModel.ResetSaves();
            _levelModel.ResetSaves();
            ReloadLevel();
        }

        public void ReloadLevel()
        {
            OnLevelEnd?.Invoke();
        }

        public void LoadNextLevel()
        {
            OnLevelEnd?.Invoke();
        }

        public void Clear()
        {
            _levelModel.Clear();
        }
    }
}
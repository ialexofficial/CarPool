using System;
using System.Collections.Generic;
using CarPool.Entities;
using CarPool.Entities.Models.Cars;
using CarPool.Entities.Views;
using CarPool.Entities.Views.Cars;
using CarPool.Tools;
using Client;
using Ji2.CommonCore;
using Ji2.CommonCore.SaveDataContainer;
using Ji2.Models;
using Ji2Core.Core.UserInput;

namespace CarPool.Level.Models
{
    public class LevelModel : LevelBase<ProgressBase>
    {
        private readonly CarFactory carFactory;
        private readonly Finish _finish;
        private readonly LevelData _levelData;
        private readonly StaticCar[] _staticCars;
        private readonly LevelsLoopProgress _levelProgress;
        private readonly PositionedDragInput _positionedDragInput;
        private readonly DragInput _dragInput;
        private readonly SpawnPoint[] _spawnPoints;
        private readonly List<MovableCarModel> _movableCarModels = new();

        private bool _isLevelEnded;
        private int _leftSwipeCount;
        private PlayerCarModel _playerCar;

        public event Action OnWin;
        public event Action OnLose;
        public event Action<int> OnSwipeCountChange;

        public MovableCar PlayerCar { get; private set; }
        public List<MovableCar> MovableCars { get; } = new();

        public LevelModel(
            Config config,
            LevelData levelData,
            TrackingCamera trackingCamera,
            LevelsLoopProgress levelProgress,
            ISaveDataContainer saveDataContainer,
            SpawnPoint[] spawnPoints,
            DragInput dragInput,
            PositionedDragInput positionedDragInput,
            UpdateService updateService,
            Finish finish,
            StaticCar[] staticCars
        ) : base(null, levelProgress.GetNextLevelData(), saveDataContainer)
        {
            carFactory = new CarFactory(
                trackingCamera,
                updateService,
                positionedDragInput
            );
            _spawnPoints = spawnPoints;
            _levelData = levelData;
            _finish = finish;
            _levelProgress = levelProgress;
            _staticCars = staticCars;
            _dragInput = dragInput;
            _positionedDragInput = positionedDragInput;

            _leftSwipeCount = _levelData.SwipeCount;
        }

        public void Build()
        {
            foreach (var spawnPoint in _spawnPoints)
            {
                switch (spawnPoint.Settings.CarType)
                {
                    case CarType.MovableCar:
                        var (movableCar, movableCarModel) = carFactory.Build<MovableCarModel>(spawnPoint);
                        movableCarModel.OnSwipeApply += OnSwipeApplied;
                        _movableCarModels.Add(movableCarModel);
                        MovableCars.Add(movableCar);
                        break;
                    case CarType.PlayerCar:
                        (PlayerCar, _playerCar) = carFactory.Build<PlayerCarModel>(spawnPoint);
                        _playerCar.OnCarStateChange += (state) =>
                            CheckLoseRequirements();
                        _playerCar.OnDestroy += () => CheckLoseRequirements(true);
                        _playerCar.OnSwipeApply += OnSwipeApplied;
                        break;
                }
            }

            _finish.OnFinish += OnFinished;

            foreach (var car in _staticCars)
            {
                car.OnDestroy += () => CheckLoseRequirements(true);
            }
        }

        public void ResetSaves()
        {
            _levelProgress.Reset();
        }

        private void OnSwipeApplied()
        {
            _leftSwipeCount -= 1;
            OnSwipeCountChange?.Invoke(_leftSwipeCount);
            CheckLoseRequirements();
        }

        private void OnFinished()
        {
            _isLevelEnded = true;
            _dragInput.Enabled = false;
            _levelProgress.IncLevel();
            OnWin?.Invoke();
        }

        private void CheckLoseRequirements(bool isCarDestroyed = false)
        {
            if (_isLevelEnded)
                return;

            if (
                isCarDestroyed ||
                (_leftSwipeCount == 0 && _playerCar.State is CarState.Stopped)
            )
            {
                _finish.IsEnabled = false;
                _isLevelEnded = true;
                _dragInput.Enabled = false;
                OnLose?.Invoke();
            }
        }

        public void Clear()
        {
            _finish.OnFinish -= OnFinished;
            
            carFactory.Clear(_playerCar);
            foreach (var car in _movableCarModels)
            {
                carFactory.Clear(car);
            }
            
            _dragInput.Enabled = true;
        }
    }
}
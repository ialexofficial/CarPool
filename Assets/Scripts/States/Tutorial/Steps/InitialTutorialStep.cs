using System;
using System.Threading;
using CarPool.Entities.Views;
using CarPool.Entities.Views.Cars;
using CarPool.UI.Views;
using Client;
using Cysharp.Threading.Tasks;
using Ji2.Context;
using Ji2.Presenters.Tutorial;
using Ji2Core.Core;
using Ji2Core.Core.States;
using Ji2Core.Core.UserInput;
using UnityEngine;

namespace CarPool.States.Tutorial.Steps
{
    public class InitialTutorialStep : ITutorialStep
    {
        private const float HelpDuration = 15f;
        
        private readonly Context _context;
        private readonly StateMachine _stateMachine;
        private readonly TutorialPointer _tutorialPointer;
        private readonly CameraProvider _cameraProvider;
        private readonly LevelsLoopProgress _levelsLoopProgress;

        private CancellationTokenSource _timerCancellationTokenSource = new();
        private CancellationTokenSource _animationCancellationTokenSource = new();
        private DragInput _dragInput;
        private MovableCar _playerCar;
        private Finish _finish;
        private Camera _camera;
        private GameState _gameState;

        public event Action Completed;
        
        public string SaveKey { get; } = "InitialTutorialStep";
        
        public InitialTutorialStep(Context context, StateMachine stateMachine, TutorialPointer tutorialPointer)
        {
            _context = context;
            _stateMachine = stateMachine;
            _tutorialPointer = tutorialPointer;

            _cameraProvider = _context.GetService<CameraProvider>();
            _levelsLoopProgress = _context.LevelsLoopProgress;
        }

        public void Run()
        {
            _dragInput = _context.GetService<DragInput>();

            _dragInput.OnDragBegin += (eventData) =>
            {
                _animationCancellationTokenSource.Cancel();
            };
            _dragInput.OnDragging += (eventData) =>
            {
                _animationCancellationTokenSource.Cancel();
            };
            _stateMachine.StateEntered += OnStateEntered;
        }

        private async void OnStateEntered(IExitableState state)
        {
            if (_gameState is not null)
            {
                _gameState.Payload.LevelPresenter.OnLevelEnd -= CancelTokens;
            }

            _gameState = state as GameState;
            if (_gameState is null)
            {
                CancelTokens();
                return;
            }
            
            _timerCancellationTokenSource.Dispose();
            _timerCancellationTokenSource = new();

            _gameState.Payload.LevelPresenter.OnLevelEnd += CancelTokens;
            _playerCar = _gameState.Payload.LevelPresenter.PlayerCar;
            _finish = _context.GetService<Finish>();
            _camera = _cameraProvider.MainCamera;
            
            if (_levelsLoopProgress.GetNextLevelData().uniqueLevelNumber == 0)
            {
                _animationCancellationTokenSource.Dispose();
                _animationCancellationTokenSource = new();
                
                await LoopAnimation(_animationCancellationTokenSource.Token);
            }

            await RunHelpTimer(_timerCancellationTokenSource.Token);
        }

        private async UniTask RunHelpTimer(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                _animationCancellationTokenSource.Dispose();
                _animationCancellationTokenSource = new();

                await UniTask.Delay(
                        TimeSpan.FromSeconds(HelpDuration),
                        cancellationToken: _animationCancellationTokenSource.Token
                    )
                    .SuppressCancellationThrow();

                await LoopAnimation(_animationCancellationTokenSource.Token);
            }
        }

        private async UniTask LoopAnimation(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Vector3 carScreenPosition = _camera.WorldToScreenPoint(_playerCar.transform.position);
                Vector3 finishScreenPosition = _camera.WorldToScreenPoint(_finish.transform.position);
                
                await _tutorialPointer.AnimateSwipe(
                    carScreenPosition,
                    (carScreenPosition - finishScreenPosition).normalized,
                    cancellationToken
                );
            }
        }

        private void CancelTokens()
        {
            _timerCancellationTokenSource.Cancel();
            _animationCancellationTokenSource.Cancel();
        }
    }
}
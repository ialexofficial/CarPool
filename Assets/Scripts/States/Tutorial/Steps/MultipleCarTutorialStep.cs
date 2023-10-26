using System;
using System.Threading;
using CarPool.Entities.Views.Cars;
using CarPool.UI.Views;
using Cysharp.Threading.Tasks;
using Ji2.Context;
using Ji2.Presenters.Tutorial;
using Ji2Core.Core;
using Ji2Core.Core.States;
using Ji2Core.Core.UserInput;
using UnityEngine;

namespace CarPool.States.Tutorial.Steps
{
    public class MultipleCarTutorialStep : ITutorialStep
    {
        private readonly Context _context;
        private readonly StateMachine _stateMachine;
        private readonly TutorialPointer _tutorialPointer;
        private readonly CameraProvider _cameraProvider;
        
        private DragInput _dragInput;
        private CancellationTokenSource _animationCancellationTokenSource = new();

        public event Action Completed;

        public string SaveKey { get; } = "MultipleCarTutorialStep";

        public MultipleCarTutorialStep(Context context, StateMachine stateMachine, TutorialPointer tutorialPointer)
        {
            _context = context;
            _stateMachine = stateMachine;
            _tutorialPointer = tutorialPointer;

            _cameraProvider = _context.GetService<CameraProvider>();
        }

        public void Run()
        {
            _dragInput = _context.GetService<DragInput>();

            _dragInput.OnDragBegin += OnDragBegan;
            _stateMachine.StateEntered += OnStateEntered;
        }

        private async void OnStateEntered(IExitableState state)
        {
            if (state is not GameState gameState)
                return;

            var movableCars = gameState.Payload.LevelPresenter.MovableCars;

            if (movableCars.Count == 0)
                return;

            _animationCancellationTokenSource = new();
            MovableCar movableCar = movableCars[0];
            Camera camera = _cameraProvider.MainCamera;

            await LoopAnimation(movableCar, camera, _animationCancellationTokenSource.Token);
            
            Complete();
        }
        
        private async UniTask LoopAnimation(
            MovableCar car,
            Camera camera, 
            CancellationToken cancellationToken
        )
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                Vector3 carScreenPosition = camera.WorldToScreenPoint(car.transform.position);
                Vector3 endPoint = camera.WorldToScreenPoint(car.transform.position + car.transform.forward);

                await _tutorialPointer.AnimateSwipe(
                    carScreenPosition,
                    (carScreenPosition - endPoint).normalized,
                    cancellationToken
                );
            }
        }

        private void OnDragBegan(Vector2 eventData)
        {
            _animationCancellationTokenSource.Cancel();
        }

        private void Complete()
        {
            Completed?.Invoke();
            _stateMachine.StateEntered -= OnStateEntered;
            _dragInput.OnDragBegin -= OnDragBegan;
            _animationCancellationTokenSource.Dispose();
        }
    }
}
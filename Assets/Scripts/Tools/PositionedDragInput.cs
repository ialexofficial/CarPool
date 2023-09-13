using System.Collections.Generic;
using System.Linq;
using Ji2Core.Core;
using Ji2Core.Core.UserInput;
using UnityEngine;

namespace CarPool.Tools
{
    public class PositionedDragInput
    {
        private readonly Vector3 GroundPosition = new Vector3(0, 0, 5.5f);
        private readonly DragInput _dragInput;
        private readonly CameraProvider _cameraProvider;
        private readonly Dictionary<IDragInputHandler, Transform> _observers = new();

        private Vector2 _beginPosition;
        private IDragInputHandler _closestHandler;

        public PositionedDragInput(
            DragInput dragInput,
            CameraProvider cameraProviderProvider
        )
        {
            _dragInput = dragInput;
            _cameraProvider = cameraProviderProvider;

            _dragInput.OnDragBegin += OnDragBegan;
            _dragInput.OnDragging += OnDragged;
            _dragInput.OnDragEnd += OnDragEnded;
            _dragInput.OnSwipe += OnSwiped;
        }

        public void Subscribe(IDragInputHandler observer, Transform transform)
        {
            _observers[observer] = transform;
        }

        public void Unsubscribe(IDragInputHandler observer)
        {
            _observers.Remove(observer);
        }

        private void OnDragBegan(Vector2 position)
        {
            _beginPosition = position;
            _closestHandler = GetClosest(position);
            _closestHandler.OnDragBegan(position);
        }

        private void OnDragged(Vector2 position)
        {
            _closestHandler.OnDragged(position);
        }

        private void OnDragEnded(Vector2 position)
        {
            _closestHandler.OnDragEnded(position);
        }

        private void OnSwiped(Vector2 swipe, float time)
        {
            _closestHandler.OnSwiped(swipe, time);
        }

        private IDragInputHandler GetClosest(Vector2 position)
        {
            Vector3 worldPosition = _cameraProvider.MainCamera.ScreenToWorldPoint(new Vector3(
                position.x, 
                position.y, 
                Vector3.Distance(GroundPosition, _cameraProvider.MainCamera.transform.position)
            ));

            (IDragInputHandler handler, Transform transform) = _observers.Aggregate((pair1, pair2) =>
                (worldPosition - pair1.Value.position).magnitude <= (worldPosition - pair2.Value.position).magnitude
                    ? pair1
                    : pair2
            );

            return handler;
        }
    }

    public interface IDragInputHandler
    {
        public void OnDragBegan(Vector2 position);
        public void OnDragged(Vector2 position);
        public void OnDragEnded(Vector2 position);
        public void OnSwiped(Vector2 swipe, float time);
    }
}
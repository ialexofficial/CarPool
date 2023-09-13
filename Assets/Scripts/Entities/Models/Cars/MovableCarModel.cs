using System;
using CarPool.Tools;
using Ji2.CommonCore;
using UnityEngine;

namespace CarPool.Entities.Models.Cars
{
    public class MovableCarModel : IDragInputHandler, IFixedUpdatable
    {
        private readonly PointerModel _pointerModel;
        private readonly MovableCarSettings _settings;

        private Vector3 _force = Vector3.zero;
        private Vector2 _beginDragPosition;
        private CarState _state = CarState.Stopped;

        public event Action<Quaternion> OnRotate;
        public event Action<Vector3> OnForceAdd;
        public event Action<CarState> OnCarStateChange;
        
        public CarState State
        {
            get => _state;
            set
            {
                _state = value;
                OnCarStateChange?.Invoke(_state);
            }
        }

        public MovableCarModel(MovableCarSettings settings, PointerModel pointerModel)
        {
            _settings = settings;
            _pointerModel = pointerModel;
        }
        
        public void OnCarStop()
        {
            State = State switch
            {
                CarState.Accelerating => CarState.Moving,
                CarState.Moving => CarState.Stopped,
                _ => State
            };
        }
        
        public bool IsInputEnabled() =>
            State is CarState.Stopped;
        
        
        void IDragInputHandler.OnDragBegan(Vector2 position)
        {
            _beginDragPosition = position;
            _pointerModel.OnBeginPositionedDrag(position);
        }

        void IDragInputHandler.OnDragged(Vector2 position)
        {
            if (!IsInputEnabled())
                return;
            
            Vector2 delta = _beginDragPosition - position;

            Vector3 direction = new Vector3(delta.x, 0, delta.y).normalized;

            OnRotate?.Invoke(Quaternion.LookRotation(direction));
            
            _pointerModel.OnPositionedDrag(position);
        }
        
        void IDragInputHandler.OnDragEnded(Vector2 position)
        {
            _pointerModel.OnEndPositionedDrag(position);
        }

        void IDragInputHandler.OnSwiped(Vector2 swipe, float time)
        {
            if (!IsInputEnabled())
                return;
            
            _force = -new Vector3(swipe.x, 0, swipe.y);

            if (_force.magnitude > _settings.MaxSwipeMagnitude)
                _force = _force.normalized * _settings.MaxSwipeMagnitude;

            _force *= _settings.SwipeStrength;
        }

        void IFixedUpdatable.OnFixedUpdate()
        {
            if (_force != Vector3.zero)
            {
                State = CarState.Accelerating;
                OnForceAdd?.Invoke(_force);
                _force = Vector3.zero;
            }
        }
    }
    
    public enum CarState
    {
        Stopped,
        Moving,
        Accelerating
    }
}
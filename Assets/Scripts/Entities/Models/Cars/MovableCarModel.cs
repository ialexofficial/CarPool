using System;
using CarPool.Tools;
using Ji2.CommonCore;
using Ji2Core.Core.Tools;
using UnityEngine;

namespace CarPool.Entities.Models.Cars
{
    public class MovableCarModel : IDragInputHandler, IFixedUpdatable
    {
        private readonly PointerModel _pointerModel;
        private readonly MovableCarSettings _settings;

        private Vector3 _velocity = Vector3.zero;
        private Vector2 _beginDragPosition;
        private CarState _state = CarState.Stopped;
        private bool _isReflected = false;
        private GameObject _lastCollidedBorder;

        public event Action<Quaternion> OnRotate;
        public event Action<Vector3> OnVelocityChange;
        public event Action OnSwipeApply;
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

        public void OnCollisionEnter(Collision collision)
        {
            if (
                !LayerUtils.CheckLayer(_settings.borderLayer, collision.gameObject.layer) ||
                _lastCollidedBorder == collision.gameObject ||
                State is CarState.Stopped
            )
                return;

            _lastCollidedBorder = collision.gameObject;
            Vector3 reflectedVelocity = Vector3.Reflect(-collision.relativeVelocity, collision.GetContact(0).normal);

            _velocity = reflectedVelocity;
            OnRotate?.Invoke(Quaternion.LookRotation(_velocity.normalized));
            _isReflected = true;
        }
        
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
            
            _velocity = -new Vector3(swipe.x, 0, swipe.y);

            if (_velocity.magnitude > _settings.MaxSwipeMagnitude)
                _velocity = _velocity.normalized * _settings.MaxSwipeMagnitude;

            _velocity *= _settings.SwipeStrength;
            _lastCollidedBorder = null;
        }

        void IFixedUpdatable.OnFixedUpdate()
        {
            if (_velocity != Vector3.zero)
            {
                State = CarState.Accelerating;
                OnVelocityChange?.Invoke(_velocity);
                _velocity = Vector3.zero;

                if (!_isReflected)
                {
                    OnSwipeApply?.Invoke();
                }
                else
                {
                    _isReflected = false;
                }
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
using System;
using UnityEngine;

namespace CarPool.Entities.Models
{
    public class PointerModel
    {
        private readonly MovableCarSettings _settings;
        private Vector2 _beginDragPosition;

        public event Action OnShow;
        public event Action<float> OnTransform;
        public event Action OnHide;

        public PointerModel(MovableCarSettings settings)
        {
            _settings = settings;
        }
        
        public void OnBeginPositionedDrag(Vector2 position)
        {
            _beginDragPosition = position;
        }

        public void OnPositionedDrag(Vector2 position)
        {
            OnShow?.Invoke();
            Vector2 delta = _beginDragPosition - position;
            float scale = delta.magnitude / _settings.MaxSwipeMagnitude;

            if (scale > 1)
                scale = 1;

            OnTransform?.Invoke(scale);
        }

        public void OnEndPositionedDrag(Vector2 position)
        {
            OnHide?.Invoke();
        }
    }
}
using System;
using CarPool.Entities.Models;

namespace CarPool.Entities.ViewModels
{
    public class PointerVM
    {
        private PointerModel _model;

        public event Action OnShow;
        public event Action<float> OnTransform;
        public event Action OnHide;
        
        public PointerVM(PointerModel model)
        {
            _model = model;

            _model.OnShow += () => OnShow?.Invoke();
            _model.OnTransform += (scale) => OnTransform?.Invoke(scale);
            _model.OnHide += () => OnHide?.Invoke();
        }
    }
}
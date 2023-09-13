using CarPool.Entities.ViewModels;
using UnityEngine;

namespace CarPool.Entities.Views
{
    public class Pointer : MonoBehaviour
    {
        private PointerVM _viewModel;
        private Vector3 _defaultScale;
        
        public void Construct(PointerVM viewModel)
        {
            _viewModel = viewModel;

            _viewModel.OnShow += OnShowed;
            _viewModel.OnTransform += OnTransformed;
            _viewModel.OnHide += OnHidden;
        }

        private void Start()
        {
            _defaultScale = transform.localScale;
            
            OnHidden();
        }

        private void OnShowed()
        {
            gameObject.SetActive(true);
        }

        private void OnTransformed(float scale)
        {
            transform.localScale = _defaultScale * scale;
        }

        private void OnHidden()
        {
            gameObject.SetActive(false);
        }
    }
}
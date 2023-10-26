using CarPool.Entities.ViewModels;
using UnityEngine;

namespace CarPool.Entities.Views
{
    public class Pointer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        private PointerVM _viewModel;
        private Vector2 _defaultScale;
        
        public void Construct(PointerVM viewModel)
        {
            _viewModel = viewModel;

            _viewModel.OnShow += OnShowed;
            _viewModel.OnTransform += OnTransformed;
            _viewModel.OnHide += OnHidden;
        }

        private void Start()
        {
            _defaultScale = spriteRenderer.size;
            
            OnHidden();
        }

        private void OnShowed()
        {
            gameObject.SetActive(true);
        }

        private void OnTransformed(float scale)
        {
            var size = _defaultScale;
            size.y += size.y * scale;
            spriteRenderer.size = size;
        }

        private void OnHidden()
        {
            gameObject.SetActive(false);
        }
    }
}
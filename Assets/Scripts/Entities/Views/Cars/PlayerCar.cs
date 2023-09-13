using CarPool.Entities.ViewModels.Cars;
using UnityEngine;

namespace CarPool.Entities.Views.Cars
{
    public class PlayerCar : MovableCar, IDestroyable
    {
        private PlayerCarVM _viewModel;

        public void Construct(PlayerCarVM viewModel)
        {
            base.Construct(viewModel);

            _viewModel = viewModel;
        }
        
        void IDestroyable.Destroy()
        {
            _rigidbody.constraints = RigidbodyConstraints.None;
            _viewModel.Destroy();
        }
    }
}
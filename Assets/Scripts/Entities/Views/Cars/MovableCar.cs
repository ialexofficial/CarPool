using CarPool.Entities.ViewModels.Cars;
using UnityEngine;

namespace CarPool.Entities.Views.Cars
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovableCar : MonoBehaviour
    {
        private const float StopVelocity = .1f;
        
        protected Rigidbody _rigidbody;
        private MovableCarVM _viewModel;

        public void Construct(MovableCarVM viewModel)
        {
            _viewModel = viewModel;

            _viewModel.OnForceAdd += OnForceAdded;
            _viewModel.OnRotate += OnRotated;
        }

        protected virtual void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        protected virtual void FixedUpdate()
        {
            if (_rigidbody.velocity.magnitude <= StopVelocity)
            {
                _viewModel.OnCarStop();
            }
        }

        private void OnForceAdded(Vector3 force)
        {
            _rigidbody.AddForce(force);
        }

        private void OnRotated(Quaternion rotation)
        {
            transform.rotation = rotation;
        }
    }
}
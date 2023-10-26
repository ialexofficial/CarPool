using System;
using CarPool.Entities.Models.Cars;
using UnityEngine;

namespace CarPool.Entities.ViewModels.Cars
{
    public class MovableCarVM
    {
        private readonly MovableCarModel _model;

        public event Action<Vector3> OnVelocityChange;
        public event Action<Quaternion> OnRotate;

        public MovableCarVM(MovableCarModel model)
        {
            _model = model;

            _model.OnVelocityChange += (velocity) => OnVelocityChange?.Invoke(velocity);
            _model.OnRotate += (rotation) => OnRotate?.Invoke(rotation);
        }

        public void OnCarStop()
        {
            _model.OnCarStop();
        }

        public void OnCollisionEnter(Collision collision)
        {
            _model.OnCollisionEnter(collision);
        }
    }
}
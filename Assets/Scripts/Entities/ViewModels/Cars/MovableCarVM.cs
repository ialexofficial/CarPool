using System;
using CarPool.Entities.Models.Cars;
using UnityEngine;

namespace CarPool.Entities.ViewModels.Cars
{
    public class MovableCarVM
    {
        private readonly MovableCarModel _model;

        public event Action<Vector3> OnForceAdd;
        public event Action<Quaternion> OnRotate;

        public MovableCarVM(MovableCarModel model)
        {
            _model = model;

            _model.OnForceAdd += (force) => OnForceAdd?.Invoke(force);
            _model.OnRotate += (rotation) => OnRotate?.Invoke(rotation);
        }

        public void OnCarStop()
        {
            _model.OnCarStop();
        }
    }
}
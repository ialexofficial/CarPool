using System;
using Ji2Core.Core.Tools;
using UnityEngine;

namespace CarPool.Entities.Views.Cars
{
    public class StaticCar : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayer;
        
        public event Action OnDestroy;
        
        private void OnCollisionEnter(Collision collision)
        {
            if (LayerMasker.CheckLayer(playerLayer, collision.gameObject.layer))
            {
                OnDestroy?.Invoke();
            }
        }
    }
}
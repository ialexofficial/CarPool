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
            if (playerLayer.CheckLayer(collision.gameObject.layer))
            {
                OnDestroy?.Invoke();
            }
        }
    }
}
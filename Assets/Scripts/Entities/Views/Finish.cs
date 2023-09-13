using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CarPool.Entities.Views
{
    public class Finish : MonoBehaviour
    {
        [SerializeField] private FinishSettings finishSettings;
        [SerializeField] private ParticleSystem finishVFX;

        public event Action OnFinish;

        public bool IsEnabled { get; set; } = true;

        private void OnTriggerStay(Collider other)
        {
            if (!IsEnabled)
                return;
            
            Rigidbody rigidbody = other.attachedRigidbody;

            if (rigidbody.velocity.magnitude <= finishSettings.FinishVelocity)
            {
                IsEnabled = false;
                
                finishVFX.Play();
                OnFinish?.Invoke();
            }
        }
    }
}
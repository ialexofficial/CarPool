using System;
using UnityEngine;

namespace CarPool.Entities.Views
{
    public class Finish : MonoBehaviour
    {
        [SerializeField] private FinishSettings finishSettings;
        [SerializeField] private ParticleSystem finishVFX;

        public event Action OnFinish;

        public bool IsEnabled { get; set; } = true;

        private void OnTriggerEnter(Collider other)
        {
            if (!IsEnabled)
                return;
            
            Rigidbody rigidbody = other.attachedRigidbody;
            IsEnabled = false;
            rigidbody.velocity = Vector3.zero;

            finishVFX.Play();
            OnFinish?.Invoke();
        }
    }
}
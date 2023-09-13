using Ji2Core.Core.Tools;
using UnityEngine;

namespace CarPool.Entities.Views
{
    [RequireComponent(typeof(Collider))]
    public class Box : MonoBehaviour, IDestroyable
    {
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private Collider[] parts;

        private Collider _collider;
        private Rigidbody _rigidbody;
        
        private void Start()
        {
            _collider = GetComponent<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void OnCollisionEnter(Collision collision)
        {
            if (LayerMasker.CheckLayer(playerLayer, collision.gameObject.layer))
            {
                collision.gameObject.GetComponent<IDestroyable>().Destroy();
                ((IDestroyable) this).Destroy();
            }
        }
        
        void IDestroyable.Destroy()
        {
            _collider.enabled = false;
            _rigidbody.isKinematic = true;

            foreach (Collider part in parts)
            {
                part.enabled = true;
                part.attachedRigidbody.isKinematic = false;
            }
        }
    }
}
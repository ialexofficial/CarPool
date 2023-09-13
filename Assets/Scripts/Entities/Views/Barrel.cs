using Ji2Core.Core.Tools;
using UnityEngine;

namespace CarPool.Entities.Views
{
    [RequireComponent(typeof(Collider))]
    public class Barrel : MonoBehaviour
    {
        [SerializeField] private LayerMask interactableLayers;
        [SerializeField] private LayerMask destroyableLayers;
        [SerializeField] private float explosionRadius;
        [SerializeField] private float explosionForce;
        [SerializeField] private ParticleSystem explosionVFX;
        [SerializeField] private Collider[] parts;

#if UNITY_EDITOR
        [SerializeField] private bool drawGizmos;
#endif

        private readonly Collider[] _overlappedColliders = new Collider[16];
        private Collider _collider;

        private void Start()
        {
            _collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!LayerMasker.CheckLayer(interactableLayers, other.gameObject.layer))
                return;

            Explode();

            int length = Physics.OverlapSphereNonAlloc(
                transform.position,
                explosionRadius,
                _overlappedColliders,
                LayerMasker.MergeLayerMasks(interactableLayers, destroyableLayers)
            );

            for (int i = 0; i < length; ++i)
            {
                var collider = _overlappedColliders[i];

                if (collider == _collider)
                    continue;

                if (LayerMasker.CheckLayer(destroyableLayers, collider.gameObject.layer))
                {
                    collider.GetComponentInParent<IDestroyable>().Destroy();
                }

                if (LayerMasker.CheckLayer(interactableLayers, collider.gameObject.layer))
                {
                    collider.GetComponentInParent<Rigidbody>()?.AddExplosionForce(
                        explosionForce,
                        transform.position,
                        explosionRadius
                    );
                }
            }
        }

        private void Explode()
        {
            _collider.enabled = false;
            foreach (Collider part in parts)
            {
                part.enabled = true;
                part.attachedRigidbody.isKinematic = false;
            }

            explosionVFX.Play();
        }
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if (!drawGizmos)
                return;

            Gizmos.color = new Color(255, 0, 0, .5f);
            Gizmos.DrawSphere(transform.position, explosionRadius);
        }
#endif
    }
}
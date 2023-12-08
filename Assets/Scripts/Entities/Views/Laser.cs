using Ji2Core.Core.Tools;
using CarPool.Tools.EzySlice;
using UnityEngine;

namespace CarPool.Entities.Views
{
    public class Laser : MonoBehaviour
    {
        [SerializeField] private GameObject light;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private Material crossSectionMaterial;
        [SerializeField] private float maxSliceForce = 5f;

        private bool _enabled = true;

        public bool Enabled
        {
            get => _enabled;
            set
            {
                _enabled = value;
                light.SetActive(_enabled);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!playerLayer.CheckLayer(other.gameObject.layer))
                return;

            var velocity = other.GetComponentInParent<Rigidbody>().velocity;

            Vector3 normal = Vector3.Cross(
                transform.up,
                velocity.normalized
            );

            var slices = other.gameObject.SliceInstantiate(
                other.bounds.center,
                normal.normalized,
                crossSectionMaterial
            );

            ConfigureSlices(other, slices, velocity);
            other.transform.parent.GetComponent<Rigidbody>().isKinematic = true;
            Destroy(other.gameObject);

            other.GetComponentInParent<IDestroyable>().Destroy();
        }

        private void ConfigureSlices(Collider other, GameObject[] slices, Vector3 velocity)
        {
            Transform firstSliceTransform = slices[0].transform;
            Transform secondSliceTransform = slices[1].transform;

            firstSliceTransform.parent = secondSliceTransform.parent = other.transform.parent;
            firstSliceTransform.localPosition = secondSliceTransform.localPosition = Vector3.zero;
            firstSliceTransform.rotation = secondSliceTransform.rotation = other.transform.rotation;

            var firstSliceCollider = firstSliceTransform.gameObject.AddComponent<BoxCollider>();
            var secondSliceCollider = secondSliceTransform.gameObject.AddComponent<BoxCollider>();

            while (other.transform.childCount > 0)
            {
                var child = other.transform.GetChild(0);

                child.parent = Vector3.Distance(child.position, firstSliceCollider.bounds.center) <
                               Vector3.Distance(child.position, secondSliceCollider.bounds.center)
                    ? firstSliceTransform
                    : secondSliceTransform;
            }

            EncapsulateChildrenToCollider(firstSliceCollider);
            EncapsulateChildrenToCollider(secondSliceCollider);

            firstSliceTransform.gameObject.AddComponent<Rigidbody>()
                .AddForce(
                    velocity + new Vector3(
                        Random.Range(0f, maxSliceForce),
                        Random.Range(0f, maxSliceForce),
                        Random.Range(0f, maxSliceForce)
                    ),
                    ForceMode.VelocityChange
                );
            secondSliceTransform.gameObject.AddComponent<Rigidbody>()
                .AddForce(velocity, ForceMode.VelocityChange);
        }

        private void EncapsulateChildrenToCollider(BoxCollider parent)
        {
            Bounds newBounds = parent.bounds;

            foreach (Renderer child in parent.GetComponentsInChildren<Renderer>())
            {
                newBounds.Encapsulate(child.bounds);
            }

            parent.center += newBounds.center - parent.bounds.center;
            parent.size += newBounds.size - parent.bounds.size;
        }
    }
}
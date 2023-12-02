using Ji2Core.Core.Tools;
using UnityEngine;

namespace CarPool.Entities.Views
{
    public class OutBorder : MonoBehaviour
    {
        [SerializeField] private LayerMask playerLayer;
        
        private void OnTriggerEnter(Collider other)
        {
            if (!playerLayer.CheckLayer(other.gameObject.layer))
                return;
            
            other.GetComponentInParent<IDestroyable>().Destroy();
        }
    }
}
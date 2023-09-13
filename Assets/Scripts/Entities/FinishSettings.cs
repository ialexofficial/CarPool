using UnityEngine;

namespace CarPool.Entities
{
    [CreateAssetMenu(menuName = "ScriptableObjects/FinishSettings")]
    public class FinishSettings : ScriptableObject
    {
        public float FinishVelocity;
    }
}
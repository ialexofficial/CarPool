using UnityEngine;

namespace CarPool.Entities.Views
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] private MovableCarSettings settings;

        public MovableCarSettings Settings => settings;
    }
}
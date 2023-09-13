using CarPool.Entities.Views;
using CarPool.Entities.Views.Cars;
using UnityEngine;

namespace CarPool.Entities
{
    [CreateAssetMenu(menuName = "ScriptableObjects/MovableCarSettings")]
    public class MovableCarSettings : ScriptableObject
    {
        public MovableCar CarPrefab;
        public CarType CarType;
        public Pointer PointerPrefab;
        public float SwipeStrength;
        public float MaxSwipeMagnitude;
    }
    
    public enum CarType
    {
        MovableCar,
        PlayerCar
    }
}
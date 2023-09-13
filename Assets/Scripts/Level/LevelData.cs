using UnityEngine;

namespace CarPool.Level
{
    [CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/LevelData")]
    public class LevelData : ScriptableObject
    {
        public int SwipeCount;
    }
}
using Client;
using UnityEngine;

namespace CarPool.Level
{
    [CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/LevelData")]
    public class LevelData : ScriptableObject, ILevelViewData
    {
        [field: SerializeField] public string Id { get; private set; }
        [field: SerializeField] public int SwipeCount { get; private set; }
    }
}
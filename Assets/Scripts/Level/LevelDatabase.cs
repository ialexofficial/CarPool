using Client;
using UnityEngine;

namespace CarPool.Level
{
    [CreateAssetMenu(fileName = "LevelDatabase", menuName = "ScriptableObjects/LevelDatabase", order = 0)]
    public class LevelDatabase : LevelsViewDataStorageBase<LevelData>
    {
    }
}
#if UNITY_EDITOR
using System.Linq;
using CarPool.Level;
using Ji2.CommonCore.SaveDataContainer;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CarPool.Editor
{
    public class BootstrapInEditor : UnityEditor.Editor
    {
        [InitializeOnEnterPlayMode]
        public static void OnPlayMode(EnterPlayModeOptions options)
        {
            if (SceneManager.GetActiveScene().name == "Bootstrap")
                return;
            
            Debug.Log("Achtung scheiBe!");

            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.EnteredPlayMode)
                {
                    LevelDatabase levelDatabase = AssetDatabase
                        .LoadMainAssetAtPath(AssetDatabase
                            .GUIDToAssetPath(
                                AssetDatabase.FindAssets("LevelDatabase", new []
                                {
                                    "Assets/Scenes"
                                })[0]
                            )
                        ) as LevelDatabase;
                    
                    PlayerPrefsSaveDataContainer dataContainer = new PlayerPrefsSaveDataContainer();
                    dataContainer.Load();
                    dataContainer.SaveValue("LastLevelNumber",
                        levelDatabase.Levels
                            .Select(data => data.name).ToList()
                            .IndexOf(SceneManager.GetActiveScene().name));
                    
                    SceneManager.LoadScene("Bootstrap");
                }
            };
        }
    }
}
#endif
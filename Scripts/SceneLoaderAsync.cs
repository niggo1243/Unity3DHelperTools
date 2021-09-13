using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace NikosAssets.Helpers
{
    [Serializable]
    public class SceneLoadedUnityEvent : UnityEvent<Scene>
    {
        
    }
    
    public class SceneLoaderAsync : MonoBehaviour
    {
        public static event Action<Scene> SceneLoadedGlobalAdditiveSuccess;
        public event Action<Scene> SceneLoadedAdditiveSuccess;
        public SceneLoadedUnityEvent SceneLoadedUnityEvent = default;

        [SerializeField]
        private int[] scenesToLoadAdditive = default;

        private void Start()
        {
            this.StartCoroutine(this.LoadScenesAsync());
        }

        private IEnumerator LoadScenesAsync()
        {
            foreach (int sceneIndex in this.scenesToLoadAdditive)
            {
                Scene scene = SceneManager.GetSceneByBuildIndex(sceneIndex);

                if (scene != null)
                {
                    if (!scene.isLoaded)
                        yield return SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

                    //event handling
                    SceneLoaderAsync.SceneLoadedGlobalAdditiveSuccess?.Invoke(scene);
                    this.SceneLoadedAdditiveSuccess?.Invoke(scene);
                    this.SceneLoadedUnityEvent?.Invoke(scene);
                }
            }
        }
    }
}

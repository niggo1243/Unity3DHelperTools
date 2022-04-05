using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace NikosAssets.Helpers
{
    [Serializable]
    public class SceneLoadedUnityEvent : UnityEvent<Scene>
    {
        
    }
    
    public class SceneLoaderAsyncMono : BaseNotesMono
    {
        public static event Action<Scene> SceneLoadedGlobalAdditiveSuccess;
        public event Action<Scene> SceneLoadedAdditiveSuccess;
        
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_EVENTS)]
        public SceneLoadedUnityEvent SceneLoadedUnityEvent = default;

        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        protected int[] scenesToLoadAdditive = default;

        protected virtual void Start()
        {
            this.StartCoroutine(this.LoadScenesAsync());
        }

        protected virtual IEnumerator LoadScenesAsync()
        {
            foreach (int sceneIndex in this.scenesToLoadAdditive)
            {
                Scene scene = SceneManager.GetSceneByBuildIndex(sceneIndex);

                if (scene != null)
                {
                    if (!scene.isLoaded)
                        yield return SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);

                    //event handling
                    SceneLoaderAsyncMono.SceneLoadedGlobalAdditiveSuccess?.Invoke(scene);
                    this.SceneLoadedAdditiveSuccess?.Invoke(scene);
                    this.SceneLoadedUnityEvent?.Invoke(scene);
                }
            }
        }
    }
}

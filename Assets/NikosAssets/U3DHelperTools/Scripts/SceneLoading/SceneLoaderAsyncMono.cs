using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace NikosAssets.Helpers.SceneLoading
{
    [Serializable]
    public class SceneLoadedUnityEvent : UnityEvent<Scene>
    {
        
    }
    
    /// <summary>
    /// A helper class to load <see cref="Scene"/>s async and emit events as well
    /// </summary>
    public class SceneLoaderAsyncMono : BaseNotesMono
    {
        public static event Action<Scene> SceneLoadedGlobalAdditiveSuccess;
        public event Action<Scene> SceneLoadedAdditiveSuccess;
        
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_EVENTS)]
        public SceneLoadedUnityEvent SceneLoadedUnityEvent = default;

        /// <summary>
        /// The <see cref="Scene"/>s to load additive
        /// </summary>
        [SerializeField]
        [Scene]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("The Scenes to load additive")]
        protected int[] scenesToLoadAdditive = default;

        /// <summary>
        /// Should we load the <see cref="Scene"/>s at <see cref="Start"/>?
        /// </summary>
        [SerializeField]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("Should we load the Scenes at Start()?")]
        protected bool loadScenesAtStart = true;
        
        protected virtual void Start()
        {
            if (loadScenesAtStart)
                this.StartCoroutine(this.LoadScenesAsync());
        }

        /// <summary>
        /// Loads the <see cref="scenesToLoadAdditive"/> Scenes and emits the
        /// <see cref="SceneLoadedUnityEvent"/>, <see cref="SceneLoadedAdditiveSuccess"/> and <see cref="SceneLoadedGlobalAdditiveSuccess"/> events
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator LoadScenesAsync()
        {
            foreach (int sceneIndex in this.scenesToLoadAdditive)
            {
                Scene scene = SceneManager.GetSceneByBuildIndex(sceneIndex);
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

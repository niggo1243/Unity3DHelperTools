using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using NikosAssets.Helpers.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace NikosAssets.Helpers.SceneLoading
{
    /// <summary>
    /// A more sophisticated version of <see cref="SceneLoaderAsyncMono"/>
    /// </summary>
    public class AdvancedSceneLoaderMono : BaseNotesMono
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_EVENTS)]
        public UnityEvent<SceneToLoadContainer> OnSceneLoadedUnityEvent;
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_EVENTS)]
        public UnityEvent<SceneToLoadContainer> OnScenePreparedUnityEvent;

        public Action<SceneToLoadContainer> OnSceneLoaded;
        public Action<SceneToLoadContainer> OnScenePrepared;
        
        /// <summary>
        /// Data class that describes how a <see cref="Scene"/> should be loaded
        /// </summary>
        [Serializable]
        public class SceneToLoadContainer
        {
            /// <summary>
            /// The <see cref="Scene"/> to load via its name
            /// </summary>
            [Scene]
            [SerializeField]
            [Tooltip("The Scene to load via its name")]
            protected string _sceneName;
            /// <summary>
            /// The <see cref="Scene"/> to load via its name
            /// </summary>
            public string SceneName => _sceneName;

            /// <summary>
            /// If the <see cref="Scene"/> is already loaded, should we reload or skip?
            /// </summary>
            [SerializeField]
            [Tooltip("If the Scene is already loaded, should we reload or skip?")]
            protected bool _reloadIfActive;
            /// <summary>
            /// If the <see cref="Scene"/> is already loaded, should we reload or skip?
            /// </summary>
            public bool ReloadIfActive => _reloadIfActive;
            
            [SerializeField]
            protected LoadSceneMode _loadSceneMode = LoadSceneMode.Additive;
            public LoadSceneMode LoadSceneMode => _loadSceneMode;
            
            [SerializeField]
            protected bool _loadAsync;
            public bool LoadAsync => _loadAsync;
            
            /// <summary>
            /// Should the <see cref="Scene"/> only be preloaded?
            /// </summary>
            [AllowNesting]
            [ShowIf(nameof(_loadAsync))]
            [SerializeField]
            [Tooltip("Should the Scene only be preloaded?")]
            protected bool _activateSceneWhenLoaded = true;

            /// <summary>
            /// Should the <see cref="Scene"/> only be preloaded?
            /// </summary>
            public bool ActivateSceneWhenLoaded
            {
                get => _activateSceneWhenLoaded;
                set
                {
                    _activateSceneWhenLoaded = value;
                    if (LoadSceneAsyncOperation != null)
                        LoadSceneAsyncOperation.allowSceneActivation = _activateSceneWhenLoaded;
                }
            }

            public Coroutine LoadSceneAsyncCor { get; set; }
            public AsyncOperation LoadSceneAsyncOperation { get; set; }
            
            public DateTime StartedLoadingAt { get; set; } = DateTime.MinValue;
            public DateTime PreparedLoadingAt { get; set; } = DateTime.MinValue;
            public DateTime FinishedLoadingAt { get; set; } = DateTime.MinValue;
        }

        /// <summary>
        /// Setup which and how the <see cref="Scene"/>s should be loaded
        /// </summary>
        [InfoBox("Note that if a 'loadSceneMode' is set to 'Single' and that Scene is fully loaded, \nno other Scene in this list will be loaded afterwards!", EInfoBoxType.Warning)]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        [Tooltip("Setup which and how the Scenes should be loaded")]
        public List<SceneToLoadContainer> sceneToLoadContainers = new List<SceneToLoadContainer>();

        protected List<SceneToLoadContainer> _asyncSceneLoaders = new List<SceneToLoadContainer>();
        
        /// <summary>
        /// What remaining <see cref="Scene"/>s are currently being loaded async?
        /// </summary>
        public List<SceneToLoadContainer> GetCurrentAsyncLoadingContainers => new List<SceneToLoadContainer>(_asyncSceneLoaders);

        protected virtual void OnDisable()
        {
            for (int i = 0; i < _asyncSceneLoaders.Count; i++)
            {
                SceneToLoadContainer container = _asyncSceneLoaders[i];
                if (container != null)
                    this.StopRunningCoroutine(container.LoadSceneAsyncCor);

                _asyncSceneLoaders.RemoveAt(i--);
            }
        }

        /// <summary>
        /// If the async loaded <see cref="Scene"/> of the <see cref="SceneToLoadContainer"/> is only preloaded (not activated), finish the loading process here 
        /// </summary>
        /// <param name="sceneToActivate">
        /// The <see cref="Scene"/> load to finish
        /// </param>
        /// <exception cref="ApplicationException">
        /// The <see cref="Scene"/> name was not found, either because it was never loaded or already fully finished loading
        /// </exception>
        public virtual void ActivateAsyncSceneOnStandBy(string sceneToActivate)
        {
            SceneToLoadContainer sceneToLoadContainer = GetCurrentAsyncLoadingContainers
                .Find(s => s.SceneName.Equals(sceneToActivate));

            if (sceneToLoadContainer != null)
                sceneToLoadContainer.ActivateSceneWhenLoaded = true;
            else
            {
                throw new ApplicationException(
                    $"Scene '{sceneToActivate} could not be activated because it was not found in the " +
                    $"{nameof(AdvancedSceneLoaderMono)} async scenes to load list!");
            }
        }

        /// <summary>
        /// Loads all <see cref="SceneToLoadContainer"/> and their respective <see cref="Scene"/>s found in <see cref="sceneToLoadContainers"/>
        /// </summary>
        public virtual void LoadAll()
        {
            foreach (SceneToLoadContainer sceneToLoadContainer in sceneToLoadContainers)
            {
                if (SceneManager.GetSceneByName(sceneToLoadContainer.SceneName).isLoaded && !sceneToLoadContainer.ReloadIfActive)
                {
                    Debug.LogWarning($"Scene '{sceneToLoadContainer.SceneName}' already loaded!");
                    continue;
                }
                
                if (sceneToLoadContainer.LoadAsync)
                    LoadAsync(sceneToLoadContainer);
                else
                    LoadSync(sceneToLoadContainer);
            }
        }

        public virtual void LoadSync(SceneToLoadContainer sceneToLoadContainer)
        {
            //setting the start load date
            sceneToLoadContainer.StartedLoadingAt = DateTime.Now;
            //load the scene (additive)
            SceneManager.LoadScene(sceneToLoadContainer.SceneName, sceneToLoadContainer.LoadSceneMode);
            //notify that the scene is done loading
            SceneToLoadContainerIsLoaded(sceneToLoadContainer);
        }
        
        public virtual void LoadAsync(SceneToLoadContainer sceneToLoadContainer)
        {
            sceneToLoadContainer.LoadSceneAsyncCor = StartCoroutine(IELoadAsync(sceneToLoadContainer));
            _asyncSceneLoaders.Add(sceneToLoadContainer);
        }

        protected virtual void SceneToLoadContainerIsPrepared(SceneToLoadContainer sceneToLoadContainer)
        {
            sceneToLoadContainer.PreparedLoadingAt = DateTime.Now;
            OnScenePrepared?.Invoke(sceneToLoadContainer);
            OnScenePreparedUnityEvent.Invoke(sceneToLoadContainer);
        }
        
        protected virtual void SceneToLoadContainerIsLoaded(SceneToLoadContainer sceneToLoadContainer)
        {
            sceneToLoadContainer.FinishedLoadingAt = DateTime.Now;
            OnSceneLoaded?.Invoke(sceneToLoadContainer);
            OnSceneLoadedUnityEvent.Invoke(sceneToLoadContainer);
        }
        
        protected virtual IEnumerator IELoadAsync(SceneToLoadContainer sceneToLoadContainer)
        {
            sceneToLoadContainer.StartedLoadingAt = DateTime.Now;
            
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneToLoadContainer.SceneName, 
                sceneToLoadContainer.LoadSceneMode);
            asyncOperation.allowSceneActivation = sceneToLoadContainer.ActivateSceneWhenLoaded;

            sceneToLoadContainer.LoadSceneAsyncOperation = asyncOperation;

            if (!asyncOperation.allowSceneActivation)
            {
                while (asyncOperation.progress < .9f)
                {
                    yield return new WaitForEndOfFrame();
                }
                
                SceneToLoadContainerIsPrepared(sceneToLoadContainer);
            }
            
            yield return asyncOperation;

            //remove the scene from async loaders
            _asyncSceneLoaders.Remove(sceneToLoadContainer);
            SceneToLoadContainerIsLoaded(sceneToLoadContainer);
        }
    }
}

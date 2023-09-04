using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using NikosAssets.Helpers.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace NikosAssets.Helpers.SceneLoading
{
    public class AdvancedSceneLoaderMono : BaseNotesMono
    {
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_EVENTS)]
        public UnityEvent<SceneToLoadContainer> OnSceneLoadedUnityEvent;
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_EVENTS)]
        public UnityEvent<SceneToLoadContainer> OnScenePreparedUnityEvent;

        public Action<SceneToLoadContainer> OnSceneLoaded;
        public Action<SceneToLoadContainer> OnScenePrepared;
        
        [Serializable]
        public class SceneToLoadContainer
        {
            [FormerlySerializedAs("sceneName")]
            [Scene]
            [SerializeField]
            protected string _sceneName;
            public string SceneName => _sceneName;

            [SerializeField]
            protected bool _reloadIfActive;
            public bool ReloadIfActive => _reloadIfActive;
            
            [FormerlySerializedAs("loadSceneMode")]
            [SerializeField]
            protected LoadSceneMode _loadSceneMode = LoadSceneMode.Additive;
            public LoadSceneMode LoadSceneMode => _loadSceneMode;
            
            [FormerlySerializedAs("loadAsync")]
            [SerializeField]
            protected bool _loadAsync;
            public bool LoadAsync => _loadAsync;
            
            [FormerlySerializedAs("activateSceneWhenLoaded")]
            [AllowNesting]
            [ShowIf(nameof(_loadAsync))]
            [SerializeField]
            protected bool _activateSceneWhenLoaded = true;

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

        [InfoBox("Note that if a 'loadSceneMode' is set to 'Single' and that Scene is fully loaded, \nno other Scene in this list will be loaded afterwards!", EInfoBoxType.Warning)]
        [BoxGroup(HelperConstants.ATTRIBUTE_FIELD_BOXGROUP_SETTINGS)]
        public List<SceneToLoadContainer> sceneToLoadContainers = new List<SceneToLoadContainer>();

        protected List<SceneToLoadContainer> _asyncSceneLoaders = new List<SceneToLoadContainer>();
        
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

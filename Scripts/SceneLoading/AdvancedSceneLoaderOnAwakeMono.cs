namespace NikosAssets.Helpers.SceneLoading
{
    /// <summary>
    /// Load all <see cref="UnityEngine.SceneManagement.Scene"/>s on <see cref="Awake"/>
    /// </summary>
    public class AdvancedSceneLoaderOnAwakeMono : AdvancedSceneLoaderMono
    {
        protected virtual void Awake()
        {
            LoadAll();
        }
    }
}

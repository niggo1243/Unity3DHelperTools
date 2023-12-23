namespace NikosAssets.Helpers.SceneLoading
{
    /// <summary>
    /// Load all <see cref="UnityEngine.SceneManagement.Scene"/>s on <see cref="Start"/>
    /// </summary>
    public class AdvancedSceneLoaderAtStartMono : AdvancedSceneLoaderMono
    {
        protected virtual void Start()
        {
            LoadAll();
        }
    }
}

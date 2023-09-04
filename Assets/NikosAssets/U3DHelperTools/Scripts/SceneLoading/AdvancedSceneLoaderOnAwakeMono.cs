namespace NikosAssets.Helpers.SceneLoading
{
    public class AdvancedSceneLoaderOnAwakeMono : AdvancedSceneLoaderMono
    {
        protected virtual void Awake()
        {
            LoadAll();
        }
    }
}

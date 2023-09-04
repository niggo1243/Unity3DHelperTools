namespace NikosAssets.Helpers.SceneLoading
{
    public class AdvancedSceneLoaderAtStartMono : AdvancedSceneLoaderMono
    {
        protected virtual void Start()
        {
            LoadAll();
        }
    }
}

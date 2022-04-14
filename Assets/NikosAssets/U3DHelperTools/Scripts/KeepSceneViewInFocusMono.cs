namespace NikosAssets.Helpers
{
    /// <summary>
    /// When entering Unity's playmode, the Game window will not be focused automatically anymore!
    /// </summary>
    public class KeepSceneViewInFocusMono : BaseNotesMono
    {
#if UNITY_EDITOR
        protected virtual void Start()
        {
            UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
        }
#endif
    }
}

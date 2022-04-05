using UnityEngine;

namespace NikosAssets.Helpers
{
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

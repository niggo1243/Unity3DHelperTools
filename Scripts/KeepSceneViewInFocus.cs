using UnityEngine;

namespace NikosAssets.Helpers
{
    public class KeepSceneViewInFocus : MonoBehaviour
    {
#if UNITY_EDITOR
        private void Start()
        {
            UnityEditor.SceneView.FocusWindowIfItsOpen(typeof(UnityEditor.SceneView));
        }
#endif
    }
}

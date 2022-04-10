namespace NikosAssets.Helpers
{
    public static class ApplicationHelper
    {
        private static bool _applicationPlayingAccurate;
        public static bool ApplicationIsPlayingAccurate =>
#if UNITY_EDITOR
            _applicationPlayingAccurate;
#else
            UnityEngine.Application.isPlaying;
#endif

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        public static void InitEditor()
        {
            try
            {
                UnityEditor.EditorApplication.playModeStateChanged -= EditorApplicationOnplayModeStateChanged;
            }
            catch
            {
                //ignored
            }

            UnityEditor.EditorApplication.playModeStateChanged += EditorApplicationOnplayModeStateChanged;
        }

        private static void EditorApplicationOnplayModeStateChanged(UnityEditor.PlayModeStateChange state)
        {
            _applicationPlayingAccurate = state != UnityEditor.PlayModeStateChange.EnteredEditMode &&
                                          state != UnityEditor.PlayModeStateChange.ExitingPlayMode;
        }
#endif
    }
}

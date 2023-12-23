using System.Linq;

namespace NikosAssets.Helpers.Experimental
{
    /// <summary>
    /// Experimental helper class to find out if your windows build is in dev or admin mode when you enter the "dev" arg (no "-" prefix!) when launching the .exe.
    /// This is independent of a release or development build
    /// </summary>
    public static class DevHelper
    {
        public const string DEV_MODE_ARG = "dev";
        public static bool IsDevMode =>
#if UNITY_EDITOR
            true;
#elif UNITY_STANDALONE_WIN
            System.Environment.GetCommandLineArgs().Contains(DEV_MODE_ARG);
#else
            false;
#endif
    }
}

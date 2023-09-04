using System.Linq;

namespace NikosAssets.Helpers.Experimental
{
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

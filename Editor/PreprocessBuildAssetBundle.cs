using GameFramework;

namespace HybridCLRLink
{
    public class PreprocessBuildAssetBundle : IPreprocessBuildAssetBundle
    {
        public void OnPreprocessBuild()
        {
            BuildAssetCommand.BuildHotfixDlls();
        }
    }
}
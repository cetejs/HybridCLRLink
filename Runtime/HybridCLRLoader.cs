using GameFramework;
using HybridCLR;
using UnityEngine;

namespace HybridCLRLink
{
    public static class HybridCLRLoader
    {
        public static string AOTDllBundleName = "AOTDlls";
        public static string HotfixDllBundleName = "HotfixDlls";

        public static void LoadAOTDll()
        {
            AssetBundle bundle = AssetManager.Instance.LoadBundle(AOTDllBundleName);
            TextAsset[] dlls = bundle.LoadAllAssets<TextAsset>();
            foreach (TextAsset dll in dlls)
            {
                LoadImageErrorCode error = RuntimeApi.LoadMetadataForAOTAssembly(dll.bytes, HomologousImageMode.SuperSet);
                GameLogger.Log($"LoadMetadataForAOTAssembly:{dll.name}. mode:{HomologousImageMode.SuperSet} ret:{error}");
            }

            bundle.Unload(true);
        }
    }
}
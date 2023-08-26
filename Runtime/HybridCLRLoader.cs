using System.Reflection;
using GameFramework;
using HybridCLR;
using UnityEngine;

namespace HybridCLRLink
{
    public static class HybridCLRLoader
    {
        public static string AOTDllBundleName = "AOTDlls";
        public static string HotfixDllBundleName = "HotfixDlls";

        public static void LoadAllDll()
        {
            LoadAOTDll();
            LoadHotfixDll();
        }

        public static void LoadAOTDll()
        {
            AssetBundle bundle = AssetManager.Instance.LoadBundle(AOTDllBundleName);
            if (bundle != null)
            {
                TextAsset[] dlls = bundle.LoadAllAssets<TextAsset>();
                foreach (TextAsset dll in dlls)
                {
                    LoadImageErrorCode error = RuntimeApi.LoadMetadataForAOTAssembly(dll.bytes, HomologousImageMode.SuperSet);
                    GameLogger.Log($"[LoadAOTDll]: {dll.name} mode: {HomologousImageMode.SuperSet} ret: {error}");
                }
            }

            AssetManager.Instance.UnloadBundle(AOTDllBundleName, true);
        }

        public static void LoadHotfixDll()
        {
#if !UNITY_EDITOR
            AssetBundle bundle = AssetManager.Instance.LoadBundle(HotfixDllBundleName);
            if (bundle != null)
            {
                TextAsset[] dlls = bundle.LoadAllAssets<TextAsset>();
                foreach (TextAsset dll in dlls)
                {
                    Assembly.Load(dll.bytes);
                    GameLogger.Log($"[LoadHotfixDll]: {dll.name}");
                }
            }

            AssetManager.Instance.UnloadBundle(AOTDllBundleName, true);
#endif
        }
    }
}
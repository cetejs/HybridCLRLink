using System.IO;
using GameFramework;
using HybridCLR.Editor;
using HybridCLR.Editor.Commands;
using UnityEditor;
using UnityEngine;

namespace HybridCLRLink
{
    internal static class BuildAssetCommand
    {
        [MenuItem("HybridCLR/Build/BuildAOTDlls")]
        public static void BuildAOTDlls()
        {
            PrebuildCommand.GenerateAll();
            CopyAOTAssembliesToBundle();
        }

        [MenuItem("HybridCLR/Build/BuildHotfixDlls")]
        public static void BuildHotfixDlls()
        {
            CompileDllCommand.CompileDllActiveBuildTarget();
            CopyHotfixAssembliesToBundle();
        }

        private static void CopyAOTAssembliesToBundle()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            string aotAssembliesSrcDir = PathUtils.Combine(PathUtils.ProjectPath, SettingsUtil.GetAssembliesPostIl2CppStripDir(target));
            string aotAssembliesDstDir = PathUtils.Combine(AssetSetting.Instance.BundleAssetPath, HybridCLRLoader.AOTDllBundleName);
            foreach (string dll in SettingsUtil.AOTAssemblyNames)
            {
                string srcDllPath = $"{aotAssembliesSrcDir}/{dll}.dll";
                if (!File.Exists(srcDllPath))
                {
                    Debug.LogError($"{srcDllPath} is not exist");
                    continue;
                }

                DirectoryUtils.CreateDirectory(aotAssembliesDstDir);
                string dllBytesPath = $"{aotAssembliesDstDir}/{dll}.txt";
                File.Copy(srcDllPath, dllBytesPath, true);
                Debug.Log($"[CopyAOTAssembliesToBundle] copy AOT dll {srcDllPath} -> {dllBytesPath}");
            }

            AssetDatabase.Refresh();
        }

        private static void CopyHotfixAssembliesToBundle()
        {
            BuildTarget target = EditorUserBuildSettings.activeBuildTarget;
            string hotfixDllSrcDir = PathUtils.Combine(PathUtils.ProjectPath, SettingsUtil.GetHotUpdateDllsOutputDirByTarget(target));
            string hotfixAssembliesDstDir = PathUtils.Combine(AssetSetting.Instance.BundleAssetPath, HybridCLRLoader.HotfixDllBundleName);
            foreach (string dll in SettingsUtil.HotUpdateAssemblyNamesIncludePreserved)
            {
                string dllPath = $"{hotfixDllSrcDir}/{dll}.dll";
                if (!File.Exists(dllPath))
                {
                    continue;
                }

                DirectoryUtils.CreateDirectory(hotfixAssembliesDstDir);
                string dllBytesPath = $"{hotfixAssembliesDstDir}/{dll}.txt";
                File.Copy(dllPath, dllBytesPath, true);
                Debug.Log($"[CopyHotfixAssembliesToBundle] copy hotfix dll {dllPath} -> {dllBytesPath}");
            }

            AssetDatabase.Refresh();
        }
    }
}
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace FrameSynthesis.XR
{
    public static class Builder
    {
        const string ProductName = "VRSashimiTanpopo";
        const string OutputPath = "dist";

        [MenuItem("Build/Build (SteamVR)")]
        public static void BuildForSteamVR()
        {
            PlatformSwitcher.SwitchToSteamVR();
            Build(Path.Combine(OutputPath, "SteamVR", ProductName + ".exe"), BuildTarget.StandaloneWindows64);
        }

        [MenuItem("Build/Build (Quest)")]
        public static void BuildForQuest()
        {
            PlatformSwitcher.SwitchToQuest();
            Build(Path.Combine(OutputPath, "Quest", ProductName + ".apk"), BuildTarget.Android, BuildOptions.CompressWithLz4);
        }

        [MenuItem("Build/Build (Rift)")]
        public static void BuildForRift()
        {
            PlatformSwitcher.SwitchToRift();
            Build(Path.Combine(OutputPath, "Rift", ProductName + ".exe"), BuildTarget.StandaloneWindows64);
        }

        [MenuItem("Build/Build (Quest release version)")]
        public static void BuildForQuestRelease()
        {
            const int BundleVersionCodeOffset = 2;
            
            PlatformSwitcher.SwitchToQuest();
            
            // Oculusのダッシュボードにアップロードするための各種設定
            // リリースチャンネルのAPKファイルの更新に必要なのでBundle Version Codeを更新。
            if (int.TryParse(Environment.GetEnvironmentVariable("BUILD_NUMBER"), out var buildNumber))
            {
                PlayerSettings.Android.bundleVersionCode = buildNumber + BundleVersionCodeOffset;
            }
            PlayerSettings.Android.useCustomKeystore = true;
            // Keystoreのパスワードを設定（セッション間で保存されないため）。
            PlayerSettings.keystorePass = Secret.Android.KeystorePass;
            PlayerSettings.keyaliasPass = Secret.Android.KeyaliasPass;
            
            var originalApplicationIdentifier = PlayerSettings.applicationIdentifier;
            PlayerSettings.applicationIdentifier = "com.framesynthesis.VRSashimiTanpopo";
            
            Build(Path.Combine(OutputPath, "Quest", ProductName + ".apk"), BuildTarget.Android, BuildOptions.CompressWithLz4);

            PlayerSettings.Android.useCustomKeystore = false;
            PlayerSettings.applicationIdentifier = originalApplicationIdentifier;
        }

        [MenuItem("Build/Open Build Folder")]
        public static void OpenBuildFolder()
        {
            Process.Start(Path.Combine(Application.dataPath, "..", OutputPath));
        }

        static void Build(string path, BuildTarget buildTarget, BuildOptions options = BuildOptions.None)
        {
            BuildPipeline.BuildPlayer(ScenePaths, path, buildTarget, options);
        }

        static string[] ScenePaths => EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();
    }
}

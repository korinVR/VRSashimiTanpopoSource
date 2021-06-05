using UnityEditor;
using UnityEngine;

namespace FrameSynthesis.XR
{
    public static class PlatformSwitcher
    {
        [MenuItem("Platform/Switch to SteamVR")]
        public static void SwitchToSteamVR()
        {
            Debug.Log("Switching to SteamVR...");

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "PLATFORM_STEAMVR");
            AssetDatabase.Refresh();

            XRPluginManagementSettings.EnablePlugin(BuildTargetGroup.Standalone, XRPluginManagementSettings.Plugin.OpenVR);
            XRPluginManagementSettings.DisablePlugin(BuildTargetGroup.Standalone, XRPluginManagementSettings.Plugin.Oculus);

            XRPluginManagementSettings.DisablePlugin(BuildTargetGroup.Android, XRPluginManagementSettings.Plugin.Oculus);

            Debug.Log("Complete.");
        }

        [MenuItem("Platform/Switch to Quest")]
        public static void SwitchToQuest()
        {
            Debug.Log("Switching to Quest...");

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
            EditorUserBuildSettings.androidBuildSubtarget = MobileTextureSubtarget.ASTC;

            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "PLATFORM_OCULUS;PLATFORM_QUEST;OVR_PLATFORM_ASYNC_MESSAGES");
            AssetDatabase.Refresh();

            XRPluginManagementSettings.EnablePlugin(BuildTargetGroup.Standalone, XRPluginManagementSettings.Plugin.Oculus);
            XRPluginManagementSettings.DisablePlugin(BuildTargetGroup.Standalone, XRPluginManagementSettings.Plugin.OpenVR);

            XRPluginManagementSettings.EnablePlugin(BuildTargetGroup.Android, XRPluginManagementSettings.Plugin.Oculus);

            Debug.Log("Complete.");
        }

        [MenuItem("Platform/Switch to Rift")]
        public static void SwitchToRift()
        {
            Debug.Log("Switching to Rift...");

            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);

            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "PLATFORM_OCULUS;PLATFORM_RIFT;OVR_PLATFORM_ASYNC_MESSAGES");
            AssetDatabase.Refresh();

            XRPluginManagementSettings.EnablePlugin(BuildTargetGroup.Standalone, XRPluginManagementSettings.Plugin.Oculus);
            XRPluginManagementSettings.DisablePlugin(BuildTargetGroup.Standalone, XRPluginManagementSettings.Plugin.OpenVR);

            Debug.Log("Complete.");
        }
    }
}

using System;
using UnityEditor;
using UnityEditor.XR.Management;
using UnityEditor.XR.Management.Metadata;
using UnityEngine;

namespace FrameSynthesis.XR
{
    // ref. https://docs.unity3d.com/Packages/com.unity.xr.management@3.2/manual/EndUser.html
    public static class XRPluginManagementSettings
    {
        public enum Plugin
        {
            Oculus,
            OpenVR
        }

        static string GetLoaderName(Plugin plugin) => plugin switch
        {
            Plugin.Oculus => "Unity.XR.Oculus.OculusLoader",
            Plugin.OpenVR => "Unity.XR.OpenVR.OpenVRLoader",
            _ => throw new NotImplementedException()
        };

        public static void EnablePlugin(BuildTargetGroup buildTargetGroup, Plugin plugin)
        {
            var buildTargetSettings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(buildTargetGroup);
            var pluginsSettings = buildTargetSettings.AssignedSettings;
            var success = XRPackageMetadataStore.AssignLoader(pluginsSettings, GetLoaderName(plugin), buildTargetGroup);
            if (success)
            {
                Debug.Log($"XR Plug-in Management: Enabled {plugin} plugin on {buildTargetGroup}");
            }
        }

        public static void DisablePlugin(BuildTargetGroup buildTargetGroup, Plugin plugin)
        {
            var buildTargetSettings = XRGeneralSettingsPerBuildTarget.XRGeneralSettingsForBuildTarget(buildTargetGroup);
            var pluginsSettings = buildTargetSettings.AssignedSettings;
            var success = XRPackageMetadataStore.RemoveLoader(pluginsSettings, GetLoaderName(plugin), buildTargetGroup);
            if (success)
            {
                Debug.Log($"XR Plug-in Management: Disabled {plugin} plugin on {buildTargetGroup}");
            }
        }
    }
}
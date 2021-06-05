using System.IO;
using FrameSynthesis.XR;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.Build.Player;

namespace VRSashimiTanpopo
{
    public class ScriptCompilationPasses
    {
        const string OutputFolder = "Temp/ScriptCompileTest";

        [Test]
        public void ScriptCompilationPassesForQuest()
        {
            PlatformSwitcher.SwitchToQuest();
            Compile(BuildTarget.Android, BuildTargetGroup.Android);
        }

        [Test]
        public void ScriptCompilationPassesForSteamVR()
        {
            PlatformSwitcher.SwitchToSteamVR();
            Compile(BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone);
        }

        [Test]
        public void ScriptCompilationPassesForRift()
        {
            PlatformSwitcher.SwitchToRift();
            Compile(BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone);
        }

        static void Compile(BuildTarget target, BuildTargetGroup group)
        {
            var settings = new ScriptCompilationSettings
            {
                target = target,
                group = group,
            };

            var result = PlayerBuildInterface.CompilePlayerScripts(settings, OutputFolder);
            var assemblies = result.assemblies;

            var isSuccess = assemblies != null && assemblies.Count != 0 && result.typeDB != null;

            if (Directory.Exists(OutputFolder))
            {
                Directory.Delete(OutputFolder, true);
            }

            Assert.IsTrue(isSuccess);
        }
    }
}

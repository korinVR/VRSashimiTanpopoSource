using System;
using System.IO;
using Cysharp.Threading.Tasks;
using FrameSynthesis.XR;
using UnityEngine;
using VRM;
using Ookii.Dialogs;
using RootMotion.FinalIK;
using UniGLTF;
using VContainer;
using VRSashimiTanpopo.Debug;
using VRSashimiTanpopo.Localization;

namespace VRSashimiTanpopo.VRM
{
    public class VRMLoader : MonoBehaviour
    {
        [SerializeField] string vrmFilename;

        [Inject] ICameraRig cameraRig;
        [Inject] IKTargets ikTargets;
        [Inject] DebugSettings debugSettings;

        void Start()
        {
            if (debugSettings.LoadTestVRM)
            {
                LoadVRMAsync(Path.Combine(Application.dataPath, "..", "VRM", vrmFilename)).Forget();
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.V))
            {
                OpenDialog();
            }
        }

        void OpenDialog()
        {
#if PLATFORM_STEAMVR
            var dialog = new VistaOpenFileDialog();
            dialog.Filter = LocalizationSettings.GetCurrentLanguage() switch
            {
                Language.English => "VRM File",
                Language.Japanese => "VRM ファイル",
                _ => throw new ArgumentOutOfRangeException()
            } + "|*.vrm";
            dialog.ShowDialog();

            if (dialog.FileName != "")
            {
                LoadVRMAsync(dialog.FileName).Forget();
            }
#endif
        }

        async UniTask LoadVRMAsync(string filename)
        {
            var bytes = File.ReadAllBytes(filename);

            var parser = new GltfParser();
            parser.ParseGlb(bytes);
            
            using var context = new VRMImporterContext(parser);
            var meta = context.ReadMeta();

            // UnityEngine.Debug.LogFormat("meta: title:{0}", meta.Title);

            await context.LoadAsync();
            
            context.EnableUpdateWhenOffscreen();
            context.ShowMeshes();
            context.DisposeOnGameObjectDestroyed();
            
            var root = context.Root;
            root.transform.SetParent(transform, false);
            context.ShowMeshes();
            
            SetupVRIK(root);
        }

        void SetupVRIK(GameObject root)
        {
            var vrik = root.AddComponent<VRIK>();

            var vrmFirstPerson = root.GetComponent<VRMFirstPerson>();
            vrmFirstPerson.Setup();
            ikTargets.SetFirstPersonOffset(vrmFirstPerson.FirstPersonOffset);
            
            vrik.solver.spine.headTarget = ikTargets.HeadTarget;
            vrik.solver.leftArm.target = cameraRig.GetTransform(TrackingPoint.LeftHand);
            vrik.solver.rightArm.target = cameraRig.GetTransform(TrackingPoint.RightHand);
            
            vrik.solver.leftLeg.target = ikTargets.LeftFootTarget;
            vrik.solver.leftLeg.swivelOffset = -40f;
            vrik.solver.leftLeg.positionWeight = 1f;
            vrik.solver.leftLeg.rotationWeight = 1f;
            
            vrik.solver.rightLeg.target = ikTargets.RightFootTarget;
            vrik.solver.rightLeg.swivelOffset = 40f;
            vrik.solver.rightLeg.positionWeight = 1f;
            vrik.solver.rightLeg.rotationWeight = 1f;

            // TODO: プレイ開始位置からある程度離れたら歩けるようにする。
            vrik.solver.locomotion.weight = 0f;

            Camera.main.cullingMask &= ~(1 << LayerName.ThirdPersonOnly);
        }
    }
}

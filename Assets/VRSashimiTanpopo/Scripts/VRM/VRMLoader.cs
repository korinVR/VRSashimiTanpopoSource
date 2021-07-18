using System;
using System.IO;
using Cysharp.Threading.Tasks;
using FrameSynthesis.XR;
using RootMotion.FinalIK;
using UniGLTF;
using UnityEngine;
using VContainer;
using VRM;
using VRSashimiTanpopo.Debug;

namespace VRSashimiTanpopo.VRM
{
    public class VRMLoader : MonoBehaviour
    {
        [Inject] ICameraRig cameraRig;
        [Inject] IKTargets ikTargets;
        [Inject] DebugSettings debugSettings;

        GltfParser gltfParser;
        GameObject vrmGameObject;

        public async UniTask<bool> OpenDialog()
        {
#if PLATFORM_STEAMVR
            var filename = await OpenFileDialog.Open("VRMファイル (*.vrm)|*.vrm", "VRMファイルの読み込み");
            if (filename != "")
            {
                try
                {
                    var bytes = File.ReadAllBytes(filename);

                    gltfParser = new GltfParser();
                    gltfParser.ParseGlb(bytes);
                }
                catch (Exception e)
                {
                    return false;
                }
                return true;
            }
#endif
            return false;
        }

        public void DestroyVRM()
        {
            if (vrmGameObject == null) return;

            Destroy(vrmGameObject);
            vrmGameObject = null;
        }

        public void LoadVRM()
        {
            LoadVRMAsync().Forget();
        }

        async UniTask LoadVRMAsync()
        {
            DestroyVRM();

            using (var context = new VRMImporterContext(gltfParser))
            {
                var meta = context.ReadMeta();
                ShowVRMMetaData(meta);
                await context.LoadAsync();

                context.EnableUpdateWhenOffscreen();
                context.ShowMeshes();
                context.DisposeOnGameObjectDestroyed();

                vrmGameObject = context.Root;
                vrmGameObject.transform.SetParent(transform, false);
                context.ShowMeshes();
            }

            var vrmFirstPerson = vrmGameObject.GetComponent<VRMFirstPerson>();
            vrmFirstPerson.Setup();
            ikTargets.SetFirstPersonOffset(vrmFirstPerson.FirstPersonOffset);

            var vrmEyeHeight = (vrmFirstPerson.FirstPersonBone.position + vrmFirstPerson.FirstPersonOffset).y;
            var playerEyeHeight = cameraRig.GetTransform(TrackingPoint.Head).position.y;

            var scale = playerEyeHeight / vrmEyeHeight;
            vrmGameObject.transform.localScale = new Vector3(scale, scale, scale);
            
            var vrik = vrmGameObject.AddComponent<VRIK>();

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

        void ShowVRMMetaData(VRMMetaObject metaObject)
        {
            UnityEngine.Debug.Log("Title: " + metaObject.Title);
            UnityEngine.Debug.Log("Version: " + metaObject.Version);
            UnityEngine.Debug.Log("Author: " + metaObject.Author);
            UnityEngine.Debug.Log("Contact Information: " + metaObject.ContactInformation);
        }
    }
}

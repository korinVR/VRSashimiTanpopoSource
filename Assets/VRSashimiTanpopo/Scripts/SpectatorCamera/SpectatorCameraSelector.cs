using System;
using Cinemachine;
using UnityEngine;

namespace VRSashimiTanpopo.SpectatorCamera
{
    public class SpectatorCameraSelector : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera[] virtualCameras;
        
        public void Select(Viewpoint viewpoint)
        {
            Array.ForEach(virtualCameras, virtualCamera => virtualCamera.Priority = 10);
            virtualCameras[(int) viewpoint].Priority = 100;
        }
    }
}
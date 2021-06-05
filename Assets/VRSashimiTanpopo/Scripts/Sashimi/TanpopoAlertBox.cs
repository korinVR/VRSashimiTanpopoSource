using System;
using FrameSynthesis;
using UnityEngine;

namespace VRSashimiTanpopo.Sashimi
{
    public class TanpopoAlertBox : MonoBehaviour
    {
        [SerializeField] Renderer alertBoxRenderer;

        MaterialPropertyBlock materialPropertyBlock;
        static readonly int Color = Shader.PropertyToID("_Color");
        Color defaultColor;

        bool active;

        void Start()
        {
            materialPropertyBlock = new MaterialPropertyBlock();
            defaultColor = alertBoxRenderer.sharedMaterial.GetColor(Color);
            alertBoxRenderer.GetPropertyBlock(materialPropertyBlock);
            
            StopAlert();
        }

        public void StartAlert()
        {
            if (active) return;
            
            active = true;
            alertBoxRenderer.enabled = true;
                
        }

        public void StopAlert()
        {
            if (!active) return;
            
            active = false;
            alertBoxRenderer.enabled = false;
        }

        void Update()
        {
            if (!active) return;
            
            var alpha = MathHelper.LinearMap((float)Math.Sin(Time.timeAsDouble * (Math.PI * 2.0) * 3), -1, 1, 0, 0.5f);
            defaultColor.a = alpha;
            
            materialPropertyBlock.SetColor(Color, defaultColor);
            alertBoxRenderer.SetPropertyBlock(materialPropertyBlock);
        }
    }
}

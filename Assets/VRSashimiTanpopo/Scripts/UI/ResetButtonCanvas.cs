using System;
using UnityEngine;
using UnityEngine.UI;

namespace VRSashimiTanpopo.UI
{
    public class ResetButtonCanvas : MonoBehaviour
    {
        [SerializeField] Button resetButton;
        
        public void Construct(Action resetButtonPressed)
        {
            resetButton.onClick.AddListener(() => resetButtonPressed?.Invoke());
        }
    }
}
using System;
using TMPro;
using UnityEngine;

namespace FrameSynthesis.XR.UI
{
    public class Button : MonoBehaviour
    {
        public event Action Pressed;

        [SerializeField] string label;

        [Header("Internal")]
        [SerializeField] Transform buttonTop;
        [SerializeField] TMP_Text labelText;

        bool pressed;

        void Start()
        {
            UpdateVisual();
        }

        void OnTriggerEnter(Collider other)
        {
            pressed = true;
            UpdateVisual();

            Pressed?.Invoke();
        }

        void OnTriggerExit(Collider other)
        {
            pressed = false;
            UpdateVisual();
        }

        void UpdateVisual()
        {
            buttonTop.localPosition = pressed ? Vector3.forward * 0.01f : Vector3.forward * 0.03f;
            labelText.text = label;
        }

        void OnValidate()
        {
            UpdateVisual();
        }
    }
}

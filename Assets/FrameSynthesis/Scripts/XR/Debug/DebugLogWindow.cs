using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrameSynthesis.XR.Debug
{
    public class DebugLogWindow : MonoBehaviour
    {
        [SerializeField] Text text;
        
        readonly Queue<string> lines = new Queue<string>();

        public void Clear()
        {
            lines.Clear();
            UpdateText();
        }

        public void Println(string message)
        {
            lines.Enqueue(message);

            if (text.preferredHeight > GetComponent<RectTransform>().rect.height)
            {
                lines.Dequeue();
            }
            
            UpdateText();
        }

        void UpdateText()
        {
            text.text = string.Join("\n", lines);
        }
    }
}
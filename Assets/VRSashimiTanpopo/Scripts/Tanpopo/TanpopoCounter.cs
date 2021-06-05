using System;
using UnityEngine;

namespace VRSashimiTanpopo.Tanpopo
{
    public class TanpopoCounter : MonoBehaviour
    {
        public event Action IncreasedToOne;
        public event Action IncreasedToTwo;
        public event Action DecreasedToZero;

        public bool IsZero => count == 0;

        int count;

        void OnTriggerEnter(Collider collider)
        {
            count++;
            
            if (count == 1)
            {
                IncreasedToOne?.Invoke();
            }
            if (count == 2)
            {
                IncreasedToTwo?.Invoke();
            }
        }

        void OnTriggerExit(Collider collider)
        {
            count--;
            
            if (count == 0)
            {
                DecreasedToZero?.Invoke();
            }
        }
    }
}

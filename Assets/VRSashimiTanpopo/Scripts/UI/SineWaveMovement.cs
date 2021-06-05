using UnityEngine;

namespace VRSashimiTanpopo.UI
{
    public class SineWaveMovement : MonoBehaviour
    {
        [SerializeField] Vector3 direction;
        [SerializeField] float periodSec;
        [SerializeField] bool relativePosition;

        Vector3 initialPosition;

        void Start()
        {
            initialPosition = transform.position;
        }

        void Update()
        {
            var phase = Time.time / periodSec;
            var offsetPosition = direction * Mathf.Sin(phase * Mathf.PI * 2f);

            if (relativePosition)
            {
                transform.position = initialPosition + offsetPosition;
            }
            else
            {
                transform.localPosition = offsetPosition;
            }
        }
    }
}

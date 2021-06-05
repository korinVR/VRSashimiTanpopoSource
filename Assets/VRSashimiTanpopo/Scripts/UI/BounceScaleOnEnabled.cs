using DG.Tweening;
using UnityEngine;

namespace VRSashimiTanpopo.UI
{
    public class BounceScaleOnEnabled : MonoBehaviour
    {
        [SerializeField] float durationSec = 0.3f;
        [SerializeField] float initialScale = 2f;
        
        void OnEnable()
        {
            GetComponent<RectTransform>().DOScale(Vector3.one, durationSec)
                .From(initialScale * Vector3.one)
                .SetEase(Ease.OutBounce);
        }
    }
}

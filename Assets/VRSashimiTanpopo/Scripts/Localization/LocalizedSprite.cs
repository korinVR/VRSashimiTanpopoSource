using System;
using UnityEngine;

namespace VRSashimiTanpopo.Localization
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class LocalizedSprite : MonoBehaviour
    {
        [SerializeField] Sprite japaneseSprite;
        [SerializeField] Sprite englishSprite;

        void Start()
        {
            UpdateSprite();
        }

        void OnValidate()
        {
            UpdateSprite();
        }

        void UpdateSprite()
        {
            GetComponent<SpriteRenderer>().sprite = LocalizationSettings.GetCurrentLanguage() switch
            {
                Language.Japanese => japaneseSprite,
                Language.English => englishSprite,
                _ => throw new NotImplementedException()
            };
        }
    }
}

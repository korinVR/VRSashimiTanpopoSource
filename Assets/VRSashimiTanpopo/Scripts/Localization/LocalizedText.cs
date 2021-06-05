using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace VRSashimiTanpopo.Localization
{
    [ExecuteInEditMode]
    public class LocalizedText : MonoBehaviour
    {
        [SerializeField, Multiline] string japaneseText;
        [SerializeField, Multiline] string englishText;
        
        void Start()
        {
            UpdateText();
        }
        
        void OnValidate()
        {
            UpdateText();
        }
        
        void UpdateText()
        {
            var text = LocalizationSettings.GetCurrentLanguage() == Language.Japanese ? japaneseText : englishText;
            
            var tmpText = GetComponent<TMP_Text>();
            if (tmpText != null)
            {
                tmpText.text = text;
            }

            var uiText = GetComponent<Text>();
            if (uiText != null)
            {
                uiText.text = text;
            }
        }
    }
}

using System;
using TMPro;
using UnityEngine;
using VRSashimiTanpopo.Localization;

namespace VRSashimiTanpopo
{
    public class ScoreMessage : MonoBehaviour
    {
        [SerializeField] TMP_Text text;

        public void Initialize(Vector3 position, int score)
        {
            transform.position = position;
            
            var sign = score > 0 ? "+" : "-";

            text.text = sign + LocalizationSettings.GetCurrentLanguage() switch
            {
                Language.Japanese => "1円",
                Language.English => "1 cent",
                _ => throw new NotImplementedException()
            };
            text.color = score > 0 ? Color.white : Color.red;
        }

        void Start()
        {
            Destroy(gameObject, 1f);
        }

        void Update()
        {
            transform.Translate(new Vector3(-0.25f, 0.1f, 0f) * Time.deltaTime);
        }
    }
}

using System;
using TMPro;
using UnityEngine;
using VRSashimiTanpopo.Localization;
using VRSashimiTanpopo.UI;

namespace VRSashimiTanpopo.ScoreBoard.InfiniteTanpopo
{
    public class InfiniteTanpopoScoreBoardView : MonoBehaviour
    {
        [SerializeField] TMP_Text workingTime;
        [SerializeField] GameObject gameOver;
        [SerializeField] TextBlinker labelBlinker;

        public void UpdateWorkingTimeText(double workingTimeSec)
        {
            var hours = (int) (workingTimeSec / 3600);
            var minutes = (int) (workingTimeSec / 60) % 60;
            var seconds = (int) workingTimeSec % 60;
            var centiSeconds = (int) (workingTimeSec % 60 * 100) % 100;

            if (hours >= 1)
            {
                hours = 0;
                minutes = 59;
                seconds = 59;
                centiSeconds = 99;
            }
            
            workingTime.text = LocalizationSettings.GetCurrentLanguage() switch
            {
                Language.Japanese => $"{minutes:D2}分{seconds:D2}秒<space=0.1em><size=80%>{centiSeconds:D2}",
                Language.English => $"{minutes:D2}<space=0.5em>min {seconds:D2}<size=80%>.{centiSeconds:D2}<size=100%><space=0.5em>sec",
                _ => throw new NotImplementedException()
            };
        }

        public void UpdateGameOverText(bool active)
        {
            gameOver.SetActive(active);
        }

        public void EnableTextBlinker(bool enabled)
        {
            labelBlinker.enabled = enabled;
        }
    }
}
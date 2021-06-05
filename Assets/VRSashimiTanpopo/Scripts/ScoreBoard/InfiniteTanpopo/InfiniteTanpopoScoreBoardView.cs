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
            
            workingTime.text = $"{hours}時間{minutes:D2}分{seconds:D2}.{centiSeconds:D2}秒";

            workingTime.text = LocalizationSettings.GetCurrentLanguage() switch
            {
                Language.Japanese => $"{hours}時間{minutes:D2}分{seconds:D2}.{centiSeconds:D2}秒",
                Language.English => $"{hours} h {minutes:D2} min {seconds:D2}.{centiSeconds:D2} s",
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
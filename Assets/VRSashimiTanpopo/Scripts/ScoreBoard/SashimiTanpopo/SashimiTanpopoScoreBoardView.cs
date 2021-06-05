using System;
using TMPro;
using UnityEngine;
using VRSashimiTanpopo.Localization;
using VRSashimiTanpopo.UI;

namespace VRSashimiTanpopo.ScoreBoard.SashimiTanpopo
{
    public class SashimiTanpopoScoreBoardView : MonoBehaviour
    {
        [SerializeField] TMP_Text scoreText;
        [SerializeField] TMP_Text timeRemainingText;
        [SerializeField] GameObject gameOver;
        [SerializeField] TextBlinker labelBlinker;

        public void UpdateTimeRemainingText(float timeRemaining)
        {
            var minutes = (int) (timeRemaining / 60f);
            var seconds = (int) (timeRemaining % 60f);

            timeRemainingText.text = $"{minutes}:{seconds:D2}";
        }

        public void UpdateScoreText(int score)
        {
            scoreText.text = LocalizationSettings.GetCurrentLanguage() switch
            {
                Language.Japanese => $"賃金 {score}円",
                Language.English => $"Earnings ${(double) score / 100:0.00}",
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
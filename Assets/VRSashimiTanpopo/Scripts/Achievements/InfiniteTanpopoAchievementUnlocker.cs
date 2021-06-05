using VRSashimiTanpopo.Model;

namespace VRSashimiTanpopo.Achievements
{
    public class InfiniteTanpopoAchievementUnlocker
    {
        readonly IAchievementManager achievementManager;
        
        double elapsedTimeSec;

        readonly int[] unlockMinutes = {0, 1, 3, 5, 10, 30, 60, 120, 300};

        public InfiniteTanpopoAchievementUnlocker(IAchievementManager achievementManager)
        {
            this.achievementManager = achievementManager;
        }

        public void UpdateTime(WorkingTimeModel workingTimeModel)
        {
            var prevSec = (int) workingTimeModel.ElapsedTimeSec.Value;
            var currentSec = (int) elapsedTimeSec;
            elapsedTimeSec = workingTimeModel.ElapsedTimeSec.Value;

            if (currentSec == prevSec) return;
            // UnityEngine.Debug.Log("sec: " + currentSec);

            for (var i = 1; i < unlockMinutes.Length; i++)
            {
                if (currentSec == unlockMinutes[i] * 60)
                {
                    achievementManager.UnlockInfiniteTanpopoAchievement(i);
                }
            }
        }
    }
}
namespace VRSashimiTanpopo.Achievements
{
    public class NullAchievementManager : IAchievementManager
    {
        readonly DebugCode debugCode = new DebugCode();
        
        public void Start()
        {
            Log("NullAchievementManager.Start");
        }
        
        public void UnlockFirstSashimiTanpopo()
        {
            Log("Unlocked: FirstSashimiTanpopo");
        }

        public void UnlockFirstInfiniteTanpopo()
        {
            Log("Unlocked: FirstInfiniteTanpopo");
        }

        public void UnlockSashimiTanpopoPerfect()
        {
            Log("Unlocked: SashimiTanpopoPerfect");
        }

        public void UnlockInfiniteTanpopoAchievement(int index)
        {
            Log("Unlocked: InfiniteTanpopo" + index);
        }

        static void Log(string text)
        {
            // UnityEngine.Debug.Log(text);
        }

        public void Tick()
        {
            if (debugCode.HasCompleted())
            {
                Log("Reset Achievements");
            }
        }
    }
}
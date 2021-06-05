using VContainer.Unity;

namespace VRSashimiTanpopo.Achievements
{
    public interface IAchievementManager : IStartable, ITickable
    {
        void UnlockFirstSashimiTanpopo();
        void UnlockFirstInfiniteTanpopo();

        void UnlockSashimiTanpopoPerfect();
        void UnlockInfiniteTanpopoAchievement(int index);
    }
}
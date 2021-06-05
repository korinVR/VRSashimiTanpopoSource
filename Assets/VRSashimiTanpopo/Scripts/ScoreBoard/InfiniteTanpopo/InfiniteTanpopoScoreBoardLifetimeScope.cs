using VContainer;
using VContainer.Unity;

namespace VRSashimiTanpopo.ScoreBoard.InfiniteTanpopo
{
    public class InfiniteTanpopoScoreBoardLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<InfiniteTanpopoScoreBoardView>();
            builder.RegisterEntryPoint<InfiniteTanpopoScoreBoardPresenter>(Lifetime.Scoped);
        }
    }
}
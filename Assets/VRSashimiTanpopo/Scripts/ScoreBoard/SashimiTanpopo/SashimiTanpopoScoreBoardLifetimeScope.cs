using VContainer;
using VContainer.Unity;

namespace VRSashimiTanpopo.ScoreBoard.SashimiTanpopo
{
    public class SashimiTanpopoScoreBoardLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<SashimiTanpopoScoreBoardView>();
            builder.RegisterEntryPoint<SashimiTanpopoScoreBoardPresenter>(Lifetime.Scoped);
        }
    }
}
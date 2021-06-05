using UniRx;

namespace VRSashimiTanpopo.Model
{
    public class GameModel
    {
        public readonly ReactiveProperty<GameMode> GameMode = new ReactiveProperty<GameMode>();
        public readonly BoolReactiveProperty Playing = new BoolReactiveProperty();
        public readonly BoolReactiveProperty IsGameOver = new BoolReactiveProperty();
    }
}

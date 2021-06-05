using UniRx;

namespace VRSashimiTanpopo.Model
{
    public class ScoreModel
    {
        readonly ReactiveProperty<int> score = new ReactiveProperty<int>();
        public IReadOnlyReactiveProperty<int> Score => score;

        public void Reset()
        {
            score.Value = 0;
        }

        public void IncrementScore()
        {
            score.Value++;
        }

        public void DecrementScore()
        {
            score.Value--;
        }

        public bool IsPerfect() => score.Value == 38;
    }
}
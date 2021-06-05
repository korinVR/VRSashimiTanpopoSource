using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace VRSashimiTanpopo.Model
{
    public class WorkingTimeModel : ITickable
    {
        readonly ReactiveProperty<double> elapsedTimeSec = new ReactiveProperty<double>();
        public IReadOnlyReactiveProperty<double> ElapsedTimeSec => elapsedTimeSec;

        bool running;

        public void Reset()
        {
            Stop();
            elapsedTimeSec.Value = 0;
        }

        public void Start()
        {
            running = true;
        }

        public void Stop()
        {
            running = false;
        }

        public void Tick()
        {
            if (!running) return;
            
            elapsedTimeSec.Value += Time.deltaTime;
        }
    }
}
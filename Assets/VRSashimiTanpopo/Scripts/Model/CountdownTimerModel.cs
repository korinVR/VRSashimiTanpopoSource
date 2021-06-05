using System;
using UniRx;
using UnityEngine;
using VContainer.Unity;

namespace VRSashimiTanpopo.Model
{
    public class CountdownTimerModel : ITickable
    {
        public readonly ReactiveProperty<float> TimeRemaining = new ReactiveProperty<float>();

        public event Action Finished;

        bool running;

        public void Reset(float timeLimitSec)
        {
            TimeRemaining.Value = timeLimitSec;
            running = false;
        }

        public void Start()
        {
            running = true;
        }

        public void Tick()
        {
            if (!running) return;
            
            TimeRemaining.Value -= Time.deltaTime;

            if (TimeRemaining.Value < 0f)
            {
                TimeRemaining.Value = 0f;
                running = false;
                Finished?.Invoke();
            }
        }
    }
}

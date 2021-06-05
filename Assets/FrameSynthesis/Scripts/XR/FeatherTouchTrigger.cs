namespace FrameSynthesis.XR
{
    public class FeatherTouchTrigger
    {
        bool State { get; set; }

        readonly float threshold;

        float minPressure;
        float maxPressure;

        public FeatherTouchTrigger(float threshold = 0.2f)
        {
            this.threshold = threshold;
        }

        public bool UpdateState(float pressure)
        {
            if (!State)
            {
                if (minPressure > pressure)
                {
                    minPressure = pressure;
                }

                if (pressure > minPressure + threshold)
                {
                    maxPressure = pressure;
                    State = true;
                }
            }
            else
            {
                if (maxPressure < pressure)
                {
                    maxPressure = pressure;
                }

                if (pressure < maxPressure - threshold)
                {
                    minPressure = pressure;
                    State = false;
                }
            }

            return State;
        }
    }
}

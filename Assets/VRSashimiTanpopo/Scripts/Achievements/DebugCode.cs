namespace VRSashimiTanpopo.Achievements
{
    public class DebugCode
    {
        readonly OVRInput.RawButton[] sequenceButtons =
        {
            OVRInput.RawButton.LThumbstickUp,
            OVRInput.RawButton.LThumbstickUp,
            OVRInput.RawButton.LThumbstickDown,
            OVRInput.RawButton.LThumbstickDown,
            OVRInput.RawButton.LThumbstickLeft,
            OVRInput.RawButton.LThumbstickRight,
            OVRInput.RawButton.LThumbstickLeft,
            OVRInput.RawButton.LThumbstickRight,
            OVRInput.RawButton.B,
            OVRInput.RawButton.A,
        };

        int position;

        public bool HasCompleted()
        {
            if (OVRInput.GetDown(OVRInput.RawButton.Any))
            {
                if (OVRInput.GetDown(sequenceButtons[position]))
                {
                    if (++position >= sequenceButtons.Length)
                    {
                        position = 0;
                        return true;
                    }
                }
                else
                {
                    position = 0;
                }
            }
            return false;
        }
    }
}

using System;

namespace FrameSynthesis.XR
{
    public interface IPlatform
    {
        bool HasExternalDisplay { get; }

        event Action InputFocusLost;
        event Action InputFocusAcquired;
    }
}

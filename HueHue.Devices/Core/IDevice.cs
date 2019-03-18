using System;
using System.Collections.Generic;
using System.Text;

namespace HueHue.Devices.Core
{
    public interface IDevice
    {
        string Type { get; }
        int MaxStripCount { get; }
        int MaxLedPerStrip { get; }

        IReadOnlyList<ILedStrip> LedStrips { get; }

        void Start();
        void Stop();
    }
}

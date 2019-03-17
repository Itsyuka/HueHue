using System;
using System.Collections.Generic;
using System.Text;
using HueHue.Common;

namespace HueHue.Devices.Core
{
    public interface ILedStrip
    {
        IReadOnlyList<ILed> Leds { get; }

        void Set(Color color);
    }
}

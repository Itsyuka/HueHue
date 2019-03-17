using System;
using System.Collections.Generic;
using System.Text;

namespace HueHue.Devices.Core
{
    public interface ILedStrip
    {
        IReadOnlyList<ILed> Leds { get; }
    }
}

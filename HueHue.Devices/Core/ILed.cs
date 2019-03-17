using System;
using System.Collections.Generic;
using System.Text;
using HueHue.Common;

namespace HueHue.Devices.Core
{
    public interface ILed
    {
        int Position { get; }
        Color Color { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using HueHue.Common;

namespace HueHue.Devices.Core
{
    public class LedStripBase : ILedStrip
    {
        private List<ILed> leds = new List<ILed>();

        public IReadOnlyList<ILed> Leds => leds.AsReadOnly();

        public LedStripBase(int ledCount = 10, Color defaultColor = null)
        {
            for (int i = 0; i < ledCount; i++)
            {
                leds.Add(new Led(i, defaultColor));
            }
        }

        public void Set(Color color)
        {
            leds.ForEach(led => led.Color = color);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using HueHue.Common;
using HueHue.Devices.Core;

namespace HueHue.Devices.Hue
{
    public class HueStrip : LedStripBase
    {
        public int Id { get; set; }

        public HueStrip() :
            base(ledCount: 10, defaultColor: Color.Black)
        {
        }

        public byte[] LedBytes()
        {
            List<byte> bytes = new List<byte>();
            
            foreach (Led ledColor in Leds)
            {
                bytes.AddRange(ledColor.Color.ToRgbBytes());
            }

            return bytes.ToArray();
        }
    }
}

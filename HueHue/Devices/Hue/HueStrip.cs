using System;
using System.Collections.Generic;
using System.Text;

namespace HueHue.Devices.Hue
{
    public class HueStrip
    {
        public int Id { get; set; }

        public List<LedColor> Leds = new List<LedColor>();

        public HueStrip()
        {
            for (int i = 0; i < 10; i++)
            {
                Leds.Add(new LedColor());
            }
        }

        public byte[] LedBytes()
        {
            List<byte> bytes = new List<byte>();
            
            foreach (LedColor ledColor in Leds)
            {
                bytes.AddRange(ledColor.ToArray());
            }

            return bytes.ToArray();
        }
    }
}

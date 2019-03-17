using System;

namespace HueHue
{
    public class LedColor
    {
        public byte Red { get; set; } = 0;
        public byte Green { get; set; } = 0;
        public byte Blue { get; set; } = 0;

        public void Set(byte r, byte g, byte b)
        {
            Red = r;
            Green = g;
            Blue = b;
        }

        public void Set(byte[] rgb)
        {
            Set(rgb[0], rgb[1], rgb[2]);
        }

        public void SetAverage(byte[] rgb)
        {
            Set((byte)((Red + rgb[0]) / 2), (byte)((Green + rgb[1]) / 2), (byte)((Blue + rgb[2]) / 2));
        }

        public byte[] ToArray()
        {
            return new byte[3] { Red, Green, Blue };
        }
    }
}

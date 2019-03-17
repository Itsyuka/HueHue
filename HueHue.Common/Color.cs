using System;

namespace HueHue.Common
{
    public class Color
    {
        public int Red { get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        #region From

        public static Color FromRgb(int r, int g, int b)
        {
            return new Color() { Red = r, Green = g, Blue = b };
        }

        public static Color FromInt(int pointer)
        {
            return new Color()
            {
                Red = (pointer >> 8) & 0xFF,
                Green = (pointer >> 16) & 0xFF,
                Blue = pointer & 0xFF
            };
        }

        public static Color FromUInt(uint pointer)
        {
            return new Color()
            {
                Red = (int)(pointer >> 8) & 0xFF,
                Green = (int)(pointer >> 16) & 0xFF,
                Blue = (int)(pointer & 0xFF)
            };
        }

        #endregion

        #region To

        public byte[] ToRgbBytes()
        {
            return new byte[3] { (byte)Red, (byte)Green, (byte)Blue };
        }

        #endregion


        #region Colors

        public static Color Black => FromRgb(0, 0, 0);

        #endregion
    }
}

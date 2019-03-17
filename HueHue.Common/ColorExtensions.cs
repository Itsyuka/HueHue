using System;
using System.Collections.Generic;
using System.Text;

namespace HueHue.Common
{
    public static class ColorExtensions
    {
        public static Color Average(this Color color1, Color color2)
        {
            return Color.FromRgb(
                (color1.Red + color2.Red) / 2,
                (color1.Green + color2.Green) / 2,
                (color1.Blue + color2.Blue) / 2
                );
        }

        public static Color Average(this Color[] colors)
        {
            int r = 0;
            int g = 0;
            int b = 0;

            foreach (Color color in colors)
            {
                r += color.Red;
                g += color.Green;
                b += color.Blue;
            }

            return Color.FromRgb(r / colors.Length, g / colors.Length, b / colors.Length);
        }
    }
}

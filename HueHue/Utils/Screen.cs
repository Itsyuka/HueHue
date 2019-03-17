using System;
using System.Collections.Generic;
using HueHue.Common;

namespace HueHue.Utils
{
    public class Screen
    {
        public unsafe static uint GetPointedValue(uint* ptr, int x, int y)
        {
            int pixelLocation = (x + (1920 * y));
            return ptr[pixelLocation];
        }

        public unsafe static Color GetAverageColor(uint* ptr, int x, int y, int radius = 1)
        {
            List<Color> colors = new List<Color>();

            // Left of X
            for (int n = 0; n < radius; n++)
            {
                int m = x - n;
                if (m >= 0 && m < 1920)
                {
                    colors.Add(Color.FromUInt(GetPointedValue(ptr, x, y)));
                }
            }

            // Right of X
            for (int n = 0; n < radius; n++)
            {
                int m = x + n;
                if (m >= 0 && m < 1920)
                {
                    colors.Add(Color.FromUInt(GetPointedValue(ptr, x, y)));
                }
            }

            // Top of Y
            for (int n = 0; n < radius; n++)
            {
                int m = y - n;
                if (m >= 0 && m < 1080)
                {
                    colors.Add(Color.FromUInt(GetPointedValue(ptr, x, y)));
                }
            }

            // Bottom of Y
            for (int n = 0; n < radius; n++)
            {
                int m = y + n;
                if (m >= 0 && m < 1080)
                {
                    colors.Add(Color.FromUInt(GetPointedValue(ptr, x, y)));
                }
            }

            return colors.ToArray().Average();
        }
    }
}

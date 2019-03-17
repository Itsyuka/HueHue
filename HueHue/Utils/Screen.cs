using System;
using System.Collections.Generic;

namespace HueHue.Utils
{
    public class Screen
    {
        public unsafe static byte[] GetBytes(int* ptr, int x, int y)
        {
            int pixelLocation = (x + (1920 * y));
            return new byte[] { (byte)((ptr[pixelLocation] >> 8) & 0xFF), (byte)((ptr[pixelLocation] >> 16) & 0xFF), (byte)(ptr[pixelLocation] & 0xFF) };
        }

        public unsafe static byte[] GetAveragePixelColorBytes(int* ptr, int x, int y, int radius = 1)
        {
            List<byte[]> colors = new List<byte[]>();

            // Left of X
            for (int n = 0; n < radius; n++)
            {
                int m = x - n;
                if (m >= 0 && m < 1920)
                {
                    colors.Add(GetBytes(ptr, m, y));
                }
            }

            // Right of X
            for (int n = 0; n < radius; n++)
            {
                int m = x + n;
                if (m >= 0 && m < 1920)
                {
                    colors.Add(GetBytes(ptr, m, y));
                }
            }

            // Top of Y
            for (int n = 0; n < radius; n++)
            {
                int m = y - n;
                if (m >= 0 && m < 1080)
                {
                    colors.Add(GetBytes(ptr, x, m));
                }
            }

            // Bottom of Y
            for (int n = 0; n < radius; n++)
            {
                int m = y + n;
                if (m >= 0 && m < 1080)
                {
                    colors.Add(GetBytes(ptr, x, m));
                }
            }

            int R = 0;
            int G = 0;
            int B = 0;

            for (int i = 0; i < colors.Count; i++)
            {
                R += colors[i][0];
                G += colors[i][1];
                B += colors[i][2];
            }

            return new byte[] { (byte)(R / colors.Count), (byte)(G / colors.Count), (byte)(B / colors.Count) };
        }
    }
}

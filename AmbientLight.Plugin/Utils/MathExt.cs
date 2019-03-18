using System;
using System.Collections.Generic;
using System.Text;

namespace AmbientLight.Plugin.Utils
{
    public static class MathExt
    {
        public static int Clamp(int value, int min, int max)
            => Math.Max(min, Math.Min(max, value));
    }
}

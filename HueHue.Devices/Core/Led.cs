using System;
using System.Collections.Generic;
using System.Text;
using HueHue.Common;

namespace HueHue.Devices.Core
{
    public class Led : ILed
    {
        public int Position { get; }

        public Color Color { get; set; }

        public Led(int position, Color defaultColor = null)
        {
            Position = position;
            Color = defaultColor ?? Color.Black;
        }

        public void SetColor(Color color)
            => Color = color;
    }
}

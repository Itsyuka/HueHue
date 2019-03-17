using System;
using System.Collections.Generic;
using System.Text;

namespace HueHue.Devices.Hue
{
    public class HuePacket
    {
        public byte MagicByte = 75;
        public byte Channel = 1;
        public byte AnimationMode = 14;
        public byte AnimationDirection = 0;
        public byte AnimationOptions = 0;
        public byte AnimationGroup = 0;
        public byte AnimationSpeed = 2;

        public byte[] LedColors = new byte[120];
    }
}

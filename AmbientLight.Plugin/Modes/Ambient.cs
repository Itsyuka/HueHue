using System;
using System.Collections.Generic;
using System.Text;
using HueHue.Devices.Core;

namespace AmbientLight.Plugin.Modes
{
    public class Ambient : IMode, IModeRunnable
    {
        public string Name => "Ambient";

        private ScreenCapture _screenCapture;

        public Ambient(IDevice device)
        {
            _screenCapture = new ScreenCapture(device);
        }

        public void Start()
            => _screenCapture.Start();

        public void Stop()
            => _screenCapture.Stop();
    }
}

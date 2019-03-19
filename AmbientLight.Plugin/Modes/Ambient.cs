using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        public Task StartAsync()
            => Task.Run(() => _screenCapture.Start());

        public Task StopAsync()
            => Task.Run(() => _screenCapture.Stop());
    }
}

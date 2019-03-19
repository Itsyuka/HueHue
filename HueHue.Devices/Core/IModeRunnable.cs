using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HueHue.Devices.Core
{
    public interface IModeRunnable
    {
        Task StartAsync();

        Task StopAsync();
    }
}

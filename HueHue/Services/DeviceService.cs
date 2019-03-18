using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HueHue.Devices.Core;
using Microsoft.Extensions.Hosting;

namespace HueHue.Services
{
    public class DeviceService : IHostedService
    {
        private IDevice _device;

        public DeviceService(IDevice device)
        {
            _device = device;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _device.Start();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _device.Stop();

            return Task.CompletedTask;
        }
    }
}

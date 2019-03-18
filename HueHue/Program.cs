using System;
using HueHue.Devices.Core;
using HueHue.Devices.Hue;
using HueHue.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HueHue
{
    class Program
    {
        static void Main(string[] args)
        {
            new HostBuilder().ConfigureServices(services =>
                {
                    services.AddSingleton<IDevice, HueDevice>();

                    services.AddHostedService<DeviceService>();
                    services.AddHostedService<PluginService>();
                })
                .RunConsoleAsync()
                .GetAwaiter()
                .GetResult();
        }
    }
}

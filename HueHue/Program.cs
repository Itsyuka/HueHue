using System;
using System.Threading.Tasks;
using HueHue.Devices.Core;
using HueHue.Devices.Hue;
using HueHue.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HueHue
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json");
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<IDevice, HueDevice>();

                    services.AddHostedService<DeviceService>();
                    services.AddHostedService<PluginService>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.AddConsole();
                })
                .RunConsoleAsync();
        }
    }
}

using System;
using System.Threading.Tasks;
using HueHue.Devices.Core;
using HueHue.PluginBase;
using Microsoft.Extensions.DependencyInjection;

namespace AmbientLight.Plugin
{
    [PluginEntry]
    public class Plugin : IPlugin
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // We don't use this yet
        }

        public Task Start(IServiceProvider services)
        {
            new ScreenCapture(services.GetService<IDevice>()).Run();
            return Task.CompletedTask;
        }
    }
}

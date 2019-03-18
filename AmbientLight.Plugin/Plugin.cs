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
        public void ConfigureServices(IServiceProvider services)
        {
            // We don't use this yet
        }
    }
}

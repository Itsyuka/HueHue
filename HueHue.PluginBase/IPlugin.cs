using System;
using System.Threading.Tasks;
using HueHue.Devices.Core;
using Microsoft.Extensions.DependencyInjection;

namespace HueHue.PluginBase
{
    public interface IPlugin
    {
        void ConfigureServices(IServiceCollection services);

        Task Start(IServiceProvider services);
    }
}

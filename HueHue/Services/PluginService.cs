using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HueHue.PluginBase;
using Microsoft.Extensions.Hosting;

namespace HueHue.Services
{
    public class PluginService : IHostedService
    {
        public List<IPlugin> Plugins { get; } = new List<IPlugin>();
        private IServiceProvider _services;

        public PluginService(IServiceProvider services)
        {
            string path = AppContext.BaseDirectory;
            string[] pluginFiles = Directory.GetFiles(path, "*.Plugin.dll");

            foreach (string pluginFile in pluginFiles)
            {
                Assembly pluginDll = Assembly.LoadFrom(pluginFile);
                Type entryPoint = pluginDll.GetTypes().FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t));

                if (entryPoint != null)
                {
                    IPlugin plugin = (IPlugin)Activator.CreateInstance(entryPoint);
                    Plugins.Add(plugin);
                }
            }
        }

        public void ConfigureServices(IServiceProvider services)
            => Plugins.ForEach(plugin => plugin.ConfigureServices(services));

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Plugins.ForEach(plugin => plugin.ConfigureServices(_services));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            // Nothing to do yet
            return Task.CompletedTask;
        }
    }
}

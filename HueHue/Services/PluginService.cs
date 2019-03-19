using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using HueHue.Devices.Core;
using HueHue.PluginBase;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HueHue.Services
{
    public class PluginService : IHostedService
    {
        public List<IPlugin> Plugins { get; } = new List<IPlugin>();

        public List<IMode> Modes { get; } = new List<IMode>();

        private readonly IConfiguration _configuration;

        private readonly ILogger<PluginService> _logger;

        public PluginService(IServiceProvider services, IConfiguration configuration, ILogger<PluginService> logger)
        {
            _configuration = configuration;
            _logger = logger;

            string path = AppContext.BaseDirectory;
            string[] pluginFiles = Directory.GetFiles(path, "*.Plugin.dll");

            foreach (string pluginFile in pluginFiles)
            {
                Assembly pluginDll = Assembly.LoadFrom(pluginFile);
                Type entryPoint = pluginDll.GetTypes().FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t));

                if (entryPoint != null)
                {
                    try
                    {
                        ConstructorInfo constructor = entryPoint.GetConstructors().FirstOrDefault();
                        object[] parameters = constructor?.GetParameters().Select(s => services.GetService(s.ParameterType)).ToArray();
                        IPlugin plugin = (IPlugin)Activator.CreateInstance(entryPoint, parameters);
                        Plugins.Add(plugin);

                        if (plugin.Modes.Count > 0)
                        {
                            Modes.AddRange(plugin.Modes);
                        }

                    } catch (Exception e)
                    {
                        _logger.LogWarning($"Could not load {entryPoint.Name}: {e.Message}");
                    }
                }
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var selectedMode = Modes.FirstOrDefault(m => m.Name == _configuration["HueHue:Mode"]);

            if (selectedMode is IModeRunnable mode)
            {
                mode.StartAsync();
            }
            
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            var selectedMode = Modes.FirstOrDefault(m => m.Name == _configuration["HueHue:Mode"]);

            if (selectedMode is IModeRunnable mode)
            {
                mode.StopAsync();
            }

            return Task.CompletedTask;
        }
    }
}

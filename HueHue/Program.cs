using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HueHue.Devices.Core;
using HueHue.Devices.Hue;
using HueHue.PluginBase;
using Microsoft.Extensions.DependencyInjection;

namespace HueHue
{
    class Program
    {
        static List<IPlugin> plugins = new List<IPlugin>();

        static void Main(string[] args)
        {
            IServiceCollection serviceCollection = new ServiceCollection()
                .AddSingleton<IDevice, HueDevice>();

            string path = AppContext.BaseDirectory;
            string[] pluginFiles = Directory.GetFiles(path, "*.Plugin.dll");

            foreach (string pluginFile in pluginFiles)
            {
                Assembly pluginDll = Assembly.LoadFrom(pluginFile);
                Type entryPoint = pluginDll.GetTypes().FirstOrDefault(t => typeof(IPlugin).IsAssignableFrom(t));

                if (entryPoint != null)
                {
                    IPlugin plugin = (IPlugin)Activator.CreateInstance(entryPoint);
                    plugin.ConfigureServices(serviceCollection);
                    plugins.Add(plugin);
                    Console.WriteLine($"{Path.GetFileNameWithoutExtension(pluginFile)} Loaded!");
                }
            }

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            IDevice device = serviceProvider.GetRequiredService<IDevice>();
            device.Start();

            foreach (IPlugin plugin in plugins)
            {
                Task.Run(() => plugin.Start(serviceProvider));
            }

            Console.WriteLine("Press enter to close");
            Console.ReadLine();
            device.Stop();
        }
    }
}

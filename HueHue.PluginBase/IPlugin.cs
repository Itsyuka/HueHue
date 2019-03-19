using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HueHue.Devices.Core;
using Microsoft.Extensions.DependencyInjection;

namespace HueHue.PluginBase
{
    public interface IPlugin
    {
        /// <summary>
        /// The plugin's name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Version of the plugin
        /// </summary>
        string Version { get; }

        IReadOnlyList<IMode> Modes { get; }
    }
}

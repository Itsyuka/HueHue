using System;
using System.Collections.Generic;
using AmbientLight.Plugin.Modes;
using HueHue.Devices.Core;
using HueHue.PluginBase;
using Microsoft.Extensions.Logging;

namespace AmbientLight.Plugin
{
    public class Plugin : IPlugin
    {
        public string Name => "AmbientLight";

        public string Version => "0.0.1";

        private List<IMode> _modes = new List<IMode>();

        public IReadOnlyList<IMode> Modes => _modes.AsReadOnly();

        private readonly IDevice _device;
        private readonly ILogger<Plugin> _logger;

        public Plugin(ILogger<Plugin> logger, IDevice device)
        {
            _logger = logger;
            _device = device;

            _modes.Add(new Ambient(device));
        }
    }
}
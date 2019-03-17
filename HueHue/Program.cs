using System;
using System.Threading.Tasks;
using HueHue.Common;
using HueHue.Devices.Hue;
using HueHue.Utils;

using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;

using Color = HueHue.Common.Color;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using Resource = SharpDX.DXGI.Resource;

namespace HueHue
{
    class Program
    {
        static async Task Main(string[] args)
        {

            const int numAdapter = 0;
            const int numOutput = 0;

            Factory1 factory = new Factory1();
            Adapter1 adapter = factory.GetAdapter1(numAdapter);
            Device device = new Device(adapter);

            Output output = adapter.GetOutput(numOutput);
            Output1 output1 = output.QueryInterface<Output1>();

            int width = ((Rectangle)output.Description.DesktopBounds).Width;
            int height = ((Rectangle)output.Description.DesktopBounds).Height;

            Texture2DDescription textureDesc = new Texture2DDescription
            {
                CpuAccessFlags = CpuAccessFlags.Read,
                BindFlags = BindFlags.None,
                Format = Format.B8G8R8A8_UNorm,
                Width = width,
                Height = height,
                OptionFlags = ResourceOptionFlags.None,
                MipLevels = 1,
                ArraySize = 1,
                SampleDescription = { Count = 1, Quality = 0 },
                Usage = ResourceUsage.Staging
            };
            Texture2D screenTexture = new Texture2D(device, textureDesc);

            OutputDuplication duplicatedOutput = output1.DuplicateOutput(device);

            HueDevice hueDevice = new HueDevice();
            hueDevice.Start();

            bool isExiting = false;

            Console.CancelKeyPress += new ConsoleCancelEventHandler((s, e) =>
            {
                if (e.SpecialKey == ConsoleSpecialKey.ControlC)
                {
                    isExiting = true;
                }
                e.Cancel = true;
            });

            Console.WriteLine("Press CTRL+C to close gracefully");

            while (!isExiting)
            {
                try
                {
                    Resource screenResource;

                    duplicatedOutput.TryAcquireNextFrame(10000, out _, out screenResource);

                    using Texture2D screenTexture2D = screenResource.QueryInterface<Texture2D>();

                    device.ImmediateContext.CopyResource(screenTexture2D, screenTexture);

                    DataBox mapSource = device.ImmediateContext.MapSubresource(screenTexture, 0, MapMode.Read, MapFlags.None);

                    int radius = 5;

                    unsafe
                    {
                        uint* sourcePtr = (uint*)mapSource.DataPointer;
                        for (int i = 0; i < hueDevice.LedStrips.Count; i++)
                        {
                            // Right, Top, Left, Bottom
                            for (int p = 0; p < 10; p++)
                            {
                                Color color;
                                if (i == 0)
                                {
                                    color = Screen.GetAverageColor(sourcePtr, 1919, Math.Clamp(1080 - (108 * p), 0, 1079), radius);
                                }
                                else if (i == 1)
                                {
                                    color = Screen.GetAverageColor(sourcePtr, Math.Clamp(1620 - (300 + (132 * p)), 300, 1620), 0, radius);
                                }
                                else if (i == 2)
                                {
                                    color = Screen.GetAverageColor(sourcePtr, 0, Math.Clamp(108 * p, 0, 1079), radius);
                                }
                                else
                                {
                                    color = Screen.GetAverageColor(sourcePtr, Math.Clamp(300 + (132 * p), 300, 1620), 1079, radius);
                                }
                                hueDevice.LedStrips[i].Leds[p].Color = hueDevice.LedStrips[i].Leds[p].Color.Average(color);
                            }
                        }
                    }

                    device.ImmediateContext.UnmapSubresource(screenTexture, 0);
                    screenResource.Dispose();
                    duplicatedOutput.ReleaseFrame();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"An error was caught: {e.Message}");
                }
                finally
                {

                }
            }

            hueDevice.SetColor(Color.Black);
            await Task.Delay(50);
            hueDevice.Stop();
        }
    }
}

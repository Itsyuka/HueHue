﻿using System;

using HueHue.Devices.Hue;
using HueHue.Utils;
using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Device = SharpDX.Direct3D11.Device;
using MapFlags = SharpDX.Direct3D11.MapFlags;
using Resource = SharpDX.DXGI.Resource;

namespace HueHue
{
    class Program
    {
        static void Main(string[] args)
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

            HueDevice hueDevice = new HueDevice(ledStrips: 4);
            hueDevice.Start();

            while (true)
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
                        int* sourcePtr = (int*)mapSource.DataPointer;
                        for (int i = 0; i < hueDevice.HueStrips.Count; i++)
                        {
                            // Right, Top, Left, Bottom
                            for (int p = 0; p < 10; p++)
                            {
                                byte[] rgb;
                                if (i == 0)
                                {
                                    rgb = Screen.GetAveragePixelColorBytes(sourcePtr, 1919, Math.Clamp(1080 - (108 * p), 0, 1079), radius);
                                }
                                else if (i == 1)
                                {
                                    rgb = Screen.GetAveragePixelColorBytes(sourcePtr, Math.Clamp(1620 - (300 + (132 * p)), 300, 1620), 0, radius);
                                }
                                else if (i == 2)
                                {
                                    rgb = Screen.GetAveragePixelColorBytes(sourcePtr, 0, Math.Clamp(108 * p, 0, 1079), radius);
                                }
                                else
                                {
                                    rgb = Screen.GetAveragePixelColorBytes(sourcePtr, Math.Clamp(300 + (132 * p), 300, 1620), 1079, radius);
                                }
                                hueDevice.HueStrips[i].Leds[p].SetAverage(rgb);
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

        }
    }
}

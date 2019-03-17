using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace HueHue.Devices.Hue
{
    public class HueDevice
    {
        private SerialPort serialPort;

        public List<HueStrip> HueStrips { get; } = new List<HueStrip>();

        private Timer timer;

        public HueDevice(string portName = "COM4", int ledStrips = 4)
        {
            serialPort = new SerialPort(portName, 256000, Parity.None, 8, StopBits.One);
            for (int i = 0; i < ledStrips; i++)
            {
                HueStrips.Add(new HueStrip { Id = i });
            }
        }

        public void Start()
        {
            if (serialPort.IsOpen)
            {
                throw new Exception("Port is already open, how did you do this?");
            }
            serialPort.Open();
            timer = new Timer(Update, new AutoResetEvent(true), 33, 33);
        }

        public void Stop()
        {
            if (!serialPort.IsOpen)
            {
                throw new Exception("Port is already closed??? nani??");
            }
            serialPort.Close();
        }

        private async void Update(object state)
        {
            TaskCompletionSource<bool> completionSource = new TaskCompletionSource<bool>();
            SerialDataReceivedEventHandler eventHandler = delegate (object sender, SerialDataReceivedEventArgs e)
            {
                completionSource.SetResult(true);
            };

            serialPort.DataReceived += eventHandler;

            SerialErrorReceivedEventHandler errorEventHandler = delegate (object sender, SerialErrorReceivedEventArgs e)
            {
                Console.WriteLine($"Error writing to Serial Device: {e.EventType.ToString()}");
            };

            serialPort.ErrorReceived += errorEventHandler;

            byte[] bytes = new byte[125];
            bytes[0] = 75;
            bytes[1] = 1;
            bytes[2] = 14;
            bytes[3] = (byte)(0 << 4 | 0 << 3 | HueStrips.Count);
            bytes[4] = 0 << 5 | 0 << 3 | 2;

            foreach (HueStrip hueStrip in HueStrips)
            {
                hueStrip.LedBytes().CopyTo(bytes, 5 + (30 * hueStrip.Id));
            }

            serialPort.Write(bytes, 0, 125);
            await Task.WhenAny(completionSource.Task, Task.Delay(100));
            await completionSource.Task;
            serialPort.DataReceived -= eventHandler;
        }
    }
}

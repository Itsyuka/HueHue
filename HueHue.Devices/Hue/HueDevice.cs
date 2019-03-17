using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using HueHue.Common;
using HueHue.Devices.Core;

namespace HueHue.Devices.Hue
{
    public class HueDevice : IDevice
    {
        private SerialPort serialPort;

        private List<HueStrip> strips = new List<HueStrip>();

        private Timer timer;

        public string Type => "HUEPLUS1.0";

        public int MaxStripCount => 4;

        public int MaxLedPerStrip => 10;

        public IReadOnlyList<ILedStrip> LedStrips => strips.AsReadOnly();

        public HueDevice(string portName = "COM4")
        {
            serialPort = new SerialPort(portName, 256000, Parity.None, 8, StopBits.One);
            for (int i = 0; i < MaxStripCount; i++)
            {
                strips.Add(new HueStrip { Id = i });
            }
        }

        public void Start()
        {
            if (serialPort.IsOpen)
            {
                throw new Exception("Port is already open, how did you do this?");
            }
            serialPort.Open();
            timer = new Timer(Update, new AutoResetEvent(true), 0, 33);
        }

        public void Stop()
        {
            if (!serialPort.IsOpen)
            {
                throw new Exception("Port is already closed??? nani??");
            }
            timer.Dispose();
            serialPort.Close();
        }

        public void SetColor(Color color)
        {
            foreach (ILedStrip strip in strips)
            {
                strip.Set(Color.Black);
            }
        }

        // Shouldn't need async anymore since we are only polling every 33ms
        private void Update(object state)
        {
            byte[] bytes = new byte[125];
            bytes[0] = 75;
            bytes[1] = 1;
            bytes[2] = 14;
            bytes[3] = (byte)(0 << 4 | 0 << 3 | strips.Count);
            bytes[4] = 0 << 5 | 0 << 3 | 2;

            foreach (HueStrip hueStrip in strips)
            {
                hueStrip.LedBytes().CopyTo(bytes, 5 + (30 * hueStrip.Id));
            }

            serialPort.Write(bytes, 0, 125);
        }
    }
}

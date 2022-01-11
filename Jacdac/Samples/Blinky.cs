﻿using Jacdac.Clients;
using System;
using System.Threading;

namespace Jacdac.Samples
{
    internal class Blinky : ISample
    {
        public uint ProductIdentifier => 0x36ad4f2b;

        public void Run(JDBus bus)
        {
            var led = new LedClient(bus, "led");
            led.Connected += (s, e) =>
            {
                var speed = 64u;
                var brightness = 128u;
                while (led.IsConnected)
                {
                    Console.WriteLine("blink");
                    // blue
                    led.Animate(0, 0, brightness, speed);
                    Thread.Sleep(500);
                    // red
                    led.Animate(brightness, 0, 0, speed);
                    Thread.Sleep(500);
                }
            };
        }

        private void Led_Connected(JDNode sender, ServiceEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
﻿using Jacdac.Clients;
using System;
using System.Threading;

namespace Jacdac.Samples
{
    internal class SlidyBlinky : ISample
    {
        public uint ProductIdentifier => 0x36ad4f2b;

        public void Run(JDBus bus)
        {
            var led = new LedClient(bus, "led");
            var slider = new PotentiometerClient(bus, "slider");
            var speed = 64u;

            while (true)
            {
                try
                {
                    // grab brightness
                    var brightness = (uint)(slider.Position * 100);
                    // blue
                    led.Animate(0, 0, brightness, speed);
                    Thread.Sleep(500);
                    // red
                    led.Animate(brightness, 0, 0, speed);
                    Thread.Sleep(500);
                }
                catch (ClientDisconnectedException)
                {
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
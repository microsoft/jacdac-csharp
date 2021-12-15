﻿using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using GHIElectronics.TinyCLR.Devices.Gpio;
using GHIElectronics.TinyCLR.Devices.Network;
using GHIElectronics.TinyCLR.Devices.Storage;
using GHIElectronics.TinyCLR.Drivers.Microchip.Winc15x0;
using GHIElectronics.TinyCLR.IO;
using GHIElectronics.TinyCLR.Pins;

namespace Jacdac.Servers
{
    public sealed class WifiServer : JDServiceServer
    {
        public readonly IKeyStorage KeyStorage;
        public readonly JDStaticRegisterServer Enabled;
        public readonly JDStaticRegisterServer IpAddress;
        public readonly JDStaticRegisterServer Eui48;
        public readonly JDStaticRegisterServer Ssid;

        private NetworkController networkController;
        private bool scanning = false;
        private string[] lastScanResults = null;

        public WifiServer(IKeyStorage keyStorage)
            : base(Jacdac.WifiConstants.ServiceClass)
        {
            this.KeyStorage = keyStorage;
            this.AddRegister(this.Enabled = new JDStaticRegisterServer(
                (ushort)Jacdac.WifiReg.Enabled, "u8", new object[] { (byte)0 }
                ));
            this.AddRegister(this.IpAddress = new JDStaticRegisterServer(
                (ushort)Jacdac.WifiReg.IpAddress, "b", new object[] { new byte[0] }
                ));
            this.AddRegister(this.Eui48 = new JDStaticRegisterServer(
                (ushort)Jacdac.WifiReg.Eui48, "b", new object[] { new byte[0] }
                ));
            this.AddRegister(this.Ssid = new JDStaticRegisterServer(
                (ushort)Jacdac.WifiReg.Ssid, "s", new object[] { "" }
                ));
            // TODO RSSi

            this.AddCommand((ushort)Jacdac.WifiCmd.AddNetwork, this.handleAddNetwork);
            this.AddCommand((ushort)Jacdac.WifiCmd.ForgetNetwork, this.handleForgetNetwork);
            this.AddCommand((ushort)Jacdac.WifiCmd.ForgetAllNetworks, this.handleForgetAllNetworks);
            this.AddCommand((ushort)Jacdac.WifiCmd.Scan, this.handleScan);

            this.Enabled.Changed += Enabled_Changed;
            this.Ssid.Changed += Ssid_Changed;
            this.ScanCompleted += WifiServer_ScanCompleted;

            this.Start();
        }

        private void WifiServer_ScanCompleted(JDNode sender, EventArgs e)
        {
            var values = this.Enabled.GetValues();
            var enabled = (byte)values[0] != 0 ? true : false;
            if (enabled && this.networkController == null)
                this.Connect();
        }

        private void Ssid_Changed(JDNode sender, EventArgs e)
        {
            var ssid = (string)this.Ssid.GetValues()[0];
            var ev = !String.IsNullOrEmpty(ssid) ? (ushort)Jacdac.WifiEvent.GotIp : (ushort)Jacdac.WifiEvent.LostIp;
            this.SendEvent(ev);
        }

        private void Enabled_Changed(JDNode sender, EventArgs e)
        {
            var values = this.Enabled.GetValues();
            var enabled = (byte)values[0] != 0 ? true : false;
            if (enabled)
                this.StartScan();
            else
                this.Stop();
        }

        public void Start()
        {
            if (this.networkController != null) return;

            this.Enabled.SetValues(new object[] { (byte)1 });
            this.StartScan();
        }

        public void Stop()
        {
            if (this.networkController == null) return;

            this.networkController.Disable();
            this.networkController.Dispose();
            this.networkController = null;

            this.Ssid.SetValues(new object[] { "" });
            this.IpAddress.SetValues(new object[] { new byte[0] });
            this.Enabled.SetValues(new object[] { (byte)0 });
        }

        private void handleScan(JDNode node, PacketEventArgs args)
        {
            this.StartScan();
        }

        public void StartScan()
        {
            if (this.scanning) return;

            this.scanning = true;
            new Thread(() =>
            {
                try
                {
                    /*
                    var ssids = Winc15x0Interface.Scan();
                    this.lastScanResults = ssids;

                    var knownSsids = this.KeyStorage.GetKeys();
                    var total = ssids.Length;
                    var known = 0;
                    for (var i = 0; i < ssids.Length; i++)
                        if (Array.IndexOf(knownSsids, ssids[i]) != -1)
                            known++;

                    this.SendEvent(
                        (ushort)Jacdac.WifiEvent.ScanComplete,
                        PacketEncoding.Pack("u16 u16", new object[] { total, known })
                    );
                    */
                    this.lastScanResults = new string[0];
                    this.ScanCompleted?.Invoke(this, EventArgs.Empty);
                }
                finally
                {
                    this.scanning = false;
                }
            }).Start();
        }

        public event NodeEventHandler ScanCompleted;

        private void handleAddNetwork(JDNode node, PacketEventArgs args)
        {
            var pkt = args.Packet;
            var values = PacketEncoding.UnPack("z z", pkt.Data);
            if (values == null) return;

            var ssid = (string)values[0];
            var password = (string)values[1];
            if (!string.IsNullOrEmpty(ssid))
                this.KeyStorage.Write(ssid, UTF8Encoding.UTF8.GetBytes(password));

            this.RaiseChanged();
        }

        private void handleForgetNetwork(JDNode node, PacketEventArgs args)
        {
            var pkt = args.Packet;
            var values = PacketEncoding.UnPack("s", pkt.Data);
            if (values == null) return;
            var ssid = (string)values[0];
            if (!string.IsNullOrEmpty(ssid))
                this.KeyStorage.Delete(ssid);
            this.RaiseChanged();
        }

        private void handleForgetAllNetworks(JDNode node, PacketEventArgs args)
        {
            this.KeyStorage.Clear();
            this.RaiseChanged();
        }

        private void Connect()
        {
            Debug.Assert(this.networkController == null);

            // find best access point
            var secrets = this.FindAccessPoint();
            if (secrets == null)
            {
                Debug.WriteLine("Wifi: no known ssid found");
                return;
            }
            Debug.WriteLine($"Wifi: connecting to {secrets[0]}");

            const string WIFI_API_NAME = "GHIElectronics.TinyCLR.NativeApis.ATWINC15xx.NetworkController";

            const int RESET = FEZBit.GpioPin.WiFiReset;
            const int SPI_CS = FEZBit.GpioPin.WiFiChipselect;
            const int SPI_INT = FEZBit.GpioPin.WiFiInterrupt;
            const int ENABLE = FEZBit.GpioPin.WiFiEnable;

            var gpioController = GpioController.GetDefault();

            var interrupt = gpioController.OpenPin(SPI_INT);
            var reset = gpioController.OpenPin(RESET);
            var cs = gpioController.OpenPin(SPI_CS);
            var en = gpioController.OpenPin(ENABLE);

            var networkCommunicationInterfaceSettings = new SpiNetworkCommunicationInterfaceSettings();
            var settings = new GHIElectronics.TinyCLR.Devices.Spi.SpiConnectionSettings()
            {
                ChipSelectLine = cs,
                ClockFrequency = 2000000,
                Mode = GHIElectronics.TinyCLR.Devices.Spi.SpiMode.Mode0,
                ChipSelectType = GHIElectronics.TinyCLR.Devices.Spi.SpiChipSelectType.Gpio,
                ChipSelectHoldTime = TimeSpan.FromTicks(10),
                ChipSelectSetupTime = TimeSpan.FromTicks(10)
            };

            networkCommunicationInterfaceSettings.SpiApiName = FEZBit.SpiBus.WiFi;
            networkCommunicationInterfaceSettings.GpioApiName = SC20100.GpioPin.Id;
            networkCommunicationInterfaceSettings.SpiSettings = settings;
            networkCommunicationInterfaceSettings.InterruptPin = interrupt;
            networkCommunicationInterfaceSettings.InterruptEdge = GpioPinEdge.FallingEdge;
            networkCommunicationInterfaceSettings.InterruptDriveMode = GpioPinDriveMode.InputPullUp;
            networkCommunicationInterfaceSettings.ResetPin = reset;
            networkCommunicationInterfaceSettings.ResetActiveState = GpioPinValue.Low;

            en.SetDriveMode(GpioPinDriveMode.Output);
            en.Write(GpioPinValue.High);

            this.networkController = NetworkController.FromName(WIFI_API_NAME);
            var networkInterfaceSetting = new WiFiNetworkInterfaceSettings()
            {
                Ssid = secrets[0],
                Password = secrets[1],
            };

            networkInterfaceSetting.DhcpEnable = true;
            networkInterfaceSetting.DynamicDnsEnable = true;

            this.networkController.SetCommunicationInterfaceSettings(networkCommunicationInterfaceSettings);
            this.networkController.SetInterfaceSettings(networkInterfaceSetting);
            this.networkController.SetAsDefaultController();

            this.networkController.NetworkAddressChanged += (sender, args) =>
            {
                var ipProperties = sender.GetIPProperties();
                var address = ipProperties.Address.GetAddressBytes();

                if (address[0] != 0 && address[1] != 0)
                {
                    this.Ssid.SetValues(new object[] { networkInterfaceSetting.Ssid });
                    this.IpAddress.SetValues(new object[] { address });
                    Debug.WriteLine($"Wifi {networkInterfaceSetting.Ssid} connected");
                }
            };
            this.networkController.Enable();
        }

        private string[] FindAccessPoint()
        {
            if (this.lastScanResults == null) return null;

            var keys = this.KeyStorage.GetKeys();
            for (var i = 0; i < this.lastScanResults.Length; i++)
            {
                var ssid = this.lastScanResults[i];
                if (Array.IndexOf(keys, ssid) != -1)
                {
                    var buffer = this.KeyStorage.Read(ssid);
                    var password = UTF8Encoding.UTF8.GetString(buffer); ;
                    return new string[] { ssid, password };
                }
            }

            return null;
        }
    }
}
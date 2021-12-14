﻿namespace Jacdac
{
    public sealed class JDDeviceServer
    {
        public readonly JDBus Bus;
        public readonly string DeviceId;
        private byte restartCounter = 0;
        private byte packetCount = 0;
        private JDServiceServer[] servers;
        public bool IsClient;

        public JDDeviceServer(JDBus bus, string deviceId, JDBusOptions options)
        {
            this.Bus = bus;
            this.DeviceId = deviceId;
            this.IsClient = options.IsClient;
            this.servers = new JDServiceServer[1 + (options.Services != null ? options.Services.Length : 0)];
            this.servers[0] = new ControlServer(options);
            if (options.Services != null)
                options.Services.CopyTo(this.servers, 1);
            for (byte i = 0; i < servers.Length; i++)
            {
                var server = this.servers[i];
                server.Device = this;
                server.ServiceIndex = i;
            }
        }

        public string ShortId
        {
            get { return Util.ShortDeviceId(this.DeviceId); }
        }

        public override string ToString()
        {
            return this.ShortId;
        }

        public void ProcessPacket(Packet pkt)
        {
            if (pkt.ServiceIndex < this.servers.Length)
            {
                var server = this.servers[pkt.ServiceIndex];
                var processed = server.ProcessPacket(pkt);
            }
        }

        public void SendPacket(Packet pkt)
        {
            this.packetCount++;
            if (!pkt.IsMultiCommand)
                pkt.DeviceId = this.DeviceId;
            var frame = Packet.ToFrame(new Packet[] { pkt });
            this.Bus.Transport.SendFrame(frame);
        }

        public void SendAnnounce()
        {
            // we do not support any services (at least yet)
            if (this.restartCounter < 0xf) this.restartCounter++;

            var servers = this.servers;
            var serviceClasses = new object[servers.Length - 1];
            for (var i = 1; i < servers.Length; ++i)
                serviceClasses[i - 1] = new object[] { servers[i].ServiceClass };
            var data = PacketEncoding.Pack("u16 u8 x[8] r: u32",
                new object[] {
                    (ushort)((ushort)this.restartCounter |
                        (this.IsClient ? (ushort)ControlAnnounceFlags.IsClient : (ushort)0) |
                        (ushort)ControlAnnounceFlags.SupportsBroadcast |
                        (ushort)ControlAnnounceFlags.SupportsFrames |
                        (ushort)ControlAnnounceFlags.SupportsACK
                    ),
                    this.packetCount,
                    serviceClasses
                });
            this.packetCount = 0;
            var pkt = Packet.From(Jacdac.Constants.CMD_ADVERTISEMENT_DATA, data);
            pkt.ServiceIndex = Jacdac.Constants.JD_SERVICE_INDEX_CTRL;
            this.SendPacket(pkt);
        }
    }
}

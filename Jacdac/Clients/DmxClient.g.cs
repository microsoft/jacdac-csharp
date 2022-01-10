/** Autogenerated file. Do not edit. */
using Jacdac;
using System;

namespace Jacdac.Clients {

    /// <summary>
    /// A service that can send DMX512-A packets with limited size. This service is designed to allow tinkering with a few DMX devices, but only allows 235 channels. More about DMX at https://en.wikipedia.org/wiki/DMX512.
    /// Implements a client for the DMX service.
    /// </summary>
    /// <seealso cref="https://microsoft.github.io/jacdac-docs/services/dmx/" />
    public partial class DmxClient : Client
    {
        public DmxClient(JDBus bus, string name)
            : base(bus, name, ServiceClasses.Dmx)
        {
        }

        /// <summary>
        /// Reads the <c>enabled</c> register value.
        /// Determines if the DMX bridge is active., 
        /// </summary>
        public bool Enabled
        {
            get
            {
                return (bool)this.GetRegisterValue((ushort)DmxReg.Enabled, DmxRegPack.Enabled);
            }
            set
            {
                
                this.SetRegisterValue((ushort)DmxReg.Enabled, DmxRegPack.Enabled, value);
            }

        }


        
        /// <summary>
        /// Send a DMX packet, up to 236bytes long, including the start code.
        /// </summary>
        public void Send(byte[] channels)
        {
            this.SendCmdPacked((ushort)DmxCmd.Send, DmxCmdPack.Send, new object[] { channels });
        }

    }
}
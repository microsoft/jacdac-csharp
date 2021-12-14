﻿using System;

namespace Jacdac.Servers
{
    public abstract class JDServer : JDNode
    {
        public byte ServiceIndex;
        public JDBus Bus;
        public readonly uint ServiceClass;
        private JDRegisterServer[] registers;

        public JDServer(uint serviceClass)
        {
            this.ServiceClass = serviceClass;
            this.registers = new JDRegisterServer[0];
        }

        public virtual void ProcessPacket(Packet pkt)
        {
            if (pkt.IsRegisterGet || pkt.IsRegisterSet)
            {
                JDRegisterServer register;
                if (this.TryGetRegister(pkt.RegisterCode, out register))
                    register.ProcessPacket(pkt);
                return;
            }
        }

        public void AddRegister(JDRegisterServer register)
        {
            lock (this)
            {
                register.Server = this;

                var registers = this.registers;
                var newRegisters = new JDRegisterServer[registers.Length + 1];
                registers.CopyTo(newRegisters, 0);
                newRegisters[newRegisters.Length - 1] = register;
                this.registers = newRegisters;
            }
        }

        public bool TryGetRegister(ushort code, out JDRegisterServer register)
        {
            var registers = this.registers;
            for (var i = 0; i < registers.Length; i++)
            {
                var r = registers[i];
                if (r.Code == code)
                {
                    register = r;
                    return true;
                }
            }
            register = null;
            return false;
        }

        public void SendPacket(Packet pkt)
        {
            var bus = this.Bus;
            if (bus == null) return;

            pkt.DeviceId = bus.SelfDevice.DeviceId;
            pkt.ServiceIndex = this.ServiceIndex;
            bus.SendPacket(pkt);
        }
    }
}

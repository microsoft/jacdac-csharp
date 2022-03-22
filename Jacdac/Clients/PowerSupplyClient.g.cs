/** Autogenerated file. Do not edit. */
using Jacdac;
using System;

namespace Jacdac.Clients 
{
    /// <summary>
    /// A power supply with a fixed or variable voltage range
    /// Implements a client for the Power supply service.
    /// </summary>
    /// <seealso cref="https://microsoft.github.io/jacdac-docs/services/powersupply/" />
    public partial class PowerSupplyClient : Client
    {
        public PowerSupplyClient(JDBus bus, string name)
            : base(bus, name, ServiceClasses.PowerSupply)
        {
        }

        /// <summary>
        /// Reads the <c>enabled</c> register value.
        /// Turns the power supply on with `true`, off with `false`., 
        /// </summary>
        public bool Enabled
        {
            get
            {
                return (bool)this.GetRegisterValueAsBool((ushort)PowerSupplyReg.Enabled, PowerSupplyRegPack.Enabled);
            }
            set
            {
                
                this.SetRegisterValue((ushort)PowerSupplyReg.Enabled, PowerSupplyRegPack.Enabled, value);
            }

        }

        /// <summary>
        /// Reads the <c>output_voltage</c> register value.
        /// The current output voltage of the power supply. Values provided must be in the range `minimum_voltage` to `maximum_voltage`, _: V
        /// </summary>
        public float OutputVoltage
        {
            get
            {
                return (float)this.GetRegisterValue((ushort)PowerSupplyReg.OutputVoltage, PowerSupplyRegPack.OutputVoltage);
            }
            set
            {
                
                this.Enabled = true;
                this.SetRegisterValue((ushort)PowerSupplyReg.OutputVoltage, PowerSupplyRegPack.OutputVoltage, value);
            }

        }

        /// <summary>
        /// Reads the <c>minimum_voltage</c> register value.
        /// The minimum output voltage of the power supply. For fixed power supplies, `minimum_voltage` should be equal to `maximum_voltage`., _: V
        /// </summary>
        public float MinimumVoltage
        {
            get
            {
                return (float)this.GetRegisterValue((ushort)PowerSupplyReg.MinimumVoltage, PowerSupplyRegPack.MinimumVoltage);
            }
        }

        /// <summary>
        /// Reads the <c>maximum_voltage</c> register value.
        /// The maximum output voltage of the power supply. For fixed power supplies, `minimum_voltage` should be equal to `maximum_voltage`., _: V
        /// </summary>
        public float MaximumVoltage
        {
            get
            {
                return (float)this.GetRegisterValue((ushort)PowerSupplyReg.MaximumVoltage, PowerSupplyRegPack.MaximumVoltage);
            }
        }


    }
}
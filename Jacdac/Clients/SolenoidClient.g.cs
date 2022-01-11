/** Autogenerated file. Do not edit. */
using Jacdac;
using System;

namespace Jacdac.Clients {

    /// <summary>
    /// A push-pull solenoid is a type of relay that pulls a coil when activated.
    /// Implements a client for the Solenoid service.
    /// </summary>
    /// <seealso cref="https://microsoft.github.io/jacdac-docs/services/solenoid/" />
    public partial class SolenoidClient : Client
    {
        public SolenoidClient(JDBus bus, string name)
            : base(bus, name, ServiceClasses.Solenoid)
        {
        }

        /// <summary>
        /// Reads the <c>pulled</c> register value.
        /// Indicates whether the solenoid is energized and pulled (on) or pushed (off)., 
        /// </summary>
        public bool Pulled
        {
            get
            {
                return (bool)this.GetRegisterValue((ushort)SolenoidReg.Pulled, SolenoidRegPack.Pulled);
            }
            set
            {
                
                this.SetRegisterValue((ushort)SolenoidReg.Pulled, SolenoidRegPack.Pulled, value);
            }

        }

        /// <summary>
        /// Tries to read the <c>variant</c> register value.
        /// Describes the type of solenoid used., 
        /// </summary>
        bool TryGetVariant(out SolenoidVariant value)
        {
            object[] values;
            if (this.TryGetRegisterValues((ushort)SolenoidReg.Variant, SolenoidRegPack.Variant, out values)) 
            {
                value = (SolenoidVariant)values[0];
                return true;
            }
            else
            {
                value = default(SolenoidVariant);
                return false;
            }
        }


    }
}
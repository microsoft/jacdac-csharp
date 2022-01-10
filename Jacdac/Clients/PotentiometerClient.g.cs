/** Autogenerated file. Do not edit. */
using Jacdac;
using System;

namespace Jacdac.Clients {

    /// <summary>
    /// A slider or rotary potentiometer.
    /// Implements a client for the Potentiometer service.
    /// </summary>
    /// <seealso cref="https://microsoft.github.io/jacdac-docs/services/potentiometer/" />
    public partial class PotentiometerClient : SensorClient
    {
        public PotentiometerClient(JDBus bus, string name)
            : base(bus, name, ServiceClasses.Potentiometer)
        {
        }

        /// <summary>
        /// Reads the <c>position</c> register value.
        /// The relative position of the slider., _: /
        /// </summary>
        public float Position
        {
            get
            {
                return (float)this.GetRegisterValue((ushort)PotentiometerReg.Position, PotentiometerRegPack.Position);
            }
        }

        /// <summary>
        /// Tries to read the <c>variant</c> register value.
        /// Specifies the physical layout of the potentiometer., 
        /// </summary>
        bool TryGetVariant(out PotentiometerVariant value)
        {
            object[] values;
            if (this.TryGetRegisterValues((ushort)PotentiometerReg.Variant, PotentiometerRegPack.Variant, out value)) 
            {
                value = (PotentiometerVariant)values[0];
                return true;
            }
            else
            {
                value = default(PotentiometerVariant);
                return false;
            }
        }


    }
}
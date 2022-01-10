/** Autogenerated file. Do not edit. */
using Jacdac;
using System;

namespace Jacdac.Clients {

    /// <summary>
    /// Measures equivalent CO₂ levels.
    /// Implements a client for the Equivalent CO₂ service.
    /// </summary>
    /// <seealso cref="https://microsoft.github.io/jacdac-docs/services/eco2/" />
    public partial class ECO2Client : SensorClient
    {
        public ECO2Client(JDBus bus, string name)
            : base(bus, name, ServiceClasses.ECO2)
        {
        }

        /// <summary>
        /// Reads the <c>e_CO2</c> register value.
        /// Equivalent CO₂ (eCO₂) readings., _: ppm
        /// </summary>
        public float ECO2
        {
            get
            {
                return (float)this.GetRegisterValue((ushort)ECO2Reg.ECO2, ECO2RegPack.ECO2);
            }
        }

        /// <summary>
        /// Tries to read the <c>e_CO2_error</c> register value.
        /// Error on the reading value., _: ppm
        /// </summary>
        bool TryGetECO2Error(out float value)
        {
            object[] values;
            if (this.TryGetRegisterValues((ushort)ECO2Reg.ECO2Error, ECO2RegPack.ECO2Error, out value)) 
            {
                value = (float)values[0];
                return true;
            }
            else
            {
                value = default(float);
                return false;
            }
        }

        /// <summary>
        /// Reads the <c>min_e_CO2</c> register value.
        /// Minimum measurable value, _: ppm
        /// </summary>
        public float MinECO2
        {
            get
            {
                return (float)this.GetRegisterValue((ushort)ECO2Reg.MinECO2, ECO2RegPack.MinECO2);
            }
        }

        /// <summary>
        /// Reads the <c>max_e_CO2</c> register value.
        /// Minimum measurable value, _: ppm
        /// </summary>
        public float MaxECO2
        {
            get
            {
                return (float)this.GetRegisterValue((ushort)ECO2Reg.MaxECO2, ECO2RegPack.MaxECO2);
            }
        }

        /// <summary>
        /// Tries to read the <c>variant</c> register value.
        /// Type of physical sensor and capabilities., 
        /// </summary>
        bool TryGetVariant(out ECO2Variant value)
        {
            object[] values;
            if (this.TryGetRegisterValues((ushort)ECO2Reg.Variant, ECO2RegPack.Variant, out value)) 
            {
                value = (ECO2Variant)values[0];
                return true;
            }
            else
            {
                value = default(ECO2Variant);
                return false;
            }
        }


    }
}
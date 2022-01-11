/** Autogenerated file. Do not edit. */
using Jacdac;
using System;

namespace Jacdac.Clients {

    /// <summary>
    /// A sensor that detects light and dark surfaces, commonly used for line following robots.
    /// Implements a client for the Reflected light service.
    /// </summary>
    /// <seealso cref="https://microsoft.github.io/jacdac-docs/services/reflectedlight/" />
    public partial class ReflectedLightClient : SensorClient
    {
        public ReflectedLightClient(JDBus bus, string name)
            : base(bus, name, ServiceClasses.ReflectedLight)
        {
        }

        /// <summary>
        /// Reads the <c>brightness</c> register value.
        /// Reports the reflected brightness. It may be a digital value or, for some sensor, analog value., _: /
        /// </summary>
        public float Brightness
        {
            get
            {
                return (float)this.GetRegisterValue((ushort)ReflectedLightReg.Brightness, ReflectedLightRegPack.Brightness);
            }
        }

        /// <summary>
        /// Tries to read the <c>variant</c> register value.
        /// Type of physical sensor used, 
        /// </summary>
        bool TryGetVariant(out ReflectedLightVariant value)
        {
            object[] values;
            if (this.TryGetRegisterValues((ushort)ReflectedLightReg.Variant, ReflectedLightRegPack.Variant, out values)) 
            {
                value = (ReflectedLightVariant)values[0];
                return true;
            }
            else
            {
                value = default(ReflectedLightVariant);
                return false;
            }
        }


    }
}
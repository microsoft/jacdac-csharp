/** Autogenerated file. Do not edit. */
using Jacdac;
using System;

namespace Jacdac.Clients {

    /// <summary>
    /// Detects the amount of light falling onto a given surface area.
     /// 
     /// Note that this is different from _luminance_, the amount of light that passes through, emits from, or reflects off an object.
    /// Implements a client for the Illuminance service.
    /// </summary>
    /// <seealso cref="https://microsoft.github.io/jacdac-docs/services/illuminance/" />
    public partial class IlluminanceClient : SensorClient
    {
        public IlluminanceClient(JDBus bus, string name)
            : base(bus, name, ServiceClasses.Illuminance)
        {
        }

        /// <summary>
        /// Reads the <c>illuminance</c> register value.
        /// The amount of illuminance, as lumens per square metre., _: lux
        /// </summary>
        public float Illuminance
        {
            get
            {
                return (float)this.GetRegisterValue((ushort)IlluminanceReg.Illuminance, IlluminanceRegPack.Illuminance);
            }
        }

        /// <summary>
        /// Tries to read the <c>illuminance_error</c> register value.
        /// Error on the reported sensor value., _: lux
        /// </summary>
        bool TryGetIlluminanceError(out float values)
        {
            object[] values;
            if (this.TryGetRegisterValues((ushort)IlluminanceReg.IlluminanceError, IlluminanceRegPack.IlluminanceError, out value)) 
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


    }
}
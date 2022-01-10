/** Autogenerated file. Do not edit. */
using Jacdac;
using System;

namespace Jacdac.Clients {

    /// <summary>
    /// A 3-axis gyroscope.
    /// Implements a client for the Gyroscope service.
    /// </summary>
    /// <seealso cref="https://microsoft.github.io/jacdac-docs/services/gyroscope/" />
    public partial class GyroscopeClient : SensorClient
    {
        public GyroscopeClient(JDBus bus, string name)
            : base(bus, name, ServiceClasses.Gyroscope)
        {
        }

        /// <summary>
        /// Reads the <c>rotation_rates</c> register value.
        /// Indicates the current rates acting on gyroscope., x: °/s,y: °/s,z: °/s
        /// </summary>
        public object[] /*(float, float, float)*/ RotationRates
        {
            get
            {
                return (object[] /*(float, float, float)*/)this.GetRegisterValues((ushort)GyroscopeReg.RotationRates, GyroscopeRegPack.RotationRates);
            }
        }

        /// <summary>
        /// Tries to read the <c>rotation_rates_error</c> register value.
        /// Error on the reading value., _: °/s
        /// </summary>
        bool TryGetRotationRatesError(out float value)
        {
            object[] values;
            if (this.TryGetRegisterValues((ushort)GyroscopeReg.RotationRatesError, GyroscopeRegPack.RotationRatesError, out value)) 
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
        /// Tries to read the <c>max_rate</c> register value.
        /// Configures the range of rotation rates.
        /// The value will be "rounded up" to one of `max_rates_supported`., _: °/s
        /// </summary>
        bool TryGetMaxRate(out float value)
        {
            object[] values;
            if (this.TryGetRegisterValues((ushort)GyroscopeReg.MaxRate, GyroscopeRegPack.MaxRate, out value)) 
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
        /// Sets the max_rate value
        /// </summary>
        public void SetMaxRate(float value)
        {
            this.SetRegisterValue((ushort)GyroscopeReg.MaxRate, GyroscopeRegPack.MaxRate, value);
        }



    }
}
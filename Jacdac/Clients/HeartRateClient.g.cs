/** Autogenerated file. Do not edit. */
using Jacdac;
using System;

namespace Jacdac.Clients {

    /// <summary>
    /// A sensor approximating the heart rate. 
     /// 
     /// 
     /// **Jacdac is NOT suitable for medical devices and should NOT be used in any kind of device to diagnose or treat any medical conditions.**
    /// Implements a client for the Heart Rate service.
    /// </summary>
    /// <seealso cref="https://microsoft.github.io/jacdac-docs/services/heartrate/" />
    public partial class HeartRateClient : SensorClient
    {
        public HeartRateClient(JDBus bus, string name)
            : base(bus, name, ServiceClasses.HeartRate)
        {
        }

        /// <summary>
        /// Reads the <c>heart_rate</c> register value.
        /// The estimated heart rate., _: bpm
        /// </summary>
        public float HeartRate
        {
            get
            {
                return (float)this.GetRegisterValue((ushort)HeartRateReg.HeartRate, HeartRateRegPack.HeartRate);
            }
        }

        /// <summary>
        /// Tries to read the <c>heart_rate_error</c> register value.
        /// The estimated error on the reported sensor data., _: bpm
        /// </summary>
        bool TryGetHeartRateError(out float value)
        {
            object[] values;
            if (this.TryGetRegisterValues((ushort)HeartRateReg.HeartRateError, HeartRateRegPack.HeartRateError, out value)) 
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
        /// Tries to read the <c>variant</c> register value.
        /// The type of physical sensor, 
        /// </summary>
        bool TryGetVariant(out HeartRateVariant value)
        {
            object[] values;
            if (this.TryGetRegisterValues((ushort)HeartRateReg.Variant, HeartRateRegPack.Variant, out value)) 
            {
                value = (HeartRateVariant)values[0];
                return true;
            }
            else
            {
                value = default(HeartRateVariant);
                return false;
            }
        }


    }
}
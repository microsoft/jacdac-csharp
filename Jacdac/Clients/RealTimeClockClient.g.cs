/** Autogenerated file. Do not edit. */
using Jacdac;
using System;

namespace Jacdac.Clients {

    /// <summary>
    /// Real time clock to support collecting data with precise time stamps.
    /// Implements a client for the Real time clock service.
    /// </summary>
    /// <seealso cref="https://microsoft.github.io/jacdac-docs/services/realtimeclock/" />
    public partial class RealTimeClockClient : SensorClient
    {
        public RealTimeClockClient(JDBus bus, string name)
            : base(bus, name, ServiceClasses.RealTimeClock)
        {
        }

        /// <summary>
        /// Reads the <c>local_time</c> register value.
        /// Current time in 24h representation.
        /// 
        /// -   `day_of_month` is day of the month, starting at `1`
        /// -   `day_of_week` is day of the week, starting at `1` as monday. Default streaming period is 1 second., 
        /// </summary>
        public object[] /*(uint, uint, uint, uint, uint, uint, uint)*/ LocalTime
        {
            get
            {
                return (object[] /*(uint, uint, uint, uint, uint, uint, uint)*/)this.GetRegisterValues((ushort)RealTimeClockReg.LocalTime, RealTimeClockRegPack.LocalTime);
            }
        }

        /// <summary>
        /// Tries to read the <c>drift</c> register value.
        /// Time drift since the last call to the `set_time` command., _: s
        /// </summary>
        bool TryGetDrift(out float value)
        {
            object[] values;
            if (this.TryGetRegisterValues((ushort)RealTimeClockReg.Drift, RealTimeClockRegPack.Drift, out value)) 
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
        /// Tries to read the <c>precision</c> register value.
        /// Error on the clock, in parts per million of seconds., _: ppm
        /// </summary>
        bool TryGetPrecision(out float value)
        {
            object[] values;
            if (this.TryGetRegisterValues((ushort)RealTimeClockReg.Precision, RealTimeClockRegPack.Precision, out value)) 
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
        /// The type of physical clock used by the sensor., 
        /// </summary>
        bool TryGetVariant(out RealTimeClockVariant value)
        {
            object[] values;
            if (this.TryGetRegisterValues((ushort)RealTimeClockReg.Variant, RealTimeClockRegPack.Variant, out value)) 
            {
                value = (RealTimeClockVariant)values[0];
                return true;
            }
            else
            {
                value = default(RealTimeClockVariant);
                return false;
            }
        }


        
        /// <summary>
        /// Sets the current time and resets the error.
        /// </summary>
        public void SetTime(uint year, uint month, uint day_of_month, uint day_of_week, uint hour, uint min, uint sec)
        {
            this.SendCmdPacked((ushort)RealTimeClockCmd.SetTime, RealTimeClockCmdPack.SetTime, new object[] { year, month, day_of_month, day_of_week, hour, min, sec });
        }

    }
}
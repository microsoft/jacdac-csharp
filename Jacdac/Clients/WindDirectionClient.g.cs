/** Autogenerated file. Do not edit. */
using Jacdac;
using System;

namespace Jacdac.Clients {

    /// <summary>
    /// A sensor that measures wind direction.
    /// Implements a client for the Wind direction service.
    /// </summary>
    /// <seealso cref="https://microsoft.github.io/jacdac-docs/services/winddirection/" />
    public partial class WindDirectionClient : SensorClient
    {
        public WindDirectionClient(JDBus bus, string name)
            : base(bus, name, ServiceClasses.WindDirection)
        {
        }

        /// <summary>
        /// Reads the <c>wind_direction</c> register value.
        /// The direction of the wind., _: °
        /// </summary>
        public uint WindDirection
        {
            get
            {
                return (uint)this.GetRegisterValue((ushort)WindDirectionReg.WindDirection, WindDirectionRegPack.WindDirection);
            }
        }

        /// <summary>
        /// Tries to read the <c>wind_direction_error</c> register value.
        /// Error on the wind direction reading, _: °
        /// </summary>
        bool TryGetWindDirectionError(out uint value)
        {
            object[] values;
            if (this.TryGetRegisterValues((ushort)WindDirectionReg.WindDirectionError, WindDirectionRegPack.WindDirectionError, out value)) 
            {
                value = (uint)values[0];
                return true;
            }
            else
            {
                value = default(uint);
                return false;
            }
        }


    }
}
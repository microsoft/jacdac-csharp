/** Autogenerated file. Do not edit. */
using Jacdac;
using System;

namespace Jacdac.Clients {

    /// <summary>
    /// A sensor that measures the heading.
    /// Implements a client for the Compass service.
    /// </summary>
    /// <seealso cref="https://microsoft.github.io/jacdac-docs/services/compass/" />
    public partial class CompassClient : SensorClient
    {
        public CompassClient(JDBus bus, string name)
            : base(bus, name, ServiceClasses.Compass)
        {
        }

        /// <summary>
        /// Reads the <c>heading</c> register value.
        /// The heading with respect to the magnetic north., _: °
        /// </summary>
        public float Heading
        {
            get
            {
                return (float)this.GetRegisterValue((ushort)CompassReg.Heading, CompassRegPack.Heading);
            }
        }

        /// <summary>
        /// Reads the <c>enabled</c> register value.
        /// Turn on or off the sensor. Turning on the sensor may start a calibration sequence., 
        /// </summary>
        public bool Enabled
        {
            get
            {
                return (bool)this.GetRegisterValue((ushort)CompassReg.Enabled, CompassRegPack.Enabled);
            }
            set
            {
                
                this.SetRegisterValue((ushort)CompassReg.Enabled, CompassRegPack.Enabled, value);
            }

        }

        /// <summary>
        /// Tries to read the <c>heading_error</c> register value.
        /// Error on the heading reading, _: °
        /// </summary>
        bool TryGetHeadingError(out float value)
        {
            object[] values;
            if (this.TryGetRegisterValues((ushort)CompassReg.HeadingError, CompassRegPack.HeadingError, out value)) 
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
        /// Starts a calibration sequence for the compass.
        /// </summary>
        public void Calibrate()
        {
            this.SendCmd((ushort)CompassCmd.Calibrate);
        }

    }
}
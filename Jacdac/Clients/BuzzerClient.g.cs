/** Autogenerated file. Do not edit. */
using Jacdac;
using System;

namespace Jacdac.Clients {

    /// <summary>
    /// A simple buzzer.
    /// Implements a client for the Buzzer service.
    /// </summary>
    /// <seealso cref="https://microsoft.github.io/jacdac-docs/services/buzzer/" />
    public partial class BuzzerClient : Client
    {
        public BuzzerClient(JDBus bus, string name)
            : base(bus, name, ServiceClasses.Buzzer)
        {
        }

        /// <summary>
        /// The volume (duty cycle) of the buzzer., _: /
        /// </summary>
        public float Volume
        {
            get
            {
                return (float)this.GetRegisterValue((ushort)BuzzerReg.Volume, BuzzerRegPack.Volume);
            }
            set
            {
                
                this.SetRegisterValue((ushort)BuzzerReg.Volume, BuzzerRegPack.Volume, value);
            }

        }


        /// <summary>
        /// Play a PWM tone with given period and duty for given duration.
        /// The duty is scaled down with `volume` register.
        /// To play tone at frequency `F` Hz and volume `V` (in `0..1`) you will want
        /// to send `P = 1000000 / F` and `D = P * V / 2`.
        /// </summary>
        public void PlayTone(uint period, uint duty, uint duration)
        {
            this.SendCmdPacked((ushort)BuzzerCmd.PlayTone, BuzzerCmdPack.PlayTone, new object[] { period, duty, duration });
        }

        /// <summary>
        /// Play a note at the given frequency and volume.
        /// </summary>
        public void PlayNote(uint frequency, float volume, uint duration)
        {
            // TODO: implement client command
            throw new NotSupportedException("client command not implemented");
        }

    }
}
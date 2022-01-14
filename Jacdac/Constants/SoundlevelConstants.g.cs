namespace Jacdac {
    public static partial class ServiceClasses
    {
        public const uint SoundLevel = 0x14ad1a5d;
    }
    public enum SoundLevelReg : ushort {
        /// <summary>
        /// Read-only ratio u0.16 (uint16_t). The sound level detected by the microphone
        ///
        /// ```
        /// const [soundLevel] = jdunpack<[number]>(buf, "u0.16")
        /// ```
        /// </summary>
        SoundLevel = 0x101,

        /// <summary>
        /// Read-write bool (uint8_t). Turn on or off the microphone.
        ///
        /// ```
        /// const [enabled] = jdunpack<[number]>(buf, "u8")
        /// ```
        /// </summary>
        Enabled = 0x1,

        /// <summary>
        /// Read-write ratio u0.16 (uint16_t). The sound level to trigger a loud event.
        ///
        /// ```
        /// const [loudThreshold] = jdunpack<[number]>(buf, "u0.16")
        /// ```
        /// </summary>
        LoudThreshold = 0x6,

        /// <summary>
        /// Read-write ratio u0.16 (uint16_t). The sound level to trigger a quiet event.
        ///
        /// ```
        /// const [quietThreshold] = jdunpack<[number]>(buf, "u0.16")
        /// ```
        /// </summary>
        QuietThreshold = 0x5,
    }

    public static class SoundLevelRegPack {
        /// <summary>
        /// Pack format for 'sound_level' register data.
        /// </summary>
        public const string SoundLevel = "u0.16";

        /// <summary>
        /// Pack format for 'enabled' register data.
        /// </summary>
        public const string Enabled = "u8";

        /// <summary>
        /// Pack format for 'loud_threshold' register data.
        /// </summary>
        public const string LoudThreshold = "u0.16";

        /// <summary>
        /// Pack format for 'quiet_threshold' register data.
        /// </summary>
        public const string QuietThreshold = "u0.16";
    }

    public enum SoundLevelEvent : ushort {
        /// <summary>
        /// Raised when a loud sound is detected
        /// </summary>
        Loud = 0x1,

        /// <summary>
        /// Raised when a period of quietness is detected
        /// </summary>
        Quiet = 0x2,
    }

}

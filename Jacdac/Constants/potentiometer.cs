namespace Jacdac {
    // Service: Potentiometer
    public static class PotentiometerConstants
    {
        public const uint ServiceClass = 0x1f274746;
    }

    public enum PotentiometerVariant: byte { // uint8_t
        Slider = 0x1,
        Rotary = 0x2,
    }

    public enum PotentiometerReg {
        /**
         * Read-only ratio u0.16 (uint16_t). The relative position of the slider.
         *
         * ```
         * const [position] = jdunpack<[number]>(buf, "u0.16")
         * ```
         */
        Position = 0x101,

        /**
         * Constant Variant (uint8_t). Specifies the physical layout of the potentiometer.
         *
         * ```
         * const [variant] = jdunpack<[PotentiometerVariant]>(buf, "u8")
         * ```
         */
        Variant = 0x107,
    }

    public static class PotentiometerRegPack {
        /**
         * Pack format for 'position' register data.
         */
        public const string Position = "u0.16";

        /**
         * Pack format for 'variant' register data.
         */
        public const string Variant = "u8";
    }

}

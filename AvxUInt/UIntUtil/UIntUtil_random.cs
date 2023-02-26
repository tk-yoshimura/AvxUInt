﻿namespace AvxUInt {
    internal static partial class UIntUtil {
        public static UInt32[] Random(Random random, int length, int bits) {
            UInt32[] value = (new UInt32[length]).Select((_, idx) => idx < bits / UIntUtil.UInt32Bits ? (UInt32)random.NextUInt32() : 0u).ToArray();

            if (bits % UIntUtil.UInt32Bits > 0) {
                value[bits / UIntUtil.UInt32Bits] = (UInt32)random.NextUInt32() >> (UIntUtil.UInt32Bits - bits % UIntUtil.UInt32Bits);
            }

            return value;
        }

        public static UInt32 NextUInt32(this Random random) {
            byte[] vs = new byte[sizeof(UInt32)];

            random.NextBytes(vs);

            return (UInt32)vs[0] | ((UInt32)vs[1] << ShiftIDX1) | ((UInt32)vs[2] << ShiftIDX2) | ((UInt32)vs[3] << ShiftIDX3);
        }
    }
}

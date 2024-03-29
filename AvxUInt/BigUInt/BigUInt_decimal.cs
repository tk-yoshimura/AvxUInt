﻿using System.Diagnostics;

namespace AvxUInt {
    public sealed partial class BigUInt<N> {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Dictionary<int, BigUInt<N>> decimals = [];

        public static BigUInt<N> Decimal(int digits) {
            if (!decimals.TryGetValue(digits, out BigUInt<N>? value)) {
                value = GenerateDecimal(digits);
                decimals.Add(digits, value);
            }

            return value;
        }

        private static BigUInt<N> GenerateDecimal(int digits) {
            ArgumentOutOfRangeException.ThrowIfNegative(digits);

            UInt64 v = 1;
            for (int i = 0; i < digits % UIntUtil.UInt64MaxDecimalDigits; i++) {
                v *= 10;
            }

            BigUInt<N> uint64_maxdec = UIntUtil.UInt64MaxDecimal;
            BigUInt<N> biguint_dec = v;

            for (int i = 0; i < digits / UIntUtil.UInt64MaxDecimalDigits; i++) {
                biguint_dec *= uint64_maxdec;
            }

            return biguint_dec;
        }
    }
}

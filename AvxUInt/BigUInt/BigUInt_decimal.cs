using System.Diagnostics;

namespace AvxUInt {
    public sealed partial class BigUInt<N> {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private static readonly Dictionary<int, BigUInt<N>> decimals = new();

        public static BigUInt<N> Decimal(int digits) {
            if (!decimals.ContainsKey(digits)) {
                decimals.Add(digits, GenerateDecimal(digits));
            }

            return decimals[digits];
        }

        private static BigUInt<N> GenerateDecimal(int digits) {
            if (digits < 0) {
                throw new ArgumentOutOfRangeException(nameof(digits));
            }

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

﻿namespace AvxUInt {
    public sealed partial class BigUInt<N> {

        public static BigUInt<N> operator *(BigUInt<N> a, BigUInt<N> b) {
            return Mul(a, b);
        }

        public static BigUInt<N> operator *(BigUInt<N> a, UInt32 b) {
            return Mul(a, b);
        }

        public static BigUInt<N> operator *(UInt32 a, BigUInt<N> b) {
            return Mul(a, b);
        }

        public static BigUInt<N> operator *(BigUInt<N> a, UInt64 b) {
            return Mul(a, b);
        }

        public static BigUInt<N> operator *(UInt64 a, BigUInt<N> b) {
            return Mul(a, b);
        }

        public static BigUInt<N> Mul(BigUInt<N> a, BigUInt<N> b) {
            BigUInt<N> ret = Zero.Copy();

            UIntUtil.Fma(ret.value, a.value, b.value);

            return ret;
        }

        public static BigUInt<N> Mul(BigUInt<N> a, UInt32 b) {
            BigUInt<N> ret = Zero.Copy();

            UIntUtil.Fma(ret.value, a.value, b);

            return ret;
        }

        public static BigUInt<N> Mul(BigUInt<N> a, UInt64 b) {
            BigUInt<N> ret = Zero.Copy();

            UIntUtil.Fma(ret.value, a.value, b);

            return ret;
        }

        public static BigUInt<N> Mul(UInt32 a, BigUInt<N> b) {
            BigUInt<N> ret = Zero.Copy();

            UIntUtil.Fma(ret.value, b.value, a);

            return ret;
        }

        public static BigUInt<N> Mul(UInt64 a, BigUInt<N> b) {
            BigUInt<N> ret = Zero.Copy();

            UIntUtil.Fma(ret.value, b.value, a);

            return ret;
        }

        public static BigUInt<Double<N>> ExpandMul(BigUInt<N> a, BigUInt<N> b) {
            BigUInt<Double<N>> ret = BigUInt<Double<N>>.Zero;

            UIntUtil.Fma(ret.value, a.value, b.value);

            return ret;
        }
    }
}
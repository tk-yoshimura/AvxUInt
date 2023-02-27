﻿using System;
using System.Runtime.Intrinsics;
using static System.Runtime.Intrinsics.X86.Avx;
using static System.Runtime.Intrinsics.X86.Avx2;

namespace AvxUInt {
    internal static partial class UIntUtil {
        /// <summary>Operate uint32 array arr_dst += a * b</summary>
        public static void Fma(UInt32[] arr_dst, UInt32[] arr_a, UInt32[] arr_b) {
            uint digits_a = (uint)Digits(arr_a), digits_b = (uint)Digits(arr_b);

            if (digits_a >= digits_b) {
                for (uint i = 0; i < digits_b; i++) {
                    Fma(i, digits_a, arr_dst, arr_a, arr_b[i]);
                }
            }
            else {
                for (uint i = 0; i < digits_a; i++) {
                    Fma(i, digits_b, arr_dst, arr_b, arr_a[i]);
                }
            }
        }

        /// <summary>Operate uint32 array arr_dst += a * b</summary>
        public static void Fma(UInt32[] arr_dst, UInt32[] arr_a, UInt32 b) {
            Fma(0u, (uint)Digits(arr_a), arr_dst, arr_a, b);
        }

        /// <summary>Operate uint32 array arr_dst += a * b</summary>
        public static void Fma(UInt32[] arr_dst, UInt32[] arr_a, UInt64 b) {
            uint digits_a = (uint)Digits(arr_a);
            (UInt32 b_hi, UInt32 b_lo) = Unpack(b);

            Fma(0u, digits_a, arr_dst, arr_a, b_lo);
            Fma(1u, digits_a, arr_dst, arr_a, b_hi);
        }

        /// <summary>Operate uint32 array arr_dst += a * b &lt;&lt; offset</summary>
        private static unsafe void Fma(uint offset, uint digits_a, UInt32[] arr_dst, UInt32[] arr_a, UInt32 b) {
            if (b == 0u) {
                return;
            }

#if DEBUG
            if (digits_a != Digits(arr_a)) {
                throw new ArgumentException($"mismatch digits of {nameof(arr_a)}", nameof(digits_a));
            }
#endif

            if (digits_a + offset > arr_dst.Length) {
                throw new OverflowException();
            }

            fixed (UInt32* va0 = arr_a, vd0 = arr_dst) {
                UInt32* va = va0, vd = vd0 + offset;

                Vector256<UInt32> y = Vector256.Create((UInt64)b).AsUInt32();
                uint r = digits_a;

                Vector256<UInt32> a0, a1, a2, a3, d0, d1, d2, d3, r0, r1, r2, r3, c0, c1, c2, c3;

                while (r >= MM256UInt32s * 4) {
                    (a0, a1, a2, a3) = LoadX4(va, va0, arr_a.Length);
                    (d0, d1, d2, d3) = LoadX4(vd, vd0, arr_dst.Length);

                    (r0, c0) = Mul(a0, y);
                    (r1, c1) = Mul(a1, y);
                    (r2, c2) = Mul(a2, y);
                    (r3, c3) = Mul(a3, y);

                    (c0, c1, c2, c3, UInt32 carry) = CarryShiftX4(c0, c1, c2, c3, 0u);

                    while (!(IsAllZero(c0) & IsAllZero(c1) & IsAllZero(c2) & IsAllZero(c3))) {
                        (r0, c0) = Add(r0, c0);
                        (r1, c1) = Add(r1, c1);
                        (r2, c2) = Add(r2, c2);
                        (r3, c3) = Add(r3, c3);

                        (c0, c1, c2, c3, carry) = CarryShiftX4(c0, c1, c2, c3, carry);
                    }

                    (d0, c0) = Add(r0, d0);
                    (d1, c1) = Add(r1, d1);
                    (d2, c2) = Add(r2, d2);
                    (d3, c3) = Add(r3, d3);

                    (c0, c1, c2, c3, carry) = CarryShiftX4(c0, c1, c2, c3, carry);

                    while (!(IsAllZero(c0) & IsAllZero(c1) & IsAllZero(c2) & IsAllZero(c3))) {
                        (d0, c0) = Add(d0, c0);
                        (d1, c1) = Add(d1, c1);
                        (d2, c2) = Add(d2, c2);
                        (d3, c3) = Add(d3, c3);

                        (c0, c1, c2, c3, carry) = CarryShiftX4(c0, c1, c2, c3, carry);
                    }

                    StoreX4(vd, d0, d1, d2, d3, vd0, arr_dst.Length);
                    Add(offset + ShiftIDX4, arr_dst, carry);

                    va += MM256UInt32s * 4;
                    vd += MM256UInt32s * 4;
                    r -= MM256UInt32s * 4;
                    offset += MM256UInt32s * 4;
                }
                if (r >= MM256UInt32s * 2) {
                    (a0, a1) = LoadX2(va, va0, arr_a.Length);
                    (d0, d1) = LoadX2(vd, vd0, arr_dst.Length);

                    (r0, c0) = Mul(a0, y);
                    (r1, c1) = Mul(a1, y);

                    (c0, c1, UInt32 carry) = CarryShiftX2(c0, c1, 0u);

                    while (!(IsAllZero(c0) & IsAllZero(c1))) {
                        (r0, c0) = Add(r0, c0);
                        (r1, c1) = Add(r1, c1);

                        (c0, c1, carry) = CarryShiftX2(c0, c1, carry);
                    }

                    (d0, c0) = Add(r0, d0);
                    (d1, c1) = Add(r1, d1);

                    (c0, c1, carry) = CarryShiftX2(c0, c1, carry);

                    while (!(IsAllZero(c0) & IsAllZero(c1))) {
                        (d0, c0) = Add(d0, c0);
                        (d1, c1) = Add(d1, c1);

                        (c0, c1, carry) = CarryShiftX2(c0, c1, carry);
                    }

                    StoreX2(vd, d0, d1, vd0, arr_dst.Length);
                    Add(offset + ShiftIDX2, arr_dst, carry);

                    va += MM256UInt32s * 2;
                    vd += MM256UInt32s * 2;
                    r -= MM256UInt32s * 2;
                    offset += MM256UInt32s * 2;
                }
                if (r >= MM256UInt32s) {
                    a0 = Load(va, va0, arr_a.Length);
                    d0 = Load(vd, vd0, arr_dst.Length);

                    (r0, c0) = Mul(a0, y);

                    (c0, UInt32 carry) = CarryShift(c0, 0u);

                    while (!IsAllZero(c0)) {
                        (r0, c0) = Add(r0, c0);

                        (c0, carry) = CarryShift(c0, carry);
                    }

                    (d0, c0) = Add(r0, d0);

                    (c0, carry) = CarryShift(c0, carry);

                    while (!IsAllZero(c0)) {
                        (d0, c0) = Add(d0, c0);

                        (c0, carry) = CarryShift(c0, carry);
                    }

                    Store(vd, d0, vd0, arr_dst.Length);
                    Add(offset + ShiftIDX1, arr_dst, carry);

                    va += MM256UInt32s;
                    vd += MM256UInt32s;
                    r -= MM256UInt32s;
                    offset += MM256UInt32s;
                }
                if (r > 0) {
                    uint rem_d = (uint)arr_dst.Length - offset;
                    Vector256<UInt32> mask_a = Mask256.Lower(r);
                    Vector256<UInt32> mask_d = Mask256.Lower(rem_d);

                    a0 = MaskLoad(va, mask_a, va0, arr_a.Length);
                    d0 = MaskLoad(vd, mask_d, vd0, arr_dst.Length);

                    (r0, c0) = Mul(a0, y);

                    (c0, UInt32 carry) = CarryShift(c0, 0u);

                    while (!IsAllZero(c0)) {
                        (r0, c0) = Add(r0, c0);

                        (c0, carry) = CarryShift(c0, carry);
                    }

                    (d0, c0) = Add(r0, d0);

                    (c0, carry) = CarryShift(c0, carry);

                    while (!IsAllZero(c0)) {
                        (d0, c0) = Add(d0, c0);

                        (c0, carry) = CarryShift(c0, carry);
                    }

                    if (rem_d < MM256UInt32s) {
                        MaskStore(vd, d0, mask_d, vd0, arr_dst.Length);
                        if (d0.GetElement((int)rem_d) > 0u) {
                            throw new OverflowException();
                        }
                    }
                    else {
                        Store(vd, d0, vd0, arr_dst.Length);
                    }

                    Add(offset + ShiftIDX1, arr_dst, carry);
                }
            }
        }
    }
}

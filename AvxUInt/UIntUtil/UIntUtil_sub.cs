﻿using System.Runtime.Intrinsics;
using static System.Runtime.Intrinsics.X86.Avx;
using static System.Runtime.Intrinsics.X86.Avx2;

namespace AvxUInt {
    internal static partial class UIntUtil {
        /// <summary>Operate uint32 array a -= b</summary>
        public static unsafe void Sub(UInt32[] arr_a, UInt32[] arr_b) {
            uint digits_b = (uint)Digits(arr_b);
            if (digits_b == 0u) {
                return;
            }
            if (digits_b == 1u) {
                Sub(arr_a, arr_b[0]);
                return;
            }
            if (digits_b == 2u) {
                Sub(arr_a, Pack(arr_b[1], arr_b[0]));
                return;
            }
            if (digits_b > arr_a.Length) {
                throw new OverflowException();
            }

            fixed (UInt32* va0 = arr_a, vb0 = arr_b) {
                UInt32* va = va0, vb = vb0;
                Vector256<UInt32> a0, a1, a2, a3, b0, b1, b2, b3;

                uint offset = 0;

                while (offset + MM256UInt32s * 4 <= digits_b) {
                    (a0, a1, a2, a3) = LoadVector256X4(va);
                    (b0, b1, b2, b3) = LoadVector256X4(vb);

                    UInt32 carry = 0;

                    while (!(IsAllZero(b0) & IsAllZero(b1) & IsAllZero(b2) & IsAllZero(b3))) {
                        (a0, b0) = Sub(a0, b0);
                        (a1, b1) = Sub(a1, b1);
                        (a2, b2) = Sub(a2, b2);
                        (a3, b3) = Sub(a3, b3);

                        (b0, b1, b2, b3, carry) = CarryShiftX4(b0, b1, b2, b3, carry);
                    }

                    StoreX4(va, a0, a1, a2, a3);
                    Sub(offset + ShiftIDX4, arr_a, carry);

                    va += MM256UInt32s * 4;
                    vb += MM256UInt32s * 4;
                    offset += (int)MM256UInt32s * 4;
                }
                if (offset + MM256UInt32s * 2 <= digits_b) {
                    (a0, a1) = LoadVector256X2(va);
                    (b0, b1) = LoadVector256X2(vb);

                    UInt32 carry = 0;

                    while (!(IsAllZero(b0) & IsAllZero(b1))) {
                        (a0, b0) = Sub(a0, b0);
                        (a1, b1) = Sub(a1, b1);

                        (b0, b1, carry) = CarryShiftX2(b0, b1, carry);
                    }

                    StoreX2(va, a0, a1);
                    Sub(offset + ShiftIDX2, arr_a, carry);

                    va += MM256UInt32s * 2;
                    vb += MM256UInt32s * 2;
                    offset += (int)MM256UInt32s * 2;
                }
                if (offset + MM256UInt32s <= digits_b) {
                    a0 = LoadVector256(va);
                    b0 = LoadVector256(vb);

                    UInt32 carry = 0;

                    while (!IsAllZero(b0)) {
                        (a0, b0) = Sub(a0, b0);

                        if (IsAllZero(b0)) {
                            break;
                        }

                        (b0, carry) = CarryShift(b0, carry);
                    }

                    Store(va, a0);
                    Sub(offset + ShiftIDX1, arr_a, carry);

                    va += MM256UInt32s;
                    vb += MM256UInt32s;
                    offset += (int)MM256UInt32s;
                }
                if (offset < digits_b) {
                    uint rem_a = (uint)arr_a.Length - offset;
                    uint rem_b = digits_b - offset;
                    Vector256<UInt32> mask_a = Mask256.Lower(rem_a);
                    Vector256<UInt32> mask_b = Mask256.Lower(rem_b);

                    a0 = MaskLoad(va, mask_a);
                    b0 = MaskLoad(vb, mask_b);

                    UInt32 carry = 0;

                    while (!IsAllZero(b0)) {
                        (a0, b0) = Sub(a0, b0);

                        if (IsAllZero(b0)) {
                            break;
                        }

                        (b0, carry) = CarryShift(b0, carry);
                    }

                    MaskStore(va, mask_a, a0);

                    if (rem_a < MM256UInt32s) {
                        if (a0.GetElement((int)rem_a) > 0u) {
                            throw new OverflowException();
                        }
                    }
                    Sub(offset + ShiftIDX1, arr_a, carry);
                }
            }
        }

        /// <summary>Operate uint32 array a -= b</summary>
        public static unsafe void Sub(UInt32[] arr_a, UInt32 b) {
            fixed (UInt32* va0 = arr_a) {
                for (uint i = 0u, length = (uint)arr_a.Length; i < length && b > 0u; i++) {
                    UInt32 a = va0[i];

                    va0[i] = unchecked(a - b);
                    b = (va0[i] <= a) ? 0u : 1u;
                }
            }

            if (b > 0u) {
                throw new OverflowException();
            }
        }
        
        /// <summary>Operate uint32 array a -= b</summary>
        public static unsafe void Sub(UInt32[] arr_a, UInt64 b) {
            if (b == 0uL) {
                return;
            }

            (UInt32 b_hi, UInt32 b_lo) = Unpack(b);

            fixed (UInt32* va0 = arr_a) {
                for (uint i = 0u, length = (uint)arr_a.Length; i < length && b_lo > 0u; i++) {
                    UInt32 a = va0[i];

                    va0[i] = unchecked(a - b_lo);
                    b_lo = (va0[i] <= a) ? 0u : 1u;
                }
                for (uint i = 1u, length = (uint)arr_a.Length; i < length && b_hi > 0u; i++) {
                    UInt32 a = va0[i];

                    va0[i] = unchecked(a - b_hi);
                    b_hi = (va0[i] <= a) ? 0u : 1u;
                }
            }

            if ((b_lo | b_hi) > 0u) {
                throw new OverflowException();
            }
        }

        /// <summary>Operate uint32 array a -= b</summary>
        public static unsafe void Sub(uint offset, UInt32[] arr_a, UInt32 b) {
            fixed (UInt32* va0 = arr_a) {
                for (uint i = offset, length = (uint)arr_a.Length; i < length && b > 0u; i++) {
                    UInt32 a = va0[i];

                    va0[i] = unchecked(a - b);
                    b = (va0[i] <= a) ? 0u : 1u;
                }
            }

            if (b > 0u) {
                throw new OverflowException();
            }
        }
    }
}
﻿using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using static System.Runtime.Intrinsics.X86.Avx;
using static System.Runtime.Intrinsics.X86.Avx2;

namespace AvxUInt {
    internal static partial class UIntUtil {

        /// <summary>Comparate uint32 array a == b</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool Equal(uint length, UInt32[] arr_a, UInt32[] arr_b) {
#if DEBUG
            Debug<ArgumentException>.Assert(length == arr_a.Length);
            Debug<ArgumentException>.Assert(length == arr_b.Length);
#endif

            fixed (UInt32* va0 = arr_a, vb0 = arr_b) {
                UInt32* va = va0, vb = vb0;

                Vector256<UInt32> a0, a1, a2, a3, b0, b1, b2, b3;

                uint r = length;
                while (r >= MM256UInt32s * 4) {
                    (a0, a1, a2, a3) = LoadVector256X4(va);
                    (b0, b1, b2, b3) = LoadVector256X4(vb);

                    uint flag =
                        ((uint)MoveMask(CompareEqual(a0, b0).AsSingle())) |
                        ((uint)MoveMask(CompareEqual(a1, b1).AsSingle()) << 8) |
                        ((uint)MoveMask(CompareEqual(a2, b2).AsSingle()) << 16) |
                        ((uint)MoveMask(CompareEqual(a3, b3).AsSingle()) << 24);

                    if (flag != ~0u) {
                        return false;
                    }

                    va += MM256UInt32s * 4;
                    vb += MM256UInt32s * 4;
                    r -= MM256UInt32s * 4;
                }
                if (r >= MM256UInt32s * 2) {
                    (a0, a1) = LoadVector256X2(va);
                    (b0, b1) = LoadVector256X2(vb);

                    uint flag =
                        ((uint)MoveMask(CompareEqual(a0, b0).AsSingle())) |
                        ((uint)MoveMask(CompareEqual(a1, b1).AsSingle()) << 8);

                    if (flag != 65535u) {
                        return false;
                    }

                    va += MM256UInt32s * 2;
                    vb += MM256UInt32s * 2;
                    r -= MM256UInt32s * 2;
                }
                if (r >= MM256UInt32s) {
                    a0 = LoadVector256(va);
                    b0 = LoadVector256(vb);

                    uint flag =
                        ((uint)MoveMask(CompareEqual(a0, b0).AsSingle()));

                    if (flag != 255u) {
                        return false;
                    }

                    r -= MM256UInt32s;
                }
                if (r > 0) {
                    Vector256<UInt32> mask = Mask256.LSV(r);

                    a0 = MaskLoad(va0, mask);
                    b0 = MaskLoad(vb0, mask);

                    uint flag =
                        ((uint)MoveMask(CompareEqual(a0, b0).AsSingle()));

                    if (flag != 255u) {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>Comparate uint32 array a &lt;= b</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool LessThanOrEqual(uint length, UInt32[] arr_a, UInt32[] arr_b) {
#if DEBUG
            Debug<ArgumentException>.Assert(length == arr_a.Length);
            Debug<ArgumentException>.Assert(length == arr_b.Length);
#endif

            fixed (UInt32* va0 = arr_a, vb0 = arr_b) {
                UInt32* va = va0 + length, vb = vb0 + length;

                Vector256<UInt32> a0, a1, a2, a3, b0, b1, b2, b3;

                uint r = length;
                while (r >= MM256UInt32s * 4) {
                    (a0, a1, a2, a3) = LoadVector256X4(va - MM256UInt32s * 4);
                    (b0, b1, b2, b3) = LoadVector256X4(vb - MM256UInt32s * 4);

                    uint gt_flag =
                        ((uint)MoveMask(CompareGreaterThan(a0, b0).AsSingle())) |
                        ((uint)MoveMask(CompareGreaterThan(a1, b1).AsSingle()) << 8) |
                        ((uint)MoveMask(CompareGreaterThan(a2, b2).AsSingle()) << 16) |
                        ((uint)MoveMask(CompareGreaterThan(a3, b3).AsSingle()) << 24);

                    uint lt_flag =
                        ((uint)MoveMask(CompareLessThan(a0, b0).AsSingle())) |
                        ((uint)MoveMask(CompareLessThan(a1, b1).AsSingle()) << 8) |
                        ((uint)MoveMask(CompareLessThan(a2, b2).AsSingle()) << 16) |
                        ((uint)MoveMask(CompareLessThan(a3, b3).AsSingle()) << 24);

                    if ((gt_flag | lt_flag) != 0u) {
                        return gt_flag < lt_flag;
                    }

                    va -= MM256UInt32s * 4;
                    vb -= MM256UInt32s * 4;
                    r -= MM256UInt32s * 4;
                }
                if (r >= MM256UInt32s * 2) {
                    (a0, a1) = LoadVector256X2(va - MM256UInt32s * 2);
                    (b0, b1) = LoadVector256X2(vb - MM256UInt32s * 2);

                    uint gt_flag =
                        ((uint)MoveMask(CompareGreaterThan(a0, b0).AsSingle())) |
                        ((uint)MoveMask(CompareGreaterThan(a1, b1).AsSingle()) << 8);

                    uint lt_flag =
                        ((uint)MoveMask(CompareLessThan(a0, b0).AsSingle())) |
                        ((uint)MoveMask(CompareLessThan(a1, b1).AsSingle()) << 8);

                    if ((gt_flag | lt_flag) != 0u) {
                        return gt_flag < lt_flag;
                    }

                    va -= MM256UInt32s * 2;
                    vb -= MM256UInt32s * 2;
                    r -= MM256UInt32s * 2;
                }
                if (r >= MM256UInt32s) {
                    a0 = LoadVector256(va - MM256UInt32s);
                    b0 = LoadVector256(vb - MM256UInt32s);

                    uint gt_flag =
                        ((uint)MoveMask(CompareGreaterThan(a0, b0).AsSingle()));

                    uint lt_flag =
                        ((uint)MoveMask(CompareLessThan(a0, b0).AsSingle()));

                    if ((gt_flag | lt_flag) != 0u) {
                        return gt_flag < lt_flag;
                    }

                    r -= MM256UInt32s;
                }
                if (r > 0) {
                    Vector256<UInt32> mask = Mask256.LSV(r);

                    a0 = MaskLoad(va0, mask);
                    b0 = MaskLoad(vb0, mask);

                    uint gt_flag =
                        ((uint)MoveMask(CompareGreaterThan(a0, b0).AsSingle()));

                    uint lt_flag =
                        ((uint)MoveMask(CompareLessThan(a0, b0).AsSingle()));

                    return gt_flag <= lt_flag;
                }
            }

            return true;
        }

        /// <summary>Comparate uint32 array a &gt;= b</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool GreaterThanOrEqual(uint length, UInt32[] arr_a, UInt32[] arr_b) {
#if DEBUG
            Debug<ArgumentException>.Assert(length == arr_a.Length);
            Debug<ArgumentException>.Assert(length == arr_b.Length);
#endif

            fixed (UInt32* va0 = arr_a, vb0 = arr_b) {
                UInt32* va = va0 + length, vb = vb0 + length;

                Vector256<UInt32> a0, a1, a2, a3, b0, b1, b2, b3;

                uint r = length;
                while (r >= MM256UInt32s * 4) {
                    (a0, a1, a2, a3) = LoadVector256X4(va - MM256UInt32s * 4);
                    (b0, b1, b2, b3) = LoadVector256X4(vb - MM256UInt32s * 4);

                    uint gt_flag =
                        ((uint)MoveMask(CompareGreaterThan(a0, b0).AsSingle())) |
                        ((uint)MoveMask(CompareGreaterThan(a1, b1).AsSingle()) << 8) |
                        ((uint)MoveMask(CompareGreaterThan(a2, b2).AsSingle()) << 16) |
                        ((uint)MoveMask(CompareGreaterThan(a3, b3).AsSingle()) << 24);

                    uint lt_flag =
                        ((uint)MoveMask(CompareLessThan(a0, b0).AsSingle())) |
                        ((uint)MoveMask(CompareLessThan(a1, b1).AsSingle()) << 8) |
                        ((uint)MoveMask(CompareLessThan(a2, b2).AsSingle()) << 16) |
                        ((uint)MoveMask(CompareLessThan(a3, b3).AsSingle()) << 24);

                    if ((gt_flag | lt_flag) != 0u) {
                        return gt_flag > lt_flag;
                    }

                    va -= MM256UInt32s * 4;
                    vb -= MM256UInt32s * 4;
                    r -= MM256UInt32s * 4;
                }
                if (r >= MM256UInt32s * 2) {
                    (a0, a1) = LoadVector256X2(va - MM256UInt32s * 2);
                    (b0, b1) = LoadVector256X2(vb - MM256UInt32s * 2);

                    uint gt_flag =
                        ((uint)MoveMask(CompareGreaterThan(a0, b0).AsSingle())) |
                        ((uint)MoveMask(CompareGreaterThan(a1, b1).AsSingle()) << 8);

                    uint lt_flag =
                        ((uint)MoveMask(CompareLessThan(a0, b0).AsSingle())) |
                        ((uint)MoveMask(CompareLessThan(a1, b1).AsSingle()) << 8);

                    if ((gt_flag | lt_flag) != 0u) {
                        return gt_flag > lt_flag;
                    }

                    va -= MM256UInt32s * 2;
                    vb -= MM256UInt32s * 2;
                    r -= MM256UInt32s * 2;
                }
                if (r >= MM256UInt32s) {
                    a0 = LoadVector256(va - MM256UInt32s);
                    b0 = LoadVector256(vb - MM256UInt32s);

                    uint gt_flag =
                        ((uint)MoveMask(CompareGreaterThan(a0, b0).AsSingle()));

                    uint lt_flag =
                        ((uint)MoveMask(CompareLessThan(a0, b0).AsSingle()));

                    if ((gt_flag | lt_flag) != 0u) {
                        return gt_flag > lt_flag;
                    }

                    r -= MM256UInt32s;
                }
                if (r > 0) {
                    Vector256<UInt32> mask = Mask256.LSV(r);

                    a0 = MaskLoad(va0, mask);
                    b0 = MaskLoad(vb0, mask);

                    uint gt_flag =
                        ((uint)MoveMask(CompareGreaterThan(a0, b0).AsSingle()));

                    uint lt_flag =
                        ((uint)MoveMask(CompareLessThan(a0, b0).AsSingle()));

                    return gt_flag >= lt_flag;
                }
            }

            return true;
        }

        /// <summary>Judge uint32 array is zero</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool IsZero(UInt32[] arr) {
            fixed (UInt32* v0 = arr) {
                UInt32* v = v0;

                Vector256<UInt32> x0, x1, x2, x3;

                uint r = (uint)arr.Length;
                while (r >= MM256UInt32s * 4) {
                    (x0, x1, x2, x3) = LoadVector256X4(v);

                    uint flag =
                        ((uint)MoveMask(CompareEqual(x0, Vector256<UInt32>.Zero).AsSingle())) |
                        ((uint)MoveMask(CompareEqual(x1, Vector256<UInt32>.Zero).AsSingle()) << 8) |
                        ((uint)MoveMask(CompareEqual(x2, Vector256<UInt32>.Zero).AsSingle()) << 16) |
                        ((uint)MoveMask(CompareEqual(x3, Vector256<UInt32>.Zero).AsSingle()) << 24);

                    if (flag != ~0u) {
                        return false;
                    }

                    v += MM256UInt32s * 4;
                    r -= MM256UInt32s * 4;
                }
                if (r >= MM256UInt32s * 2) {
                    (x0, x1) = LoadVector256X2(v);

                    uint flag =
                        ((uint)MoveMask(CompareEqual(x0, Vector256<UInt32>.Zero).AsSingle())) |
                        ((uint)MoveMask(CompareEqual(x1, Vector256<UInt32>.Zero).AsSingle()) << 8);

                    if (flag != 65535u) {
                        return false;
                    }

                    v += MM256UInt32s * 2;
                    r -= MM256UInt32s * 2;
                }
                if (r >= MM256UInt32s) {
                    x0 = LoadVector256(v);

                    uint flag =
                        ((uint)MoveMask(CompareEqual(x0, Vector256<UInt32>.Zero).AsSingle()));

                    if (flag != 255u) {
                        return false;
                    }

                    r -= MM256UInt32s;
                }
                if (r > 0) {
                    x0 = MaskLoad(v0, Mask256.LSV(r));

                    uint flag =
                        ((uint)MoveMask(CompareEqual(x0, Vector256<UInt32>.Zero).AsSingle()));

                    if (flag != 255u) {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>Judge uint32 array is full</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe bool IsFull(UInt32[] arr) {
            fixed (UInt32* v0 = arr) {
                UInt32* v = v0;
                Vector256<UInt32> fulls = Vector256.Create(~0u);

                Vector256<UInt32> x0, x1, x2, x3;

                uint r = (uint)arr.Length;
                while (r >= MM256UInt32s * 4) {
                    (x0, x1, x2, x3) = LoadVector256X4(v);

                    uint flag =
                        ((uint)MoveMask(CompareEqual(x0, fulls).AsSingle())) |
                        ((uint)MoveMask(CompareEqual(x1, fulls).AsSingle()) << 8) |
                        ((uint)MoveMask(CompareEqual(x2, fulls).AsSingle()) << 16) |
                        ((uint)MoveMask(CompareEqual(x3, fulls).AsSingle()) << 24);

                    if (flag != ~0u) {
                        return false;
                    }

                    v += MM256UInt32s * 4;
                    r -= MM256UInt32s * 4;
                }
                if (r >= MM256UInt32s * 2) {
                    (x0, x1) = LoadVector256X2(v);

                    uint flag =
                        ((uint)MoveMask(CompareEqual(x0, fulls).AsSingle())) |
                        ((uint)MoveMask(CompareEqual(x1, fulls).AsSingle()) << 8);

                    if (flag != 65535u) {
                        return false;
                    }

                    v += MM256UInt32s * 2;
                    r -= MM256UInt32s * 2;
                }
                if (r >= MM256UInt32s) {
                    x0 = LoadVector256(v);

                    uint flag =
                        ((uint)MoveMask(CompareEqual(x0, fulls).AsSingle()));

                    if (flag != 255u) {
                        return false;
                    }

                    r -= MM256UInt32s;
                }
                if (r > 0) {
                    x0 = MaskLoad(v0, Mask256.LSV(r));

                    uint flag =
                        ((uint)MoveMask(CompareEqual(x0, fulls).AsSingle()));

                    if (flag != (255u >> (int)(MM256UInt32s - r))) {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>Count leading match bits</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int MatchBits(uint length, UInt32[] arr_a, UInt32[] arr_b) {
#if DEBUG
            Debug<ArgumentException>.Assert(length == arr_a.Length);
            Debug<ArgumentException>.Assert(length == arr_b.Length);
#endif

            int matches = 0;

            fixed (UInt32* va0 = arr_a, vb0 = arr_b) {
                for (int i = arr_a.Length - 1; i >= 0; i--) {
                    UInt32 xor = va0[i] ^ vb0[i];

                    if (xor == 0u) {
                        matches += UInt32Bits;
                    }
                    else {
                        matches += LeadingZeroCount(xor);
                        break;
                    }
                }
            }

            return matches;
        }
    }
}
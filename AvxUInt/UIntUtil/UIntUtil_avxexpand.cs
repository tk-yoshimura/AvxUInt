﻿using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using static System.Runtime.Intrinsics.X86.Avx;
using static System.Runtime.Intrinsics.X86.Avx2;

namespace AvxUInt {
    internal static partial class UIntUtil {
        /// <summary>Comparate uint32 array a &gt; b</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector256<UInt32> CompareGreaterThan(Vector256<UInt32> x, Vector256<UInt32> y) {
            Vector256<UInt32> gt = Xor(CompareEqual(Max(x, y), y), Vector256.Create(~0u));

            return gt;
        }

        /// <summary>Comparate uint32 array a &lt; b</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector256<UInt32> CompareLessThan(Vector256<UInt32> x, Vector256<UInt32> y) {
            Vector256<UInt32> lt = Xor(CompareEqual(Max(y, x), x), Vector256.Create(~0u));

            return lt;
        }

        /// <summary>Comparate uint32 array a != b</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector256<UInt32> CompareNotEqual(Vector256<UInt32> x, Vector256<UInt32> y) {
            Vector256<UInt32> neq = Xor(CompareEqual(x, y), Vector256.Create(~0u));

            return neq;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Vector256<UInt32> MaskLoad(UInt32* ptr, Vector256<UInt32> mask, UInt32* ptr0, int arr_length) {
#if DEBUG
            uint mask_bits = (uint)MoveMask(mask.AsSingle());
            uint mask_length = (mask_bits > 0) ? MM256UInt32s - Lzcnt.LeadingZeroCount(mask_bits << ShiftIDX3) : 0;

            Debug<AccessViolationException>.Assert(ptr >= ptr0 && ptr - ptr0 + mask_length <= arr_length);
#endif

            Vector256<UInt32> v0 = Avx2.MaskLoad(ptr, mask);

            return v0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe Vector256<UInt32> Load(UInt32* ptr, UInt32* ptr0, int arr_length) {
#if DEBUG
            Debug<AccessViolationException>.Assert(ptr >= ptr0 && ptr - ptr0 + MM256UInt32s <= arr_length);
#endif
            Vector256<UInt32> v0 = Avx2.LoadVector256(ptr);

            return v0;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe (Vector256<UInt32> v0, Vector256<UInt32> v1) LoadX2(UInt32* ptr, UInt32* ptr0, int arr_length) {
#if DEBUG
            Debug<AccessViolationException>.Assert(ptr >= ptr0 && ptr - ptr0 + MM256UInt32s * 2 <= arr_length);
#endif
            Vector256<UInt32> v0 = Avx2.LoadVector256(ptr);
            Vector256<UInt32> v1 = Avx2.LoadVector256(ptr + MM256UInt32s);

            return (v0, v1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe (Vector256<UInt32> v0, Vector256<UInt32> v1, Vector256<UInt32> v2, Vector256<UInt32> v3) LoadX4(UInt32* ptr, UInt32* ptr0, int arr_length) {
#if DEBUG
            Debug<AccessViolationException>.Assert(ptr >= ptr0 && ptr - ptr0 + MM256UInt32s * 4 <= arr_length);
#endif      
            Vector256<UInt32> v0 = Avx2.LoadVector256(ptr);
            Vector256<UInt32> v1 = Avx2.LoadVector256(ptr + MM256UInt32s);
            Vector256<UInt32> v2 = Avx2.LoadVector256(ptr + MM256UInt32s * 2);
            Vector256<UInt32> v3 = Avx2.LoadVector256(ptr + MM256UInt32s * 3);

            return (v0, v1, v2, v3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void MaskStore(UInt32* ptr, Vector256<UInt32> v0, Vector256<UInt32> mask, UInt32* ptr0, int arr_length) {
#if DEBUG
            uint mask_bits = (uint)MoveMask(mask.AsSingle());
            uint mask_length = (mask_bits > 0) ? MM256UInt32s - Lzcnt.LeadingZeroCount(mask_bits << ShiftIDX3) : 0;

            Debug<AccessViolationException>.Assert(ptr >= ptr0 && ptr - ptr0 + mask_length <= arr_length);
#endif
            Avx2.MaskStore(ptr, mask, v0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void Store(UInt32* ptr, Vector256<UInt32> v0, UInt32* ptr0, int arr_length) {
#if DEBUG
            Debug<AccessViolationException>.Assert(ptr >= ptr0 && ptr - ptr0 + MM256UInt32s <= arr_length);
#endif
            Avx2.Store(ptr, v0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void StoreX2(UInt32* ptr, Vector256<UInt32> v0, Vector256<UInt32> v1, UInt32* ptr0, int arr_length) {
#if DEBUG
            Debug<AccessViolationException>.Assert(ptr >= ptr0 && ptr - ptr0 + MM256UInt32s * 2 <= arr_length);
#endif
            Avx2.Store(ptr, v0);
            Avx2.Store(ptr + MM256UInt32s, v1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void StoreX4(UInt32* ptr, Vector256<UInt32> v0, Vector256<UInt32> v1, Vector256<UInt32> v2, Vector256<UInt32> v3, UInt32* ptr0, int arr_length) {
#if DEBUG
            Debug<AccessViolationException>.Assert(ptr >= ptr0 && ptr - ptr0 + MM256UInt32s * 4 <= arr_length);
#endif
            Avx2.Store(ptr, v0);
            Avx2.Store(ptr + MM256UInt32s, v1);
            Avx2.Store(ptr + MM256UInt32s * 2, v2);
            Avx2.Store(ptr + MM256UInt32s * 3, v3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsAllZero(Vector256<UInt32> x) => TestZ(x, x);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Vector256<UInt32> ret, Vector256<UInt32> carry) Add(Vector256<UInt32> a, Vector256<UInt32> b) {
            Vector256<UInt32> ret = Avx2.Add(a, b);
            Vector256<UInt32> carry = ShiftRightLogical(CompareGreaterThan(a, ret), UInt32Bits - 1);

            return (ret, carry);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Vector256<UInt32> ret, Vector256<UInt32> carry) Sub(Vector256<UInt32> a, Vector256<UInt32> b) {
            Vector256<UInt32> ret = Avx2.Subtract(a, b);
            Vector256<UInt32> carry = ShiftRightLogical(CompareLessThan(a, ret), UInt32Bits - 1);

            return (ret, carry);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Vector256<UInt32> r0, UInt32 carry)
            CarryShift(Vector256<UInt32> v0, UInt32 carry) {

            Vector256<UInt32> perm = Vector256.Create(7u, 0u, 1u, 2u, 3u, 4u, 5u, 6u);
            Vector256<UInt32> u0 = PermuteVar8x32(v0, perm);

            Vector256<UInt32> r0 = Blend(Vector256<UInt32>.Zero, u0, 0b11111110);

            carry += u0.GetElement(0);

            return (r0, carry);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Vector256<UInt32> r0, Vector256<UInt32> r1, UInt32 carry)
            CarryShiftX2(Vector256<UInt32> v0, Vector256<UInt32> v1, UInt32 carry) {

            Vector256<UInt32> perm = Vector256.Create(7u, 0u, 1u, 2u, 3u, 4u, 5u, 6u);
            Vector256<UInt32> u0 = PermuteVar8x32(v0, perm);
            Vector256<UInt32> u1 = PermuteVar8x32(v1, perm);

            Vector256<UInt32> r0 = Blend(Vector256<UInt32>.Zero, u0, 0b11111110);
            Vector256<UInt32> r1 = Blend(u0, u1, 0b11111110);

            carry += u1.GetElement(0);

            return (r0, r1, carry);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Vector256<UInt32> r0, Vector256<UInt32> r1, Vector256<UInt32> r2, Vector256<UInt32> r3, UInt32 carry)
            CarryShiftX4(Vector256<UInt32> v0, Vector256<UInt32> v1, Vector256<UInt32> v2, Vector256<UInt32> v3, UInt32 carry) {

            Vector256<UInt32> perm = Vector256.Create(7u, 0u, 1u, 2u, 3u, 4u, 5u, 6u);
            Vector256<UInt32> u0 = PermuteVar8x32(v0, perm);
            Vector256<UInt32> u1 = PermuteVar8x32(v1, perm);
            Vector256<UInt32> u2 = PermuteVar8x32(v2, perm);
            Vector256<UInt32> u3 = PermuteVar8x32(v3, perm);

            Vector256<UInt32> r0 = Blend(Vector256<UInt32>.Zero, u0, 0b11111110);
            Vector256<UInt32> r1 = Blend(u0, u1, 0b11111110);
            Vector256<UInt32> r2 = Blend(u1, u2, 0b11111110);
            Vector256<UInt32> r3 = Blend(u2, u3, 0b11111110);

            carry += u3.GetElement(0);

            return (r0, r1, r2, r3, carry);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static (Vector256<UInt32> ret, Vector256<UInt32> carry) Mul(Vector256<UInt32> a, Vector256<UInt32> b) {
            Vector256<UInt32> zero = Vector256<UInt32>.Zero;

            Vector256<UInt32> al = UnpackLow(a, zero), ah = UnpackHigh(a, zero);

            Vector256<UInt32> a0 = Permute2x128(al, ah, 0b00100000), a1 = Permute2x128(al, ah, 0b00110001);

            Vector256<UInt32> x0 = Avx2.Multiply(a0, b).AsUInt32();
            Vector256<UInt32> x1 = Avx2.Multiply(a1, b).AsUInt32();

            Vector256<UInt32> r = Permute4x64(Shuffle(x0.AsSingle(), x1.AsSingle(), 0b10001000).AsDouble(), 0b11011000).AsUInt32();
            Vector256<UInt32> c = Permute4x64(Shuffle(x0.AsSingle(), x1.AsSingle(), 0b11011101).AsDouble(), 0b11011000).AsUInt32();

            return (r, c);
        }
    }
}

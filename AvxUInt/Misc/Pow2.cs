﻿namespace AvxUInt {
    public interface IConstant {
        int Value { get; }
    }

    internal struct Plus1<N> : IConstant where N : struct, IConstant {
        public readonly int Value => checked(default(N).Value + 1);
    }

    internal struct Plus2<N> : IConstant where N : struct, IConstant {
        public readonly int Value => checked(default(N).Value + 2);
    }

    internal struct Plus4<N> : IConstant where N : struct, IConstant {
        public readonly int Value => checked(default(N).Value + 4);
    }

    internal struct Plus8<N> : IConstant where N : struct, IConstant {
        public readonly int Value => checked(default(N).Value + 8);
    }

    internal struct Plus16<N> : IConstant where N : struct, IConstant {
        public readonly int Value => checked(default(N).Value + 16);
    }

    internal struct Plus32<N> : IConstant where N : struct, IConstant {
        public readonly int Value => checked(default(N).Value + 32);
    }

    internal struct Plus64<N> : IConstant where N : struct, IConstant {
        public readonly int Value => checked(default(N).Value + 64);
    }

    public struct Double<N> : IConstant where N : struct, IConstant {
        public readonly int Value => checked(default(N).Value * 2);
    }

    public static class Pow2 {

        public struct N4 : IConstant { public readonly int Value => 4; }
        public struct N8 : IConstant { public readonly int Value => 8; }
        public struct N16 : IConstant { public readonly int Value => 16; }
        public struct N32 : IConstant { public readonly int Value => 32; }
        public struct N64 : IConstant { public readonly int Value => 64; }
        public struct N128 : IConstant { public readonly int Value => 128; }
        public struct N256 : IConstant { public readonly int Value => 256; }
        public struct N512 : IConstant { public readonly int Value => 512; }
        public struct N1024 : IConstant { public readonly int Value => 1024; }
    }
}

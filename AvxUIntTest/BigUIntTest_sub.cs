using AvxUInt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace AvxUIntTest {
    public static class SubTests<N> where N : struct, IConstant {
        public static void SubTest() {
            Random random = new(1234);

            List<(BigUInt<N> b, BigInteger n)> vs = new();

            int length = default(N).Value;
            for (int i = 1; i <= BigUInt<N>.Bits; i += 15) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<N>.Length, i);
                UInt32[] bits_swapbit = (UInt32[])bits.Clone();
                bits_swapbit[random.Next(length)] ^= 1u << random.Next(UIntUtil.UInt32Bits);
                UInt32[] bits_reverse = bits_swapbit.Reverse().ToArray();

                BigUInt<N> b = new(bits, enable_clone: false);
                BigUInt<N> b_swapbit = new(bits_swapbit, enable_clone: false);
                BigUInt<N> b_reverse = new(bits_reverse, enable_clone: false);

                vs.Add((b, (BigInteger)b));
                vs.Add((b_swapbit, (BigInteger)b_swapbit));
                vs.Add((b_swapbit, (BigInteger)b_swapbit));
                vs.Add((b_reverse, (BigInteger)b_reverse));
            }
            {
                BigUInt<N> v = BigUInt<N>.Full;
                while (v > 0) {
                    vs.Add((v, v));
                    v >>= 15;
                }
            }

            vs.Add((1, 1));

            Console.WriteLine($"length={BigUInt<N>.Length}");

            int normal_passes = 0, overflow_passes = 0;

            foreach ((BigUInt<N> v1, BigInteger n1) in vs) {
                foreach ((BigUInt<N> v2, BigInteger n2) in vs) {
                    BigInteger n = n1 - n2;

                    if (n >= 0) {
                        NormalTest(n, v1, v2, n1, n2);

                        normal_passes++;
                    }
                    else {
                        Assert.ThrowsException<OverflowException>(() => {
                            _ = v1 - v2;
                        });

                        overflow_passes++;
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(overflow_passes)}: {overflow_passes}");
        }

        public static void SubFullTest() {
            List<(BigUInt<N> v1, BigUInt<N> v2, BigInteger n1, BigInteger n2)> vs = new();
            BigInteger maxn = BigUInt<N>.Full, v = maxn / 2;

            while (v > 0) {
                BigInteger u = maxn - v;

                BigInteger v0 = v - 1, v1 = v, v2 = v + 1;
                BigInteger u0 = u - 1, u1 = u, u2 = u + 1;

                vs.Add((BigUInt<N>.Parse(v0.ToString()), BigUInt<N>.Parse(u0.ToString()), v0, u0));
                vs.Add((BigUInt<N>.Parse(v0.ToString()), BigUInt<N>.Parse(u1.ToString()), v0, u1));
                vs.Add((BigUInt<N>.Parse(v0.ToString()), BigUInt<N>.Parse(u2.ToString()), v0, u2));

                vs.Add((BigUInt<N>.Parse(v1.ToString()), BigUInt<N>.Parse(u0.ToString()), v1, u0));
                vs.Add((BigUInt<N>.Parse(v1.ToString()), BigUInt<N>.Parse(u1.ToString()), v1, u1));
                vs.Add((BigUInt<N>.Parse(v1.ToString()), BigUInt<N>.Parse(u2.ToString()), v1, u2));

                vs.Add((BigUInt<N>.Parse(v2.ToString()), BigUInt<N>.Parse(u0.ToString()), v2, u0));
                vs.Add((BigUInt<N>.Parse(v2.ToString()), BigUInt<N>.Parse(u1.ToString()), v2, u1));
                vs.Add((BigUInt<N>.Parse(v2.ToString()), BigUInt<N>.Parse(u2.ToString()), v2, u2));

                v >>= 1;
            }

            Console.WriteLine($"length={BigUInt<N>.Length}");

            int normal_passes = 0, overflow_passes = 0;

            foreach ((BigUInt<N> v1, BigUInt<N> v2, BigInteger n1, BigInteger n2) in vs) {
                BigInteger n = n1 - n2;

                if (n >= 0) {
                    NormalTest(n, v1, v2, n1, n2);

                    normal_passes++;
                }
                else {
                    Assert.ThrowsException<OverflowException>(() => {
                        _ = v1 - v2;
                    });

                    overflow_passes++;
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(overflow_passes)}: {overflow_passes}");
        }

        public static void SubSparseTest() {
            Random random = new(1234);

            List<(BigUInt<N> b, BigInteger n)> vs = new();

            int length = default(N).Value;
            for (int i = 0; i < 100; i++) {
                UInt32[] bits = (new UInt32[length]).Select(_ => random.Next(4) > 1 ? 0u : 1u << random.Next(32)).ToArray();

                BigUInt<N> b = new(bits, enable_clone: false);

                vs.Add((b, (BigInteger)b));
            }
            for (int i = 0; i < 100; i++) {
                UInt32[] bits = (new UInt32[length]).Select(_ => random.Next(8) > 1 ? 0u : 1u << random.Next(32)).ToArray();

                BigUInt<N> b = new(bits, enable_clone: false);

                vs.Add((b, (BigInteger)b));
            }
            {
                BigUInt<N> v = BigUInt<N>.Full;
                while (v > 0) {
                    vs.Add((v, v));
                    v >>= 15;
                }
            }

            vs.Add((1, 1));

            Console.WriteLine($"length={BigUInt<N>.Length}");

            int normal_passes = 0, overflow_passes = 0;

            foreach ((BigUInt<N> v1, BigInteger n1) in vs) {
                foreach ((BigUInt<N> v2, BigInteger n2) in vs) {
                    BigInteger n = n1 - n2;

                    if (n >= 0) {
                        NormalTest(n, v1, v2, n1, n2);

                        normal_passes++;
                    }
                    else {
                        Assert.ThrowsException<OverflowException>(() => {
                            _ = v1 - v2;
                        });

                        overflow_passes++;
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(overflow_passes)}: {overflow_passes}");
        }

        public static void SubBlockTest() {
            Random random = new(1234);

            List<(BigUInt<N> b, BigInteger n)> vs = new();

            int length = default(N).Value;
            for (int i = 0; i < 100; i++) {
                UInt32[] bits = (new UInt32[length]).Select(_ => random.Next(4) > 1 ? 0u : ~0u).ToArray();

                BigUInt<N> b = new(bits, enable_clone: false);

                vs.Add((b, (BigInteger)b));
            }
            for (int i = 0; i < 100; i++) {
                UInt32[] bits = (new UInt32[length]).Select(_ => random.Next(8) > 1 ? 0u : ~0u).ToArray();

                BigUInt<N> b = new(bits, enable_clone: false);

                vs.Add((b, (BigInteger)b));
            }
            {
                BigUInt<N> v = BigUInt<N>.Full;
                while (v > 0) {
                    vs.Add((v, v));
                    v >>= 15;
                }
            }

            vs.Add((1, 1));

            Console.WriteLine($"length={BigUInt<N>.Length}");

            int normal_passes = 0, overflow_passes = 0;

            foreach ((BigUInt<N> v1, BigInteger n1) in vs) {
                foreach ((BigUInt<N> v2, BigInteger n2) in vs) {
                    BigInteger n = n1 - n2;

                    if (n >= 0) {
                        NormalTest(n, v1, v2, n1, n2);

                        normal_passes++;
                    }
                    else {
                        Assert.ThrowsException<OverflowException>(() => {
                            _ = v1 - v2;
                        });

                        overflow_passes++;
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(overflow_passes)}: {overflow_passes}");
        }

        public static void SubCarryTest() {
            BigUInt<N> v1 = new(Enumerable.Repeat(0u, BigUInt<N>.Length - 1).Concat(new UInt32[] { 0x80000000u }).ToArray());
            BigUInt<N> v2 = new(Enumerable.Repeat(~0u, BigUInt<N>.Length - 1).Concat(new UInt32[] { 0x7FFFFFFFu }).ToArray());
            BigUInt<N> v3 = new((new UInt32[] { 1u }).Concat(Enumerable.Repeat(0u, BigUInt<N>.Length - 2)).Concat(new UInt32[] { 0x80000000u }).ToArray());
            BigUInt<N> v4 = new((new UInt32[] { 1u }).Concat(Enumerable.Repeat(0u, BigUInt<N>.Length - 2)).Concat(new UInt32[] { 0x7FFFFFFFu }).ToArray());

            Assert.AreEqual(1u, v1 - v2);
            Assert.AreEqual(1u, v3 - v1);
            Assert.AreEqual(new(Enumerable.Repeat(~0u, BigUInt<N>.Length - 1).Concat(new UInt32[] { 0u }).ToArray()), v1 - v4);

            Assert.ThrowsException<OverflowException>(() => {
                _ = v2 - v1;
            });

            Assert.ThrowsException<OverflowException>(() => {
                _ = v1 - v3;
            });

            Assert.ThrowsException<OverflowException>(() => {
                _ = v4 - v1;
            });
        }

        public static void SubShiftTest() {
            BigUInt<N> v = new((new UInt32[BigUInt<N>.Length]).Select((_, idx) => ((idx & 1) == 0) ? ~0u : 0u).ToArray());
            UInt64 ui = 0x9234567893568135uL;

            Console.WriteLine($"length={BigUInt<N>.Length}");

            int normal_passes = 0, overflow_passes = 0;

            for (int sft = -40; sft < BigUInt<N>.Bits + 32; sft++) {
                BigInteger n = (BigInteger)v - ((sft >= 0) ? (BigInteger)ui << sft : (BigInteger)ui >> (-sft));

                if (n >= 0) {
                    BigUInt<N> u = BigUInt<N>.Sub(v, ui, sft);

                    Assert.AreEqual(n, (BigInteger)u, $"{sft}");

                    normal_passes++;
                }
                else {
                    Assert.ThrowsException<OverflowException>(() => {
                        _ = BigUInt<N>.Sub(v, ui, sft);
                    }, $"{sft}");

                    overflow_passes++;
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(overflow_passes)}: {overflow_passes}");
        }

        private static void NormalTest(BigInteger n, BigUInt<N> v1, BigUInt<N> v2, BigInteger n1, BigInteger n2) {
            Assert.AreEqual(n, (BigInteger)(v1 - v2), $"{n1}-{n2}");

            if (v1.Digits <= 2) {
                Assert.AreEqual(n, (BigInteger)(UIntUtil.Pack(v1.Value[1], v1.Value[0]) - v2), $"{n1}-{n2}");
            }

            if (v1.Digits <= 1) {
                Assert.AreEqual(n, (BigInteger)(v1.Value[0] - v2), $"{n1}-{n2}");
            }

            if (v2.Digits <= 2) {
                Assert.AreEqual(n, (BigInteger)(v1 - UIntUtil.Pack(v2.Value[1], v2.Value[0])), $"{n1}-{n2}");
            }

            if (v2.Digits <= 1) {
                Assert.AreEqual(n, (BigInteger)(v1 - v2.Value[0]), $"{n1}-{n2}");
            }
        }
    }

    [TestClass]
    public class SubTests {
        [TestMethod]
        public void SubTest() {
            SubTests<N4>.SubTest();
            SubTests<N5>.SubTest();
            SubTests<N6>.SubTest();
            SubTests<N7>.SubTest();
            SubTests<N8>.SubTest();
            SubTests<N9>.SubTest();
            SubTests<N10>.SubTest();
            SubTests<N11>.SubTest();
            SubTests<N12>.SubTest();
            SubTests<N13>.SubTest();
            SubTests<N14>.SubTest();
            SubTests<N15>.SubTest();
            SubTests<N16>.SubTest();
            SubTests<N17>.SubTest();
            SubTests<N18>.SubTest();
            SubTests<N19>.SubTest();
            SubTests<N20>.SubTest();
            SubTests<N21>.SubTest();
            SubTests<N22>.SubTest();
            SubTests<N23>.SubTest();
            SubTests<N24>.SubTest();
            SubTests<N25>.SubTest();
            SubTests<N31>.SubTest();
            SubTests<N32>.SubTest();
            SubTests<N33>.SubTest();
            SubTests<N47>.SubTest();
            SubTests<N48>.SubTest();
            SubTests<N50>.SubTest();
            SubTests<N53>.SubTest();
            SubTests<N56>.SubTest();
            SubTests<N59>.SubTest();
            SubTests<N63>.SubTest();
            SubTests<N64>.SubTest();
            SubTests<N65>.SubTest();
        }

        [TestMethod]
        public void SubFullTest() {
            SubTests<N4>.SubFullTest();
            SubTests<N5>.SubFullTest();
            SubTests<N6>.SubFullTest();
            SubTests<N7>.SubFullTest();
            SubTests<N8>.SubFullTest();
            SubTests<N9>.SubFullTest();
            SubTests<N10>.SubFullTest();
            SubTests<N11>.SubFullTest();
            SubTests<N12>.SubFullTest();
            SubTests<N13>.SubFullTest();
            SubTests<N14>.SubFullTest();
            SubTests<N15>.SubFullTest();
            SubTests<N16>.SubFullTest();
            SubTests<N17>.SubFullTest();
            SubTests<N18>.SubFullTest();
            SubTests<N19>.SubFullTest();
            SubTests<N20>.SubFullTest();
            SubTests<N21>.SubFullTest();
            SubTests<N22>.SubFullTest();
            SubTests<N23>.SubFullTest();
            SubTests<N24>.SubFullTest();
            SubTests<N25>.SubFullTest();
            SubTests<N31>.SubFullTest();
            SubTests<N32>.SubFullTest();
            SubTests<N33>.SubFullTest();
            SubTests<N47>.SubFullTest();
            SubTests<N48>.SubFullTest();
            SubTests<N50>.SubFullTest();
            SubTests<N53>.SubFullTest();
            SubTests<N56>.SubFullTest();
            SubTests<N59>.SubFullTest();
            SubTests<N63>.SubFullTest();
            SubTests<N64>.SubFullTest();
            SubTests<N65>.SubFullTest();
        }

        [TestMethod]
        public void SubSparseTest() {
            SubTests<N4>.SubSparseTest();
            SubTests<N5>.SubSparseTest();
            SubTests<N6>.SubSparseTest();
            SubTests<N7>.SubSparseTest();
            SubTests<N8>.SubSparseTest();
            SubTests<N9>.SubSparseTest();
            SubTests<N10>.SubSparseTest();
            SubTests<N11>.SubSparseTest();
            SubTests<N12>.SubSparseTest();
            SubTests<N13>.SubSparseTest();
            SubTests<N14>.SubSparseTest();
            SubTests<N15>.SubSparseTest();
            SubTests<N16>.SubSparseTest();
            SubTests<N17>.SubSparseTest();
            SubTests<N18>.SubSparseTest();
            SubTests<N19>.SubSparseTest();
            SubTests<N20>.SubSparseTest();
            SubTests<N21>.SubSparseTest();
            SubTests<N22>.SubSparseTest();
            SubTests<N23>.SubSparseTest();
            SubTests<N24>.SubSparseTest();
            SubTests<N25>.SubSparseTest();
            SubTests<N31>.SubSparseTest();
            SubTests<N32>.SubSparseTest();
            SubTests<N33>.SubSparseTest();
            SubTests<N47>.SubSparseTest();
            SubTests<N48>.SubSparseTest();
            SubTests<N50>.SubSparseTest();
            SubTests<N53>.SubSparseTest();
            SubTests<N56>.SubSparseTest();
            SubTests<N59>.SubSparseTest();
            SubTests<N63>.SubSparseTest();
            SubTests<N64>.SubSparseTest();
            SubTests<N65>.SubSparseTest();
        }

        [TestMethod]
        public void SubBlockTest() {
            SubTests<N4>.SubBlockTest();
            SubTests<N5>.SubBlockTest();
            SubTests<N6>.SubBlockTest();
            SubTests<N7>.SubBlockTest();
            SubTests<N8>.SubBlockTest();
            SubTests<N9>.SubBlockTest();
            SubTests<N10>.SubBlockTest();
            SubTests<N11>.SubBlockTest();
            SubTests<N12>.SubBlockTest();
            SubTests<N13>.SubBlockTest();
            SubTests<N14>.SubBlockTest();
            SubTests<N15>.SubBlockTest();
            SubTests<N16>.SubBlockTest();
            SubTests<N17>.SubBlockTest();
            SubTests<N18>.SubBlockTest();
            SubTests<N19>.SubBlockTest();
            SubTests<N20>.SubBlockTest();
            SubTests<N21>.SubBlockTest();
            SubTests<N22>.SubBlockTest();
            SubTests<N23>.SubBlockTest();
            SubTests<N24>.SubBlockTest();
            SubTests<N25>.SubBlockTest();
            SubTests<N31>.SubBlockTest();
            SubTests<N32>.SubBlockTest();
            SubTests<N33>.SubBlockTest();
            SubTests<N47>.SubBlockTest();
            SubTests<N48>.SubBlockTest();
            SubTests<N50>.SubBlockTest();
            SubTests<N53>.SubBlockTest();
            SubTests<N56>.SubBlockTest();
            SubTests<N59>.SubBlockTest();
            SubTests<N63>.SubBlockTest();
            SubTests<N64>.SubBlockTest();
            SubTests<N65>.SubBlockTest();
        }

        [TestMethod]
        public void SubCarryTest() {
            SubTests<N4>.SubCarryTest();
            SubTests<N5>.SubCarryTest();
            SubTests<N6>.SubCarryTest();
            SubTests<N7>.SubCarryTest();
            SubTests<N8>.SubCarryTest();
            SubTests<N9>.SubCarryTest();
            SubTests<N10>.SubCarryTest();
            SubTests<N11>.SubCarryTest();
            SubTests<N12>.SubCarryTest();
            SubTests<N13>.SubCarryTest();
            SubTests<N14>.SubCarryTest();
            SubTests<N15>.SubCarryTest();
            SubTests<N16>.SubCarryTest();
            SubTests<N17>.SubCarryTest();
            SubTests<N18>.SubCarryTest();
            SubTests<N19>.SubCarryTest();
            SubTests<N20>.SubCarryTest();
            SubTests<N21>.SubCarryTest();
            SubTests<N22>.SubCarryTest();
            SubTests<N23>.SubCarryTest();
            SubTests<N24>.SubCarryTest();
            SubTests<N25>.SubCarryTest();
            SubTests<N31>.SubCarryTest();
            SubTests<N32>.SubCarryTest();
            SubTests<N33>.SubCarryTest();
            SubTests<N47>.SubCarryTest();
            SubTests<N48>.SubCarryTest();
            SubTests<N50>.SubCarryTest();
            SubTests<N53>.SubCarryTest();
            SubTests<N56>.SubCarryTest();
            SubTests<N59>.SubCarryTest();
            SubTests<N63>.SubCarryTest();
            SubTests<N64>.SubCarryTest();
            SubTests<N65>.SubCarryTest();
            SubTests<Pow2.N128>.SubCarryTest();
            SubTests<Pow2.N256>.SubCarryTest();
        }

        [TestMethod]
        public void SubShiftTest() {
            SubTests<N4>.SubShiftTest();
            SubTests<N5>.SubShiftTest();
            SubTests<N6>.SubShiftTest();
            SubTests<N7>.SubShiftTest();
            SubTests<N8>.SubShiftTest();
            SubTests<N9>.SubShiftTest();
            SubTests<N10>.SubShiftTest();
            SubTests<N11>.SubShiftTest();
            SubTests<N12>.SubShiftTest();
            SubTests<N13>.SubShiftTest();
            SubTests<N14>.SubShiftTest();
            SubTests<N15>.SubShiftTest();
            SubTests<N16>.SubShiftTest();
            SubTests<N17>.SubShiftTest();
            SubTests<N18>.SubShiftTest();
            SubTests<N19>.SubShiftTest();
            SubTests<N20>.SubShiftTest();
            SubTests<N21>.SubShiftTest();
            SubTests<N22>.SubShiftTest();
            SubTests<N23>.SubShiftTest();
            SubTests<N24>.SubShiftTest();
            SubTests<N25>.SubShiftTest();
            SubTests<N31>.SubShiftTest();
            SubTests<N32>.SubShiftTest();
            SubTests<N33>.SubShiftTest();
            SubTests<N47>.SubShiftTest();
            SubTests<N48>.SubShiftTest();
            SubTests<N50>.SubShiftTest();
            SubTests<N53>.SubShiftTest();
            SubTests<N56>.SubShiftTest();
            SubTests<N59>.SubShiftTest();
            SubTests<N63>.SubShiftTest();
            SubTests<N64>.SubShiftTest();
            SubTests<N65>.SubShiftTest();
            SubTests<Pow2.N128>.SubShiftTest();
            SubTests<Pow2.N256>.SubShiftTest();
        }
    }
}

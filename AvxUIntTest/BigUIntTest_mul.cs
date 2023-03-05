using AvxUInt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace AvxUIntTest {
    public static class MulTests<N> where N : struct, IConstant {
        public static void MulTest() {
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

            BigInteger maxn = BigUInt<N>.Full;

            Console.WriteLine($"length={BigUInt<N>.Length}");

            int normal_passes = 0, overflow_passes = 0;

            foreach ((BigUInt<N> v1, BigInteger n1) in vs) {
                foreach ((BigUInt<N> v2, BigInteger n2) in vs) {
                    BigInteger n = n1 * n2;

                    if (n <= maxn) {
                        NormalTest(n, v1, v2, n1, n2);

                        normal_passes++;
                    }
                    else {
                        Assert.ThrowsException<OverflowException>(() => {
                            _ = v1 * v2;
                        });

                        overflow_passes++;
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(overflow_passes)}: {overflow_passes}");
        }

        public static void MulFullTest() {
            List<(BigUInt<N> v1, BigUInt<N> v2, BigInteger n1, BigInteger n2)> vs = new();
            BigInteger maxn = BigUInt<N>.Full, v = maxn / 2;

            while (v > 1) {
                BigInteger u = maxn / v;

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
                BigInteger n = n1 * n2;

                if (n <= maxn) {
                    NormalTest(n, v1, v2, n1, n2);

                    normal_passes++;
                }
                else {
                    Assert.ThrowsException<OverflowException>(() => {
                        _ = v1 * v2;
                    });

                    overflow_passes++;
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(overflow_passes)}: {overflow_passes}");
        }

        public static void MulSparseTest() {
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

            BigInteger maxn = BigUInt<N>.Full;

            Console.WriteLine($"length={BigUInt<N>.Length}");

            int normal_passes = 0, overflow_passes = 0;

            foreach ((BigUInt<N> v1, BigInteger n1) in vs) {
                foreach ((BigUInt<N> v2, BigInteger n2) in vs) {
                    BigInteger n = n1 * n2;

                    if (n <= maxn) {
                        NormalTest(n, v1, v2, n1, n2);

                        normal_passes++;
                    }
                    else {
                        Assert.ThrowsException<OverflowException>(() => {
                            _ = v1 * v2;
                        });

                        overflow_passes++;
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(overflow_passes)}: {overflow_passes}");
        }

        public static void MulBlockTest() {
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

            BigInteger maxn = BigUInt<N>.Full;

            Console.WriteLine($"length={BigUInt<N>.Length}");

            int normal_passes = 0, overflow_passes = 0;

            foreach ((BigUInt<N> v1, BigInteger n1) in vs) {
                foreach ((BigUInt<N> v2, BigInteger n2) in vs) {
                    BigInteger n = n1 * n2;

                    if (n <= maxn) {
                        NormalTest(n, v1, v2, n1, n2);

                        normal_passes++;
                    }
                    else {
                        Assert.ThrowsException<OverflowException>(() => {
                            _ = v1 * v2;
                        });

                        overflow_passes++;
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(overflow_passes)}: {overflow_passes}");
        }

        public static void MulCarryTest() {
            BigUInt<N> v1 = new(Enumerable.Repeat(~0u, BigUInt<N>.Length / 2).Concat(Enumerable.Repeat(0u, BigUInt<N>.Length - BigUInt<N>.Length / 2)).ToArray());
            BigUInt<N> v2 = new(Enumerable.Repeat(~0u, BigUInt<N>.Length - BigUInt<N>.Length / 2).Concat(Enumerable.Repeat(0u, BigUInt<N>.Length / 2)).ToArray());

            BigInteger n = (BigInteger)v1 * (BigInteger)v2;

            Assert.AreEqual(n, (BigInteger)(v1 * v2));
        }

        private static void NormalTest(BigInteger n, BigUInt<N> v1, BigUInt<N> v2, BigInteger n1, BigInteger n2) {
            Assert.AreEqual(n, (BigInteger)(v1 * v2), $"{n1}*{n2}");

            if (v1.Digits <= 2) {
                Assert.AreEqual(n, (BigInteger)(UIntUtil.Pack(v1.Value[1], v1.Value[0]) * v2), $"{n1}*{n2}");
            }

            if (v1.Digits <= 1) {
                Assert.AreEqual(n, (BigInteger)(v1.Value[0] * v2), $"{n1}*{n2}");
            }

            if (v2.Digits <= 2) {
                Assert.AreEqual(n, (BigInteger)(v1 * UIntUtil.Pack(v2.Value[1], v2.Value[0])), $"{n1}*{n2}");
            }

            if (v2.Digits <= 1) {
                Assert.AreEqual(n, (BigInteger)(v1 * v2.Value[0]), $"{n1}*{n2}");
            }
        }
    }

    [TestClass]
    public class MulTests {
        [TestMethod]
        public void MulTest() {
            MulTests<N4>.MulTest();
            MulTests<N5>.MulTest();
            MulTests<N6>.MulTest();
            MulTests<N7>.MulTest();
            MulTests<N8>.MulTest();
            MulTests<N9>.MulTest();
            MulTests<N10>.MulTest();
            MulTests<N11>.MulTest();
            MulTests<N12>.MulTest();
            MulTests<N13>.MulTest();
            MulTests<N14>.MulTest();
            MulTests<N15>.MulTest();
            MulTests<N16>.MulTest();
            MulTests<N17>.MulTest();
            MulTests<N23>.MulTest();
            MulTests<N24>.MulTest();
            MulTests<N25>.MulTest();
            MulTests<N31>.MulTest();
            MulTests<N32>.MulTest();
            MulTests<N33>.MulTest();
            MulTests<N47>.MulTest();
            MulTests<N48>.MulTest();
            MulTests<N50>.MulTest();
            MulTests<N53>.MulTest();
            MulTests<N56>.MulTest();
            MulTests<N59>.MulTest();
            MulTests<N63>.MulTest();
            MulTests<N64>.MulTest();
            MulTests<N65>.MulTest();
        }

        [TestMethod]
        public void MulFullTest() {
            MulTests<N4>.MulFullTest();
            MulTests<N5>.MulFullTest();
            MulTests<N6>.MulFullTest();
            MulTests<N7>.MulFullTest();
            MulTests<N8>.MulFullTest();
            MulTests<N9>.MulFullTest();
            MulTests<N10>.MulFullTest();
            MulTests<N11>.MulFullTest();
            MulTests<N12>.MulFullTest();
            MulTests<N13>.MulFullTest();
            MulTests<N14>.MulFullTest();
            MulTests<N15>.MulFullTest();
            MulTests<N16>.MulFullTest();
            MulTests<N17>.MulFullTest();
            MulTests<N23>.MulFullTest();
            MulTests<N24>.MulFullTest();
            MulTests<N25>.MulFullTest();
            MulTests<N31>.MulFullTest();
            MulTests<N32>.MulFullTest();
            MulTests<N33>.MulFullTest();
            MulTests<N47>.MulFullTest();
            MulTests<N48>.MulFullTest();
            MulTests<N50>.MulFullTest();
            MulTests<N53>.MulFullTest();
            MulTests<N56>.MulFullTest();
            MulTests<N59>.MulFullTest();
            MulTests<N63>.MulFullTest();
            MulTests<N64>.MulFullTest();
            MulTests<N65>.MulFullTest();
        }

        [TestMethod]
        public void MulSparseTest() {
            MulTests<N4>.MulSparseTest();
            MulTests<N5>.MulSparseTest();
            MulTests<N6>.MulSparseTest();
            MulTests<N7>.MulSparseTest();
            MulTests<N8>.MulSparseTest();
            MulTests<N9>.MulSparseTest();
            MulTests<N10>.MulSparseTest();
            MulTests<N11>.MulSparseTest();
            MulTests<N12>.MulSparseTest();
            MulTests<N13>.MulSparseTest();
            MulTests<N14>.MulSparseTest();
            MulTests<N15>.MulSparseTest();
            MulTests<N16>.MulSparseTest();
            MulTests<N17>.MulSparseTest();
            MulTests<N23>.MulSparseTest();
            MulTests<N24>.MulSparseTest();
            MulTests<N25>.MulSparseTest();
            MulTests<N31>.MulSparseTest();
            MulTests<N32>.MulSparseTest();
            MulTests<N33>.MulSparseTest();
            MulTests<N47>.MulSparseTest();
            MulTests<N48>.MulSparseTest();
            MulTests<N50>.MulSparseTest();
            MulTests<N53>.MulSparseTest();
            MulTests<N56>.MulSparseTest();
            MulTests<N59>.MulSparseTest();
            MulTests<N63>.MulSparseTest();
            MulTests<N64>.MulSparseTest();
            MulTests<N65>.MulSparseTest();
        }

        [TestMethod]
        public void MulBlockTest() {
            MulTests<N4>.MulBlockTest();
            MulTests<N5>.MulBlockTest();
            MulTests<N6>.MulBlockTest();
            MulTests<N7>.MulBlockTest();
            MulTests<N8>.MulBlockTest();
            MulTests<N9>.MulBlockTest();
            MulTests<N10>.MulBlockTest();
            MulTests<N11>.MulBlockTest();
            MulTests<N12>.MulBlockTest();
            MulTests<N13>.MulBlockTest();
            MulTests<N14>.MulBlockTest();
            MulTests<N15>.MulBlockTest();
            MulTests<N16>.MulBlockTest();
            MulTests<N17>.MulBlockTest();
            MulTests<N23>.MulBlockTest();
            MulTests<N24>.MulBlockTest();
            MulTests<N25>.MulBlockTest();
            MulTests<N31>.MulBlockTest();
            MulTests<N32>.MulBlockTest();
            MulTests<N33>.MulBlockTest();
            MulTests<N47>.MulBlockTest();
            MulTests<N48>.MulBlockTest();
            MulTests<N50>.MulBlockTest();
            MulTests<N53>.MulBlockTest();
            MulTests<N56>.MulBlockTest();
            MulTests<N59>.MulBlockTest();
            MulTests<N63>.MulBlockTest();
            MulTests<N64>.MulBlockTest();
            MulTests<N65>.MulBlockTest();
        }

        [TestMethod]
        public void MulCarryTest() {
            MulTests<N4>.MulCarryTest();
            MulTests<N5>.MulCarryTest();
            MulTests<N6>.MulCarryTest();
            MulTests<N7>.MulCarryTest();
            MulTests<N8>.MulCarryTest();
            MulTests<N9>.MulCarryTest();
            MulTests<N10>.MulCarryTest();
            MulTests<N11>.MulCarryTest();
            MulTests<N12>.MulCarryTest();
            MulTests<N13>.MulCarryTest();
            MulTests<N14>.MulCarryTest();
            MulTests<N15>.MulCarryTest();
            MulTests<N16>.MulCarryTest();
            MulTests<N17>.MulCarryTest();
            MulTests<N23>.MulCarryTest();
            MulTests<N24>.MulCarryTest();
            MulTests<N25>.MulCarryTest();
            MulTests<N31>.MulCarryTest();
            MulTests<N32>.MulCarryTest();
            MulTests<N33>.MulCarryTest();
            MulTests<N47>.MulCarryTest();
            MulTests<N48>.MulCarryTest();
            MulTests<N50>.MulCarryTest();
            MulTests<N53>.MulCarryTest();
            MulTests<N56>.MulCarryTest();
            MulTests<N59>.MulCarryTest();
            MulTests<N63>.MulCarryTest();
            MulTests<N64>.MulCarryTest();
            MulTests<N65>.MulCarryTest();
        }
    }
}

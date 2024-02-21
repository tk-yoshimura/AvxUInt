using AvxUInt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace AvxUIntTest {
    public static class DivTests<N> where N : struct, IConstant {
        public static void DivTest() {
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

            int normal_passes = 0, divzero_passes = 0;

            foreach ((BigUInt<N> v1, BigInteger n1) in vs) {
                foreach ((BigUInt<N> v2, BigInteger n2) in vs) {
                    if (n2 > 0) {
                        NormalTest(v1, n1, v2, n2);

                        normal_passes++;
                    }
                    else {
                        Assert.ThrowsException<DivideByZeroException>(() => {
                            _ = v1 / v2;
                        });
                        Assert.ThrowsException<DivideByZeroException>(() => {
                            _ = v1 % v2;
                        });

                        divzero_passes++;
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(divzero_passes)}: {divzero_passes}");
        }

        public static void DivFullTest() {
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

            int normal_passes = 0, divzero_passes = 0;

            foreach ((BigUInt<N> v1, BigUInt<N> v2, BigInteger n1, BigInteger n2) in vs) {
                if (n2 > 0) {
                    NormalTest(v1, n1, v2, n2);

                    normal_passes++;
                }
                else {
                    Assert.ThrowsException<DivideByZeroException>(() => {
                        _ = v1 / v2;
                    });
                    Assert.ThrowsException<DivideByZeroException>(() => {
                        _ = v1 % v2;
                    });

                    divzero_passes++;
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(divzero_passes)}: {divzero_passes}");
        }

        public static void DivSparseTest() {
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

            int normal_passes = 0, divzero_passes = 0;

            foreach ((BigUInt<N> v1, BigInteger n1) in vs) {
                foreach ((BigUInt<N> v2, BigInteger n2) in vs) {
                    if (n2 > 0) {
                        NormalTest(v1, n1, v2, n2);

                        normal_passes++;
                    }
                    else {
                        Assert.ThrowsException<DivideByZeroException>(() => {
                            _ = v1 / v2;
                        });
                        Assert.ThrowsException<DivideByZeroException>(() => {
                            _ = v1 % v2;
                        });

                        divzero_passes++;
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(divzero_passes)}: {divzero_passes}");
        }

        public static void DivBlockTest() {
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

            int normal_passes = 0, divzero_passes = 0;

            foreach ((BigUInt<N> v1, BigInteger n1) in vs) {
                foreach ((BigUInt<N> v2, BigInteger n2) in vs) {
                    if (n2 > 0) {
                        NormalTest(v1, n1, v2, n2);

                        normal_passes++;
                    }
                    else {
                        Assert.ThrowsException<DivideByZeroException>(() => {
                            _ = v1 / v2;
                        });
                        Assert.ThrowsException<DivideByZeroException>(() => {
                            _ = v1 % v2;
                        });

                        divzero_passes++;
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(divzero_passes)}: {divzero_passes}");
        }

        private static void NormalTest(BigUInt<N> v1, BigInteger n1, BigUInt<N> v2, BigInteger n2) {
            BigInteger q = n1 / n2, r = n1 % n2;

            Assert.AreEqual(q, (BigInteger)(v1 / v2), $"{n1}/{n2}");
            Assert.AreEqual(r, (BigInteger)(v1 % v2), $"{n1}%{n2}");

            if (v1.Digits <= 2) {
                Assert.AreEqual(q, (BigInteger)(UIntUtil.Pack(v1.Value[1], v1.Value[0]) / v2), $"{n1}/{n2}");
                Assert.AreEqual(r, (BigInteger)(UIntUtil.Pack(v1.Value[1], v1.Value[0]) % v2), $"{n1}%{n2}");
            }

            if (v1.Digits <= 1) {
                Assert.AreEqual(q, (BigInteger)(v1.Value[0] / v2), $"{n1}/{n2}");
                Assert.AreEqual(r, (BigInteger)(v1.Value[0] % v2), $"{n1}%{n2}");
            }

            if (v2.Digits <= 2) {
                Assert.AreEqual(q, (BigInteger)(v1 / UIntUtil.Pack(v2.Value[1], v2.Value[0])), $"{n1}/{n2}");
                Assert.AreEqual(r, (BigInteger)(v1 % UIntUtil.Pack(v2.Value[1], v2.Value[0])), $"{n1}%{n2}");
            }

            if (v2.Digits <= 1) {
                Assert.AreEqual(q, (BigInteger)(v1 / v2.Value[0]), $"{n1}/{n2}");
                Assert.AreEqual(r, (BigInteger)(v1 % v2.Value[0]), $"{n1}%{n2}");
            }
        }
    }

    [TestClass]
    public class DivTests {
        [TestMethod]
        public void DivTest() {
            DivTests<N4>.DivTest();
            DivTests<N5>.DivTest();
            DivTests<N6>.DivTest();
            DivTests<N7>.DivTest();
            DivTests<N8>.DivTest();
            DivTests<N9>.DivTest();
            DivTests<N10>.DivTest();
            DivTests<N11>.DivTest();
            DivTests<N12>.DivTest();
            DivTests<N13>.DivTest();
            DivTests<N14>.DivTest();
            DivTests<N15>.DivTest();
            DivTests<N16>.DivTest();
            DivTests<N17>.DivTest();
            DivTests<N18>.DivTest();
            DivTests<N19>.DivTest();
            DivTests<N20>.DivTest();
            DivTests<N21>.DivTest();
            DivTests<N22>.DivTest();
            DivTests<N23>.DivTest();
            DivTests<N24>.DivTest();
            DivTests<N25>.DivTest();
            DivTests<N31>.DivTest();
            DivTests<N32>.DivTest();
            DivTests<N33>.DivTest();
            DivTests<N47>.DivTest();
            DivTests<N48>.DivTest();
            DivTests<N50>.DivTest();
            DivTests<N53>.DivTest();
            DivTests<N56>.DivTest();
            DivTests<N59>.DivTest();
            DivTests<N63>.DivTest();
            DivTests<N64>.DivTest();
            DivTests<N65>.DivTest();
        }

        [TestMethod]
        public void DivFullTest() {
            DivTests<N4>.DivFullTest();
            DivTests<N5>.DivFullTest();
            DivTests<N6>.DivFullTest();
            DivTests<N7>.DivFullTest();
            DivTests<N8>.DivFullTest();
            DivTests<N9>.DivFullTest();
            DivTests<N10>.DivFullTest();
            DivTests<N11>.DivFullTest();
            DivTests<N12>.DivFullTest();
            DivTests<N13>.DivFullTest();
            DivTests<N14>.DivFullTest();
            DivTests<N15>.DivFullTest();
            DivTests<N16>.DivFullTest();
            DivTests<N17>.DivFullTest();
            DivTests<N18>.DivFullTest();
            DivTests<N19>.DivFullTest();
            DivTests<N20>.DivFullTest();
            DivTests<N21>.DivFullTest();
            DivTests<N22>.DivFullTest();
            DivTests<N23>.DivFullTest();
            DivTests<N24>.DivFullTest();
            DivTests<N25>.DivFullTest();
            DivTests<N31>.DivFullTest();
            DivTests<N32>.DivFullTest();
            DivTests<N33>.DivFullTest();
            DivTests<N47>.DivFullTest();
            DivTests<N48>.DivFullTest();
            DivTests<N50>.DivFullTest();
            DivTests<N53>.DivFullTest();
            DivTests<N56>.DivFullTest();
            DivTests<N59>.DivFullTest();
            DivTests<N63>.DivFullTest();
            DivTests<N64>.DivFullTest();
            DivTests<N65>.DivFullTest();
        }

        [TestMethod]
        public void DivSparseTest() {
            DivTests<N4>.DivSparseTest();
            DivTests<N5>.DivSparseTest();
            DivTests<N6>.DivSparseTest();
            DivTests<N7>.DivSparseTest();
            DivTests<N8>.DivSparseTest();
            DivTests<N9>.DivSparseTest();
            DivTests<N10>.DivSparseTest();
            DivTests<N11>.DivSparseTest();
            DivTests<N12>.DivSparseTest();
            DivTests<N13>.DivSparseTest();
            DivTests<N14>.DivSparseTest();
            DivTests<N15>.DivSparseTest();
            DivTests<N16>.DivSparseTest();
            DivTests<N17>.DivSparseTest();
            DivTests<N18>.DivSparseTest();
            DivTests<N19>.DivSparseTest();
            DivTests<N20>.DivSparseTest();
            DivTests<N21>.DivSparseTest();
            DivTests<N22>.DivSparseTest();
            DivTests<N23>.DivSparseTest();
            DivTests<N24>.DivSparseTest();
            DivTests<N25>.DivSparseTest();
            DivTests<N31>.DivSparseTest();
            DivTests<N32>.DivSparseTest();
            DivTests<N33>.DivSparseTest();
            DivTests<N47>.DivSparseTest();
            DivTests<N48>.DivSparseTest();
            DivTests<N50>.DivSparseTest();
            DivTests<N53>.DivSparseTest();
            DivTests<N56>.DivSparseTest();
            DivTests<N59>.DivSparseTest();
            DivTests<N63>.DivSparseTest();
            DivTests<N64>.DivSparseTest();
            DivTests<N65>.DivSparseTest();
        }

        [TestMethod]
        public void DivBlockTest() {
            DivTests<N4>.DivBlockTest();
            DivTests<N5>.DivBlockTest();
            DivTests<N6>.DivBlockTest();
            DivTests<N7>.DivBlockTest();
            DivTests<N8>.DivBlockTest();
            DivTests<N9>.DivBlockTest();
            DivTests<N10>.DivBlockTest();
            DivTests<N11>.DivBlockTest();
            DivTests<N12>.DivBlockTest();
            DivTests<N13>.DivBlockTest();
            DivTests<N14>.DivBlockTest();
            DivTests<N15>.DivBlockTest();
            DivTests<N16>.DivBlockTest();
            DivTests<N17>.DivBlockTest();
            DivTests<N18>.DivBlockTest();
            DivTests<N19>.DivBlockTest();
            DivTests<N20>.DivBlockTest();
            DivTests<N21>.DivBlockTest();
            DivTests<N22>.DivBlockTest();
            DivTests<N23>.DivBlockTest();
            DivTests<N24>.DivBlockTest();
            DivTests<N25>.DivBlockTest();
            DivTests<N31>.DivBlockTest();
            DivTests<N32>.DivBlockTest();
            DivTests<N33>.DivBlockTest();
            DivTests<N47>.DivBlockTest();
            DivTests<N48>.DivBlockTest();
            DivTests<N50>.DivBlockTest();
            DivTests<N53>.DivBlockTest();
            DivTests<N56>.DivBlockTest();
            DivTests<N59>.DivBlockTest();
            DivTests<N63>.DivBlockTest();
            DivTests<N64>.DivBlockTest();
            DivTests<N65>.DivBlockTest();
        }
    }
}

using AvxUInt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace AvxUIntTest {
    public static class FmasTests<N> where N : struct, IConstant {
        public static void FmaTest() {
            Random random = new(5678);

            List<(BigUInt<N> b, BigInteger n)> vs = new();
            List<(BigUInt<N> b, BigInteger n)> us = new();

            for (int i = 0; i < 64; i++) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<N>.Length, random.Next(BigUInt<N>.Bits + 1));

                BigUInt<N> b = new(bits, enable_clone: false);

                vs.Add((b, (BigInteger)b));
            }
            vs.Add((BigUInt<N>.Full, (BigInteger)BigUInt<N>.Full));
            vs.Add((BigUInt<N>.Zero, (BigInteger)BigUInt<N>.Zero));
            for (int i = 0; i < 8; i++) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<N>.Length, random.Next(BigUInt<N>.Bits / 2, BigUInt<N>.Bits + 1));

                BigUInt<N> b = new(bits, enable_clone: false);

                us.Add((b, (BigInteger)b));
            }
            us.Add((BigUInt<N>.Full, (BigInteger)BigUInt<N>.Full));
            us.Add((BigUInt<N>.Zero, (BigInteger)BigUInt<N>.Zero));

            BigInteger maxn = BigUInt<N>.Full;

            Console.WriteLine($"length={BigUInt<N>.Length}");

            int normal_passes = 0, overflow_passes = 0;

            foreach ((BigUInt<N> vc, BigInteger c) in us) {
                foreach ((BigUInt<N> va, BigInteger a) in vs) {
                    foreach ((BigUInt<N> vb, BigInteger b) in vs) {
                        BigInteger n = c + a * b;

                        if (n <= maxn) {
                            Assert.AreEqual(n, (BigInteger)BigUInt<N>.Fma(vc, va, vb), $"{vc}+{va}*{vb}");

                            if (vb.Digits <= 2) {
                                Assert.AreEqual(n, (BigInteger)BigUInt<N>.Fma(vc, va, UIntUtil.Pack(vb.Value[1], vb.Value[0])), $"{vc}+{va}*{vb}");
                            }

                            if (vb.Digits <= 1) {
                                Assert.AreEqual(n, (BigInteger)BigUInt<N>.Fma(vc, va, vb.Value[0]), $"{vc}+{va}*{vb}");
                            }

                            normal_passes++;
                        }
                        else {
                            Assert.ThrowsException<OverflowException>(() => {
                                _ = BigUInt<N>.Fma(vc, va, vb);
                            });

                            overflow_passes++;
                        }
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(overflow_passes)}: {overflow_passes}");
        }

        public static void FmaDoubleTest() {
            Random random = new(5678);

            List<(BigUInt<N> b, BigInteger n)> vs = new();
            List<(BigUInt<Double<N>> b, BigInteger n)> us = new();

            for (int i = 0; i < 32; i++) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<N>.Length, random.Next(BigUInt<N>.Bits + 1));

                BigUInt<N> b = new(bits, enable_clone: false);

                vs.Add((b, (BigInteger)b));
            }
            vs.Add((BigUInt<N>.Full, (BigInteger)BigUInt<N>.Full));
            vs.Add((BigUInt<N>.Zero, (BigInteger)BigUInt<N>.Zero));
            for (int i = 0; i < 16; i++) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<Double<N>>.Length, random.Next(BigUInt<N>.Bits / 2, BigUInt<Double<N>>.Bits + 1));

                BigUInt<Double<N>> b = new(bits, enable_clone: false);

                us.Add((b, (BigInteger)b));
            }
            us.Add((BigUInt<Double<N>>.Full, (BigInteger)BigUInt<Double<N>>.Full));
            us.Add((BigUInt<Double<N>>.Zero, (BigInteger)BigUInt<Double<N>>.Zero));

            BigInteger maxn = BigUInt<Double<N>>.Full;

            Console.WriteLine($"length={BigUInt<N>.Length}");

            int normal_passes = 0, overflow_passes = 0;

            foreach ((BigUInt<Double<N>> vc, BigInteger c) in us) {
                foreach ((BigUInt<N> va, BigInteger a) in vs) {
                    foreach ((BigUInt<N> vb, BigInteger b) in vs) {
                        BigInteger n = c + a * b;

                        if (n <= maxn) {
                            Assert.AreEqual(n, (BigInteger)BigUInt<N>.Fma(vc, va, vb), $"{vc}+{va}*{vb}");

                            if (vb.Digits <= 2) {
                                Assert.AreEqual(n, (BigInteger)BigUInt<N>.Fma(vc, va, UIntUtil.Pack(vb.Value[1], vb.Value[0])), $"{vc}+{va}*{vb}");
                            }

                            if (vb.Digits <= 1) {
                                Assert.AreEqual(n, (BigInteger)BigUInt<N>.Fma(vc, va, vb.Value[0]), $"{vc}+{va}*{vb}");
                            }

                            normal_passes++;
                        }
                        else {
                            Assert.ThrowsException<OverflowException>(() => {
                                _ = BigUInt<N>.Fma(vc, va, vb);
                            });

                            overflow_passes++;
                        }
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(overflow_passes)}: {overflow_passes}");
        }

        public static void FmaCarryTest() {
            BigUInt<Double<N>> vc = new(Enumerable.Repeat(~0u, BigUInt<N>.Length).Concat(Enumerable.Repeat(0u, BigUInt<N>.Length)).ToArray());
            BigUInt<N> v1 = new(Enumerable.Repeat(~0u, BigUInt<N>.Length / 2).Concat(Enumerable.Repeat(0u, BigUInt<N>.Length - BigUInt<N>.Length / 2)).ToArray());
            BigUInt<N> v2 = new(Enumerable.Repeat(~0u, BigUInt<N>.Length - BigUInt<N>.Length / 2).Concat(Enumerable.Repeat(0u, BigUInt<N>.Length / 2)).ToArray());

            BigInteger n = (BigInteger)vc + (BigInteger)v1 * (BigInteger)v2;

            Assert.AreEqual(n, (BigInteger)BigUInt<N>.Fma(vc, v1, v2));
        }

        public static void FmsTest() {
            Random random = new(5678);

            List<(BigUInt<N> b, BigInteger n)> vs = new();
            List<(BigUInt<N> b, BigInteger n)> us = new();

            for (int i = 0; i < 64; i++) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<N>.Length, random.Next(BigUInt<N>.Bits + 1));

                BigUInt<N> b = new(bits, enable_clone: false);

                vs.Add((b, (BigInteger)b));
            }
            vs.Add((BigUInt<N>.Full, (BigInteger)BigUInt<N>.Full));
            vs.Add((BigUInt<N>.Zero, (BigInteger)BigUInt<N>.Zero));
            for (int i = 0; i < 8; i++) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<N>.Length, random.Next(BigUInt<N>.Bits / 2, BigUInt<N>.Bits + 1));

                BigUInt<N> b = new(bits, enable_clone: false);

                us.Add((b, (BigInteger)b));
            }
            us.Add((BigUInt<N>.Full, (BigInteger)BigUInt<N>.Full));
            us.Add((BigUInt<N>.Zero, (BigInteger)BigUInt<N>.Zero));

            BigInteger maxn = BigUInt<N>.Full;

            Console.WriteLine($"length={BigUInt<N>.Length}");

            int normal_passes = 0, overflow_passes = 0;

            foreach ((BigUInt<N> vc, BigInteger c) in us) {
                foreach ((BigUInt<N> va, BigInteger a) in vs) {
                    foreach ((BigUInt<N> vb, BigInteger b) in vs) {
                        BigInteger n = c - a * b;

                        if (n >= 0) {
                            Assert.AreEqual(n, (BigInteger)BigUInt<N>.Fms(vc, va, vb), $"{vc}-{va}*{vb}");

                            if (vb.Digits <= 2) {
                                Assert.AreEqual(n, (BigInteger)BigUInt<N>.Fms(vc, va, UIntUtil.Pack(vb.Value[1], vb.Value[0])), $"{vc}-{va}*{vb}");
                            }

                            if (vb.Digits <= 1) {
                                Assert.AreEqual(n, (BigInteger)BigUInt<N>.Fms(vc, va, vb.Value[0]), $"{vc}-{va}*{vb}");
                            }

                            normal_passes++;
                        }
                        else {
                            Assert.ThrowsException<OverflowException>(() => {
                                _ = BigUInt<N>.Fms(vc, va, vb);
                            });

                            overflow_passes++;
                        }
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(overflow_passes)}: {overflow_passes}");
        }

        public static void FmsDoubleTest() {
            Random random = new(5678);

            List<(BigUInt<N> b, BigInteger n)> vs = new();
            List<(BigUInt<Double<N>> b, BigInteger n)> us = new();

            for (int i = 0; i < 32; i++) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<N>.Length, random.Next(BigUInt<N>.Bits + 1));

                BigUInt<N> b = new(bits, enable_clone: false);

                vs.Add((b, (BigInteger)b));
            }
            vs.Add((BigUInt<N>.Full, (BigInteger)BigUInt<N>.Full));
            vs.Add((BigUInt<N>.Zero, (BigInteger)BigUInt<N>.Zero));
            for (int i = 0; i < 16; i++) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<Double<N>>.Length, random.Next(BigUInt<N>.Bits / 2, BigUInt<Double<N>>.Bits + 1));

                BigUInt<Double<N>> b = new(bits, enable_clone: false);

                us.Add((b, (BigInteger)b));
            }
            us.Add((BigUInt<Double<N>>.Full, (BigInteger)BigUInt<Double<N>>.Full));
            us.Add((BigUInt<Double<N>>.Zero, (BigInteger)BigUInt<Double<N>>.Zero));

            BigInteger maxn = BigUInt<Double<N>>.Full;

            Console.WriteLine($"length={BigUInt<N>.Length}");

            int normal_passes = 0, overflow_passes = 0;

            foreach ((BigUInt<Double<N>> vc, BigInteger c) in us) {
                foreach ((BigUInt<N> va, BigInteger a) in vs) {
                    foreach ((BigUInt<N> vb, BigInteger b) in vs) {
                        BigInteger n = c - a * b;

                        if (n >= 0) {
                            Assert.AreEqual(n, (BigInteger)BigUInt<N>.Fms(vc, va, vb), $"{vc}-{va}*{vb}");

                            if (vb.Digits <= 2) {
                                Assert.AreEqual(n, (BigInteger)BigUInt<N>.Fms(vc, va, UIntUtil.Pack(vb.Value[1], vb.Value[0])), $"{vc}-{va}*{vb}");
                            }

                            if (vb.Digits <= 1) {
                                Assert.AreEqual(n, (BigInteger)BigUInt<N>.Fms(vc, va, vb.Value[0]), $"{vc}-{va}*{vb}");
                            }

                            normal_passes++;
                        }
                        else {
                            Assert.ThrowsException<OverflowException>(() => {
                                _ = BigUInt<N>.Fms(vc, va, vb);
                            });

                            overflow_passes++;
                        }
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(overflow_passes)}: {overflow_passes}");
        }

        public static void FmsCarryTest() {
            BigUInt<Double<N>> vc = new(Enumerable.Repeat(0u, BigUInt<N>.Length).Concat(new UInt32[] { 1u }).Concat(Enumerable.Repeat(0u, BigUInt<N>.Length - 1)).ToArray());
            BigUInt<N> v1 = new(Enumerable.Repeat(~0u, BigUInt<N>.Length / 2).Concat(Enumerable.Repeat(0u, BigUInt<N>.Length - BigUInt<N>.Length / 2)).ToArray());
            BigUInt<N> v2 = new(Enumerable.Repeat(~0u, BigUInt<N>.Length - BigUInt<N>.Length / 2).Concat(Enumerable.Repeat(0u, BigUInt<N>.Length / 2)).ToArray());

            BigInteger n = (BigInteger)vc - (BigInteger)v1 * (BigInteger)v2;

            Assert.AreEqual(n, (BigInteger)BigUInt<N>.Fms(vc, v1, v2));
        }
    }

    [TestClass]
    public class FmaTests {
        [TestMethod]
        public void FmaTest() {
            FmasTests<N4>.FmaTest();
            FmasTests<N5>.FmaTest();
            FmasTests<N6>.FmaTest();
            FmasTests<N7>.FmaTest();
            FmasTests<N8>.FmaTest();
            FmasTests<N9>.FmaTest();
            FmasTests<N10>.FmaTest();
            FmasTests<N11>.FmaTest();
            FmasTests<N12>.FmaTest();
            FmasTests<N13>.FmaTest();
            FmasTests<N14>.FmaTest();
            FmasTests<N15>.FmaTest();
            FmasTests<N16>.FmaTest();
            FmasTests<N17>.FmaTest();
            FmasTests<N23>.FmaTest();
            FmasTests<N24>.FmaTest();
            FmasTests<N25>.FmaTest();
            FmasTests<N31>.FmaTest();
            FmasTests<N32>.FmaTest();
            FmasTests<N33>.FmaTest();
            FmasTests<N47>.FmaTest();
            FmasTests<N48>.FmaTest();
            FmasTests<N50>.FmaTest();
            FmasTests<N53>.FmaTest();
            FmasTests<N56>.FmaTest();
            FmasTests<N59>.FmaTest();
            FmasTests<N63>.FmaTest();
            FmasTests<N64>.FmaTest();
            FmasTests<N65>.FmaTest();
        }

        [TestMethod]
        public void FmaDoubleTest() {
            FmasTests<N4>.FmaDoubleTest();
            FmasTests<N5>.FmaDoubleTest();
            FmasTests<N6>.FmaDoubleTest();
            FmasTests<N7>.FmaDoubleTest();
            FmasTests<N8>.FmaDoubleTest();
            FmasTests<N9>.FmaDoubleTest();
            FmasTests<N10>.FmaDoubleTest();
            FmasTests<N11>.FmaDoubleTest();
            FmasTests<N12>.FmaDoubleTest();
            FmasTests<N13>.FmaDoubleTest();
            FmasTests<N14>.FmaDoubleTest();
            FmasTests<N15>.FmaDoubleTest();
            FmasTests<N16>.FmaDoubleTest();
            FmasTests<N17>.FmaDoubleTest();
            FmasTests<N23>.FmaDoubleTest();
            FmasTests<N24>.FmaDoubleTest();
            FmasTests<N25>.FmaDoubleTest();
            FmasTests<N31>.FmaDoubleTest();
            FmasTests<N32>.FmaDoubleTest();
            FmasTests<N33>.FmaDoubleTest();
            FmasTests<N47>.FmaDoubleTest();
            FmasTests<N48>.FmaDoubleTest();
            FmasTests<N50>.FmaDoubleTest();
            FmasTests<N53>.FmaDoubleTest();
            FmasTests<N56>.FmaDoubleTest();
            FmasTests<N59>.FmaDoubleTest();
            FmasTests<N63>.FmaDoubleTest();
            FmasTests<N64>.FmaDoubleTest();
            FmasTests<N65>.FmaDoubleTest();
        }
        
        [TestMethod]
        public void FmaCarryTest() {
            FmasTests<N4>.FmaCarryTest();
            FmasTests<N5>.FmaCarryTest();
            FmasTests<N6>.FmaCarryTest();
            FmasTests<N7>.FmaCarryTest();
            FmasTests<N8>.FmaCarryTest();
            FmasTests<N9>.FmaCarryTest();
            FmasTests<N10>.FmaCarryTest();
            FmasTests<N11>.FmaCarryTest();
            FmasTests<N12>.FmaCarryTest();
            FmasTests<N13>.FmaCarryTest();
            FmasTests<N14>.FmaCarryTest();
            FmasTests<N15>.FmaCarryTest();
            FmasTests<N16>.FmaCarryTest();
            FmasTests<N17>.FmaCarryTest();
            FmasTests<N23>.FmaCarryTest();
            FmasTests<N24>.FmaCarryTest();
            FmasTests<N25>.FmaCarryTest();
            FmasTests<N31>.FmaCarryTest();
            FmasTests<N32>.FmaCarryTest();
            FmasTests<N33>.FmaCarryTest();
            FmasTests<N47>.FmaCarryTest();
            FmasTests<N48>.FmaCarryTest();
            FmasTests<N50>.FmaCarryTest();
            FmasTests<N53>.FmaCarryTest();
            FmasTests<N56>.FmaCarryTest();
            FmasTests<N59>.FmaCarryTest();
            FmasTests<N63>.FmaCarryTest();
            FmasTests<N64>.FmaCarryTest();
            FmasTests<N65>.FmaCarryTest();
            FmasTests<Pow2.N128>.FmaCarryTest();
            FmasTests<Pow2.N256>.FmaCarryTest();
        }
    }

    [TestClass]
    public class FmsTests {
        [TestMethod]
        public void FmsTest() {
            FmasTests<N4>.FmsTest();
            FmasTests<N5>.FmsTest();
            FmasTests<N6>.FmsTest();
            FmasTests<N7>.FmsTest();
            FmasTests<N8>.FmsTest();
            FmasTests<N9>.FmsTest();
            FmasTests<N10>.FmsTest();
            FmasTests<N11>.FmsTest();
            FmasTests<N12>.FmsTest();
            FmasTests<N13>.FmsTest();
            FmasTests<N14>.FmsTest();
            FmasTests<N15>.FmsTest();
            FmasTests<N16>.FmsTest();
            FmasTests<N17>.FmsTest();
            FmasTests<N23>.FmsTest();
            FmasTests<N24>.FmsTest();
            FmasTests<N25>.FmsTest();
            FmasTests<N31>.FmsTest();
            FmasTests<N32>.FmsTest();
            FmasTests<N33>.FmsTest();
            FmasTests<N47>.FmsTest();
            FmasTests<N48>.FmsTest();
            FmasTests<N50>.FmsTest();
            FmasTests<N53>.FmsTest();
            FmasTests<N56>.FmsTest();
            FmasTests<N59>.FmsTest();
            FmasTests<N63>.FmsTest();
            FmasTests<N64>.FmsTest();
            FmasTests<N65>.FmsTest();
        }

        [TestMethod]
        public void FmsDoubleTest() {
            FmasTests<N4>.FmsDoubleTest();
            FmasTests<N5>.FmsDoubleTest();
            FmasTests<N6>.FmsDoubleTest();
            FmasTests<N7>.FmsDoubleTest();
            FmasTests<N8>.FmsDoubleTest();
            FmasTests<N9>.FmsDoubleTest();
            FmasTests<N10>.FmsDoubleTest();
            FmasTests<N11>.FmsDoubleTest();
            FmasTests<N12>.FmsDoubleTest();
            FmasTests<N13>.FmsDoubleTest();
            FmasTests<N14>.FmsDoubleTest();
            FmasTests<N15>.FmsDoubleTest();
            FmasTests<N16>.FmsDoubleTest();
            FmasTests<N17>.FmsDoubleTest();
            FmasTests<N23>.FmsDoubleTest();
            FmasTests<N24>.FmsDoubleTest();
            FmasTests<N25>.FmsDoubleTest();
            FmasTests<N31>.FmsDoubleTest();
            FmasTests<N32>.FmsDoubleTest();
            FmasTests<N33>.FmsDoubleTest();
            FmasTests<N47>.FmsDoubleTest();
            FmasTests<N48>.FmsDoubleTest();
            FmasTests<N50>.FmsDoubleTest();
            FmasTests<N53>.FmsDoubleTest();
            FmasTests<N56>.FmsDoubleTest();
            FmasTests<N59>.FmsDoubleTest();
            FmasTests<N63>.FmsDoubleTest();
            FmasTests<N64>.FmsDoubleTest();
            FmasTests<N65>.FmsDoubleTest();
        }

        [TestMethod]
        public void FmsCarryTest() {
            FmasTests<N4>.FmsCarryTest();
            FmasTests<N5>.FmsCarryTest();
            FmasTests<N6>.FmsCarryTest();
            FmasTests<N7>.FmsCarryTest();
            FmasTests<N8>.FmsCarryTest();
            FmasTests<N9>.FmsCarryTest();
            FmasTests<N10>.FmsCarryTest();
            FmasTests<N11>.FmsCarryTest();
            FmasTests<N12>.FmsCarryTest();
            FmasTests<N13>.FmsCarryTest();
            FmasTests<N14>.FmsCarryTest();
            FmasTests<N15>.FmsCarryTest();
            FmasTests<N16>.FmsCarryTest();
            FmasTests<N17>.FmsCarryTest();
            FmasTests<N23>.FmsCarryTest();
            FmasTests<N24>.FmsCarryTest();
            FmasTests<N25>.FmsCarryTest();
            FmasTests<N31>.FmsCarryTest();
            FmasTests<N32>.FmsCarryTest();
            FmasTests<N33>.FmsCarryTest();
            FmasTests<N47>.FmsCarryTest();
            FmasTests<N48>.FmsCarryTest();
            FmasTests<N50>.FmsCarryTest();
            FmasTests<N53>.FmsCarryTest();
            FmasTests<N56>.FmsCarryTest();
            FmasTests<N59>.FmsCarryTest();
            FmasTests<N63>.FmsCarryTest();
            FmasTests<N64>.FmsCarryTest();
            FmasTests<N65>.FmsCarryTest();
            FmasTests<Pow2.N128>.FmsCarryTest();
            FmasTests<Pow2.N256>.FmsCarryTest();
        }
    }
}

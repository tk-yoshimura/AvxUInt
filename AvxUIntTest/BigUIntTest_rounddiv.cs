using AvxUInt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace AvxUIntTest {
    public static class RoundDivTests<N, M> where N : struct, IConstant where M : struct, IConstant {
        public static void RoundDivTest() {
            Random random = new(5678);

            List<(BigUInt<N> b, BigInteger n)> vs = new();
            List<(BigUInt<M> b, BigInteger n)> us = new();

            for (int i = 0; i < 64; i++) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<N>.Length, random.Next(BigUInt<N>.Bits + 1));

                BigUInt<N> b = new(bits, enable_clone: false);

                vs.Add((b, (BigInteger)b));
            }
            vs.Add((BigUInt<N>.Full, (BigInteger)BigUInt<N>.Full));
            vs.Add((BigUInt<N>.Zero, (BigInteger)BigUInt<N>.Zero));

            for (int i = 0; i < 64; i++) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<M>.Length, random.Next(BigUInt<M>.Bits + 1));

                BigUInt<M> b = new(bits, enable_clone: false);

                us.Add((b, (BigInteger)b));
            }
            us.Add((BigUInt<M>.Full, (BigInteger)BigUInt<M>.Full));
            us.Add((BigUInt<M>.Zero, (BigInteger)BigUInt<M>.Zero));

            Console.WriteLine($"length={BigUInt<N>.Length}");

            int normal_passes = 0, divzero_passes = 0;

            foreach ((BigUInt<M> va, BigInteger a) in us) {
                foreach ((BigUInt<N> vb, BigInteger b) in vs) {
                    if (b > 0) {
                        BigInteger n = a / b + (((a % b) * 2 >= b) ? 1 : 0);

                        Assert.AreEqual(n, (BigInteger)BigUInt<N>.RoundDiv(va, vb), $"{va}*{vb}");

                        normal_passes++;
                    }
                    else {
                        Assert.ThrowsException<DivideByZeroException>(() => {
                            _ = BigUInt<N>.RoundDiv(va, vb);
                        });

                        divzero_passes++;
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(divzero_passes)}: {divzero_passes}");
        }

        public static void RoundDivFullTest() {
            Random random = new(5678);

            List<(BigUInt<N> b, BigInteger n)> vs = new();
            List<(BigUInt<M> b, BigInteger n)> us = new();

            for (int cnt = 0; cnt < BigUInt<N>.Length; cnt++) {
                UInt32[] bits_full = (new UInt32[BigUInt<N>.Length]).Select((_, idx) => idx >= cnt ? ~0u : 0u).ToArray();
                UInt32[] bits_8000 = (new UInt32[BigUInt<N>.Length]).Select((_, idx) => idx >= cnt ? 0x80000000u : 0u).ToArray();
                UInt32[] bits_7FFF = (new UInt32[BigUInt<N>.Length]).Select((_, idx) => idx >= cnt ? 0x7FFFFFFFu : 0u).ToArray();

                BigUInt<N> b_full = new(bits_full, enable_clone: false);
                BigUInt<N> b_8000 = new(bits_full, enable_clone: false);
                BigUInt<N> b_7FFF = new(bits_full, enable_clone: false);

                vs.Add((b_full, (BigInteger)b_full));
                vs.Add((b_8000, (BigInteger)b_8000));
                vs.Add((b_7FFF, (BigInteger)b_7FFF));
            }

            for (int cnt = 0; cnt < BigUInt<N>.Length; cnt++) {
                UInt32[] bits_full = (new UInt32[BigUInt<M>.Length]).Select((_, idx) => idx >= cnt ? ~0u : 0u).ToArray();
                UInt32[] bits_8000 = (new UInt32[BigUInt<M>.Length]).Select((_, idx) => idx >= cnt ? 0x80000000u : 0u).ToArray();
                UInt32[] bits_7FFF = (new UInt32[BigUInt<M>.Length]).Select((_, idx) => idx >= cnt ? 0x7FFFFFFFu : 0u).ToArray();

                BigUInt<M> b_full = new(bits_full, enable_clone: false);
                BigUInt<M> b_8000 = new(bits_full, enable_clone: false);
                BigUInt<M> b_7FFF = new(bits_full, enable_clone: false);

                us.Add((b_full, (BigInteger)b_full));
                us.Add((b_8000, (BigInteger)b_8000));
                us.Add((b_7FFF, (BigInteger)b_7FFF));
            }

            Console.WriteLine($"length={BigUInt<N>.Length}");

            int normal_passes = 0, divzero_passes = 0;

            foreach ((BigUInt<M> va, BigInteger a) in us) {
                foreach ((BigUInt<N> vb, BigInteger b) in vs) {
                    if (b > 0) {
                        BigInteger n = a / b + (((a % b) * 2 >= b) ? 1 : 0);

                        Assert.AreEqual(n, (BigInteger)BigUInt<N>.RoundDiv(va, vb), $"{va}*{vb}");

                        normal_passes++;
                    }
                    else {
                        Assert.ThrowsException<DivideByZeroException>(() => {
                            _ = BigUInt<N>.RoundDiv(va, vb);
                        });

                        divzero_passes++;
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(divzero_passes)}: {divzero_passes}");
        }
    }

    public static class RoundDivTests<N> where N : struct, IConstant {
        public static void RoundDivTest() {
            Random random = new(5678);

            List<(BigUInt<N> b, BigInteger n)> vs = new();
            List<(BigUInt<N> b, BigInteger n)> us = new();

            for (int i = 0; i < 64; i++) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<N>.Length, random.Next(BigUInt<N>.Bits + 1));

                BigUInt<N> b = new(bits, enable_clone: false);

                vs.Add((b, (BigInteger)b));
            }            
            for (int i = 0; i < 64; i++) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<N>.Length, UIntUtil.UInt64Bits);

                BigUInt<N> b = new(bits, enable_clone: false);

                vs.Add((b, (BigInteger)b));
            }
            vs.Add((BigUInt<N>.Full, (BigInteger)BigUInt<N>.Full));
            vs.Add((BigUInt<N>.Zero, (BigInteger)BigUInt<N>.Zero));

            for (int i = 0; i < 64; i++) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<N>.Length, random.Next(BigUInt<N>.Bits + 1));

                BigUInt<N> b = new(bits, enable_clone: false);

                us.Add((b, (BigInteger)b));
            }
            us.Add((BigUInt<N>.Full, (BigInteger)BigUInt<N>.Full));
            us.Add((BigUInt<N>.Zero, (BigInteger)BigUInt<N>.Zero));

            Console.WriteLine($"length={BigUInt<N>.Length}");

            int normal_passes = 0, divzero_passes = 0;

            foreach ((BigUInt<N> va, BigInteger a) in us) {
                foreach ((BigUInt<N> vb, BigInteger b) in vs) {
                    if (b > 0) {
                        BigInteger n = a / b + (((a % b) * 2 >= b) ? 1 : 0);

                        Assert.AreEqual(n, (BigInteger)BigUInt<N>.RoundDiv(va, vb), $"{va}/{vb}");

                        if (vb.Digits <= 2) {
                            Assert.AreEqual(n, (BigInteger)BigUInt<N>.RoundDiv(va, UIntUtil.Pack(vb.Value[1], vb.Value[0])), $"{va}/{vb}");
                        }

                        normal_passes++;
                    }
                    else {
                        Assert.ThrowsException<DivideByZeroException>(() => {
                            _ = BigUInt<N>.RoundDiv(va, vb);
                        });

                        divzero_passes++;
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(divzero_passes)}: {divzero_passes}");
        }

        public static void RoundDivFullTest() {
            Random random = new(5678);

            List<(BigUInt<N> b, BigInteger n)> vs = new();
            List<(BigUInt<N> b, BigInteger n)> us = new();

            for (int cnt = 0; cnt < BigUInt<N>.Length; cnt++) {
                UInt32[] bits_full = (new UInt32[BigUInt<N>.Length]).Select((_, idx) => idx >= cnt ? ~0u : 0u).ToArray();
                UInt32[] bits_8000 = (new UInt32[BigUInt<N>.Length]).Select((_, idx) => idx >= cnt ? 0x80000000u : 0u).ToArray();
                UInt32[] bits_7FFF = (new UInt32[BigUInt<N>.Length]).Select((_, idx) => idx >= cnt ? 0x7FFFFFFFu : 0u).ToArray();

                BigUInt<N> b_full = new(bits_full, enable_clone: false);
                BigUInt<N> b_8000 = new(bits_full, enable_clone: false);
                BigUInt<N> b_7FFF = new(bits_full, enable_clone: false);

                vs.Add((b_full, (BigInteger)b_full));
                vs.Add((b_8000, (BigInteger)b_8000));
                vs.Add((b_7FFF, (BigInteger)b_7FFF));
            }

            for (int cnt = 0; cnt < BigUInt<N>.Length; cnt++) {
                UInt32[] bits_full = (new UInt32[BigUInt<N>.Length]).Select((_, idx) => idx >= cnt ? ~0u : 0u).ToArray();
                UInt32[] bits_8000 = (new UInt32[BigUInt<N>.Length]).Select((_, idx) => idx >= cnt ? 0x80000000u : 0u).ToArray();
                UInt32[] bits_7FFF = (new UInt32[BigUInt<N>.Length]).Select((_, idx) => idx >= cnt ? 0x7FFFFFFFu : 0u).ToArray();

                BigUInt<N> b_full = new(bits_full, enable_clone: false);
                BigUInt<N> b_8000 = new(bits_full, enable_clone: false);
                BigUInt<N> b_7FFF = new(bits_full, enable_clone: false);

                us.Add((b_full, (BigInteger)b_full));
                us.Add((b_8000, (BigInteger)b_8000));
                us.Add((b_7FFF, (BigInteger)b_7FFF));
            }

            Console.WriteLine($"length={BigUInt<N>.Length}");

            int normal_passes = 0, divzero_passes = 0;

            foreach ((BigUInt<N> va, BigInteger a) in us) {
                foreach ((BigUInt<N> vb, BigInteger b) in vs) {
                    if (b > 0) {
                        BigInteger n = a / b + (((a % b) * 2 >= b) ? 1 : 0);

                        Assert.AreEqual(n, (BigInteger)BigUInt<N>.RoundDiv(va, vb), $"{va}*{vb}");

                        normal_passes++;
                    }
                    else {
                        Assert.ThrowsException<DivideByZeroException>(() => {
                            _ = BigUInt<N>.RoundDiv(va, vb);
                        });

                        divzero_passes++;
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(divzero_passes)}: {divzero_passes}");
        }
    }

    [TestClass]
    public class RoundDivTests {
        [TestMethod]
        public void RoundDivTest() {
            RoundDivTests<N4>.RoundDivTest();
            RoundDivTests<N5>.RoundDivTest();
            RoundDivTests<N6>.RoundDivTest();
            RoundDivTests<N7>.RoundDivTest();
            RoundDivTests<N8>.RoundDivTest();
            RoundDivTests<N9>.RoundDivTest();
            RoundDivTests<N10>.RoundDivTest();
            RoundDivTests<N11>.RoundDivTest();
            RoundDivTests<N12>.RoundDivTest();
            RoundDivTests<N13>.RoundDivTest();
            RoundDivTests<N14>.RoundDivTest();
            RoundDivTests<N15>.RoundDivTest();
            RoundDivTests<N16>.RoundDivTest();
            RoundDivTests<N17>.RoundDivTest();
            RoundDivTests<N18>.RoundDivTest();
            RoundDivTests<N19>.RoundDivTest();
            RoundDivTests<N20>.RoundDivTest();
            RoundDivTests<N21>.RoundDivTest();
            RoundDivTests<N22>.RoundDivTest();
            RoundDivTests<N23>.RoundDivTest();
            RoundDivTests<N24>.RoundDivTest();
            RoundDivTests<N25>.RoundDivTest();
            RoundDivTests<N31>.RoundDivTest();
            RoundDivTests<N32>.RoundDivTest();
            RoundDivTests<N33>.RoundDivTest();
            RoundDivTests<N47>.RoundDivTest();
            RoundDivTests<N48>.RoundDivTest();
            RoundDivTests<N50>.RoundDivTest();
            RoundDivTests<N53>.RoundDivTest();
            RoundDivTests<N56>.RoundDivTest();
            RoundDivTests<N59>.RoundDivTest();
            RoundDivTests<N63>.RoundDivTest();
            RoundDivTests<N64>.RoundDivTest();

            RoundDivTests<N4, N5>.RoundDivTest();
            RoundDivTests<N5, N6>.RoundDivTest();
            RoundDivTests<N6, N7>.RoundDivTest();
            RoundDivTests<N7, N8>.RoundDivTest();
            RoundDivTests<N8, N9>.RoundDivTest();
            RoundDivTests<N9, N10>.RoundDivTest();
            RoundDivTests<N10, N11>.RoundDivTest();
            RoundDivTests<N11, N12>.RoundDivTest();
            RoundDivTests<N12, N13>.RoundDivTest();
            RoundDivTests<N13, N14>.RoundDivTest();
            RoundDivTests<N14, N15>.RoundDivTest();
            RoundDivTests<N15, N16>.RoundDivTest();
            RoundDivTests<N16, N17>.RoundDivTest();
            RoundDivTests<N17, N18>.RoundDivTest();
            RoundDivTests<N18, N19>.RoundDivTest();
            RoundDivTests<N19, N20>.RoundDivTest();
            RoundDivTests<N20, N21>.RoundDivTest();
            RoundDivTests<N21, N22>.RoundDivTest();
            RoundDivTests<N22, N23>.RoundDivTest();
            RoundDivTests<N23, N24>.RoundDivTest();
            RoundDivTests<N24, N25>.RoundDivTest();
            RoundDivTests<N25, N31>.RoundDivTest();
            RoundDivTests<N31, N32>.RoundDivTest();
            RoundDivTests<N32, N33>.RoundDivTest();
            RoundDivTests<N33, N47>.RoundDivTest();
            RoundDivTests<N47, N48>.RoundDivTest();
            RoundDivTests<N48, N50>.RoundDivTest();
            RoundDivTests<N50, N53>.RoundDivTest();
            RoundDivTests<N53, N56>.RoundDivTest();
            RoundDivTests<N56, N59>.RoundDivTest();
            RoundDivTests<N59, N63>.RoundDivTest();
            RoundDivTests<N63, N64>.RoundDivTest();
            RoundDivTests<N64, N65>.RoundDivTest();
        }

        [TestMethod]
        public void RoundDivFullTest() {
            RoundDivTests<N4>.RoundDivFullTest();
            RoundDivTests<N5>.RoundDivFullTest();
            RoundDivTests<N6>.RoundDivFullTest();
            RoundDivTests<N7>.RoundDivFullTest();
            RoundDivTests<N8>.RoundDivFullTest();
            RoundDivTests<N9>.RoundDivFullTest();
            RoundDivTests<N10>.RoundDivFullTest();
            RoundDivTests<N11>.RoundDivFullTest();
            RoundDivTests<N12>.RoundDivFullTest();
            RoundDivTests<N13>.RoundDivFullTest();
            RoundDivTests<N14>.RoundDivFullTest();
            RoundDivTests<N15>.RoundDivFullTest();
            RoundDivTests<N16>.RoundDivFullTest();
            RoundDivTests<N17>.RoundDivFullTest();
            RoundDivTests<N18>.RoundDivFullTest();
            RoundDivTests<N19>.RoundDivFullTest();
            RoundDivTests<N20>.RoundDivFullTest();
            RoundDivTests<N21>.RoundDivFullTest();
            RoundDivTests<N22>.RoundDivFullTest();
            RoundDivTests<N23>.RoundDivFullTest();
            RoundDivTests<N24>.RoundDivFullTest();
            RoundDivTests<N25>.RoundDivFullTest();
            RoundDivTests<N31>.RoundDivFullTest();
            RoundDivTests<N32>.RoundDivFullTest();
            RoundDivTests<N33>.RoundDivFullTest();
            RoundDivTests<N47>.RoundDivFullTest();
            RoundDivTests<N48>.RoundDivFullTest();
            RoundDivTests<N50>.RoundDivFullTest();
            RoundDivTests<N53>.RoundDivFullTest();
            RoundDivTests<N56>.RoundDivFullTest();
            RoundDivTests<N59>.RoundDivFullTest();
            RoundDivTests<N63>.RoundDivFullTest();
            RoundDivTests<N64>.RoundDivFullTest();

            RoundDivTests<N4, N5>.RoundDivFullTest();
            RoundDivTests<N5, N6>.RoundDivFullTest();
            RoundDivTests<N6, N7>.RoundDivFullTest();
            RoundDivTests<N7, N8>.RoundDivFullTest();
            RoundDivTests<N8, N9>.RoundDivFullTest();
            RoundDivTests<N9, N10>.RoundDivFullTest();
            RoundDivTests<N10, N11>.RoundDivFullTest();
            RoundDivTests<N11, N12>.RoundDivFullTest();
            RoundDivTests<N12, N13>.RoundDivFullTest();
            RoundDivTests<N13, N14>.RoundDivFullTest();
            RoundDivTests<N14, N15>.RoundDivFullTest();
            RoundDivTests<N15, N16>.RoundDivFullTest();
            RoundDivTests<N16, N17>.RoundDivFullTest();
            RoundDivTests<N17, N18>.RoundDivFullTest();
            RoundDivTests<N18, N19>.RoundDivFullTest();
            RoundDivTests<N19, N20>.RoundDivFullTest();
            RoundDivTests<N20, N21>.RoundDivFullTest();
            RoundDivTests<N21, N22>.RoundDivFullTest();
            RoundDivTests<N22, N23>.RoundDivFullTest();
            RoundDivTests<N23, N24>.RoundDivFullTest();
            RoundDivTests<N24, N25>.RoundDivFullTest();
            RoundDivTests<N25, N31>.RoundDivFullTest();
            RoundDivTests<N31, N32>.RoundDivFullTest();
            RoundDivTests<N32, N33>.RoundDivFullTest();
            RoundDivTests<N33, N47>.RoundDivFullTest();
            RoundDivTests<N47, N48>.RoundDivFullTest();
            RoundDivTests<N48, N50>.RoundDivFullTest();
            RoundDivTests<N50, N53>.RoundDivFullTest();
            RoundDivTests<N53, N56>.RoundDivFullTest();
            RoundDivTests<N56, N59>.RoundDivFullTest();
            RoundDivTests<N59, N63>.RoundDivFullTest();
            RoundDivTests<N63, N64>.RoundDivFullTest();
            RoundDivTests<N64, N65>.RoundDivFullTest();
        }
    }
}

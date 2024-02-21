using AvxUInt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace AvxUIntTest {
    public static class ExpandMulTests<N> where N : struct, IConstant {
        public static void ExpandMulTest() {
            Random random = new(5678);

            List<(BigUInt<N> b, BigInteger n)> vs = new();

            for (int i = 0; i < 64; i++) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<N>.Length, random.Next(BigUInt<N>.Bits + 1));

                BigUInt<N> b = new(bits, enable_clone: false);

                vs.Add((b, (BigInteger)b));
            }
            vs.Add((BigUInt<N>.Full, (BigInteger)BigUInt<N>.Full));
            vs.Add((BigUInt<N>.Zero, (BigInteger)BigUInt<N>.Zero));

            BigInteger maxn = BigUInt<Plus2<N>>.Full;

            Console.WriteLine($"length={BigUInt<N>.Length}");

            int normal_passes = 0, overflow_passes = 0;

            foreach ((BigUInt<N> va, BigInteger a) in vs) {
                foreach ((BigUInt<N> vb, BigInteger b) in vs) {
                    BigInteger n = a * b;

                    if (n <= maxn) {
                        Assert.AreEqual(n, (BigInteger)BigUInt<N>.Mul<Plus2<N>>(va, vb), $"{va}*{vb}");

                        normal_passes++;
                    }
                    else {
                        Assert.ThrowsException<OverflowException>(() => {
                            _ = BigUInt<N>.Mul<Plus2<N>>(va, vb);
                        });

                        overflow_passes++;
                    }
                }
            }

            Console.WriteLine($"{nameof(normal_passes)}: {normal_passes}");
            Console.WriteLine($"{nameof(overflow_passes)}: {overflow_passes}");
        }
    }

    [TestClass]
    public class ExpandMulTests {
        [TestMethod]
        public void ExpandMulTest() {
            ExpandMulTests<N4>.ExpandMulTest();
            ExpandMulTests<N5>.ExpandMulTest();
            ExpandMulTests<N6>.ExpandMulTest();
            ExpandMulTests<N7>.ExpandMulTest();
            ExpandMulTests<N8>.ExpandMulTest();
            ExpandMulTests<N9>.ExpandMulTest();
            ExpandMulTests<N10>.ExpandMulTest();
            ExpandMulTests<N11>.ExpandMulTest();
            ExpandMulTests<N12>.ExpandMulTest();
            ExpandMulTests<N13>.ExpandMulTest();
            ExpandMulTests<N14>.ExpandMulTest();
            ExpandMulTests<N15>.ExpandMulTest();
            ExpandMulTests<N16>.ExpandMulTest();
            ExpandMulTests<N17>.ExpandMulTest();
            ExpandMulTests<N18>.ExpandMulTest();
            ExpandMulTests<N19>.ExpandMulTest();
            ExpandMulTests<N20>.ExpandMulTest();
            ExpandMulTests<N21>.ExpandMulTest();
            ExpandMulTests<N22>.ExpandMulTest();
            ExpandMulTests<N23>.ExpandMulTest();
            ExpandMulTests<N24>.ExpandMulTest();
            ExpandMulTests<N25>.ExpandMulTest();
            ExpandMulTests<N31>.ExpandMulTest();
            ExpandMulTests<N32>.ExpandMulTest();
            ExpandMulTests<N33>.ExpandMulTest();
            ExpandMulTests<N47>.ExpandMulTest();
            ExpandMulTests<N48>.ExpandMulTest();
            ExpandMulTests<N50>.ExpandMulTest();
            ExpandMulTests<N53>.ExpandMulTest();
            ExpandMulTests<N56>.ExpandMulTest();
            ExpandMulTests<N59>.ExpandMulTest();
            ExpandMulTests<N63>.ExpandMulTest();
            ExpandMulTests<N64>.ExpandMulTest();
            ExpandMulTests<N65>.ExpandMulTest();
        }
    }
}

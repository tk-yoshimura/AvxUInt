using AvxUInt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;
using System.Numerics;

namespace AvxUIntTest {
    public static class CmpTests<N> where N : struct, IConstant {
        private static readonly int length = default(N).Value;

        private static readonly ReadOnlyCollection<(BigUInt<N> bits, BigInteger bint)> tests;
        static CmpTests() {
            List<(BigUInt<N> bits, BigInteger bint)> vs = new();
            for (uint v = ~0u; v > 0; v /= 15) {
                for (int s = 1; s <= length / 2; s++) {
                    UInt32[] bits = new UInt32[length];

                    for (int k = 0; k < length; k += s) {
                        bits[k] = v;
                    }

                    BigUInt<N> b = new(bits);

                    vs.Add((b, (BigInteger)b));
                }
            }

            Random random = new(1234);
            for (int i = 0; i < 32; i++) {
                UInt32[] bits = UIntUtil.Random(random, length, random.Next(length * UIntUtil.UInt32Bits + 1));
                UInt32[] bits_swapbit = (UInt32[])bits.Clone();
                bits_swapbit[random.Next(length)] ^= 1u << random.Next(UIntUtil.UInt32Bits);

                BigUInt<N> b = new(bits), b_swapbit = new(bits_swapbit);
                vs.Add((b, (BigInteger)b));
                vs.Add((b_swapbit, (BigInteger)b_swapbit));
            }

            Console.WriteLine($"{nameof(length)}={length}: {vs.Count} testcases");

            tests = Array.AsReadOnly(vs.ToArray());
        }

        public static void EqualTest() {
            foreach ((BigUInt<N> n1, BigInteger b1) in tests) {
                foreach ((BigUInt<N> n2, BigInteger b2) in tests) {
                    Assert.AreEqual(b1 == b2, n1 == n2, $"\n{length}\n{n1.ToHexcode()}\n{n2.ToHexcode()}");
                }
            }
        }

        public static void LessThanOrEqualTest() {
            foreach ((BigUInt<N> n1, BigInteger b1) in tests) {
                foreach ((BigUInt<N> n2, BigInteger b2) in tests) {
                    Assert.AreEqual(b1 <= b2, n1 <= n2, $"\n{length}\n{n1.ToHexcode()}\n{n2.ToHexcode()}");
                }
            }
        }

        public static void GreaterThanOrEqualTest() {
            foreach ((BigUInt<N> n1, BigInteger b1) in tests) {
                foreach ((BigUInt<N> n2, BigInteger b2) in tests) {
                    Assert.AreEqual(b1 >= b2, n1 >= n2, $"\n{length}\n{n1.ToHexcode()}\n{n2.ToHexcode()}");
                }
            }
        }

        public static void NotEqualTest() {
            foreach ((BigUInt<N> n1, BigInteger b1) in tests) {
                foreach ((BigUInt<N> n2, BigInteger b2) in tests) {
                    Assert.AreEqual(b1 != b2, n1 != n2, $"\n{length}\n{n1.ToHexcode()}\n{n2.ToHexcode()}");
                }
            }
        }

        public static void LessThanTest() {
            foreach ((BigUInt<N> n1, BigInteger b1) in tests) {
                foreach ((BigUInt<N> n2, BigInteger b2) in tests) {
                    Assert.AreEqual(b1 < b2, n1 < n2, $"\n{length}\n{n1.ToHexcode()}\n{n2.ToHexcode()}");
                }
            }
        }

        public static void GreaterThanTest() {
            foreach ((BigUInt<N> n1, BigInteger b1) in tests) {
                foreach ((BigUInt<N> n2, BigInteger b2) in tests) {
                    Assert.AreEqual(b1 > b2, n1 > n2, $"\n{length}\n{n1.ToHexcode()}\n{n2.ToHexcode()}");
                }
            }
        }

        public static void IsZeroTest() {
            BigInteger zero = 0;
            Assert.IsTrue(zero == BigUInt<N>.Zero);

            foreach ((BigUInt<N> n1, BigInteger b1) in tests) {
                Assert.AreEqual(b1 == zero, n1.IsZero);
            }
        }

        public static void IsFullTest() {
            BigInteger full = (BigInteger.One << (length * UIntUtil.UInt32Bits)) - 1;
            Assert.IsTrue(full == BigUInt<N>.Full);

            foreach ((BigUInt<N> n1, BigInteger b1) in tests) {
                Assert.AreEqual(b1 == full, n1.IsFull);
            }
        }
    }

    [TestClass]
    public class CmpTests {
        [TestMethod]
        public void EqualN8Test() {
            CmpTests<N4>.EqualTest();
            CmpTests<N5>.EqualTest();
            CmpTests<N6>.EqualTest();
            CmpTests<N7>.EqualTest();
            CmpTests<N8>.EqualTest();
        }

        [TestMethod]
        public void EqualN16Test() {
            CmpTests<N9>.EqualTest();
            CmpTests<N10>.EqualTest();
            CmpTests<N11>.EqualTest();
            CmpTests<N12>.EqualTest();
            CmpTests<N13>.EqualTest();
            CmpTests<N14>.EqualTest();
            CmpTests<N15>.EqualTest();
            CmpTests<N16>.EqualTest();
        }

        [TestMethod]
        public void EqualN32Test() {
            CmpTests<N17>.EqualTest();
            CmpTests<N23>.EqualTest();
            CmpTests<N24>.EqualTest();
            CmpTests<N25>.EqualTest();
            CmpTests<N31>.EqualTest();
            CmpTests<N32>.EqualTest();
        }

        [TestMethod]
        public void EqualN64Test() {
            CmpTests<N33>.EqualTest();
            CmpTests<N47>.EqualTest();
            CmpTests<N48>.EqualTest();
            CmpTests<N50>.EqualTest();
            CmpTests<N53>.EqualTest();
            CmpTests<N56>.EqualTest();
            CmpTests<N59>.EqualTest();
            CmpTests<N63>.EqualTest();
            CmpTests<N64>.EqualTest();
            CmpTests<N65>.EqualTest();
        }

        [TestMethod]
        public void LessThanOrEqualN8Test() {
            CmpTests<N4>.LessThanOrEqualTest();
            CmpTests<N5>.LessThanOrEqualTest();
            CmpTests<N6>.LessThanOrEqualTest();
            CmpTests<N7>.LessThanOrEqualTest();
            CmpTests<N8>.LessThanOrEqualTest();
        }

        [TestMethod]
        public void LessThanOrEqualN16Test() {
            CmpTests<N9>.LessThanOrEqualTest();
            CmpTests<N10>.LessThanOrEqualTest();
            CmpTests<N11>.LessThanOrEqualTest();
            CmpTests<N12>.LessThanOrEqualTest();
            CmpTests<N13>.LessThanOrEqualTest();
            CmpTests<N14>.LessThanOrEqualTest();
            CmpTests<N15>.LessThanOrEqualTest();
            CmpTests<N16>.LessThanOrEqualTest();
        }

        [TestMethod]
        public void LessThanOrEqualN32Test() {
            CmpTests<N17>.LessThanOrEqualTest();
            CmpTests<N23>.LessThanOrEqualTest();
            CmpTests<N24>.LessThanOrEqualTest();
            CmpTests<N25>.LessThanOrEqualTest();
            CmpTests<N31>.LessThanOrEqualTest();
            CmpTests<N32>.LessThanOrEqualTest();
        }

        [TestMethod]
        public void LessThanOrEqualN64Test() {
            CmpTests<N33>.LessThanOrEqualTest();
            CmpTests<N47>.LessThanOrEqualTest();
            CmpTests<N48>.LessThanOrEqualTest();
            CmpTests<N50>.LessThanOrEqualTest();
            CmpTests<N53>.LessThanOrEqualTest();
            CmpTests<N56>.LessThanOrEqualTest();
            CmpTests<N59>.LessThanOrEqualTest();
            CmpTests<N63>.LessThanOrEqualTest();
            CmpTests<N64>.LessThanOrEqualTest();
            CmpTests<N65>.LessThanOrEqualTest();
        }

        [TestMethod]
        public void GreaterThanOrEqualN8Test() {
            CmpTests<N4>.GreaterThanOrEqualTest();
            CmpTests<N5>.GreaterThanOrEqualTest();
            CmpTests<N6>.GreaterThanOrEqualTest();
            CmpTests<N7>.GreaterThanOrEqualTest();
            CmpTests<N8>.GreaterThanOrEqualTest();
        }

        [TestMethod]
        public void GreaterThanOrEqualN16Test() {
            CmpTests<N9>.GreaterThanOrEqualTest();
            CmpTests<N10>.GreaterThanOrEqualTest();
            CmpTests<N11>.GreaterThanOrEqualTest();
            CmpTests<N12>.GreaterThanOrEqualTest();
            CmpTests<N13>.GreaterThanOrEqualTest();
            CmpTests<N14>.GreaterThanOrEqualTest();
            CmpTests<N15>.GreaterThanOrEqualTest();
            CmpTests<N16>.GreaterThanOrEqualTest();
        }

        [TestMethod]
        public void GreaterThanOrEqualN32Test() {
            CmpTests<N17>.GreaterThanOrEqualTest();
            CmpTests<N23>.GreaterThanOrEqualTest();
            CmpTests<N24>.GreaterThanOrEqualTest();
            CmpTests<N25>.GreaterThanOrEqualTest();
            CmpTests<N31>.GreaterThanOrEqualTest();
            CmpTests<N32>.GreaterThanOrEqualTest();
        }

        [TestMethod]
        public void GreaterThanOrEqualN64Test() {
            CmpTests<N33>.GreaterThanOrEqualTest();
            CmpTests<N47>.GreaterThanOrEqualTest();
            CmpTests<N48>.GreaterThanOrEqualTest();
            CmpTests<N50>.GreaterThanOrEqualTest();
            CmpTests<N53>.GreaterThanOrEqualTest();
            CmpTests<N56>.GreaterThanOrEqualTest();
            CmpTests<N59>.GreaterThanOrEqualTest();
            CmpTests<N63>.GreaterThanOrEqualTest();
            CmpTests<N64>.GreaterThanOrEqualTest();
            CmpTests<N65>.GreaterThanOrEqualTest();
        }

        [TestMethod]
        public void NotEqualN8Test() {
            CmpTests<N4>.NotEqualTest();
            CmpTests<N5>.NotEqualTest();
            CmpTests<N6>.NotEqualTest();
            CmpTests<N7>.NotEqualTest();
            CmpTests<N8>.NotEqualTest();
        }

        [TestMethod]
        public void NotEqualN16Test() {
            CmpTests<N9>.NotEqualTest();
            CmpTests<N10>.NotEqualTest();
            CmpTests<N11>.NotEqualTest();
            CmpTests<N12>.NotEqualTest();
            CmpTests<N13>.NotEqualTest();
            CmpTests<N14>.NotEqualTest();
            CmpTests<N15>.NotEqualTest();
            CmpTests<N16>.NotEqualTest();
        }

        [TestMethod]
        public void NotEqualN32Test() {
            CmpTests<N17>.NotEqualTest();
            CmpTests<N23>.NotEqualTest();
            CmpTests<N24>.NotEqualTest();
            CmpTests<N25>.NotEqualTest();
            CmpTests<N31>.NotEqualTest();
            CmpTests<N32>.NotEqualTest();
            CmpTests<N33>.NotEqualTest();
        }

        [TestMethod]
        public void LessThanN8Test() {
            CmpTests<N4>.LessThanTest();
            CmpTests<N5>.LessThanTest();
            CmpTests<N6>.LessThanTest();
            CmpTests<N7>.LessThanTest();
            CmpTests<N8>.LessThanTest();
        }

        [TestMethod]
        public void LessThanN16Test() {
            CmpTests<N9>.LessThanTest();
            CmpTests<N10>.LessThanTest();
            CmpTests<N11>.LessThanTest();
            CmpTests<N12>.LessThanTest();
            CmpTests<N13>.LessThanTest();
            CmpTests<N14>.LessThanTest();
            CmpTests<N15>.LessThanTest();
            CmpTests<N16>.LessThanTest();
        }

        [TestMethod]
        public void LessThanN32Test() {
            CmpTests<N17>.LessThanTest();
            CmpTests<N23>.LessThanTest();
            CmpTests<N24>.LessThanTest();
            CmpTests<N25>.LessThanTest();
            CmpTests<N31>.LessThanTest();
            CmpTests<N32>.LessThanTest();
            CmpTests<N33>.LessThanTest();
        }

        [TestMethod]
        public void GreaterThanN8Test() {
            CmpTests<N4>.GreaterThanTest();
            CmpTests<N5>.GreaterThanTest();
            CmpTests<N6>.GreaterThanTest();
            CmpTests<N7>.GreaterThanTest();
            CmpTests<N8>.GreaterThanTest();
        }

        [TestMethod]
        public void GreaterThanN16Test() {
            CmpTests<N9>.GreaterThanTest();
            CmpTests<N10>.GreaterThanTest();
            CmpTests<N11>.GreaterThanTest();
            CmpTests<N12>.GreaterThanTest();
            CmpTests<N13>.GreaterThanTest();
            CmpTests<N14>.GreaterThanTest();
            CmpTests<N15>.GreaterThanTest();
            CmpTests<N16>.GreaterThanTest();
        }

        [TestMethod]
        public void GreaterThanN32Test() {
            CmpTests<N17>.GreaterThanTest();
            CmpTests<N23>.GreaterThanTest();
            CmpTests<N24>.GreaterThanTest();
            CmpTests<N25>.GreaterThanTest();
            CmpTests<N31>.GreaterThanTest();
            CmpTests<N32>.GreaterThanTest();
            CmpTests<N33>.GreaterThanTest();
        }

        [TestMethod]
        public void IsZeroN8Test() {
            CmpTests<N4>.IsZeroTest();
            CmpTests<N5>.IsZeroTest();
            CmpTests<N6>.IsZeroTest();
            CmpTests<N7>.IsZeroTest();
            CmpTests<N8>.IsZeroTest();
        }

        [TestMethod]
        public void IsZeroN16Test() {
            CmpTests<N9>.IsZeroTest();
            CmpTests<N10>.IsZeroTest();
            CmpTests<N11>.IsZeroTest();
            CmpTests<N12>.IsZeroTest();
            CmpTests<N13>.IsZeroTest();
            CmpTests<N14>.IsZeroTest();
            CmpTests<N15>.IsZeroTest();
            CmpTests<N16>.IsZeroTest();
        }

        [TestMethod]
        public void IsZeroN32Test() {
            CmpTests<N17>.IsZeroTest();
            CmpTests<N23>.IsZeroTest();
            CmpTests<N24>.IsZeroTest();
            CmpTests<N25>.IsZeroTest();
            CmpTests<N31>.IsZeroTest();
            CmpTests<N32>.IsZeroTest();
            CmpTests<N33>.IsZeroTest();
        }

        [TestMethod]
        public void IsFullN8Test() {
            CmpTests<N4>.IsFullTest();
            CmpTests<N5>.IsFullTest();
            CmpTests<N6>.IsFullTest();
            CmpTests<N7>.IsFullTest();
            CmpTests<N8>.IsFullTest();
        }

        [TestMethod]
        public void IsFullN16Test() {
            CmpTests<N9>.IsFullTest();
            CmpTests<N10>.IsFullTest();
            CmpTests<N11>.IsFullTest();
            CmpTests<N12>.IsFullTest();
            CmpTests<N13>.IsFullTest();
            CmpTests<N14>.IsFullTest();
            CmpTests<N15>.IsFullTest();
            CmpTests<N16>.IsFullTest();
        }

        [TestMethod]
        public void IsFullN32Test() {
            CmpTests<N17>.IsFullTest();
            CmpTests<N23>.IsFullTest();
            CmpTests<N24>.IsFullTest();
            CmpTests<N25>.IsFullTest();
            CmpTests<N31>.IsFullTest();
            CmpTests<N32>.IsFullTest();
            CmpTests<N33>.IsFullTest();
        }
    }
}

using AvxUInt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace AvxUIntTest {
    public static class ToStringTests<N> where N : struct, IConstant {
        public static void ToStringTest() {
            Random random = new(1234);

            for (int i = 0; i <= 2500; i++) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<N>.Length, random.Next(BigUInt<N>.Bits + 1));

                BigUInt<N> v = new(bits, enable_clone: false);
                BigInteger bi = v;

                Assert.AreEqual(bi.ToString(), v.ToString());

                Console.WriteLine(v);
            }

            Assert.AreEqual("0", BigUInt<N>.Zero.ToString());
        }

        public static void ToStringFullTest() {
            BigUInt<N> v = BigUInt<N>.Full;
            BigInteger bi = v;

            Assert.AreEqual(bi.ToString(), v.ToString());

            Console.WriteLine(v);
        }
    }

    [TestClass]
    public class ToStringTests {
        [TestMethod]
        public void ToStringTest() {
            ToStringTests<N4>.ToStringTest();
            ToStringTests<N5>.ToStringTest();
            ToStringTests<N6>.ToStringTest();
            ToStringTests<N7>.ToStringTest();
            ToStringTests<N8>.ToStringTest();
            ToStringTests<N9>.ToStringTest();
            ToStringTests<N10>.ToStringTest();
            ToStringTests<N11>.ToStringTest();
            ToStringTests<N12>.ToStringTest();
            ToStringTests<N13>.ToStringTest();
            ToStringTests<N14>.ToStringTest();
            ToStringTests<N15>.ToStringTest();
            ToStringTests<N16>.ToStringTest();
            ToStringTests<N17>.ToStringTest();
            ToStringTests<N18>.ToStringTest();
            ToStringTests<N19>.ToStringTest();
            ToStringTests<N20>.ToStringTest();
            ToStringTests<N21>.ToStringTest();
            ToStringTests<N22>.ToStringTest();
            ToStringTests<N23>.ToStringTest();
            ToStringTests<N24>.ToStringTest();
            ToStringTests<N25>.ToStringTest();
            ToStringTests<N31>.ToStringTest();
            ToStringTests<N32>.ToStringTest();
            ToStringTests<N33>.ToStringTest();
            ToStringTests<N47>.ToStringTest();
            ToStringTests<N48>.ToStringTest();
            ToStringTests<N50>.ToStringTest();
            ToStringTests<N53>.ToStringTest();
            ToStringTests<N56>.ToStringTest();
            ToStringTests<N59>.ToStringTest();
            ToStringTests<N63>.ToStringTest();
            ToStringTests<N64>.ToStringTest();
            ToStringTests<N65>.ToStringTest();
        }

        [TestMethod]
        public void ToStringFullTest() {
            ToStringTests<N4>.ToStringFullTest();
            ToStringTests<N5>.ToStringFullTest();
            ToStringTests<N6>.ToStringFullTest();
            ToStringTests<N7>.ToStringFullTest();
            ToStringTests<N8>.ToStringFullTest();
            ToStringTests<N9>.ToStringFullTest();
            ToStringTests<N10>.ToStringFullTest();
            ToStringTests<N11>.ToStringFullTest();
            ToStringTests<N12>.ToStringFullTest();
            ToStringTests<N13>.ToStringFullTest();
            ToStringTests<N14>.ToStringFullTest();
            ToStringTests<N15>.ToStringFullTest();
            ToStringTests<N16>.ToStringFullTest();
            ToStringTests<N17>.ToStringFullTest();
            ToStringTests<N18>.ToStringFullTest();
            ToStringTests<N19>.ToStringFullTest();
            ToStringTests<N20>.ToStringFullTest();
            ToStringTests<N21>.ToStringFullTest();
            ToStringTests<N22>.ToStringFullTest();
            ToStringTests<N23>.ToStringFullTest();
            ToStringTests<N24>.ToStringFullTest();
            ToStringTests<N25>.ToStringFullTest();
            ToStringTests<N31>.ToStringFullTest();
            ToStringTests<N32>.ToStringFullTest();
            ToStringTests<N33>.ToStringFullTest();
            ToStringTests<N47>.ToStringFullTest();
            ToStringTests<N48>.ToStringFullTest();
            ToStringTests<N50>.ToStringFullTest();
            ToStringTests<N53>.ToStringFullTest();
            ToStringTests<N56>.ToStringFullTest();
            ToStringTests<N59>.ToStringFullTest();
            ToStringTests<N63>.ToStringFullTest();
            ToStringTests<N64>.ToStringFullTest();
            ToStringTests<N65>.ToStringFullTest();
        }
    }
}

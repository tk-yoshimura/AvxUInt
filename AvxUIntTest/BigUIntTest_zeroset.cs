using AvxUInt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace AvxUIntTest {
    public static class ZerosetTests<N> where N : struct, IConstant {
        public static void ZerosetTest() {
            uint length = (uint)default(N).Value;

            UInt32[] n1 = BigUInt<N>.Full.Value.ToArray();
            UIntUtil.Zeroset(n1);

            UInt32[] n2 = BigUInt<N>.Full.Value.ToArray();
            UIntUtil.Zeroset(n2, 1, length - 1u);

            UInt32[] n3 = BigUInt<N>.Full.Value.ToArray();
            UIntUtil.Zeroset(n3, 2, 1);

            UInt32[] n4 = BigUInt<N>.Full.Value.ToArray();
            UIntUtil.Zeroset(n4, 3, 0);

            CollectionAssert.AreEqual(new UInt32[length], n1);
            CollectionAssert.AreEqual((new UInt32[length]).Select((_, idx) => idx < 1 ? ~0u : 0u).ToArray(), n2);
            CollectionAssert.AreEqual((new UInt32[length]).Select((_, idx) => idx == 2 ? 0u : ~0u).ToArray(), n3);
            CollectionAssert.AreEqual((new UInt32[length]).Select((_) => ~0u).ToArray(), n4);

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                UIntUtil.Zeroset(n2, 1, length);
            });

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => {
                UIntUtil.Zeroset(n2, 2, length - 1);
            });
        }

        public static void ZerosetLowerBitsTest() {
            Random random = new(1234);

            for (uint sft = 0; sft <= BigUInt<N>.Bits + 4; sft++) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<N>.Length, BigUInt<N>.Bits);
                BigUInt<N> v = new(bits, enable_clone: false);
                BigInteger n = v;

                UIntUtil.ZerosetLowerBit(bits, sft);
                BigUInt<N> u = new(bits, enable_clone: false);

                Assert.AreEqual((n >> (int)sft) << (int)sft, (BigInteger)u);
            }
        }
    }

    [TestClass]
    public class ZerosetTests {
        [TestMethod]
        public void ZerosetN8Test() {
            ZerosetTests<N4>.ZerosetTest();
            ZerosetTests<N5>.ZerosetTest();
            ZerosetTests<N6>.ZerosetTest();
            ZerosetTests<N7>.ZerosetTest();
            ZerosetTests<N8>.ZerosetTest();
        }

        [TestMethod]
        public void ZerosetN16Test() {
            ZerosetTests<N9>.ZerosetTest();
            ZerosetTests<N10>.ZerosetTest();
            ZerosetTests<N11>.ZerosetTest();
            ZerosetTests<N12>.ZerosetTest();
            ZerosetTests<N13>.ZerosetTest();
            ZerosetTests<N14>.ZerosetTest();
            ZerosetTests<N15>.ZerosetTest();
            ZerosetTests<N16>.ZerosetTest();
        }

        [TestMethod]
        public void ZerosetN32Test() {
            ZerosetTests<N17>.ZerosetTest();
            ZerosetTests<N18>.ZerosetTest();
            ZerosetTests<N19>.ZerosetTest();
            ZerosetTests<N20>.ZerosetTest();
            ZerosetTests<N21>.ZerosetTest();
            ZerosetTests<N22>.ZerosetTest();
            ZerosetTests<N23>.ZerosetTest();
            ZerosetTests<N24>.ZerosetTest();
            ZerosetTests<N25>.ZerosetTest();
            ZerosetTests<N31>.ZerosetTest();
            ZerosetTests<N32>.ZerosetTest();
        }

        [TestMethod]
        public void ZerosetN64Test() {
            ZerosetTests<N33>.ZerosetTest();
            ZerosetTests<N47>.ZerosetTest();
            ZerosetTests<N48>.ZerosetTest();
            ZerosetTests<N50>.ZerosetTest();
            ZerosetTests<N53>.ZerosetTest();
            ZerosetTests<N56>.ZerosetTest();
            ZerosetTests<N59>.ZerosetTest();
            ZerosetTests<N63>.ZerosetTest();
            ZerosetTests<N64>.ZerosetTest();
            ZerosetTests<N65>.ZerosetTest();
        }

        [TestMethod]
        public void ZerosetLowerBitsN8Test() {
            ZerosetTests<N4>.ZerosetLowerBitsTest();
            ZerosetTests<N5>.ZerosetLowerBitsTest();
            ZerosetTests<N6>.ZerosetLowerBitsTest();
            ZerosetTests<N7>.ZerosetLowerBitsTest();
            ZerosetTests<N8>.ZerosetLowerBitsTest();
        }

        [TestMethod]
        public void ZerosetLowerBitsN16Test() {
            ZerosetTests<N9>.ZerosetLowerBitsTest();
            ZerosetTests<N10>.ZerosetLowerBitsTest();
            ZerosetTests<N11>.ZerosetLowerBitsTest();
            ZerosetTests<N12>.ZerosetLowerBitsTest();
            ZerosetTests<N13>.ZerosetLowerBitsTest();
            ZerosetTests<N14>.ZerosetLowerBitsTest();
            ZerosetTests<N15>.ZerosetLowerBitsTest();
            ZerosetTests<N16>.ZerosetLowerBitsTest();
        }

        [TestMethod]
        public void ZerosetLowerBitsN32Test() {
            ZerosetTests<N17>.ZerosetLowerBitsTest();
            ZerosetTests<N18>.ZerosetLowerBitsTest();
            ZerosetTests<N19>.ZerosetLowerBitsTest();
            ZerosetTests<N20>.ZerosetLowerBitsTest();
            ZerosetTests<N21>.ZerosetLowerBitsTest();
            ZerosetTests<N22>.ZerosetLowerBitsTest();
            ZerosetTests<N23>.ZerosetLowerBitsTest();
            ZerosetTests<N24>.ZerosetLowerBitsTest();
            ZerosetTests<N25>.ZerosetLowerBitsTest();
            ZerosetTests<N31>.ZerosetLowerBitsTest();
            ZerosetTests<N32>.ZerosetLowerBitsTest();
        }

        [TestMethod]
        public void ZerosetLowerBitsN64Test() {
            ZerosetTests<N33>.ZerosetLowerBitsTest();
            ZerosetTests<N47>.ZerosetLowerBitsTest();
            ZerosetTests<N48>.ZerosetLowerBitsTest();
            ZerosetTests<N50>.ZerosetLowerBitsTest();
            ZerosetTests<N53>.ZerosetLowerBitsTest();
            ZerosetTests<N56>.ZerosetLowerBitsTest();
            ZerosetTests<N59>.ZerosetLowerBitsTest();
            ZerosetTests<N63>.ZerosetLowerBitsTest();
            ZerosetTests<N64>.ZerosetLowerBitsTest();
            ZerosetTests<N65>.ZerosetLowerBitsTest();
        }
    }
}

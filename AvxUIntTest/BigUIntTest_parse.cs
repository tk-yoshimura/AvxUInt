using AvxUInt;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AvxUIntTest {
    public static class ParseTests<N> where N : struct, IConstant {
        public static void ParseTest() {
            Random random = new(1234);

            for (int i = 0; i <= 2500; i++) {
                UInt32[] bits = UIntUtil.Random(random, BigUInt<N>.Length, random.Next(BigUInt<N>.Bits + 1));

                BigUInt<N> v = new(bits, enable_clone: false);
                BigUInt<N> v2 = new(v.ToString());

                Assert.AreEqual(v, v2);
            }

            Assert.AreEqual(BigUInt<N>.Zero, new BigUInt<N>("0"));
        }

        public static void ParseFullTest() {
            BigUInt<N> v = BigUInt<N>.Full;
            BigUInt<N> v2 = new(v.ToString());

            Assert.AreEqual(v, v2);
        }

        public static void ParseOverflowTest() {
            Assert.ThrowsException<OverflowException>(() => {
                BigUInt<N> v = BigUInt<N>.Full;
                BigUInt<N> v2 = new(v.ToString()[..^1] + '9');
            });
        }
    }


    [TestClass]
    public class ParseTests {
        [TestMethod]
        public void ParseTest() {
            ParseTests<N4>.ParseTest();
            ParseTests<N5>.ParseTest();
            ParseTests<N6>.ParseTest();
            ParseTests<N7>.ParseTest();
            ParseTests<N8>.ParseTest();
            ParseTests<N9>.ParseTest();
            ParseTests<N10>.ParseTest();
            ParseTests<N11>.ParseTest();
            ParseTests<N12>.ParseTest();
            ParseTests<N13>.ParseTest();
            ParseTests<N14>.ParseTest();
            ParseTests<N15>.ParseTest();
            ParseTests<N16>.ParseTest();
            ParseTests<N17>.ParseTest();
            ParseTests<N18>.ParseTest();
            ParseTests<N19>.ParseTest();
            ParseTests<N20>.ParseTest();
            ParseTests<N21>.ParseTest();
            ParseTests<N22>.ParseTest();
            ParseTests<N23>.ParseTest();
            ParseTests<N24>.ParseTest();
            ParseTests<N25>.ParseTest();
            ParseTests<N31>.ParseTest();
            ParseTests<N32>.ParseTest();
            ParseTests<N33>.ParseTest();
            ParseTests<N47>.ParseTest();
            ParseTests<N48>.ParseTest();
            ParseTests<N50>.ParseTest();
            ParseTests<N53>.ParseTest();
            ParseTests<N56>.ParseTest();
            ParseTests<N59>.ParseTest();
            ParseTests<N63>.ParseTest();
            ParseTests<N64>.ParseTest();
            ParseTests<N65>.ParseTest();
        }

        [TestMethod]
        public void ParseFullTest() {
            ParseTests<N4>.ParseFullTest();
            ParseTests<N5>.ParseFullTest();
            ParseTests<N6>.ParseFullTest();
            ParseTests<N7>.ParseFullTest();
            ParseTests<N8>.ParseFullTest();
            ParseTests<N9>.ParseFullTest();
            ParseTests<N10>.ParseFullTest();
            ParseTests<N11>.ParseFullTest();
            ParseTests<N12>.ParseFullTest();
            ParseTests<N13>.ParseFullTest();
            ParseTests<N14>.ParseFullTest();
            ParseTests<N15>.ParseFullTest();
            ParseTests<N16>.ParseFullTest();
            ParseTests<N17>.ParseFullTest();
            ParseTests<N18>.ParseFullTest();
            ParseTests<N19>.ParseFullTest();
            ParseTests<N20>.ParseFullTest();
            ParseTests<N21>.ParseFullTest();
            ParseTests<N22>.ParseFullTest();
            ParseTests<N23>.ParseFullTest();
            ParseTests<N24>.ParseFullTest();
            ParseTests<N25>.ParseFullTest();
            ParseTests<N31>.ParseFullTest();
            ParseTests<N32>.ParseFullTest();
            ParseTests<N33>.ParseFullTest();
            ParseTests<N47>.ParseFullTest();
            ParseTests<N48>.ParseFullTest();
            ParseTests<N50>.ParseFullTest();
            ParseTests<N53>.ParseFullTest();
            ParseTests<N56>.ParseFullTest();
            ParseTests<N59>.ParseFullTest();
            ParseTests<N63>.ParseFullTest();
            ParseTests<N64>.ParseFullTest();
            ParseTests<N65>.ParseFullTest();
        }

        [TestMethod]
        public void ParseOverflowTest() {
            ParseTests<N4>.ParseOverflowTest();
            ParseTests<N5>.ParseOverflowTest();
            ParseTests<N6>.ParseOverflowTest();
            ParseTests<N7>.ParseOverflowTest();
            ParseTests<N8>.ParseOverflowTest();
            ParseTests<N9>.ParseOverflowTest();
            ParseTests<N10>.ParseOverflowTest();
            ParseTests<N11>.ParseOverflowTest();
            ParseTests<N12>.ParseOverflowTest();
            ParseTests<N13>.ParseOverflowTest();
            ParseTests<N14>.ParseOverflowTest();
            ParseTests<N15>.ParseOverflowTest();
            ParseTests<N16>.ParseOverflowTest();
            ParseTests<N17>.ParseOverflowTest();
            ParseTests<N18>.ParseOverflowTest();
            ParseTests<N19>.ParseOverflowTest();
            ParseTests<N20>.ParseOverflowTest();
            ParseTests<N21>.ParseOverflowTest();
            ParseTests<N22>.ParseOverflowTest();
            ParseTests<N23>.ParseOverflowTest();
            ParseTests<N24>.ParseOverflowTest();
            ParseTests<N25>.ParseOverflowTest();
            ParseTests<N31>.ParseOverflowTest();
            ParseTests<N32>.ParseOverflowTest();
            ParseTests<N33>.ParseOverflowTest();
            ParseTests<N47>.ParseOverflowTest();
            ParseTests<N48>.ParseOverflowTest();
            ParseTests<N50>.ParseOverflowTest();
            ParseTests<N53>.ParseOverflowTest();
            ParseTests<N56>.ParseOverflowTest();
            ParseTests<N59>.ParseOverflowTest();
            ParseTests<N63>.ParseOverflowTest();
            ParseTests<N64>.ParseOverflowTest();
            ParseTests<N65>.ParseOverflowTest();
        }
    }
}

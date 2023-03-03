using AvxUInt;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Numerics;

namespace AvxUIntTest {
    [TestClass]
    public class ConvertTests {
        [TestMethod]
        public void ConvertTest() {
            BigUInt<N8> n1 = new(new UInt32[] { 0x1234u, 0x5678u, 0x9ABCu, 0xDEF0u, 0x12340u, 0x56780u, 0x9ABC0u, 0xDEF00u }, enable_clone: false);
            BigUInt<N8> n2 = new(new UInt32[] { 0x1234u, 0x5678u, 0x9ABCu, 0xDEF0u, 0x12340u, 0x56780u, 0x9ABC0u, 0u }, enable_clone: false);
            BigUInt<N10> n3 = new(new UInt32[] { 0xDEF0u, 0x12340u, 0x1234u, 0x5678u, 0x9ABCu, 0xDEF0u, 0x12340u, 0x56780u, 0x9ABC0u, 0xDEF00u }, enable_clone: false);
            BigUInt<N10> n4 = new(new UInt32[] { 0xDEF0u, 0x12340u, 0x1234u, 0x5678u, 0x9ABCu, 0xDEF0u, 0x12340u, 0x56780u, 0x9ABC0u, 0u }, enable_clone: false);

            Assert.AreEqual(new BigUInt<N8>(new UInt32[] { 0x1234u, 0x5678u, 0x9ABCu, 0xDEF0u, 0x12340u, 0x56780u, 0x9ABC0u, 0xDEF00u }, enable_clone: false), n1.Convert<N8>());
            Assert.AreEqual(new BigUInt<N9>(new UInt32[] { 0x1234u, 0x5678u, 0x9ABCu, 0xDEF0u, 0x12340u, 0x56780u, 0x9ABC0u, 0xDEF00u, 0u }, enable_clone: false), n1.Convert<N9>());
            Assert.AreEqual(new BigUInt<N7>(new UInt32[] { 0x1234u, 0x5678u, 0x9ABCu, 0xDEF0u, 0x12340u, 0x56780u, 0x9ABC0u }, enable_clone: false), n2.Convert<N7>());

            Assert.AreEqual(new BigUInt<N10>(new UInt32[] { 0xDEF0u, 0x12340u, 0x1234u, 0x5678u, 0x9ABCu, 0xDEF0u, 0x12340u, 0x56780u, 0x9ABC0u, 0xDEF00u }, enable_clone: false), n3.Convert<N10>());
            Assert.AreEqual(new BigUInt<N11>(new UInt32[] { 0xDEF0u, 0x12340u, 0x1234u, 0x5678u, 0x9ABCu, 0xDEF0u, 0x12340u, 0x56780u, 0x9ABC0u, 0xDEF00u, 0u }, enable_clone: false), n3.Convert<N11>());
            Assert.AreEqual(new BigUInt<N9>(new UInt32[] { 0xDEF0u, 0x12340u, 0x1234u, 0x5678u, 0x9ABCu, 0xDEF0u, 0x12340u, 0x56780u, 0x9ABC0u }, enable_clone: false), n4.Convert<N9>());
            
            Assert.ThrowsException<OverflowException>(() => {
                _ = n1.Convert<N7>();
            });
            Assert.ThrowsException<OverflowException>(() => {
                _ = n2.Convert<N6>();
            });
            Assert.ThrowsException<OverflowException>(() => {
                _ = n3.Convert<N9>();
            });
            Assert.ThrowsException<OverflowException>(() => {
                _ = n4.Convert<N8>();
            });
        }
    }
}

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Primrose.Structs.Native;
using System.Runtime.InteropServices;

namespace Primrose.Test
{
    [TestClass]
    public class PFS0Tests
    {
        [TestMethod]
        public void PFS0HeaderSize()
        {
            Assert.AreEqual(0x10, Marshal.SizeOf(typeof(PFS0Header)));
        }

        [TestMethod]
        public void PFS0FileEntrySize()
        {
            Assert.AreEqual(0x18, Marshal.SizeOf(typeof(PFS0FileEntry)));
        }
    }
}

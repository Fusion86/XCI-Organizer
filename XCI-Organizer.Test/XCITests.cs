using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.InteropServices;
using XCI_Organizer.Classes;
using XCI_Organizer.Models;
using XCI_Organizer.Structs;

namespace XCI_Organizer.Test
{
    [TestClass]
    public class XCITests
    {
        [TestMethod]
        public void GamecardHeaderSize()
        {
            Assert.AreEqual(0x200, Marshal.SizeOf(typeof(GamecardHeader)));
        }

        [TestMethod]
        public void LoadXCI()
        {
            Keyset keyset = Keyset.Load("keys.txt");
            XCI xci = XCI.Load(@"L:\ROM\Nintendo Switch\Fire Emblem Warriors.xci", keyset);

            Assert.IsTrue(xci != null);
        }
    }
}

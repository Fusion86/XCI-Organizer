using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Runtime.InteropServices;
using XCI_Organizer.Classes;
using XCI_Organizer.Models;
using XCI_Organizer.Structs.Native;

namespace XCI_Organizer.Test
{
    [TestClass]
    public class XCITests
    {
        private const string XCI_PATH = @"L:\ROM\Nintendo Switch\Fire Emblem Warriors.xci";

        [TestMethod]
        public void GamecardHeaderSize()
        {
            Assert.AreEqual(0x200, Marshal.SizeOf(typeof(GamecardHeader)));
        }

        [TestMethod]
        public void LoadXCI()
        {
            Keyset keyset = Keyset.Load("keys.txt");
            XCI xci = XCI.Load(XCI_PATH, keyset);

            Assert.IsTrue(xci != null);
        }

        [TestMethod]
        public void DumpNormal()
        {
            XCI xci = XCI.Load(XCI_PATH, null);

            Directory.CreateDirectory(Path.Combine("dump", "normal"));
            Directory.CreateDirectory(Path.Combine("dump", "update"));

            foreach (var file in xci.FileSystem.GetNormalPartition().Files)
            {
                string path = Path.Combine("dump", "normal", file.Name);
                file.Dump(path);
                Console.WriteLine($"Dumped to '{path}'");
            }

            foreach (var file in xci.FileSystem.GetUpdatePartition().Files)
            {
                string path = Path.Combine("dump", "update", file.Name);
                file.Dump(path);
                Console.WriteLine($"Dumped to '{path}'");
            }
        }
    }
}

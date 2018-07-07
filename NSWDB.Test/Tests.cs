using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;

namespace NSWDB.Test
{
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void LoadLocal()
        {
            // Init
            WebClient client = new WebClient();
            client.DownloadFile("http://nswdb.com/xml.php", "NSWreleases.xml");

            // Test
            NSWDB db = new NSWDB();
            db.LoadLocal("NSWreleases.xml");

            // Cleanup
            try
            {
                File.Delete("NSWreleases.xml");
            }
            catch (Exception) { }

            // Test results
            Assert.IsTrue(db.Releases.Count > 0, "No releases!");
            Console.WriteLine($"Loaded {db.Releases.Count} releases");
        }

        [TestMethod]
        public void LoadRemote()
        {
            NSWDB db = new NSWDB();
            db.LoadRemote().Wait();

            Assert.IsTrue(db.Releases.Count > 0, "No releases!");

            Console.WriteLine($"Loaded {db.Releases.Count} releases");
        }
    }
}

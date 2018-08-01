using Microsoft.VisualStudio.TestTools.UnitTesting;
using Primrose.Models;

namespace Primrose.Test
{
    [TestClass]
    public class NSPTests
    {
        private const string NSP_PATH = @"L:\ROM\Nintendo Switch\NSP\Sonic Mania [DLC][01009aa000fab001][v0].nsp";

        [TestMethod]
        public void LoadNSP()
        {
            PFS0 nsp = PFS0.Load(NSP_PATH);

            Assert.IsNotNull(nsp);
        }
    }
}

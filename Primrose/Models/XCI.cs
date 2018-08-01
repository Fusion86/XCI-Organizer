using System;
using System.IO;
using Primrose.Classes;
using Primrose.Extensions;
using Primrose.Structs.Native;

namespace Primrose.Models
{
    public class XCI
    {
        public readonly string Path;

        public XCIHeader GamecardHeader { get; private set; }
        public HFS0Root FileSystem { get; private set; }

        public Keyset Keyset; // FIXME: Not sure how I want to handle the keys

        private XCI(string path)
        {
            Path = path;
        }

        /// <summary>
        /// Load XCI file from disk
        /// </summary>
        /// <param name="path">Path to XCI file</param>
        /// <param name="verify">Verify hashes and signatures</param>
        /// <returns></returns>
        public static XCI Load(string path, Keyset keyset, bool verify = true)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                XCI xci = new XCI(path);
                xci.Keyset = keyset;

                // Gamecard Header
                byte[] bytes = br.ReadBytes(0x200); // sizeof XCIHeader
                xci.GamecardHeader = bytes.ToStruct<XCIHeader>();

                if (xci.GamecardHeader.MagicString != "HEAD")
                    throw new Exception("Invalid GamecardHeader magic! Maybe this isn't an XCI file?");

                // TODO: Gamecard Info
                // TODO: Gamecard Certificate
                // TODO: Initial Data

                // HFS0, see comments in HFS0Root and HFS0Partition
                fs.Position = 0xF000; // start of HFS0Header
                xci.FileSystem = HFS0Root.Load(path, fs);

                return xci;
            }
        }
    }
}

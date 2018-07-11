using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XCI_Organizer.Classes;
using XCI_Organizer.Extensions;
using XCI_Organizer.Structs;
using XTSSharp;

namespace XCI_Organizer.Models
{
    public class XCI
    {
        public readonly string Path;

        public GamecardHeader GamecardHeader;

        public HFS0Header HFS0Header;
        public HFS0FileEntry[] HFS0FileEntries;
        public string[] HFS0StringTable;

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
            using (FileStream fs = new FileStream(path, FileMode.Open))
            using (BinaryReader br = new BinaryReader(fs))
            {
                XCI xci = new XCI(path);
                xci.Keyset = keyset;

                // Gamecard Header
                byte[] bytes = br.ReadBytes(0x200); // sizeof GamecardHeader
                xci.GamecardHeader = bytes.ToStruct<GamecardHeader>();

                if (xci.GamecardHeader.MagicString != "HEAD")
                    throw new Exception("Invalid GamecardHeader magic! Maybe this isn't an XCI file?");

                // TODO: Gamecard Info
                // TODO: Gamecard Certificate
                // TODO: Initial Data

                // HFS0
                fs.Position = 0xF000; // start of HFS0Header
                bytes = br.ReadBytes(0x10); // sizeof HFS0Header
                xci.HFS0Header = bytes.ToStruct<HFS0Header>();

                if (xci.HFS0Header.MagicString != "HFS0")
                    throw new Exception("Invalid HFS0Header magic!");

                // HFS0 File Entry Table
                xci.HFS0FileEntries = new HFS0FileEntry[xci.HFS0Header.NumberOfFiles]; // Create array for loop

                for (int i = 0; i < xci.HFS0Header.NumberOfFiles; i++)
                {
                    bytes = br.ReadBytes(0x40); // sizeof HFS0FileEntry
                    xci.HFS0FileEntries[i] = bytes.ToStruct<HFS0FileEntry>();
                }

                // HFS0 String Table
                xci.HFS0StringTable = new string[xci.HFS0Header.NumberOfFiles];
                List<byte> cStr = new List<byte>();
                byte b;

                for (int i = 0; i < xci.HFS0Header.NumberOfFiles; i++)
                {
                    while (true)
                    {
                        b = br.ReadByte(); // This shouldn't be too bad since I **assume** that the FileStream uses buffering in one way or another

                        if (b == 0) break; // Break on zero (because c-string zero means that we've reached the end of the string)
                        else cStr.Add(b);
                    }

                    xci.HFS0StringTable[i] = Encoding.ASCII.GetString(cStr.ToArray());
                    cStr.Clear(); // Clear for next loop
                }

                return xci;
            }
        }

        public object GetNCAStream(string name)
        {
            for (int i = 0; i < HFS0StringTable.Length; i++)
                if (name == HFS0StringTable[i])
                    return GetNCAStream(i);

            // If not found
            return null;
        }

        public object GetNCAStream(int i)
        {
            Xts xts = XtsAes128.Create(Keyset.HeaderKey1, Keyset.HeaderKey2); // Maybe Keyset.HeaderKey is enough

            using (FileStream fs = new FileStream(Path, FileMode.Open))
            {
                return null;
            }
        }
    }
}

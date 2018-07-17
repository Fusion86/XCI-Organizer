// http://switchbrew.org/index.php?title=Gamecard_Format

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XCI_Organizer.Extensions;
using XCI_Organizer.Interfaces;
using XCI_Organizer.Logging;
using XCI_Organizer.Structs.Native;

namespace XCI_Organizer.Models
{
    public abstract class HFS0EntryBase : IHFS0Object
    {
        private static readonly ILog Logger = LogProvider.For<HFS0EntryBase>();

        public XCI XCI { get; }

        public string Name { get; }
        public long Offset { get; }
        public long Size { get; }

        protected HFS0Header Header;
        protected HFS0FileEntry[] FileEntries;
        protected string[] StringTable;

        public HFS0EntryBase(XCI xci, string name, Stream stream, long offset, long size)
        {
            XCI = xci;
            Name = name;
            Offset = offset;

            // Create BinaryReader and leave the stream open after we are finished
            using (BinaryReader br = new BinaryReader(stream, Encoding.UTF8, true))
            {
                stream.Position = offset; // Is this a good idea?

                // HFS0 Header
                byte[] bytes = br.ReadBytes(0x10); // sizeof HFS0Header
                Header = bytes.ToStruct<HFS0Header>();

                if (Header.MagicString != "HFS0")
                    throw new Exception("Invalid HFS0Header magic!");

                // HFS0 File Entry Table
                FileEntries = new HFS0FileEntry[Header.NumberOfFiles]; // Create array for loop

                for (int i = 0; i < Header.NumberOfFiles; i++)
                {
                    bytes = br.ReadBytes(0x40); // sizeof HFS0FileEntry
                    FileEntries[i] = bytes.ToStruct<HFS0FileEntry>();
                }

                long stringTableOffset = stream.Position; // Remeber current position so that we can set the stream.Position to the end of the 00-padded String Table

                // HFS0 String Table
                StringTable = new string[Header.NumberOfFiles];
                List<byte> cStr = new List<byte>();
                byte b;

                for (int i = 0; i < Header.NumberOfFiles; i++)
                {
                    while (true)
                    {
                        b = br.ReadByte(); // This shouldn't be too bad since I **assume** that the FileStream uses buffering in one way or another

                        if (b == 0) break; // Break on zero (because C-string zero means that we've reached the end of the string)
                        else cStr.Add(b);
                    }

                    StringTable[i] = Encoding.ASCII.GetString(cStr.ToArray());
                    cStr.Clear(); // Clear for next loop
                }

                // Set stream.Position to the end of the HFS0 String Table (incl. 00-padding)
                stream.Position = stringTableOffset + Header.StringTableSize;

                // Set size
                Size = stream.Position - Offset;
            }
        }
    }
}

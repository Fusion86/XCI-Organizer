using Primrose.Extensions;
using Primrose.Interfaces;
using Primrose.Structs.Native;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Primrose.Models
{
    public class PFS0 : IFSObject
    {
        public string Path { get; }
        public string Name { get; }
        public long Offset { get; }
        public long Size { get; }
        public long HeaderSize { get; }

        private PFS0Header Header;
        private PFS0FileEntry[] FileEntries;
        private string[] StringTable;

        public PFS0(string path, string name, Stream stream, long offset, long size)
        {
            Path = path;
            Name = name;
            Offset = offset;
            Size = size;

            // Create BinaryReader and leave the stream open after we are finished
            using (BinaryReader br = new BinaryReader(stream, Encoding.UTF8, true))
            {
                stream.Position = offset; // Is this a good idea?

                // PFS0 Header
                byte[] bytes = br.ReadBytes(0x10); // sizeof PFS0Header
                Header = bytes.ToStruct<PFS0Header>();

                if (Header.MagicString != "PFS0")
                    throw new Exception("Invalid PFS0 magic! Maybe this isn't an XCI file?");

                FileEntries = new PFS0FileEntry[Header.NumberOfFiles];

                for (int i = 0; i < Header.NumberOfFiles; i++)
                {
                    bytes = br.ReadBytes(0x18); // sizeof PFS0FileEntry
                    FileEntries[i] = bytes.ToStruct<PFS0FileEntry>();
                }

                long stringTableOffset = stream.Position; // Remember the current position so that we can set the stream.Position to the end of the 00-padded String Table

                // PFS0 String Table
                StringTable = new string[Header.NumberOfFiles];
                Utils.ReadStringTable(br, ref StringTable);

                // Set stream.Position to the end of the PFS0 String Table (incl. 00-padding)
                stream.Position = stringTableOffset + Header.StringTableSize;

                // Set header size
                HeaderSize = stream.Position - Offset;
            }
        }

        public static PFS0 Load(string path, bool verify = true)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return new PFS0(path, null, fs, 0, 0);
            }
        }

        public IEnumerable<FileEntry> Files
        {
            get
            {
                for (int i = 0; i < Header.NumberOfFiles; i++)
                {
                    yield return new FileEntry(
                        Path,
                        StringTable[i], // Filename (usually xxx.nca or xxx.cnmt.nca)
                        Offset + HeaderSize + (long)FileEntries[i].Offset, // Offset = partitionHeaderOffset + partitionHeaderSize + fileOffset
                        (long)FileEntries[i].Size // Filesize
                    );
                }
            }
        }
    }
}

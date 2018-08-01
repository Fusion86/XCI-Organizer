// http://switchbrew.org/index.php?title=Gamecard_Format

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Primrose.Extensions;
using Primrose.Interfaces;
using Primrose.Logging;
using Primrose.Structs.Native;

namespace Primrose.Models
{
    public class HFS0 : IFSObject
    {
        private static readonly ILog Logger = LogProvider.For<HFS0>();

        public string Path { get; }
        public string Name { get; }
        public long Offset { get; }
        public long Size { get; }
        public long HeaderSize { get; }

        protected HFS0Header Header;
        protected HFS0FileEntry[] FileEntries;
        protected string[] StringTable;

        public HFS0(string path, string name, Stream stream, long offset, long size)
        {
            Path = path;
            Name = name;
            Offset = offset;
            Size = size;

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

                long stringTableOffset = stream.Position; // Remember the current position so that we can set the stream.Position to the end of the 00-padded String Table

                // HFS0 String Table
                StringTable = new string[Header.NumberOfFiles];
                Utils.ReadStringTable(br, ref StringTable);

                // Set stream.Position to the end of the HFS0 String Table (incl. 00-padding)
                stream.Position = stringTableOffset + Header.StringTableSize;

                // Set header size
                HeaderSize = stream.Position - Offset;
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

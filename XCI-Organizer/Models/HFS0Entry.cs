// http://switchbrew.org/index.php?title=Gamecard_Format

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using XCI_Organizer.Extensions;
using XCI_Organizer.Structs;

namespace XCI_Organizer.Models
{
    [DebuggerDisplay("HFS0Entry: {Name,nq}")]
    public class HFS0Entry
    {
        public readonly string Name;

        protected HFS0Header Header;
        protected HFS0FileEntry[] FileEntries;
        protected string[] StringTable;

        public HFS0Entry(string name, Stream stream)
        {
            Name = name;

            // Create BinaryReader and leave the stream open after we are finished
            using (BinaryReader br = new BinaryReader(stream, Encoding.UTF8, true))
            {
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

                long stringTablePosition = stream.Position; // Remeber current position so that we can set the stream.Position to the end of the 00-padded String Table

                // HFS0 String Table
                StringTable = new string[Header.NumberOfFiles];
                List<byte> cStr = new List<byte>();
                byte b;

                for (int i = 0; i < Header.NumberOfFiles; i++)
                {
                    while (true)
                    {
                        b = br.ReadByte(); // This shouldn't be too bad since I **assume** that the FileStream uses buffering in one way or another

                        if (b == 0) break; // Break on zero (because c-string zero means that we've reached the end of the string)
                        else cStr.Add(b);
                    }

                    StringTable[i] = Encoding.ASCII.GetString(cStr.ToArray());
                    cStr.Clear(); // Clear for next loop
                }

                // Set stream.Position to the end of the HFS0 String Table (incl. 00-padding)
                // This is not actually required for the program to work, but I feel like we should be nice and give the calling function a consistent/logical stream.Position back
                stream.Position = stringTablePosition + Header.StringTableSize;
            }
        }

        // Maybe rename to GetFiles and make it a method and not a property?
        public IEnumerable<object> Files
        {
            get
            {
                for (int i = 0; i < Header.NumberOfFiles; i++)
                {
                    // TODO: Implement friendly way to access files
                    // obj.Name = StringTable[i];
                    // obj.Stream = stream to file (with stream.Position set)
                    // Maybe the .Stream can also decrypt the NCAs on the fly (if they are actually encrypted)?

                    yield return null;
                }
            }
        }
    }
}

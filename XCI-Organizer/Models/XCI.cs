﻿using System;
using System.IO;
using XCI_Organizer.Classes;
using XCI_Organizer.Extensions;
using XCI_Organizer.Structs;

namespace XCI_Organizer.Models
{
    public class XCI
    {
        public readonly string Path;

        public GamecardHeader GamecardHeader;
        public HFS0Header HFS0Header;
        public HFS0FileEntry[] HFS0FileEntries;

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

                // TODO: HFS0 String Table
                // TODO: HFS0 Raw File Data (maybe keep stream alive so that we can always access this?)

                return xci;
            }
        }
    }
}
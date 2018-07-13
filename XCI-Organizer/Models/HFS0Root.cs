// http://switchbrew.org/index.php?title=Gamecard_Format

using System.IO;
using XCI_Organizer.Logging;

namespace XCI_Organizer.Models
{
    // Another fitting name would be HFS0FileSystem or HFS0RootNode
    public class HFS0Root : HFS0Entry
    {
        private static readonly ILog Logger = LogProvider.For<HFS0Root>();

        public HFS0Entry[] Partitions;

        private HFS0Root(Stream stream) : base("root", stream)
        {

        }

        /// <summary>
        /// Load all HFS0 partitions starting at the root. The calling function is expected to set stream.Position at the start of the root/first HFS0 Header (always 0xF000 for XCIs)
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static HFS0Root Load(Stream stream)
        {
            // The "SHA-256 File System" or "HFS0" starts at offset 0xF000 in the Gamecard. The first 0x200 bytes act as a global header and represent the root partition which points to the other partitions ("normal", "logo", "update" and "secure").
            // Where "logo" only exists in 4.0.0+ gamecards and if "logo" exists then "normal" is empty (not verified)

            Logger.Info("stream.Position = " + stream.Position);

            // Root HFS0
            HFS0Root root = new HFS0Root(stream);

            long offset = stream.Position; // Offset where to load the other headers, for gamecards this should be 0xF200
            Logger.Info("offset = " + offset);

            // Since we just loaded the root HFS0 partition all the files inside it (FileEntries) are actually also HFS0 parititons
            root.Partitions = new HFS0Entry[root.Header.NumberOfFiles];
            for (int i = 0; i < root.Header.NumberOfFiles; i++)
            {
                stream.Position = offset + (long)root.FileEntries[i].Offset;
                root.Partitions[i] = new HFS0Entry(root.StringTable[i], stream);
            }

            return root;
        }
    }
}

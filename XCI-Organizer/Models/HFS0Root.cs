// http://switchbrew.org/index.php?title=Gamecard_Format

using System.Diagnostics;
using System.IO;
using System.Linq;
using XCI_Organizer.Logging;

namespace XCI_Organizer.Models
{
    [DebuggerDisplay("HFS0Root: {Name,nq}")]
    public class HFS0Root : HFS0EntryBase
    {
        private static readonly ILog Logger = LogProvider.For<HFS0Root>();

        public HFS0Entry[] Partitions;

        private HFS0Root(XCI xci, Stream stream, long offset) : base(xci, "root", stream, offset, 0)
        {

        }

        /// <summary>
        /// Load all HFS0 partitions starting at the root. The calling function is expected to set stream.Position at the start of the root/first HFS0 Header (always 0xF000 for XCIs)
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static HFS0Root Load(XCI xci, Stream stream)
        {
            // The "SHA-256 File System" or "HFS0" starts at offset 0xF000 in the Gamecard. The first 0x200 bytes act as a global header and represent the root partition which points to the other partitions ("normal", "logo", "update" and "secure").
            // Where "logo" only exists in 4.0.0+ gamecards and if "logo" exists then "normal" is empty (but still exists)

            Logger.Info("stream.Position = " + stream.Position);

            // Root HFS0
            HFS0Root root = new HFS0Root(xci, stream, stream.Position);

            // Since we just loaded the root HFS0 partition all the files inside it (FileEntries) are actually also HFS0 parititons
            root.Partitions = new HFS0Entry[root.Header.NumberOfFiles];
            for (int i = 0; i < root.Header.NumberOfFiles; i++)
            {
                root.Partitions[i] = new HFS0Entry(
                    xci,
                    root.StringTable[i],
                    stream,
                    root.Offset + root.Size + (long)root.FileEntries[i].Offset, // Partition offset
                                                                                // root.Offset                          = 0xF000 for gamecards
                                                                                // root.Size                            = 0x200 for gamecards
                                                                                // root.FileEntries[i].Offset           = partition offset relative to root.Offset + root.Size
                                                                                // absolute offset for first partition  = 0xF200 

                    (long)root.FileEntries[i].Size
                );
            }

            return root;
        }

        public HFS0Entry GetUpdatePartition() => Partitions.FirstOrDefault(x => x.Name == "update");
        public HFS0Entry GetNormalPartition() => Partitions.FirstOrDefault(x => x.Name == "normal");
        public HFS0Entry GetSecurePartition() => Partitions.FirstOrDefault(x => x.Name == "secure");
        public HFS0Entry GetLogoPartition() => Partitions.FirstOrDefault(x => x.Name == "logo");
    }
}

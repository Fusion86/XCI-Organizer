using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace XCI_Organizer.Models
{
    [DebuggerDisplay("HFS0Entry: {Name,nq}")]
    public class HFS0Entry : HFS0EntryBase
    {
        public HFS0Entry(XCI xci, string name, Stream stream, long offset, long size) : base(xci, name, stream, offset, size)
        {

        }

        public IEnumerable<NCA> Files
        {
            get
            {
                for (int i = 0; i < Header.NumberOfFiles; i++)
                {
                    yield return new NCA(
                        _xci, StringTable[i], // Filename (usually xxx.nca or xxx.cnmt.nca)
                        Offset + Size + (long)FileEntries[i].Offset, // Offset = partitionHeaderOffset + partitionHeaderSize + fileOffset (where fileOffset is relative to the start of partitionOffset)
                        (long)FileEntries[i].Size // Filesize
                    );
                }
            }
        }
    }
}

// http://switchbrew.org/index.php?title=NCA_Format

using System.Diagnostics;
using System.IO;
using XCI_Organizer.Helper;
using XCI_Organizer.Interfaces;

namespace XCI_Organizer.Models
{
    // Another name for NCA would be HFS0File
    [DebuggerDisplay("NCA: {Name,nq}")]
    public class NCA : IHFS0Object
    {
        public XCI XCI { get; }

        public string Name { get; }
        public long Offset { get; }
        public long Size { get; }

        public NCA(XCI xci, string name, long offset, long size)
        {
            XCI = xci;
            Name = name;
            Offset = offset;
            Size = size;
        }

        /// <summary>
        /// Remember to dispose!
        /// </summary>
        /// <returns></returns>
        public Stream GetStream()
        {
            FileStream fs = new FileStream(XCI.Path, FileMode.Open, FileAccess.Read, FileShare.Read);
            fs.Position = Offset;
            return new SubStream(fs, Size);
        }

        public void Dump(string path)
        {
            using (FileStream fs = File.Create(path))
            using (Stream stream = GetStream())
            {
                stream.CopyTo(fs);
            }
        }
    }
}

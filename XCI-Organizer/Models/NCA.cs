// http://switchbrew.org/index.php?title=NCA_Format

using System.Diagnostics;
using System.IO;
using XCI_Organizer.Helper;

namespace XCI_Organizer.Models
{
    [DebuggerDisplay("NCA: {Name,nq}")]
    public class NCA
    {
        private XCI _xci;

        public readonly string Name;
        public readonly long Offset;
        public readonly long Size;

        public NCA(XCI xci, string name, long offset, long size)
        {
            _xci = xci;
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
            FileStream fs = new FileStream(_xci.Path, FileMode.Open, FileAccess.Read, FileShare.Read);
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

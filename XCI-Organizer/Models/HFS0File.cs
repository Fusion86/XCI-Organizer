using System.Diagnostics;
using System.IO;
using XCI_Organizer.Helper;

namespace XCI_Organizer.Models
{
    [DebuggerDisplay("HFS0File: {Name,nq}")]
    public class HFS0File
    {
        private XCI _xci;

        public readonly string Name;
        public readonly long Offset;
        public readonly long Size;

        public HFS0File(XCI xci, string name, long offset, long size)
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
            // TODO: Decrypt NCA stream, but we probably want to do that in the NCA class (maybe `NCA nca = new NCA(hfs0file);` and then GetStream returns a decrypted stream?)
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

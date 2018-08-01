// http://switchbrew.org/index.php?title=NCA_Format

using System.Diagnostics;
using System.IO;
using Primrose.Helper;
using Primrose.Interfaces;

namespace Primrose.Models
{
    [DebuggerDisplay("FileEntry: {Name,nq}")]
    public class FileEntry : IFSObject
    {
        public string Path { get; }
        public string Name { get; }
        public long Offset { get; }
        public long Size { get; }

        public FileEntry(string path, string name, long offset, long size)
        {
            Path = path;
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
            FileStream fs = new FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read);
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

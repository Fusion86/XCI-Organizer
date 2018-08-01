// https://github.com/SciresM/hactool/blob/af428a1bdea5069fef6a88a037732a161abbe282/settings.h

using System.IO;
using System.Linq;
using Primrose.Extensions;

namespace Primrose.Classes
{
    public class Keyset
    {
        public byte[] HeaderKey; // Length = 0x20, aka 2 keys

        #region Helpers

        public byte[] HeaderKey1 => HeaderKey.Take(0x10).ToArray();
        public byte[] HeaderKey2 => HeaderKey.Skip(0x10).ToArray();

        #endregion

        public static Keyset Load(string path)
        {
            var keys = File.ReadLines(path)
                .Select(line => line.Replace(" ", "").Split('='))
                .Where(line => line[0].Length > 0 && line[1].Length > 0)
                .ToDictionary(line => line[0], line => line[1]);

            Keyset keyset = new Keyset();

            if (keys.TryGetValue("header_key", out string key))
                keyset.HeaderKey = key.ParseHexString();

            return keyset;
        }
    }
}

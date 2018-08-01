using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Primrose
{
    public static class Utils
    {
        public static void ReadStringTable(BinaryReader br, ref string[] stringTable)
        {
            if (stringTable == null)
                throw new Exception("StringTable not initialized!");

            List<byte> cStr = new List<byte>();
            byte b;

            for (int i = 0; i < stringTable.Length; i++)
            {
                while (true)
                {
                    b = br.ReadByte(); // This shouldn't be too bad since I **assume** that the FileStream uses buffering in one way or another

                    if (b == 0) break; // Break on zero (because C-string zero means that we've reached the end of the string)
                    else cStr.Add(b);
                }

                stringTable[i] = Encoding.ASCII.GetString(cStr.ToArray());
                cStr.Clear(); // Clear for next loop
            }
        }
    }
}

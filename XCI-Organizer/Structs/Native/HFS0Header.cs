// http://switchbrew.org/index.php?title=Gamecard_Format

using System;
using System.Runtime.InteropServices;
using System.Text;
using XCI_Organizer.Helper;

namespace XCI_Organizer.Structs.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct HFS0Header
    {
        #region Native

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)]
        public byte[] Magic; // Magicnum "HFS0"

        [Endian(Endianness.LittleEndian)]
        public UInt32 NumberOfFiles;

        [Endian(Endianness.LittleEndian)]
        public UInt32 StringTableSize;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)]
        public byte[] Reserved;

        #endregion

        public string MagicString => Encoding.ASCII.GetString(Magic);
    }
}

// https://switchbrew.org/index.php?title=NCA_Format#PFS0
// https://github.com/SciresM/hactool/blob/master/pfs0.h

using Primrose.Helper;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Primrose.Structs.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct PFS0Header
    {
        // Currently the same as HFS0Header

        #region Native

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)]
        public byte[] Magic; // Magicnum "PFS0"

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

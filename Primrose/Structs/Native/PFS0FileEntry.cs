// https://switchbrew.org/index.php?title=NCA_Format#PFS0
// https://github.com/SciresM/hactool/blob/master/pfs0.h

using Primrose.Helper;
using System;
using System.Runtime.InteropServices;

namespace Primrose.Structs.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct PFS0FileEntry
    {
        // Currently the same as HFS0FileEntry except that it doesn't have HashedSize and Hash

        [Endian(Endianness.LittleEndian)]
        public UInt64 Offset;

        [Endian(Endianness.LittleEndian)]
        public UInt64 Size;

        [Endian(Endianness.LittleEndian)]
        public UInt32 StringTableOffset; // Filename

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)]
        public byte[] Reserved;
    }
}

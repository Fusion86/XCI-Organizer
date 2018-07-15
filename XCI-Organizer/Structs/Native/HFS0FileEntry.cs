// http://switchbrew.org/index.php?title=Gamecard_Format

using System;
using System.Runtime.InteropServices;
using XCI_Organizer.Helper;

namespace XCI_Organizer.Structs.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct HFS0FileEntry
    {
        [Endian(Endianness.LittleEndian)]
        public UInt64 Offset;

        [Endian(Endianness.LittleEndian)]
        public UInt64 Size;

        [Endian(Endianness.LittleEndian)]
        public UInt32 StringTableOffset; // Filename

        [Endian(Endianness.LittleEndian)]
        public UInt32 HashedSize; // For HFS0s, this is the size of the pre-filedata portion, for NCAs this is usually 0x200

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.U1)]
        public byte[] Reserved; // Zero?

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20, ArraySubType = UnmanagedType.U1)]
        public byte[] Hash; // SHA-256 hash of the first (size of hashed region) bytes of filedata
    }
}

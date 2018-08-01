// http://switchbrew.org/index.php?title=Gamecard_Format

using System;
using System.Runtime.InteropServices;
using System.Text;
using Primrose.Enums;
using Primrose.Helper;

namespace Primrose.Structs.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct XCIHeader
    {
        #region Native

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100, ArraySubType = UnmanagedType.U1)]
        public byte[] HeaderSignature; // RSA-2048 PKCS #1 signature over the header (data from 0x100 to 0x200)

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)]
        public byte[] Magic; // Magicnum "HEAD"

        [Endian(Endianness.LittleEndian)]
        public UInt32 SecureAreaStartAddress; // Secure Area Start Address (in Media Units which are 0x200 bytes)

        [Endian(Endianness.LittleEndian)]
        public UInt32 BackupAreaStartAddress; // Backup Area Start Address (always 0xFFFFFFFF)

        public byte TitleKEKIndex; // Title KEK Index (high nibble) and KEK Index (low nibble)
        public CartridgeSize GamecardSize; // (0xFA = 1GB, 0xF8 = 2GB, 0xF0 = 4GB, 0xE0 = 8GB, 0xE1 = 16GB, 0xE2 = 32GB)
        public byte GamecardHeaderVersion;
        public byte GamecardFlags; // bit0 = AutoBoot, bit1 = HistoryErase

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.U1)]
        public byte[] PackageId; // Used for challenge–response authentication

        [Endian(Endianness.LittleEndian)]
        public UInt64 ValidDataEndAddress; // Valid Data End Address (in Media Units which are 0x200 bytes)

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x10, ArraySubType = UnmanagedType.U1)]
        public byte[] GamecardInfoIV; // GamecardInfo IV (reversed)

        [Endian(Endianness.LittleEndian)]
        public UInt64 HFS0PartitionOffset;

        [Endian(Endianness.LittleEndian)]
        public UInt64 HFS0HeaderSize;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20, ArraySubType = UnmanagedType.U1)]
        public byte[] HFS0Hash; // SHA-256 hash of the HFS0 Header

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20, ArraySubType = UnmanagedType.U1)]
        public byte[] InitialDataHash; // SHA-256 hash of the Initial Data

        [Endian(Endianness.LittleEndian)]
        public UInt32 SecureModeFlag; // Always 1, which means Secure Mode is available

        [Endian(Endianness.LittleEndian)]
        public UInt32 TitleKeyFlag; // Always 2

        [Endian(Endianness.LittleEndian)]
        public UInt32 KeyFlag; // Always 0

        [Endian(Endianness.LittleEndian)]
        public UInt32 NormalAreaEndAddress; // In Media Units which are 0x200 bytes

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x70, ArraySubType = UnmanagedType.U1)]
        public byte[] GamecardInfo; // AES-128-CBC encrypted

        #endregion

        public string MagicString => Encoding.ASCII.GetString(Magic);
    }
}

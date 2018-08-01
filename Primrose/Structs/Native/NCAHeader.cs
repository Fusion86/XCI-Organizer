using System;
using System.Runtime.InteropServices;
using System.Text;
using Primrose.Enums;
using Primrose.Helper;

namespace Primrose.Structs.Native
{
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct NCAHeader
    {
        #region Native

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100, ArraySubType = UnmanagedType.U1)]
        public byte[] FixedKeySignature; // RSA-2048 signature over the 0x200-bytes starting at offset 0x200 using fixed key

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x100, ArraySubType = UnmanagedType.U1)]
        public byte[] NPDMKeySignature; // RSA-2048 signature over the 0x200-bytes starting at offset 0x200 using key from NPDM, or zeroes if not a program

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)]
        public byte[] Magic; // Magicnum (Normally "NCA3", "NCA2" for some pre-1.0.0 NCAs)

        public byte Distribution; // 0 for system NCAs, 1 for a NCA from gamecard
        public NCAContentType ContentType;
        public byte CryptoType; // Crypto Type. Only used stating with 3.0.0. Normally 0. 2 = Crypto supported starting with 3.0.0.
        public byte KeyIndex; // Key index, must be 0-2.

        [Endian(Endianness.LittleEndian)]
        public UInt64 Size; // Size of the entire NCA

        [Endian(Endianness.LittleEndian)]
        public UInt64 TitleId;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4, ArraySubType = UnmanagedType.U1)]
        public byte[] Padding; // _0x218

        [Endian(Endianness.LittleEndian)]
        public UInt32 SdkVersion;

        [Endian(Endianness.LittleEndian)]
        public byte CryptoType2; // Crypto-Type2. Selects which crypto-sysver to use. 0x3 = 3.0.1, 0x4 = 4.0.0, 0x5 = 5.0.0.

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 15, ArraySubType = UnmanagedType.U1)]
        public byte[] Padding2; // _0x221

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 10, ArraySubType = UnmanagedType.U1)]
        public byte[] RightsId;

        #endregion

        public string MagicString => Encoding.ASCII.GetString(Magic);
    }
}

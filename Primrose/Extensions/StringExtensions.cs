﻿using System;

namespace Primrose.Extensions
{
    public static class StringExtensions
    {
        public static byte[] ParseHexString(this string str)
        {
            int num = str.Length;
            byte[] bytes = new byte[num / 2];
            for (int i = 0; i < num; i += 2)
                bytes[i / 2] = Convert.ToByte(str.Substring(i, 2), 16);
            return bytes;
        }
    }
}

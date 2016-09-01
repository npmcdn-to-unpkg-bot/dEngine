// StringExtensions.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;

namespace dEngine.Utility.Extensions
{
    /// <summary/>
    public static class StringExtensions
    {
        /// <summary>
        /// Converts a string to an array of bytes.
        /// </summary>
        public static byte[] ToByteArray(this string str)
        {
            var chars = str.ToCharArray();
            var bytes = new byte[chars.Length];
            for (var i = 0; i < chars.Length; i++)
            {
                bytes[i] = Convert.ToByte(chars[i]);
            }
            return bytes;
        }
    }
}
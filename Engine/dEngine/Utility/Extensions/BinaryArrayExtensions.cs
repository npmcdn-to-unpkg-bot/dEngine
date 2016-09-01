// BinaryArrayExtensions.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

#pragma warning disable 1591

namespace dEngine.Utility.Extensions
{
    public static class BinaryArrayExtensions
    {
        /// <summary />
        /// <param name="byteArray">The source array to get reversed bytes for</param>
        /// <param name="startIndex">The index in the source array at which to begin the reverse</param>
        /// <param name="count">The number of bytes to reverse</param>
        /// <returns>A new array containing the reversed bytes, or a sub set of the array not reversed.</returns>
        public static byte[] ReverseForBigEndian(this byte[] byteArray, int startIndex, int count)
        {
            if (BitConverter.IsLittleEndian)
                return byteArray.Reverse(startIndex, count);
            return byteArray.SubArray(startIndex, count);
        }

        public static byte[] Reverse(this byte[] byteArray, int startIndex, int count)
        {
            var ret = new byte[count];
            for (var i = startIndex + (count - 1); i >= startIndex; --i)
            {
                var b = byteArray[i];
                ret[startIndex + (count - 1) - i] = b;
            }
            return ret;
        }

        public static byte[] SubArray(this byte[] byteArray, int startIndex, int count)
        {
            var ret = new byte[count];
            for (var i = 0; i < count; ++i)
                ret[0] = byteArray[i + startIndex];
            return ret;
        }
    }
}
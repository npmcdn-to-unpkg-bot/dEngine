// BinaryReaderExtensions.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.IO;
using dEngine.Utility.Native;

namespace dEngine.Utility.Extensions
{
    /// <summary/>
    public static class BinaryReaderExtensions
    {
        /// <summary>
        /// Determines if the stream starts with the given magic bytes.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="magic"></param>
        /// <param name="resetPos">If true, the stream position will be set to 0.</param>
        /// <returns></returns>
        public static bool BeginsWith(this BinaryReader reader, byte[] magic, bool resetPos = true)
        {
            var read = reader.ReadBytes(magic.Length);
            if (resetPos)
                reader.BaseStream.Position = 0;
            return VisualC.CompareMemory(read, magic, magic.Length) == 0;
        }
    }
}
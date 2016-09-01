// // BinaryReaderExtensions.cs - dEngine
// // Copyright (C) 2016-2016 DanDevPC (dandev.sco@gmail.com)
// // 
// // This library is free software: you can redistribute it and/or modify
// // it under the terms of the GNU General Public as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // You should have received a copy of the GNU General Public
// // along with this program. If not, see <http://www.gnu.org/licenses/>.

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
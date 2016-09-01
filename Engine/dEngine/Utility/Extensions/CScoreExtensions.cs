// CScoreExtensions.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.IO;
using CSCore;

namespace dEngine.Utility.Extensions
{
    internal static class CScoreExtensions
    {
        public static byte[] ToByteArray(this IWaveSource source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            using (var buffer = new MemoryStream())
            {
                int read;
                var temporaryBuffer = new byte[source.WaveFormat.BytesPerSecond];
                while ((read = source.Read(temporaryBuffer, 0, temporaryBuffer.Length)) > 0)
                    buffer.Write(temporaryBuffer, 0, read);
                return buffer.ToArray();
            }
        }
    }
}
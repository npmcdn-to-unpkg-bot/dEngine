// RawAsset.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.IO;

namespace dEngine.Data
{
    /// <summary>
    /// A raw binary asset.
    /// </summary>
    public sealed class RawAsset : AssetBase
    {
        public Stream Stream;

        /// <summary/>
        public override ContentType ContentType { get; } = ContentType.Unknown;

        protected override void OnLoad(BinaryReader reader)
        {
            Stream.Position = 0; 
            Stream = reader.BaseStream; // TODO: might need to copy to a new stream
        }

        /// <summary/>
        protected override void Dispose(bool disposing)
        {
            throw new System.NotImplementedException();
        }

        /// <summary/>
        public static implicit operator Stream(RawAsset raw)
        {
            return raw.Stream;
        }
    }
}
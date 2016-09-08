// AssetBase.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.IO;
using System.Text;
using dEngine.Utility.Extensions;

namespace dEngine.Data
{
    /// <summary>
    /// Base class for assets.
    /// </summary>
    public abstract class AssetBase : IDisposable
    {
        private static readonly byte[] _assetHeader = Encoding.UTF8.GetBytes("ASSETBIN");

        /// <summary>
        /// Determines whether the asset is disposed.
        /// </summary>
        protected bool _disposed;

        /// <summary>
        /// The path to the source file.
        /// </summary>
        public string SourceFile { get; set; } = string.Empty;

        /// <summary>
        /// A list of tags.
        /// </summary>
        public Tags Tags { get; set; } = new Tags();

        /// <summary>
        /// The <see cref="ContentType" /> of this asset.
        /// </summary>
        public abstract ContentType ContentType { get; }

        /// <summary>
        /// Determines if the geometry has been disposed.
        /// </summary>
        public bool IsDisposed => _disposed;

        /// <summary>
        /// Determines if data was successfuly loaded into the asset.
        /// </summary>
        public bool IsLoaded { get; protected set; }

        /// <summary>
        /// Disposes of the asset.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Returns the content type of the given stream.
        /// </summary>
        public static ContentType? PeekContent(Stream stream)
        {
            ContentType? type;
            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                if (!reader.BeginsWith(_assetHeader, false))
                    type = null;
                else
                    type = (ContentType)reader.ReadByte();
            }
            stream.Position = 0;
            return type;
        }

        /// <summary>
        /// Saves the asset to a stream.
        /// </summary>
        internal void Save(Stream stream)
        {
            using (var writer = new BinaryWriter(stream, Encoding.UTF8, true))
            {
                writer.Write(_assetHeader);
                writer.Write(SourceFile ?? string.Empty);
                Tags.Save(writer);
                OnSave(writer);
            }
        }

        /// <summary>
        /// Loads the asset from a stream.
        /// </summary>
        internal void Load(Stream stream)
        {
            using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
            {
                var isAssetBin = reader.BeginsWith(_assetHeader);
                if (isAssetBin)
                {
                    reader.BaseStream.Position += _assetHeader.Length;
                    SourceFile = reader.ReadString();
                    OnLoad(reader);
                }
                else if (!OnNonAsset(reader))
                    throw new InvalidOperationException("Asset has invalid header.");
            }
        }

        /// <summary>
        /// Invoked when the stream header does not match the expected asset header.
        /// </summary>
        /// <returns>
        /// A boolean determining if the file was successfully loaded.
        /// </returns>
        protected virtual bool OnNonAsset(BinaryReader reader)
        {
            return false;
        }

        /// <summary />
        protected virtual void OnSave(BinaryWriter writer)
        {
        }

        /// <summary />
        protected virtual void OnLoad(BinaryReader reader)
        {
        }

        /// <summary />
        ~AssetBase()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispoes of the asset.
        /// </summary>
        /// <param name="disposing">Determines whether object was disposed or deconstructed.</param>
        protected abstract void Dispose(bool disposing);

#pragma warning disable 1591
        [InstBeforeSerialization]
        protected virtual void BeforeSerialization()
        {
            if (_disposed)
                throw new InvalidOperationException("Cannot serialize disposed asset.");
        }

        [InstAfterSerialization]
        protected virtual void AfterSerialization()
        {
        }

        [InstBeforeDeserialization]
        protected virtual void BeforeDeserialization()
        {
        }

        [InstAfterDeserialization]
        protected virtual void AfterDeserialization()
        {
        }
#pragma warning restore 1591
    }
}
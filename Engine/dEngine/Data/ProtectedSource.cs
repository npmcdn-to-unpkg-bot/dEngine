// ProtectedSource.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.IO;
using dEngine.Instances.Attributes;

namespace dEngine.Data
{
    /// <summary>
    /// Asset that contains a text string.
    /// </summary>
    [TypeId(26)]
    public class TextSource : AssetBase
    {
        /// <summary>
        /// The source code.
        /// </summary>
        public string Text { get; set; }

        /// <inheritdoc />
        public override ContentType ContentType => ContentType.Text;

        /// <inheritdoc />
        protected override void OnSave(BinaryWriter writer)
        {
            writer.Write(Text.ToCharArray());
        }

        /// <inheritdoc />
        protected override void OnLoad(BinaryReader reader)
        {
            Text = new string(reader.ReadChars((int)reader.BaseStream.Length));
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
        }
    }
}
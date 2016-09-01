// AnimationData.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.IO;
using dEngine.Instances.Attributes;

namespace dEngine.Data
{
    /// <summary>
    /// Container class for animation data;
    /// </summary>
    [TypeId(23)]
    public class AnimationData : AssetBase
    {
        /// <inheritdoc />
        public override ContentType ContentType => ContentType.Animation;

        protected override void OnSave(BinaryWriter writer)
        {
            //base.OnSave(writer);
        }

        protected override void OnLoad(BinaryReader reader)
        {
            //base.OnLoad(reader);
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
            }

            _disposed = true;
        }
    }
}
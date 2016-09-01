// Faces.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Collections;
using System.IO;

#pragma warning disable 1591

namespace dEngine
{
    /// <summary>
    /// A struct of booleans for 6 sides.
    /// </summary>
    public struct Faces : IDataType
    {
        private BitArray _bitArray;

        [InstMember(1)]
        internal byte Bitfield
        {
            get
            {
                var bytes = new byte[1];
                _bitArray.CopyTo(bytes, 0);
                return bytes[0];
            }
            set { _bitArray = new BitArray(new[] {value}); }
        }

        public bool Right => _bitArray.Get(0);
        public bool Top => _bitArray.Get(1);
        public bool Back => _bitArray.Get(2);
        public bool Left => _bitArray.Get(3);
        public bool Bottom => _bitArray.Get(4);
        public bool Front => _bitArray.Get(5);

        public Faces(params NormalId[] normals)
        {
            _bitArray = new BitArray(6);

            var paramCount = normals.Length;
            for (var i = 0; i < paramCount; i++)
                _bitArray.Set((int)normals[i], true);
        }

        public static Faces @new(params NormalId[] normals)
        {
            return new Faces(normals);
        }

        public void Load(BinaryReader reader)
        {
            Bitfield = reader.ReadByte();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(Bitfield);
        }
    }
}
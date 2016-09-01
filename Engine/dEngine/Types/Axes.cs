// Axes.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections;
using System.IO;

#pragma warning disable 1591

namespace dEngine
{
    /// <summary>
    /// A struct of booleans for 3 axes.
    /// </summary>
    public struct Axes : IDataType
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

        public bool Z => _bitArray.Get(0);
        public bool Y => _bitArray.Get(1);
        public bool X => _bitArray.Get(2);

        public Axes(params Axis[] axes)
        {
            _bitArray = new BitArray(3);

            var paramCount = axes.Length;
            for (var i = 0; i < paramCount; i++)
                _bitArray.Set((int)axes[i], true);
        }

        public Axes(params NormalId[] normals)
        {
            _bitArray = new BitArray(3);

            var paramCount = normals.Length;
            for (var i = 0; i < paramCount; i++)
            {
                int axis;
                switch (normals[i])
                {
                    case NormalId.Left:
                    case NormalId.Right:
                        axis = (int)Axis.X;
                        break;
                    case NormalId.Top:
                    case NormalId.Bottom:
                        axis = (int)Axis.Y;
                        break;
                    case NormalId.Front:
                    case NormalId.Back:
                        axis = (int)Axis.Z;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                _bitArray.Set(axis, true);
            }
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
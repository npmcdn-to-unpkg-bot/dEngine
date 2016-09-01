// ColourSequence.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Collections.Generic;
using System.IO;

namespace dEngine
{
    /// <summary>
    /// A sequence of colours.
    /// </summary>
    public class ColourSequence : Sequence<Colour>, IDataType
    {
        internal ColourSequence()
        {
        }

        /// <summary />
        public ColourSequence(Colour value) : base(value)
        {
        }

        /// <summary />
        public ColourSequence(Keypoint[] values) : base(values)
        {
        }

        /// <summary />
        public ColourSequence(IEnumerable<Keypoint> values) : base(values)
        {
        }

        /// <summary />
        public void Load(BinaryReader reader)
        {
            var keyCount = reader.ReadInt32();
            _keypoints = new Keypoint[keyCount];
            for (var i = 0; i < keyCount; i++)
            {
                var time = reader.ReadSingle();
                var value = new Colour();
                value.Load(reader);
                _keypoints[i] = new Keypoint(time, value);
            }
        }

        /// <summary />
        public void Save(BinaryWriter writer)
        {
            var keyCount = _keypoints.Length;
            writer.Write(keyCount);
            for (var i = 0; i < keyCount; i++)
            {
                var keypoint = _keypoints[i];
                writer.Write(keypoint.Time);
                keypoint.Value.Save(writer);
            }
        }
    }
}
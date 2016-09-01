// NumberSequence.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

#pragma warning disable 1591

namespace dEngine
{
    /// <summary>
    /// A sequence of numbers.
    /// </summary>
    public sealed class NumberSequence : Sequence<double>, IDataType
    {
        public NumberSequence(double value) : base(value)
        {
        }

        public NumberSequence(Keypoint[] values) : base(values)
        {
        }

        public NumberSequence(IEnumerable<Keypoint> values) : base(values)
        {
        }

        public NumberSequence(params double[] values)
        {
            _keypoints = new Keypoint[values.Length];
            for (var i = 0; i < values.Length; i++)
                _keypoints[i] = new Keypoint((float)i/values.Length, values[i]);
        }


        /// <summary />
        public void Load(BinaryReader reader)
        {
            var keyCount = reader.ReadInt32();
            _keypoints = new Keypoint[keyCount];
            for (var i = 0; i < keyCount; i++)
            {
                var time = reader.ReadSingle();
                var value = reader.ReadDouble();
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
                writer.Write(keypoint.Value);
            }
        }

        public static explicit operator NumberSequence(string str)
        {
            var numbers = str.Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();
            var keypoints = new Keypoint[numbers.Length];
            if (numbers.Length > 1)
                for (var i = 0; i < numbers.Length/2; i += 2)
                    keypoints[i] = new Keypoint((float)numbers[i], numbers[i + 1]);
            return new NumberSequence();
        }
    }
}
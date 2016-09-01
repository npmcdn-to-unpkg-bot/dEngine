// Sequence.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Collections.Generic;
using System.Linq;

namespace dEngine
{
    /// <summary>
    /// Base class for sequences.
    /// </summary>
    public abstract class Sequence<T> where T : struct
    {
        /// <summary/>
        [InstMember(1)] protected Keypoint[] _keypoints;

        /// <summary>
        /// Empty constructor.
        /// </summary>
        protected Sequence()
        {
        }

        /// <summary>
        /// Creates a sequence using two keypoints of the same value.
        /// </summary>
        protected Sequence(T value)
        {
            _keypoints = new[] {new Keypoint(0, value), new Keypoint(1, value)};
        }

        /// <summary>
        /// Creates a sequence from an array of keypoints.
        /// </summary>
        protected Sequence(Keypoint[] values)
        {
            _keypoints = values;
        }

        /// <summary>
        /// Creates a sequence from a table of keypoints.
        /// </summary>
        protected Sequence(IEnumerable<Keypoint> values)
        {
            _keypoints = values.ToArray();
        }

        /// <summary>
        /// The sequence values.
        /// </summary>
        public Keypoint[] Keypoints => _keypoints; // TODO: see if array is acceptable, or LuaTable required

        /// <summary>
        /// Creates a new keypoint.
        /// </summary>
        public static Keypoint NewKeypoint(float time, T value)
        {
            return new Keypoint(time, value);
        }

        /// <summary />
        public override string ToString()
        {
            var str = "";
            var len = _keypoints.Length;
            for (var i = 0; i < len; i++)
            {
                str += _keypoints[i];
                if (i == len - 1)
                    str += ", ";
            }
            return str;
        }

        /// <summary>
        /// A keypoint for a sequence.
        /// </summary>
        public struct Keypoint
        {
            /// <summary>
            /// The time in the sequence that the keypoint will be positioned at.
            /// </summary>
            [InstMember(1)] public readonly float Time;

            /// <summary>
            /// The value of the keypoint/
            /// </summary>
            [InstMember(2)] public readonly T Value;

            /// <summary>
            /// Creates a sequence keypoint.
            /// </summary>
            public Keypoint(float time, T value)
            {
                Time = time;
                Value = value;
            }

            /// <summary>
            /// Returns a string representation of the keypoint.
            /// </summary>
            public override string ToString()
            {
                return $"{Value}, {Time}";
            }
        }
    }
}
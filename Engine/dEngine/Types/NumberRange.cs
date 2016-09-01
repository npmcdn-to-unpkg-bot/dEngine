// NumberRange.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.IO;

namespace dEngine
{
    /// <summary>
    /// A range represents a minimum and maximum value.
    /// </summary>
    public struct NumberRange : IDataType, IEquatable<NumberRange>
    {
        /// <summary>
        /// The minimum value of the range.
        /// </summary>
        [InstMember(1)] public float Min;

        /// <summary>
        /// The maximum value of the range.
        /// </summary>
        [InstMember(2)] public float Max;

        /// <summary>
        /// Creates a new range with min and max both set to the given value.
        /// </summary>
        /// <param name="value"></param>
        public NumberRange(float value)
        {
            Min = value;
            Max = value;
        }

        /// <summary>
        /// Creates a new range with min and max both set to the given value.
        /// </summary>
        /// <param name="min">The minimum value.</param>
        /// <param name="max">The maximum value.</param>
        public NumberRange(float min, float max)
        {
            Min = min;
            Max = max;
        }

        /// <summary>
        /// Determines if the min and max are equal for two <see cref="NumberRange" />s.
        /// </summary>
        public static bool operator ==(NumberRange a, NumberRange b)
        {
            return (a.Min == b.Min) && (a.Max == b.Max);
        }

        /// <summary>
        /// Determines if the min and max are not equal for two <see cref="NumberRange" />s.
        /// </summary>
        public static bool operator !=(NumberRange a, NumberRange b)
        {
            return (a.Min != b.Min) || (a.Max != b.Max);
        }

        /// <summary />
        public bool Equals(NumberRange other)
        {
            return Min.Equals(other.Min) && Max.Equals(other.Max);
        }

        /// <summary />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is NumberRange && Equals((NumberRange)obj);
        }

        /// <summary />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Min.GetHashCode()*397) ^ Max.GetHashCode();
            }
        }

        /// <summary />
        public override string ToString()
        {
            return $"{Min}, {Max}";
        }

        /// <summary />
        public void Load(BinaryReader reader)
        {
            Min = reader.ReadSingle();
            Max = reader.ReadSingle();
        }

        /// <summary />
        public void Save(BinaryWriter writer)
        {
            writer.Write(Min);
            writer.Write(Max);
        }
    }
}
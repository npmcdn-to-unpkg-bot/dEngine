// Region3.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.IO;

namespace dEngine
{
    /// <summary>
    /// A Region3 is used to represent a size and a location in 3D space, given two corners.
    /// </summary>
    public struct Region3 : IDataType, IEquatable<Region3>
    {
        /// <summary>
        /// Creates a Region3 out of two Vector3s.
        /// </summary>
        public Region3(Vector3 min, Vector3 max)
        {
            if ((min.X > max.X) || (min.Y > max.Y) || (min.Z > max.Z))
                throw new InvalidOperationException("Min must be < Max.");

            Min = min;
            Max = max;
        }

        /// <summary>
        /// The minimum vector.
        /// </summary>
        public Vector3 Min { get; }

        /// <summary>
        /// The maximum vector.
        /// </summary>
        public Vector3 Max { get; }

        /// <summary>
        /// Expands the <see cref="Region3" /> so that it is aligned with the voxel grid.
        /// </summary>
        /// <param name="resolution">The resolution of the grid alignment.</param>
        /// <returns>The expanded <see cref="Region3" /></returns>
        public Region3 ExpandToGrid(float resolution)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tests equality of two <see cref="Region3" /> values.
        /// </summary>
        public bool Equals(Region3 other)
        {
            return (Min == other.Min) && (Max == other.Max);
        }

        /// <summary />
        public void Load(BinaryReader reader)
        {
            var min = new Vector3();
            var max = new Vector3();
            min.Load(reader);
            max.Load(reader);
        }

        /// <summary />
        public void Save(BinaryWriter writer)
        {
            Min.Save(writer);
            Max.Save(writer);
        }
    }
}
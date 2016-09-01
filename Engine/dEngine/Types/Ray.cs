// Ray.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.IO;

namespace dEngine
{
    /// <summary>
    /// A ray is an infinite line starting at <see cref="Origin" /> and going towards <see cref="Direction" />.
    /// </summary>
    public struct Ray : IDataType, IEquatable<Ray>
    {
        /// <summary>
        /// The origin Position.
        /// </summary>
        [InstMember(1)] public Vector3 Origin;

        /// <summary>
        /// The direction vector.
        /// </summary>
        [InstMember(2)] public Vector3 Direction;

        /// <summary>
        /// Creates a ray with provided origin and direction.
        /// </summary>
        public Ray(Vector3 origin, Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        /// <summary>
        /// Creates a ray with provided origin and direction.
        /// </summary>
        public Ray(ref Vector3 origin, ref Vector3 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        /// <summary>
        /// Creates a ray with provided origin and direction.
        /// </summary>
        public Ray(float originX, float originY, float originZ, float dirX, float dirY, float dirZ)
        {
            Origin = new Vector3(originX, originY, originZ);
            Direction = new Vector3(dirX, dirY, dirZ);
        }

        /// <summary>
        /// Returns a copy of the ray with a normalized direction vector.
        /// </summary>
        public Ray unit => new Ray(Origin, Direction.unit);

        /// <summary>
        /// A ray that uses <see cref="Vector3.Zero" /> postion and direction.
        /// </summary>
        public static Ray Zero = new Ray(Vector3.Zero, Vector3.Zero);


        /// <summary>
        /// Gets a point along the ray at the specified length.
        /// </summary>
        /// <param name="length">The length along the ray from the ray Position in terms of the ray's direction.</param>
        /// <returns>The point along the ray at the given location.</returns>
        public Vector3 GetPoint(float length)
        {
            return Direction*length + Origin;
        }

        /// <summary>
        /// Returns the closest point on the ray.
        /// </summary>
        /// <param name="point">The position to get the closest point to.</param>
        /// <returns></returns>
        public Vector3 ClosestPoint(Vector3 point)
        {
            var w = point - Origin;
            var dir = Direction;

            float vsq;
            float proj;
            Vector3.Dot(ref dir, ref dir, out vsq);
            Vector3.Dot(ref w, ref dir, out proj);
            return Origin + proj/vsq*Direction;
        }

        /// <summary>
        /// Returns the distance from the ray to the point.
        /// </summary>
        public float Distance(Vector3 point)
        {
            Vector3 c;
            var dir = Direction;
            var r = point - Origin;
            Vector3.Cross(ref dir, ref r, out c);
            return c.magnitude;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{{{Origin}}}{{{Direction}}}";
        }

        /// <summary />
        public void Load(BinaryReader reader)
        {
            var origin = new Vector3();
            var direction = new Vector3();
            origin.Load(reader);
            direction.Load(reader);
            Origin = origin;
            Direction = direction;
        }

        /// <summary />
        public void Save(BinaryWriter writer)
        {
            Origin.Save(writer);
            Direction.Save(writer);
        }

        /// <summary />
        public bool Equals(Ray other)
        {
            return Direction.Equals(other.Direction) && Origin.Equals(other.Origin);
        }

        /// <summary />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Ray && Equals((Ray)obj);
        }

        /// <summary />
        public override int GetHashCode()
        {
            unchecked
            {
                return (Direction.GetHashCode()*397) ^ Origin.GetHashCode();
            }
        }

        /// <summary />
        public static bool operator ==(Ray left, Ray right)
        {
            return left.Equals(right);
        }

        /// <summary />
        public static bool operator !=(Ray left, Ray right)
        {
            return !left.Equals(right);
        }
    }
}
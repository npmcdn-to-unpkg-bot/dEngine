// BoundingBoxExtensions.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using SharpDX;

namespace dEngine.Utility.Extensions
{
    internal static class BoundingBoxExtensions
    {
        public static CFrame GetCFrame(this OrientedBoundingBox bb)
        {
            return (CFrame)bb.Transformation;
        }

        public static Vector3 GetdEngineSize(this OrientedBoundingBox bb)
        {
            return (Vector3)bb.Size;
        }

        public static CFrame GetCFrame(this BoundingBox bb)
        {
            return new CFrame((Vector3)(bb.Maximum + bb.Minimum)/2.0f);
        }

        public static Vector3 GetSize(this BoundingBox bb)
        {
            return (Vector3)(bb.Maximum - bb.Minimum);
        }
    }
}
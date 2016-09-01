// EnumExtensions.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

namespace dEngine
{
    internal static class EnumExtensions
    {
        public static Axis ToAxis(this NormalId normalId)
        {
            switch (normalId)
            {
                case NormalId.Left:
                    return Axis.X;
                case NormalId.Right:
                    return Axis.X;
                case NormalId.Top:
                    return Axis.Y;
                case NormalId.Bottom:
                    return Axis.Y;
                case NormalId.Front:
                    return Axis.Z;
                case NormalId.Back:
                    return Axis.Z;
                default:
                    throw new ArgumentOutOfRangeException(nameof(normalId), normalId, null);
            }
        }
    }
}
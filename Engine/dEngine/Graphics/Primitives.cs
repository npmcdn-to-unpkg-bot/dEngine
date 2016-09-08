// Primitives.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Diagnostics;
using dEngine.Data;
using dEngine.Services;

namespace dEngine.Graphics
{
    /// <summary>
    /// RenderBufferObject for primitives.
    /// </summary>
    internal static class Primitives
    {
        private static bool _primitivesLoaded;

        internal static Geometry CubeGeometry;
        internal static Geometry CylinderGeometry;
        internal static Geometry SphereGeometry;
        internal static Geometry WedgeGeometry;
        internal static Geometry ConeGeometry;
        internal static Geometry PlaneGeometry;

        /*
        static Primitives()
        {
            Load();
        }
        */

        private static Geometry ImportPrimitiveMesh(string name)
        {
            var data = ContentProvider.DownloadStream(new Uri($"internal://content/meshes/primitives/{name}.mesh")).Result;
            Debug.Assert(data != null, $"{name} stream != null");
            var geometry = new Geometry();
            geometry.Load(data);
            return geometry;
        }

        /// <summary>
        /// Loads primitive geometry.
        /// </summary>
        internal static void Load()
        {
            CubeGeometry = ImportPrimitiveMesh("Cube");
            ConeGeometry = ImportPrimitiveMesh("Cone");
            WedgeGeometry = ImportPrimitiveMesh("Wedge");
            SphereGeometry = ImportPrimitiveMesh("Sphere");
            CylinderGeometry = ImportPrimitiveMesh("Cylinder");
            PlaneGeometry = ImportPrimitiveMesh("Plane");

            _primitivesLoaded = true;
        }

        public static Geometry GeometryFromShape(Shape shape)
        {
            switch (shape)
            {
                case Shape.Cube:
                    return CubeGeometry;
                case Shape.Sphere:
                    return SphereGeometry;
                case Shape.Cylinder:
                    return CylinderGeometry;
                case Shape.Wedge:
                    return WedgeGeometry;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
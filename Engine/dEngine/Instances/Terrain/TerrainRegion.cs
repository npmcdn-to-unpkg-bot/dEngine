// TerrainRegion.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.IO;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A TerrainRegion stores the data for a region of <see cref="Terrain" />.
    /// </summary>
    [TypeId(136)]
    [Uncreatable]
    public class TerrainRegion : Instance
    {
        internal Stream Data { get; set; }

        /// <summary>
        /// The size of the region in cells.
        /// </summary>
        public Vector3 SizeInCells { get; }
    }
}
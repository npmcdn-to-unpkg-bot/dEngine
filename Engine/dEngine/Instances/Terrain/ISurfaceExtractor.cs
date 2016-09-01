// ISurfaceExtractor.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Graphics.Structs;

namespace dEngine.Instances
{
    internal interface ISurfaceExtractor
    {
        void GenLodCell(Terrain.Chunk chunk, int lod, out TerrainVertex[] vertices, out ushort[] indices);
    }
}
// TransvoxelExtractor.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Collections.Generic;
using System.Diagnostics;
using dEngine.Graphics.Structs;

namespace dEngine.Instances
{
    using static TransvoxelTables;

    internal class TransvoxelExtractor : ISurfaceExtractor
    {
        private readonly RegularCellCache _cache;
        private readonly Terrain _volume;

        public TransvoxelExtractor(Terrain volume)
        {
            _volume = volume;
            _cache = new RegularCellCache(Terrain.ChunkSize);
            UseCache = true;
        }

        public bool UseCache { get; set; }

        public void GenLodCell(Terrain.Chunk chunk, int lod, out TerrainVertex[] resultVertices,
            out ushort[] resultIndices)
        {
            var vertices = new List<TerrainVertex>();
            var indices = new List<ushort>();

            const short chunkSize = Terrain.ChunkSize;

            for (short x = 0; x < chunkSize; x++)
                for (short y = 0; y < chunkSize; y++)
                    for (short z = 0; z < chunkSize; z++)
                    {
                        var cellPos = new Vector3int16(x, y, z);
                        PolygonizeCell(chunk.Position, cellPos, ref vertices, ref indices, lod);
                    }

            resultVertices = vertices.ToArray();
            resultIndices = indices.ToArray();
        }

        private void PolygonizeCell(Vector3int16 offsetPos, Vector3int16 pos, ref List<TerrainVertex> vertices,
            ref List<ushort> indices, int lod)
        {
            Debug.Assert(lod >= 1, "Level of Detail must be greater than 1");
            offsetPos *= Terrain.ChunkSize;
            offsetPos += pos*lod;

            var directionMask = (byte)((pos.x > 0 ? 1 : 0) | ((pos.z > 0 ? 1 : 0) << 1) | ((pos.y > 0 ? 1 : 0) << 2));

            var density = new sbyte[8];

            for (var i = 0; i < density.Length; i++)
                density[i] = _volume[offsetPos + CornerIndex[i]*lod].Density;

            var caseCode = GetCaseCode(density);

            if ((caseCode ^ ((density[7] >> 7) & 0xFF)) == 0) //for this cases there is no triangulation
                return;

            var cornerNormals = new Vector3[8];
            for (var i = 0; i < 8; i++)
            {
                var p = offsetPos + CornerIndex[i]*lod;
                var nx = (_volume.GetCell(p.x + 1, p.y, p.z).Density - _volume[p - Vector3int16.Right].Density)*0.5f;
                var ny = (_volume[p + Vector3int16.Up].Density - _volume[p - Vector3int16.Up].Density)*0.5f;
                var nz = (_volume[p + Vector3int16.Backward].Density - _volume[p - Vector3int16.Backward].Density)*0.5f;

                cornerNormals[i].Set(nx, ny, nz);
                cornerNormals[i].Normalize();
            }

            var regularCellClass = RegularCellClass[caseCode];
            var vertexLocations = RegularVertexData[caseCode];

            var c = RegularCellData[regularCellClass];
            var vertexCount = c.GetVertexCount();
            var triangleCount = c.GetTriangleCount();
            var indexOffset = c.Indices(); //index offsets for current cell
            var mappedIndizes = new ushort[indexOffset.Length]; //array with real indizes for current cell

            for (var i = 0; i < vertexCount; i++)
            {
                var edge = (byte)(vertexLocations[i] >> 8);
                var reuseIndex = (byte)(edge & 0xF); //Vertex id which should be created or reused 1,2 or 3
                var rDir = (byte)(edge >> 4); //the direction to go to reach a previous cell for reusing 

                var v1 = (byte)(vertexLocations[i] & 0x0F); //Second Corner Index
                var v0 = (byte)((vertexLocations[i] >> 4) & 0x0F); //First Corner Index

                var d0 = density[v0];
                var d1 = density[v1];

                //Vector3f n0 = cornerNormals[v0];
                //Vector3f n1 = cornerNormals[v1];

                Debug.Assert(v1 > v0);

                var t = (d1 << 8)/(d1 - d0);
                var u = 0x0100 - t;
                var t0 = t/256f;
                var t1 = u/256f;

                var index = -1;

                if (UseCache && (v1 != 7) && ((rDir & directionMask) == rDir))
                {
                    Debug.Assert(reuseIndex != 0);
                    var cell = _cache.GetReusedIndex(pos, rDir);
                    index = cell.Verts[reuseIndex];
                }

                if (index == -1)
                {
                    var normal = cornerNormals[v0]*t0 + cornerNormals[v1]*t1;
                    GenerateVertex(ref offsetPos, ref pos, ref vertices, lod, t, ref v0, ref v1, ref d0, ref d1,
                        ref normal);
                    index = vertices.Count - 1;
                }

                if ((rDir & 8) != 0)
                    _cache.SetReusableIndex(pos, reuseIndex, (ushort)(vertices.Count - 1));

                mappedIndizes[i] = (ushort)index;
            }

            for (var t = 0; t < triangleCount; t++)
                for (var i = 0; i < 3; i++)
                    indices.Add(mappedIndizes[c.Indices()[t*3 + i]]);
        }

        private void GenerateVertex(ref Vector3int16 chunkPos, ref Vector3int16 cellPos,
            ref List<TerrainVertex> vertices,
            int lod, long t, ref byte v0, ref byte v1, ref sbyte d0, ref sbyte d1, ref Vector3 normal)
        {
            var p0 = (Vector3)(chunkPos + CornerIndex[v0]*lod);
            var p1 = (Vector3)(chunkPos + CornerIndex[v1]*lod);
            var q = InterpolateVoxelVector(t, ref p0, ref p1);

            vertices.Add(new TerrainVertex(ref q, ref normal));
        }

        private static Vector3 InterpolateVoxelVector(long t, ref Vector3 p0, ref Vector3 p1)
        {
            var u = 0x0100 - t; //256 - t
            var s = 1.0f/256.0f;
            var Q = p0*t + p1*u; //Density Interpolation
            Q *= s; // shift to shader ! 
            return Q;
        }

        private static byte GetCaseCode(sbyte[] density)
        {
            byte code = 0;
            byte konj = 0x01;
            for (var i = 0; i < density.Length; i++)
            {
                code |= (byte)((density[i] >> (density.Length - 1 - i)) & konj);
                konj <<= 1;
            }

            return code;
        }
    }
}
// Geometry.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.IO;
using System.Linq;
using dEngine.Graphics;
using dEngine.Graphics.Structs;
using dEngine.Instances.Attributes;
using dEngine.Instances.Materials;
using dEngine.Utility.Extensions;
using dEngine.Utility.FileFormats.Model;
using dEngine.Utility.Native;
using Microsoft.Scripting.Utils;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace dEngine.Data
{
    /// <summary>
    /// Represents a geometry. Instances of a geometry are rendered with a <see cref="RenderObject" />.
    /// </summary>
    [TypeId(21)]
    public class Geometry : AssetBase, IEquatable<Geometry>
    {
        private static readonly byte[] _robloxMeshVersion1 = CollectionUtils.Select("version 1.00".ToCharArray(), Convert.ToByte).ToArray();
        private BoundingBox _boundingBox;

        /// <summary>
        /// Creates an empty geometry.
        /// </summary>
        public Geometry()
        {
            Material = Material.Smooth;
            Vertices = new Vertex[0];
            Weights = null;
            Indices = new int[0];
        }

        /// <summary>
        /// Creates a new geometry.
        /// </summary>
        /// <param name="name">The name of the geometry.</param>
        /// <param name="vertices">An array of vertices.</param>
        /// <param name="indices">An array of indices.</param>
        /// <param name="topology">The type of primitive to draw with.</param>
        /// <param name="weights">An optional array of weights, if provided this geometry will be treated as a skeletal mesh.</param>
        /// <param name="material">The material to use.</param>
        internal Geometry(string name, Vertex[] vertices, int[] indices, WeightedVertex[] weights = null,
            PrimitiveTopology topology = PrimitiveTopology.TriangleList, Material material = null)
        {
            Name = name;
            Vertices = vertices;
            Indices = indices;
            Weights = weights;
            PrimitiveTopology = topology;
            Guid = Guid.NewGuid();
            Material = material ?? Material.Smooth;

            ComputeBoundingBox();

            if ((weights != null) && (weights.Length > 0))
                IsSkinned = true;

            Renderer.InvokeResourceDependent(DoBuildBuffersJob);

            IsLoaded = true;
        }

        internal Buffer VertexBuffer { get; set; }
        internal Buffer VertexWeightBuffer { get; set; }
        internal Buffer IndexBuffer { get; set; }
        internal VertexBufferBinding VertexBufferBinding { get; set; }
        internal VertexBufferBinding VertexWeightBufferBinding { get; set; }

        /// <summary>
        /// The vertices.
        /// </summary>
        internal Vertex[] Vertices { get; private set; }

        /// <summary>
        /// The bone data.
        /// </summary>
        internal WeightedVertex[] Weights { get; private set; }

        /// <summary>
        /// The indices.
        /// </summary>
        public int[] Indices { get; private set; }

        /// <summary>
        /// The primitive topology that this geometry renders with.
        /// </summary>
        public PrimitiveTopology PrimitiveTopology { get; private set; }

        /// <summary>
        /// The name of the geometry.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// The GUID of the geometry.
        /// </summary>
        public Guid Guid { get; }

        /// <summary>
        /// The content url of the material to use.
        /// </summary>
        public Material Material { get; set; }

        /// <summary>
        /// If true, this mesh uses bone weights.
        /// </summary>
        public bool IsSkinned { get; private set; }

        /// <summary>
        /// The number of indices.
        /// </summary>
        public int IndexCount => Indices.Length;

        /// <summary>
        /// The number of triangle.
        /// </summary>
        public int TriangleCount => Indices.Length/2;

        /// <inheritdoc />
        public override ContentType ContentType => IsSkinned ? ContentType.SkeletalMesh : ContentType.StaticMesh;

        /// <summary>
        /// Used by the FBX importer.
        /// </summary>
        internal int MaterialIndex { get; set; }

        /// <summary>
        /// Determins if two geometries are equal.
        /// </summary>
        public bool Equals(Geometry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Guid, other.Guid);
        }

        /// <summary>
        /// Fired when the buffers are rebuilt.
        /// </summary>
        public event Action Rebuilt;

        private void LoadRobloxMesh(Stream stream)
        {
            using (var stringReader = new StreamReader(stream))
            {
                stringReader.ReadLine();
                var test = stringReader.ReadLine();
                var triangleCount = int.Parse(test);
                var dataLine = stringReader.ReadLine();
                dataLine = dataLine.Substring(1, dataLine.Length - 1);
                var vectors = dataLine.Split(new[] {"]["}, StringSplitOptions.RemoveEmptyEntries);

                var vertCount = triangleCount*3;
                Vertices = new Vertex[vertCount];

                for (var j = 0; j < vertCount; j += 3)
                {
                    var positionValues = vectors[j].Split(',');
                    var normalValues = vectors[j + 1].Split(',');
                    var textureValues = vectors[j + 2].Split(',');

                    var pos = new Vector3(float.Parse(positionValues[0]), float.Parse(positionValues[1]),
                        float.Parse(positionValues[2]));
                    var normal = new Vector3(float.Parse(normalValues[0]), float.Parse(normalValues[1]),
                        float.Parse(normalValues[2]));
                    var texCoord = new Vector2(float.Parse(textureValues[0]), float.Parse(textureValues[1]));
                    Vector3 tangent;
                    Vector3 bitangent;

                    // TODO: compute tangents
                    tangent = new Vector3();
                    bitangent = new Vector3();

                    Vertices[j] = new Vertex(pos, normal, texCoord, Colour.Zero, tangent, bitangent);
                }
            }
        }

        /// <summary />
        protected override void OnSave(BinaryWriter writer)
        {
            base.OnSave(writer);

            var vertCount = Vertices.Length;
            writer.Write(vertCount);
            for (var i = 0; i < vertCount; i++)
                Vertices[i].Save(writer);

            var vertWeightCount = Weights?.Length ?? 0;
            writer.Write(vertWeightCount);
            for (var i = 0; i < vertWeightCount; i++)
                Weights?[i].Save(writer);

            var indexCount = Indices.Length;
            writer.Write(indexCount);
            for (var i = 0; i < indexCount; i++)
                writer.Write(Indices[i]);
        }

        /// <summary />
        protected override void OnLoad(BinaryReader reader)
        {
            if (reader.BeginsWith(_robloxMeshVersion1)) // special case for roblox mesh
                LoadRobloxMesh(reader.BaseStream);

            reader.BaseStream.Position = 0;
            base.OnLoad(reader);

            var vertCount = reader.ReadInt32();
            Vertices = new Vertex[vertCount];
            for (var i = 0; i < vertCount; i++)
            {
                var vert = new Vertex();
                vert.Load(reader);
                Vertices[i] = vert;
            }

            var vertWeightCount = reader.ReadInt32();
            Weights = new WeightedVertex[vertWeightCount];
            for (var i = 0; i < vertWeightCount; i++)
            {
                var vert = new WeightedVertex();
                vert.Load(reader);
                Weights[i] = vert;
            }

            var indexCount = reader.ReadInt32();
            Indices = new int[indexCount];
            for (var i = 0; i < indexCount; i++)
            {
                Indices[i] = reader.ReadInt32();
            }
        }

        /// <summary />
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
            }

            VertexBuffer?.Dispose();
            VertexWeightBuffer?.Dispose();
            IndexBuffer?.Dispose();

            Rebuilt = null;
            _disposed = true;
        }

        private void ComputeBoundingBox()
        {
            var result1 = new Vector3(float.MaxValue);
            var result2 = new Vector3(float.MinValue);
            for (var index = 0; index < Vertices.Length; ++index)
            {
                Vector3.Min(ref result1, ref Vertices[index].Position, out result1);
                Vector3.Max(ref result2, ref Vertices[index].Position, out result2);
            }
            _boundingBox = new BoundingBox((SharpDX.Vector3)result1, (SharpDX.Vector3)result2);
        }

        /// <inheritdoc />
        protected override void AfterDeserialization()
        {
            if (Weights != null)
                IsSkinned = true;

            ComputeBoundingBox();

            Renderer.InvokeResourceDependent(DoBuildBuffersJob);

            IsLoaded = true;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return (obj.GetType() == GetType()) && Equals((Geometry)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        private void DoBuildBuffersJob()
        {
            lock (Renderer.Locker)
            {
                BuildBuffers(Renderer.Context);
            }
        }

        internal void BuildBuffers(DeviceContext context)
        {
            VertexBuffer?.Dispose();
            IndexBuffer?.Dispose();

            VertexBuffer = Buffer.Create(context.Device, BindFlags.VertexBuffer, Vertices);
            VertexBufferBinding = new VertexBufferBinding(VertexBuffer, Vertex.Stride, 0);
            IndexBuffer = Buffer.Create(context.Device, BindFlags.IndexBuffer, Indices);

            if (IsSkinned)
            {
                VertexWeightBuffer?.Dispose();
                VertexWeightBuffer = Buffer.Create(context.Device, BindFlags.VertexBuffer, Weights);
                VertexWeightBufferBinding = new VertexBufferBinding(VertexWeightBuffer, WeightedVertex.Stride, 0);
            }

            Rebuilt?.Invoke();
        }

        internal Vector3 GetOffset()
        {
            return (Vector3)((_boundingBox.Minimum + _boundingBox.Maximum)/2);
        }

        internal Vector3 GetSize()
        {
            return (Vector3)(_boundingBox.Maximum - _boundingBox.Minimum);
        }
    }
}
// MeshImporter.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Assimp;
using dEditor.Framework.Services;
using dEditor.Modules.Dialogs.MeshImport;
using dEngine;
using dEngine.Data;
using dEngine.Graphics.Structs;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using dEngine.Serializer.V1;
using SharpDX;
using Bone = dEngine.Instances.Bone;
using Vector2 = dEngine.Vector2;
using Vector3 = dEngine.Vector3;

namespace dEditor.Framework.Content
{
    public static class MeshImporter
    {
        public static AssimpContext AssimpContext = new AssimpContext { Scale = 0.01f };

        public static string[] Import(ContentManager.ImportContext context)
        {
            MeshImportSettings settings;

            if (context.SkipDialog)
                settings = new MeshImportSettings();
            else
            {
                var dialog = new MeshImportViewModel(context);
                Editor.ShowDialog(dialog);
                settings = dialog.ImportSettings;
            }

            var steps = PostProcessSteps.ValidateDataStructure
                        | PostProcessSteps.GenerateUVCoords
                        | PostProcessSteps.Triangulate
                        | PostProcessSteps.SortByPrimitiveType
                        | PostProcessSteps.FlipUVs
                        | PostProcessSteps.FlipWindingOrder
                        | PostProcessSteps.LimitBoneWeights
                        | PostProcessSteps.ImproveCacheLocality
                        | PostProcessSteps.RemoveComponent
                        | PostProcessSteps.RemoveRedundantMaterials;

            if (!settings.KeepOverlappingVertices) steps |= PostProcessSteps.JoinIdenticalVertices;
            if (!settings.ImportAsSkeletal) steps |= PostProcessSteps.Debone;
            if (settings.MergeMeshes) steps |= PostProcessSteps.OptimizeMeshes;
            
            switch (settings.NormalImportMethod)
            {
                case NormalImportMethod.ComputeNormals:
                    steps |= PostProcessSteps.GenerateNormals;
                    steps |= PostProcessSteps.CalculateTangentSpace;
                    break;
                case NormalImportMethod.ComputeNormalsSmooth:
                    steps |= PostProcessSteps.GenerateSmoothNormals;
                    steps |= PostProcessSteps.CalculateTangentSpace;
                    break;
                case NormalImportMethod.ImportNormals:
                    steps |= PostProcessSteps.CalculateTangentSpace;
                    break;
                case NormalImportMethod.ImportNormalsAndTangents:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            var scene = AssimpContext.ImportFileFromStream(context.Stream, steps, context.Extension);
            Skeleton skeleton = null;
            var importedSkeleton = false;
            var importedAnimation = false;
            var importedMesh = false;
            var importedMaterials = false;
            var geometries = new Dictionary<string, Geometry>(scene.MeshCount);
            var animations = new AnimationData[scene.AnimationCount];
            var materials = new dEngine.Instances.Materials.Material[scene.MaterialCount];
            var textures = new dEngine.Data.Texture[scene.TextureCount];

            if (settings.ImportAsSkeletal)
                if (settings.Skeleton == null)
                {
                    SkeletonNodeData nodeData;

                    if (!FindSkeleton(ref scene, out nodeData))
                        throw new InvalidOperationException("Could not import skeleton.");

                    skeleton = ImportSkeleton(ref nodeData);
                    importedSkeleton = true;
                }
                else
                {
                    skeleton = Inst.Deserialize<Skeleton>(settings.Skeleton.Asset);
                }

            if (scene.HasMaterials)
            {
                importedMaterials = true;
                
                for (var i = 0; i < scene.MaterialCount; i++)
                {
                    var mat = scene.Materials[i];

                    var material = new dEngine.Instances.Materials.Material();
                    
                    materials[i] = material;
                }
            }

            for (var i = 0; i < scene.MeshCount; i++)
            {
                var mesh = scene.Meshes[i];
                var meshName = string.IsNullOrWhiteSpace(mesh.Name) ? $"{context.Name} ({i})" : mesh.Name;

                var vertices = new Vertex[mesh.VertexCount];
                WeightedVertex[] weights = null;

                if (mesh.HasBones && settings.ImportAsSkeletal)
                    weights = new WeightedVertex[mesh.VertexCount];

                for (var j = 0; j < mesh.VertexCount; j++)
                {
                    var texCoordChannel = mesh.HasTextureCoords(0)
                        ? mesh.TextureCoordinateChannels[0][j]
                        : new Vector3D(0, 0, 0);
                    var vertexColourChannel = mesh.HasVertexColors(0)
                        ? mesh.VertexColorChannels[0][j]
                        : new Color4D(0, 0, 0, 1);

                    var tangent = mesh.HasTangentBasis ? mesh.Tangents[j] : new Vector3D(0, 0, 0);
                    var bitangent = mesh.HasTangentBasis ? mesh.Tangents[j] : new Vector3D(0, 0, 0);

                    vertices[j] = new Vertex
                    {
                        Position = ConvertAssimpVector(mesh.Vertices[j]),
                        Normal = ConvertAssimpVector(mesh.Normals[j]),
                        Colour = new Colour(vertexColourChannel.R, vertexColourChannel.G, vertexColourChannel.B, vertexColourChannel.A),
                        TexCoord = new Vector2(texCoordChannel.X, texCoordChannel.Y),
                        Tangent = ConvertAssimpVector(tangent),
                        BiTangent = ConvertAssimpVector(bitangent)
                    };

                    // TODO: add bone data
                }

                importedMesh = true;
                geometries[mesh.Name] = new Geometry(meshName, vertices.ToArray(), mesh.GetIndices(), weights?.ToArray())
                { MaterialIndex = mesh.MaterialIndex };
            }

            if (settings.MergeMeshes && !settings.ImportAsSkeletal)
            {
                var vertices = new Vertex[0];
                var weights = new WeightedVertex[0];
                var indices = new int[0];

                foreach (var mesh in geometries.Values)
                {
                    var lastVertices = vertices;
                    vertices = new Vertex[lastVertices.Length + mesh.Vertices.Length];
                    lastVertices.CopyTo(vertices, 0);
                    mesh.Vertices.CopyTo(vertices, lastVertices.Length);

                    var lastWeights = weights;
                    weights = new WeightedVertex[lastWeights.Length + (mesh.Weights?.Length ?? 0)];
                    lastWeights.CopyTo(weights, 0);
                    mesh.Weights?.CopyTo(weights, lastWeights.Length);

                    var lastIndices = indices;
                    indices = new int[indices.Length + mesh.Indices.Length];
                    lastIndices.CopyTo(indices, 0);
                    mesh.Indices.Select(i => i + lastVertices.Length).ToArray().CopyTo(indices, lastIndices.Length);

                    mesh.Dispose();
                }

                geometries.Clear();
                geometries[context.Name] = new Geometry(context.Name, vertices, indices, weights);
            }

            for (var i = 0; i < scene.AnimationCount; i++)
            {
                var animation = scene.Animations[i];

                animations[i] = new AnimationData();
                importedAnimation = true;

                // TODO: import animations
            }
            
            #region File Creation

            foreach (var material in materials)
            {
                var materialPath = Path.Combine(context.OutputDirectory, $"{material.Name}.material");
                if (materialPath == null) continue;
                using (var file = File.Create(materialPath))
                {
                    Inst.Serialize(material, file);
                }
            }

            foreach (var geo in geometries.Values)
            {
                Stream stream;
                if (ContentManager.CreateFile(context, geo.Name, "mesh", out stream))
                using (stream)
                {
                    geo.Material = materials[geo.MaterialIndex];
                    geo.Save(stream);
                }
            }

            if (importedSkeleton)
            {
                Stream stream;
                if (ContentManager.CreateFile(context, context.Name, "skeleton", out stream))
                {
                    using (stream)
                    {
                        Inst.Serialize(skeleton, stream);
                    }
                }
            }

            foreach (var animation in animations)
            {
                Stream stream;
                if (ContentManager.CreateFile(context, context.Name, "skeleton", out stream))
                {
                    using (stream)
                    {
                        animation.Save(stream);
                    }
                }
            }

            #endregion
            
            return context.Results.ToArray();
        }

        private static Vector3 ConvertAssimpVector(Vector3D assimp)
        {
            return new Vector3(assimp.X, assimp.Y, assimp.Z);
        }

        private static Matrix ToSharpDX(Matrix4x4 matrix)
        {
            var result = Matrix.Identity;

            result.M11 = matrix.A1;
            result.M12 = matrix.B1;
            result.M13 = matrix.C1;
            result.M14 = matrix.D1;

            result.M21 = matrix.A2;
            result.M22 = matrix.B2;
            result.M23 = matrix.C2;
            result.M24 = matrix.D2;

            result.M31 = matrix.A3;
            result.M32 = matrix.B3;
            result.M33 = matrix.C3;
            result.M34 = matrix.D3;

            result.M41 = matrix.A4;
            result.M42 = matrix.B4;
            result.M43 = matrix.C4;
            result.M44 = matrix.D4;

            return result;
        }

        private static Skeleton ImportSkeleton(ref SkeletonNodeData nodeData)
        {
            var skeleton = new Skeleton();
            var rootBone = ImportBones(nodeData.RootNode, nodeData.RootNode.Parent, null, ref nodeData);
            skeleton.RootBone = rootBone;
            return skeleton;
        }

        private static Bone ImportBones(Node aiNode, Node aiParent, Bone parent, ref SkeletonNodeData nodeData)
        {
            Debug.Assert(aiNode != null);
            Debug.Assert(aiParent != null);

            Bone bone = null;
            if (!aiNode.Name.Contains("_$AssimpFbx$"))
            {
                const string mangling = "_$AssimpFbxNull$";

                if (aiNode.Name.Contains(mangling))
                {
                    bone = new Bone
                    {
                        Name = aiNode.Name.Replace(mangling, string.Empty),
                        CFrame = (CFrame)ToSharpDX(GetRelativeTransform(aiNode, aiParent))
                    };
                }
                else if (nodeData.Nodes.Contains(aiNode))
                {
                    bone = new Bone
                    {
                        Name = aiNode.Name
                    };

                    Matrix offsetMatrix;
                    Matrix parentOffsetMatrix;
                    var isOffsetMatrixValid = nodeData.DeformationBones.TryGetValue(aiNode.Name, out offsetMatrix);
                    var isParentOffsetMatrixValid = nodeData.DeformationBones.TryGetValue(aiParent.Name,
                        out parentOffsetMatrix);
                    if (isOffsetMatrixValid && isParentOffsetMatrixValid)
                        bone.CFrame = (CFrame)(Matrix.Invert(offsetMatrix) * parentOffsetMatrix);
                    else if (isOffsetMatrixValid && ((aiNode == nodeData.RootNode) || (aiParent == nodeData.RootNode)))
                        bone.CFrame = (CFrame)Matrix.Invert(offsetMatrix);
                    else
                        bone.CFrame = (CFrame)ToSharpDX(GetRelativeTransform(aiNode, aiParent));
                }
            }

            if (bone != null)
            {
                bone.Parent = parent;

                aiParent = aiNode;
                parent = bone;
            }

            foreach (var child in aiNode.Children)
                ImportBones(child, aiParent, parent, ref nodeData);

            return bone;
        }

        private static Matrix4x4 GetRelativeTransform(Node node, Node ancestor)
        {
            Debug.Assert(node != null);

            // Get transform of node relative to ancestor.
            var transform = node.Transform;
            var parent = node.Parent;
            while ((parent != null) && (parent != ancestor))
            {
                transform *= parent.Transform;
                parent = parent.Parent;
            }

            if ((parent == null) && (ancestor != null))
                throw new ArgumentException($"Node \"{ancestor.Name}\" is not an ancestor of \"{node.Name}\".");

            return transform;
        }

        private static bool FindSkeleton(ref Scene scene, out SkeletonNodeData nodeData)
        {
            if (!scene.HasMeshes)
                throw new NotSupportedException(
                    "The FBX importer requires meshes; a skeleton can not be imported on its own.");

            nodeData = new SkeletonNodeData
            {
                Nodes = new List<Node>(),
                DeformationBones = FindDeformationBones(ref scene)
            };

            if (nodeData.DeformationBones.Count == 0)
                return false;

            var rootBones = new HashSet<Node>();
            foreach (var boneName in nodeData.DeformationBones.Keys)
                rootBones.Add(FindRootBone(ref scene, boneName));

            if (rootBones.Count > 1)
                throw new NotSupportedException("Multiple skeletons are not supported.");

            nodeData.RootNode = rootBones.First();

            GetSubtree(nodeData.RootNode, nodeData.Nodes);

            return true;
        }

        private static void GetSubtree(Node node, List<Node> list)
        {
            Debug.Assert(node != null);
            Debug.Assert(list != null);

            list.Add(node);
            foreach (var child in node.Children)
                GetSubtree(child, list);
        }

        private static Node FindRootBone(ref Scene scene, string boneName)
        {
            Debug.Assert(scene != null);
            Debug.Assert(!string.IsNullOrEmpty(boneName));

            var node = scene.RootNode.FindNode(boneName);
            Debug.Assert(node != null, "Node referenced by mesh not found in model.");

            var rootBone = node;
            while ((node != scene.RootNode) && !node.HasMeshes)
            {
                if (!node.Name.Contains("$AssimpFbx$"))
                    rootBone = node;

                node = node.Parent;
            }

            return rootBone;
        }

        private static Dictionary<string, Matrix> FindDeformationBones(ref Scene scene)
        {
            Debug.Assert(scene != null);

            var offsetMatrices = new Dictionary<string, Matrix>();
            if (scene.HasMeshes)
                foreach (var mesh in scene.Meshes)
                    if (mesh.HasBones)
                        foreach (var bone in mesh.Bones)
                            if (!offsetMatrices.ContainsKey(bone.Name))
                                offsetMatrices[bone.Name] = ToSharpDX(bone.OffsetMatrix);

            return offsetMatrices;
        }
        
        public class SkeletonNodeData
        {
            public Node RootNode { get; set; }
            public List<Node> Nodes { get; set; }
            public Dictionary<string, Matrix> DeformationBones { get; set; }
        }
    }

    public class MeshImportSettings
    {
        private bool _importAsSkeletal;

        /// <summary>
        /// Determines if the mesh should be treated a a Sketal or Static mesh.
        /// </summary>
        [EditorVisible("Mesh", "Import as Skeletal")]
        public bool ImportAsSkeletal
        {
            get { return _importAsSkeletal; }
            set
            {
                _importAsSkeletal = value;
                ImportAsSkeletalChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// Determines whether or not duplicate vertices will be removed.
        /// </summary>
        [EditorVisible("Mesh", "Keep Overlapping Vertices")]
        public bool KeepOverlappingVertices { get; set; } = false;

        /// <summary>
        /// Determines whether or not duplicate vertices will be removed.
        /// </summary>
        [EditorVisible("Mesh", "Normal Import Method")]
        public NormalImportMethod NormalImportMethod { get; set; } = NormalImportMethod.ImportNormals;

        /// <summary>
        /// Determines if meshes should be merged.
        /// </summary>
        [EditorVisible("Static", "Merge Meshes")]
        public bool MergeMeshes { get; set; } = true;

        public event Action<bool> ImportAsSkeletalChanged;

        #region Skeletal

        /// <summary>
        /// The existing skeleton to use for this mesh. If none is provided, the skeleton will be imported from the file.
        /// </summary>
        [EditorVisible("Skeletal", "Skeleton")]
        public Content<RawAsset> Skeleton { get; set; }

        /// <summary>
        /// Determines if the skeleton reference pose should be updated.
        /// </summary>
        [EditorVisible("Skeletal", "Update Reference Pose")]
        public bool UpdateReferencePose { get; set; }

        /// <summary>
        /// Determines if the first frame should be used as the reference pose.
        /// </summary>
        [EditorVisible("Skeletal", "Use First Frame As Reference Pose")]
        public bool UseFirstFrameAsReferencePose { get; set; }

        /// <summary>
        /// If enabled, morph target swill be imported from the FBX file.
        /// </summary>
        [EditorVisible("Skeletal", "Import Morph Targets")]
        public bool ImportMorphTargets { get; set; }

        #endregion
    }
}
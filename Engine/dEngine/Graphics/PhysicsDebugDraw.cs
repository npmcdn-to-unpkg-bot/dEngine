// PhysicsDebugDraw.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using BulletSharp;
using C5;
using dEngine.Graphics.Structs;
using dEngine.Instances;
using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;
using Matrix = BulletSharp.Math.Matrix;
using Point = dEngine.Graphics.Structs.Point;

namespace dEngine.Graphics
{
    internal class PointArray : ArrayList<Point>
    {
        public Point[] Array => array;
    }

    internal class PhysicsDebugDraw : DebugDraw
    {
        private static readonly GfxShader LineShader = Shaders.Get("Line");
        private static readonly GfxShader.Pass LinePass = LineShader.GetPass();
        private readonly PointArray _points;
        private bool _bufferDirty;
        private Camera _camera;

        private DeviceContext _context;
        private int _lastLength = -1;
        private Buffer _lineBuffer;
        private VertexBufferBinding _lineBufferBinding;
        private int _lineCount;

        static PhysicsDebugDraw()
        {
        }

        /// <summary />
        public PhysicsDebugDraw()
        {
            _points = new PointArray();
        }

        public override DebugDrawModes DebugMode { get; set; } = DebugDrawModes.DrawWireframe;

        public void Draw(ref DeviceContext context, ref Camera camera)
        {
            _context = context;
            _camera = camera;

            if (_lineCount > 0)
            {
                if (_bufferDirty)
                {
                    var length = Point.Stride*2*_lineCount;

                    if (length > _lastLength)
                    {
                        _lineBuffer = Buffer.Create(Renderer.Device, BindFlags.VertexBuffer, _points.Array, 0,
                            ResourceUsage.Dynamic, CpuAccessFlags.Write);
                        _lineBufferBinding = new VertexBufferBinding(_lineBuffer, Point.Stride, 0);
                        _lastLength = length;
                    }
                    else
                    {
                        DataStream stream;
                        context.MapSubresource(_lineBuffer, MapMode.WriteDiscard, MapFlags.None, out stream);

                        var count = _points.Count;
                        for (var i = 0; i < count; i++)
                            stream.Write(_points[i]);

                        context.UnmapSubresource(_lineBuffer, 0);
                        _bufferDirty = false;
                    }
                }

                LinePass.Use(ref context);
                context.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.LineList;
                context.InputAssembler.SetVertexBuffers(0, _lineBufferBinding);
                context.Draw(_lineCount*2, _points.Offset);

                context.InputAssembler.PrimitiveTopology = SharpDX.Direct3D.PrimitiveTopology.TriangleList; // reset topology
                _points.Clear();
                _lineCount = 0;
            }
        }

        public override void DrawLine(ref BulletSharp.Math.Vector3 from, ref BulletSharp.Math.Vector3 to,
            ref BulletSharp.Math.Vector3 color)
        {
            var pointA = new Point(ref from, ref color);
            var pointB = new Point(ref to, ref color);

            _points.Add(pointA);
            _points.Add(pointB);

            _lineCount++;

            _bufferDirty = true;
        }

        public override void Draw3dText(ref BulletSharp.Math.Vector3 location, string textString)
        {
        }

        public override void ReportErrorWarning(string warningString)
        {
            Engine.Logger.Warn(warningString);
        }

        internal class CapsuleHead : CapsulePart
        {
            public CapsuleHead(float radius, float halfHeight, int upAxis, ref Matrix transform,
                ref BulletSharp.Math.Vector3 color) : base(radius, halfHeight, upAxis, ref transform, ref color)
            {
            }

            public override void UpdateRenderData()
            {
                base.UpdateRenderData();
                _renderData.Size = new Vector3(_radius);
            }
        }

        internal class CapsuleBody : CapsulePart
        {
            public CapsuleBody(float radius, float halfHeight, int upAxis, ref Matrix transform,
                ref BulletSharp.Math.Vector3 color) : base(radius, halfHeight, upAxis, ref transform, ref color)
            {
            }

            public override void UpdateRenderData()
            {
                base.UpdateRenderData();
                _renderData.Size = new Vector3(_radius, _halfHeight*2, _radius);
            }
        }

        internal abstract class CapsulePart : IRenderable
        {
            protected readonly BulletSharp.Math.Vector3 _color;
            protected readonly float _halfHeight;
            protected readonly float _radius;
            protected readonly Matrix _transform;
            protected InstanceRenderData _renderData;

            protected CapsulePart(float radius, float halfHeight, int upAxis, ref Matrix transform,
                ref BulletSharp.Math.Vector3 color)
            {
                _radius = radius;
                _halfHeight = halfHeight;
                _transform = transform;
                _color = color;
                throw new NotImplementedException();
            }

            public RenderObject RenderObject { get; set; }
            public int RenderIndex { get; set; }

            public InstanceRenderData RenderData
            {
                get { return _renderData; }
                set { _renderData = value; }
            }

            public virtual void UpdateRenderData()
            {
                _renderData.ModelMatrix = (SharpDX.Matrix)(CFrame)_transform;
                _renderData.Colour = new Colour(_color.X, _color.Y, _color.Z);
            }
        }
    }
}
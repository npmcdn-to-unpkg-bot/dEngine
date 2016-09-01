// ConstantBuffer.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct3D11;
using Buffer = SharpDX.Direct3D11.Buffer;

namespace dEngine.Graphics
{
    internal class ConstantBuffer<T> : IDisposable where T : struct
    {
        public Buffer Buffer;
        public T Data;
        public bool BufferCreated;

        public ConstantBuffer()
        {
            Renderer.InvokeResourceDependent(() => { Buffer = CreateBuffer(); });
        }

        public void Dispose()
        {
            Buffer.Dispose();
            BufferCreated = false;
        }

        private Buffer CreateBuffer()
        {
            BufferCreated = true;
            var size = Math.Max(Marshal.SizeOf<T>(), 16);
            var remainder = size%16;
            size = remainder == 0 ? size : size + 16 - remainder;
            return new Buffer(Renderer.Device, size, ResourceUsage.Dynamic,
                BindFlags.ConstantBuffer, CpuAccessFlags.Write, ResourceOptionFlags.None, 0);
        }

        public void Update(ref DeviceContext context)
        {
            if (Buffer?.IsDisposed != false)
                return;
            DataStream stream;
            context.MapSubresource(Buffer, MapMode.WriteDiscard, 0, out stream);
            {
                stream.Write(Data);
            }
            context.UnmapSubresource(Buffer, 0);
        }

        public static implicit operator Buffer(ConstantBuffer<T> constants)
        {
            return constants.Buffer;
        }
    }
}
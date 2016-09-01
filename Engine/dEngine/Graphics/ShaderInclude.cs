// ShaderInclude.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.IO;
using System.Reflection;
using SharpDX.D3DCompiler;

namespace dEngine.Graphics
{
    internal class ShaderInclude : Include
    {
        private Stream _includeStream;
        public IDisposable Shadow { get; set; }

        public Stream Open(IncludeType type, string fileName, Stream parentStream)
        {
            var stream =
                Assembly.GetExecutingAssembly().GetManifestResourceStream($"dEngine.Graphics.Shaders.{fileName}.shader");
            _includeStream = stream;

            return _includeStream;
        }

        public void Close(Stream stream = null)
        {
            Dispose();
        }

        public void Dispose()
        {
            _includeStream?.Dispose();
        }
    }
}
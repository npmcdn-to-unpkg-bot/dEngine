// RasterizerStates.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using SharpDX.Direct3D11;

namespace dEngine.Graphics.States
{
    internal static class RasterizerStates
    {
        public static RasterizerState NoCulling;
        public static RasterizerState Shadow;
        public static RasterizerState Wireframe;
        public static RasterizerState BackFaceCull;

        internal static void Load()
        {
            BackFaceCull = new RasterizerState(Renderer.Device, new RasterizerStateDescription
            {
                IsAntialiasedLineEnabled = false,
                CullMode = CullMode.Back,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                IsDepthClipEnabled = true,
                FillMode = FillMode.Solid,
                IsFrontCounterClockwise = false,
                SlopeScaledDepthBias = 0
            });

            NoCulling = new RasterizerState(Renderer.Device, new RasterizerStateDescription
            {
                FillMode = FillMode.Solid,
                CullMode = CullMode.None,
                DepthBias = 0,
                DepthBiasClamp = 0,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = false,
                IsFrontCounterClockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0
            });

            Shadow = new RasterizerState(Renderer.Device, new RasterizerStateDescription
            {
                CullMode = CullMode.Front,
                DepthBias = 0,
                DepthBiasClamp = 0,
                SlopeScaledDepthBias = 0,
                FillMode = FillMode.Solid,
                IsAntialiasedLineEnabled = false,
                IsDepthClipEnabled = true,
                IsFrontCounterClockwise = false,
                IsMultisampleEnabled = false,
                IsScissorEnabled = false
            });

            Wireframe = new RasterizerState(Renderer.Device, new RasterizerStateDescription
            {
                IsAntialiasedLineEnabled = false,
                CullMode = CullMode.None,
                DepthBias = 0,
                DepthBiasClamp = 0.0f,
                IsDepthClipEnabled = true,
                FillMode = FillMode.Wireframe,
                IsFrontCounterClockwise = false,
                IsMultisampleEnabled = true,
                IsScissorEnabled = false,
                SlopeScaledDepthBias = 0
            });
        }
    }
}
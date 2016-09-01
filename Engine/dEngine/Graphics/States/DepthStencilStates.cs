// DepthStencilStates.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using SharpDX.Direct3D11;

namespace dEngine.Graphics.States
{
    internal static class DepthStencilStates
    {
        internal static DepthStencilState Standard;
        internal static DepthStencilState NoDepth;
        internal static DepthStencilState DepthEnabled;

        internal static void Load()
        {
            Standard?.Dispose();
            NoDepth?.Dispose();

            MakeNoDepthDesc();
            MakeDepthEnabledState();
            Standard = new DepthStencilState(Renderer.Device, DepthStencilStateDescription.Default());
        }

        private static void MakeNoDepthDesc()
        {
            var depthDesc = new DepthStencilStateDescription
            {
                IsDepthEnabled = false,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.LessEqual,
                IsStencilEnabled = false,
                StencilReadMask = byte.MaxValue,
                StencilWriteMask = byte.MaxValue,
                FrontFace =
                {
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Replace,
                    Comparison = Comparison.Always
                }
            };
            depthDesc.BackFace = depthDesc.FrontFace;
            NoDepth = new DepthStencilState(Renderer.Device, depthDesc);
        }

        private static void MakeDepthEnabledState()
        {
            var depthDesc = new DepthStencilStateDescription
            {
                IsDepthEnabled = true,
                DepthWriteMask = DepthWriteMask.Zero,
                DepthComparison = Comparison.LessEqual,
                IsStencilEnabled = false,
                StencilReadMask = byte.MaxValue,
                StencilWriteMask = byte.MaxValue,
                FrontFace =
                {
                    DepthFailOperation = StencilOperation.Keep,
                    FailOperation = StencilOperation.Keep,
                    PassOperation = StencilOperation.Replace,
                    Comparison = Comparison.Always
                }
            };
            depthDesc.BackFace = depthDesc.FrontFace;
            DepthEnabled = new DepthStencilState(Renderer.Device, depthDesc);
        }
    }
}
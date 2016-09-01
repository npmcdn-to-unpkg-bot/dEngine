// SamplerStates.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using SharpDX.Direct3D11;

namespace dEngine.Graphics.States
{
    internal static class SamplerStates
    {
        internal static SamplerState Default;
        internal static SamplerState Brick;
        internal static SamplerState Wrap;
        internal static SamplerState Point;
        internal static SamplerState Shadow;
        internal static SamplerState ShadowPcf;
        internal static SamplerState[] States;

        internal static void Load()
        {
            Default?.Dispose();
            Brick?.Dispose();
            Wrap?.Dispose();
            Point?.Dispose();
            Shadow?.Dispose();
            ShadowPcf?.Dispose();

            Default = new SamplerState(Renderer.Device, SamplerStateDescription.Default());

            Wrap = new SamplerState(Renderer.Device, new SamplerStateDescription
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap
            });

            Point = new SamplerState(Renderer.Device, new SamplerStateDescription
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Wrap,
                Filter = Filter.MinMagMipPoint,
                ComparisonFunction = Comparison.Always,
                MaximumLod = 1,
                MinimumLod = 0,
                MipLodBias = 0,
                MaximumAnisotropy = 1
            });

            Shadow = new SamplerState(Renderer.Device, new SamplerStateDescription
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                Filter = Filter.ComparisonMinMagMipPoint,
                ComparisonFunction = Comparison.LessEqual
            });

            ShadowPcf = new SamplerState(Renderer.Device, new SamplerStateDescription
            {
                AddressU = TextureAddressMode.Clamp,
                AddressV = TextureAddressMode.Clamp,
                AddressW = TextureAddressMode.Clamp,
                Filter = Filter.ComparisonMinMagMipLinear,
                ComparisonFunction = Comparison.LessEqual
            });

            Brick = new SamplerState(Renderer.Device, new SamplerStateDescription
            {
                AddressU = TextureAddressMode.Wrap,
                AddressV = TextureAddressMode.Wrap,
                AddressW = TextureAddressMode.Wrap,
                Filter = Filter.Anisotropic,
                MaximumAnisotropy = 11,
                MipLodBias = 0,
                MinimumLod = 0,
                MaximumLod = 11
            });

            States = new[] {Default, Brick, Wrap, Point, Shadow, ShadowPcf};
        }
    }
}
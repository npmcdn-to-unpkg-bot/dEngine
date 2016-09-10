// DirectDrawSurface.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.IO;
using dEngine.Graphics;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.WIC;
#pragma warning disable 1591

namespace dEngine.Utility
{
    public class DdsBitmapDecoder : BitmapEncoder
    {
        public DdsDecoder DdsDecoder { get; }

        public DdsBitmapDecoder(IntPtr nativePtr) : base(nativePtr)
        {
            DdsDecoder = QueryInterface<DdsDecoder>();
        }

        public DdsBitmapDecoder(ImagingFactory factory) : base(factory, ContainerFormatGuids.Dds)
        {
            DdsDecoder = QueryInterface<DdsDecoder>();
        }

        public DdsBitmapDecoder(ImagingFactory factory, Stream stream = null)
            : base(factory, ContainerFormatGuids.Dds, stream)
        {
            DdsDecoder = QueryInterface<DdsDecoder>();
        }

        public DdsBitmapDecoder(ImagingFactory factory, Guid guidVendorRef, Stream stream = null)
            : base(factory, ContainerFormatGuids.Dds, guidVendorRef, stream)
        {
            DdsDecoder = QueryInterface<DdsDecoder>();
        }

        public DdsBitmapDecoder(ImagingFactory factory, WICStream stream = null)
            : base(factory, ContainerFormatGuids.Dds, stream)
        {
            DdsDecoder = QueryInterface<DdsDecoder>();
        }

        public DdsBitmapDecoder(ImagingFactory factory, Guid guidVendorRef, WICStream stream = null)
            : base(factory, ContainerFormatGuids.Dds, guidVendorRef, stream)
        {
            DdsDecoder = QueryInterface<DdsDecoder>();
        }
    }

    public class DdsBitmapEncoder : BitmapEncoder
    {
        public DdsEncoder DdsEncoder { get; }

        public DdsBitmapEncoder(ImagingFactory factory, Stream stream) : base(factory, ContainerFormatGuids.Dds, stream)
        {
        }
        
        public DdsBitmapEncoder(Data.Texture texture) : base(Renderer.ImagingFactory, ContainerFormatGuids.Dds)
        {
            DdsEncoder = QueryInterface<DdsEncoder>();
            DdsEncoder.Parameters = new DdsParameters()
            {
                DxgiFormat = Format.BC3_UNorm, Width = texture.Width, Height = texture.Height,
                MipLevels = 1,
                ArraySize = texture.NativeTexture.Description.ArraySize,
                Dimension = texture.NativeTexture.Description.OptionFlags.HasFlag(ResourceOptionFlags.TextureCube) ? DdsDimension.DdsTextureCube : DdsDimension.DdsTexture2D
            };
        }
    }
}
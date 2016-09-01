// TextureUtility.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using AlphaMode = SharpDX.Direct2D1.AlphaMode;
using BclBitmap = System.Drawing.Bitmap;
using BclPixelFormat = System.Drawing.Imaging.PixelFormat;
using PixelFormat = SharpDX.Direct2D1.PixelFormat;
using Rectangle = System.Drawing.Rectangle;

namespace dEngine.Graphics
{
    internal static class TextureUtility
    {
        [DllImport("kernel32.dll", EntryPoint = "CopyMemory", SetLastError = false)]
        public static extern void CopyMemory(IntPtr dest, IntPtr src, uint count);

        private static BclBitmap CropBitmap(BclBitmap bitmap, Rectangle cropArea)
        {
            var bmpCrop = bitmap.Clone(cropArea, bitmap.PixelFormat);
            return bmpCrop;
        }

        public static Texture2D CreateTexture2DArray(int tileWidth, int tileHeight, IList<BclBitmap> bitmaps,
            int arraySize = 1, int mipLevels = 1, ResourceOptionFlags optionFlags = ResourceOptionFlags.None)
        {
            var dataRectangles = new DataRectangle[arraySize*mipLevels];

            var texDesc = new Texture2DDescription
            {
                Width = tileWidth,
                Height = tileHeight,
                ArraySize = arraySize,
                BindFlags = BindFlags.ShaderResource,
                Usage = ResourceUsage.Immutable,
                CpuAccessFlags = CpuAccessFlags.None,
                Format = Format.R8G8B8A8_UNorm,
                MipLevels = mipLevels,
                OptionFlags = optionFlags,
                SampleDescription = new SampleDescription(1, 0)
            };

            for (var faceIndex = 0; faceIndex < bitmaps.Count; faceIndex++)
            {
                var face = bitmaps[faceIndex];

                if ((mipLevels == 1) && ((face.Width != tileWidth) || (face.Height != tileHeight)))
                    throw new InvalidDataException("Provided bitmaps have inconsistent dimensions.");

                var data = face.LockBits(new Rectangle(0, 0, face.Width, face.Height), ImageLockMode.ReadOnly,
                    BclPixelFormat.Format32bppPArgb);

                var stride = data.Stride;
                var buffer = new DataStream(data.Height*stride, true, true);

                for (var y = 0; y < data.Height; ++y)
                {
                    var offset = stride*y;

                    var bytes = new byte[stride];

                    Marshal.Copy(data.Scan0 + offset, bytes, 0, stride);

                    var subOffset = 0;
                    for (var x = 0; x < data.Width; x++)
                    {
                        var b = bytes[subOffset++];
                        var g = bytes[subOffset++];
                        var r = bytes[subOffset++];
                        var a = bytes[subOffset++];
                        var rgba = r | (g << 8) | (b << 16) | (a << 24);
                        buffer.Write(rgba);
                    }
                }

                dataRectangles[faceIndex] = new DataRectangle(buffer.DataPointer, stride);

                face.UnlockBits(data);
            }

            return new Texture2D(Renderer.Device, texDesc, dataRectangles);
        }

        public static Texture2D CreateCubemap(BclBitmap bitmap)
        {
            var tileDim = bitmap.Width/4;

            var top = CropBitmap(bitmap, new Rectangle(tileDim, 0, tileDim, tileDim));
            var left = CropBitmap(bitmap, new Rectangle(0, tileDim, tileDim, tileDim));
            var front = CropBitmap(bitmap, new Rectangle(tileDim, tileDim, tileDim, tileDim));
            var bottom = CropBitmap(bitmap, new Rectangle(tileDim, tileDim*2, tileDim, tileDim));
            var right = CropBitmap(bitmap, new Rectangle(tileDim*2, tileDim, tileDim, tileDim));
            var back = CropBitmap(bitmap, new Rectangle(tileDim*3, tileDim, tileDim, tileDim));
            var faces = new[] {right, left, top, bottom, front, back};

            return CreateTexture2DArray(tileDim, tileDim, faces, 6, 1, ResourceOptionFlags.TextureCube);
        }

        public static Bitmap1 LoadBitmap2D(BclBitmap bitmap)
        {
            var sourceArea = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            var imageRes = new Size(bitmap.Width, bitmap.Height);

            var bitmapData = bitmap.LockBits(sourceArea, ImageLockMode.ReadOnly,
                BclPixelFormat.Format32bppPArgb);

            var stride = bitmap.Width*sizeof(int);
            var bytesSize = bitmapData.Stride*bitmapData.Height;

            var byteArray = new byte[bytesSize];

            Marshal.Copy(bitmapData.Scan0, byteArray, 0, bytesSize);

            bitmap.UnlockBits(bitmapData);

            var tempStream = new DataStream(bytesSize, true, true);

            for (var y = 0; y < bitmap.Height; y++)
            {
                var offset = stride*y;
                for (var x = 0; x < bitmap.Width; x++)
                {
                    var b = byteArray[offset++];
                    var g = byteArray[offset++];
                    var r = byteArray[offset++];
                    var a = byteArray[offset++];
                    var rgba = r | (g << 8) | (b << 16) | (a << 24);
                    tempStream.Write(rgba);
                }
            }

            tempStream.Position = 0;

            var bitmapProperties =
                new BitmapProperties1(new PixelFormat(Format.R8G8B8A8_UNorm,
                    AlphaMode.Premultiplied));

            var newBitmap = new Bitmap1(Renderer.Context2D, new Size2(bitmap.Width, bitmap.Height), tempStream,
                stride,
                bitmapProperties);

            return newBitmap;
        }
    }
}
// ThumbnailManager.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using dEngine;
using dEngine.Data;
using dEngine.Instances;
using dEngine.Serializer.V1;

namespace dEditor.Widgets.Assets
{
    public static class ThumbnailManager
    {
        private static Dictionary<string, BitmapImage> _thumbnailCache;
        private static readonly Part _baseplate;

        static ThumbnailManager()
        {
            _baseplate = new Part {Size = new Vector3(512, 20, 512)};
            _thumbnailCache = new Dictionary<string, BitmapImage>();
        }

        /*
        private static BitmapSource Snapshot(Model model, int width, int height)
        {
            var modelSize = model.GetModelSize();

            var h = modelSize.Y;
            var dist = modelSize.magnitude;
            var fov = 2 * Mathf.Atan(h / (2 * dist)) * (180 / Mathf.Pi);

            _baseplate.Position = new Vector3(0, -modelSize.Y - 20, 0);
            _baseplate.Parent = model;

            var camera = new ThumbnailCamera
            {
                ThumbnailSize = new Vector2(width, height),
                FieldOfView = fov,
                CFrame =
                    CFrame.Angles(0, Mathf.Deg2Rad * -157.5f, 0) * CFrame.Angles(Mathf.Deg2Rad * -11.75f, 0, 0) *
                    new CFrame(new Vector3(0, 0, dist * 1.5f))
            };
            camera.Snapshot(model).Wait();
            var bytes = camera.BackBuffer.GetBytesPBGRA();
            //camera.Destroy();

            return BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32, BitmapPalettes.WebPalette,
                bytes, width * 4);
        }
        */

        public static BitmapSource GetThumbnail(string file, int width, int height)
        {
            using (var stream = File.OpenRead(file))
            {
                var contentType = AssetBase.PeekContent(stream);

                switch (contentType)
                {
                    /*
					case ContentType.SkeletalMesh:
					case ContentType.StaticMesh:
						var model = new Model();
						var geometry = Proto.Deserialize<dEngine.Data.Geometry>(stream);
						var offset = geometry.GetOffset();
						var scale = geometry.GetSize();
						var part = new Part {Parent = model, Size = scale, CFrame = new CFrame(-offset)};
						var mesh = new StaticMesh {Parent = part, Offset = -offset};
						mesh.SetGeometry(geometry);
						return Snapshot(model, width, height);
                        */
                    case ContentType.Texture:
                        var texture = new Texture();
                        texture.Load(stream);
                        return BitmapSource.Create(width, height, 96, 96, PixelFormats.Pbgra32,
                            BitmapPalettes.WebPalette,
                            texture.GetBytesPBGRA(), width*4);
                    /*
                    case ContentType.Game:
                        break;
                    case ContentType.Place:
                        break;
                    case ContentType.Model:
                        break;
                    case ContentType.Texture:
                        break;
                    case ContentType.Sound:
                        break;
                    case ContentType.Animation:
                        break;
                    case ContentType.Script:
                        break;
                    case ContentType.Cubemap:
                        break;
                    case ContentType.Video:
                        break;
                    case ContentType.Skeleton:
                        break;
                    case ContentType.Material:
                        break;
                        */
                    case ContentType.Unknown:
                    case null:
                    default:
                        return
                            new BitmapImage(new Uri($"/dEditor;component/Content/Icons/Thumbs/placeholder.png",
                                UriKind.Relative));
                }
            }
        }
    }
}
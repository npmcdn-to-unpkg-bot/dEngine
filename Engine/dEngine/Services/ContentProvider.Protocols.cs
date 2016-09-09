// ContentProvider.Protocols.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.IO;
using dEngine.Graphics;
using dEngine.Utility.Extensions;
using SharpDX.WIC;
using Steamworks;

namespace dEngine.Services
{
    public partial class ContentProvider
    {
        private static void RegisterProtocols()
        {
            Protocols["internal"] = new InternalProtocol();
            Protocols["temp"] = new TempProtocol();
            Protocols["file"] = new FileProtocol();
            Protocols["desktop"] = new DesktopProtocol();
            Protocols["http"] = Protocols["https"] = new HttpProtocol();
            Protocols["rbxassetid"] = new RbxAssetIdProtocol();
            Protocols["rbxasset"] = new RbxAssetProtocol();
            Protocols["avatar"] = new AvatarProtocol();
            Protocols["soundcloud"] = new SoundcloudProtocol();
        }

        internal static string GetAbsolutePath(Uri uri)
        {
            return uri.AbsoluteUri.Substring(uri.Scheme.Length + 3);
        }

        private class InternalProtocol : IContentProtocol
        {
            public Stream Fetch(Uri uri)
            {
                return GetEmbeddedFile(GetAbsolutePath(uri));
            }
        }

        private class TempProtocol : IContentProtocol
        {
            public Stream Fetch(Uri uri)
            {
                return File.OpenRead(Path.Combine(Engine.TempPath, GetAbsolutePath(uri)));
            }
        }

        private class FileProtocol : IContentProtocol
        {
            public Stream Fetch(Uri uri)
            {
                return File.Open(uri.AbsolutePath, FileMode.Open);
            }
        }

        private class DesktopProtocol : IContentProtocol
        {
            public Stream Fetch(Uri uri)
            {
                return File.Open(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), GetAbsolutePath(uri)), FileMode.Open);
            }
        }

        private class HttpProtocol : IContentProtocol
        {
            public Stream Fetch(Uri uri)
            {
                return HttpService.Get(uri).Result;
            }
        }

        private class RbxAssetIdProtocol : IContentProtocol
        {
            public Stream Fetch(Uri uri)
            {
                return HttpService.Get(new Uri("http://www.roblox.com/asset/?id=" + uri.Host)).Result;
            }
        }

        private class RbxAssetProtocol : IContentProtocol
        {
            public Stream Fetch(Uri uri)
            {
                throw new NotSupportedException("The rbxasset content protocol is not supported.");
            }
        }

        private class AvatarProtocol : IContentProtocol
        {
            public Stream Fetch(Uri uri)
            {
                var segments = uri.Segments;
                var userId = uint.Parse(uri.Host);
                var size = (AvatarSize)Enum.Parse(typeof(AvatarSize), segments[1].UppercaseFirst());
                return GetAvatar(size,
                    userId == 0
                        ? LoginService.SteamId
                        : new CSteamID(new AccountID_t(userId), EUniverse.k_EUniversePublic,
                            EAccountType.k_EAccountTypeIndividual));
            }

            internal static Stream GetAvatar(AvatarSize avatarSize, CSteamID steamId)
            {
                int avatar;

                switch (avatarSize)
                {
                    case AvatarSize.Small:
                        avatar = SteamFriends.GetSmallFriendAvatar(steamId);
                        break;
                    case AvatarSize.Medium:
                        avatar = SteamFriends.GetMediumFriendAvatar(steamId);
                        break;
                    case AvatarSize.Large:
                        avatar = SteamFriends.GetLargeFriendAvatar(steamId);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(avatarSize), avatarSize, null);
                }

                uint w;
                uint h;
                var ret = SteamUtils.GetImageSize(avatar, out w, out h);
                var width = (int)w;
                var height = (int)h;

                if (!ret || (width <= 0) || (height <= 0))
                    throw new InvalidDataException();

                var length = width * height * 4;
                var bytes = new byte[length];
                SteamUtils.GetImageRGBA(avatar, bytes, length);

                var outputStream = new MemoryStream();
                var formatGuid = ContainerFormatGuids.Png;
                var stride = PixelFormat.GetStride(PixelFormat.Format32bppPRGBA, width);

                using (var encoder = new BitmapEncoder(Renderer.ImagingFactory, formatGuid))
                {
                    encoder.Initialize(outputStream);

                    var bitmapFrameEncode = new BitmapFrameEncode(encoder);
                    bitmapFrameEncode.Initialize();
                    bitmapFrameEncode.SetSize(width, height);
                    bitmapFrameEncode.SetPixelFormat(ref formatGuid);
                    bitmapFrameEncode.WritePixels(height, stride, bytes);

                    bitmapFrameEncode.Commit();
                    encoder.Commit();
                }

                return outputStream;
            }
        }

        private class SoundcloudProtocol : IContentProtocol
        {
            public Stream Fetch(Uri uri)
            {
                var trackId = int.Parse(uri.Host);
                var track = SoundService.Service.GetSoundcloudTrackInfo(trackId);
                if ((bool)track["downloadable"])
                {
                    var url = track["download_url"] + "?client_id={ContentProvider.GetSoundcloudClientId()}";
                    return HttpService.Get(new Uri(url)).Result;
                }
                else if ((bool)track["streamable"])
                {
                    var url = track["stream_url"] + "?client_id={ContentProvider.GetSoundcloudClientId()}";
                    return HttpService.Get(new Uri(url)).Result;
                }
                else
                {
                    throw new InvalidDataException(
                        $"Cannot download or stream soundcloud track {trackId} ({track["title"]})");
                }
            }
        }
    }
}
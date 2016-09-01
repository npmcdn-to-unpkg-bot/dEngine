// SocialService.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.IO;
using dEngine.Graphics;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using NLog;
using SharpDX.WIC;
using Steamworks;

namespace dEngine.Services
{
    /// <summary>
    /// A service for using the social features of Steam.
    /// </summary>
    [TypeId(91)]
    [ExplorerOrder(-1)]
    public partial class SocialService : Service
    {
        /// <inheritdoc />
        public SocialService()
        {
            Service = this;
        }

        internal static object GetExisting()
        {
            return DataModel.GetService<SocialService>();
        }
    }

    public partial class SocialService
    {
        private static Logger LoggerInternal = LogManager.GetLogger(nameof(SocialService));
        internal static SocialService Service;
        internal static bool IsInitialized { get; private set; }

        internal static void Init()
        {
            LoggerInternal = LogManager.GetLogger(nameof(SocialService));

            IsInitialized = true;

            LoggerInternal.Info("SocialService initialized.");
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

            var length = width*height*4;
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
}
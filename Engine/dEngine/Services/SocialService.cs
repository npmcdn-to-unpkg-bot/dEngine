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
    }
}
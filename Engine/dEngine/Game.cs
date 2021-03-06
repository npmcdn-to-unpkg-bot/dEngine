﻿// Game.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Concurrent;
using dEngine.Instances;
using dEngine.Instances.Diagnostics;
using dEngine.Services;
using dEngine.Services.Networking;
using dEngine.Utility;
using Dynamitey;

namespace dEngine
{
    /// <summary>
    /// Manager class for the scene.
    /// </summary>
    public static class Game
    {
        /// <summary>
        /// If true, <see cref="Init" /> has finished.
        /// </summary>
        public static bool IsInitialized;

        static Game()
        {
            Instances = new ConcurrentDictionary<string, WeakReference<Instance>>(StringComparer.Ordinal);
            Worlds = new ConcurrentDictionary<IWorld, byte>();
            DataModel = new DataModel {Name = Engine.GameName ?? nameof(DataModel)};
            Stats = new DebugStats {Parent = DataModel, ParentLocked = true};
        }

        internal static ConcurrentDictionary<string, WeakReference<Instance>> Instances { get; }
        internal static ConcurrentDictionary<IWorld, byte> Worlds { get; set; }

        /// <summary>
        /// Fired when Init() finishes.
        /// </summary>
        public static event EventHandler Initialized;

        /// <summary>
        /// Fired when fired when an instance is created.
        /// </summary>
        public static event Action<Instance> InstanceAdded;

        /// <summary>
        /// Fired when fired when an instance is destroyed.
        /// </summary>
        public static event Action<Instance> InstanceRemoved;

        internal static void AddWorld(IWorld world)
        {
            Worlds.TryAdd(world);
        }

        internal static void RemoveWorld(IWorld world)
        {
            Worlds.TryRemove(world);
        }

        internal static void Init()
        {
            Logger.Info("Game initializing.");

            DataModel.Workspace = Workspace = DataModel.GetService<Workspace>();
            Workspace.SetGame(DataModel);

            DataModel.GetService<RunService>();
            DataModel.GetService<ContentProvider>();
            DataModel.GetService<InputService>();
            DataModel.GetService<SoundService>();
            DataModel.GetService<ReplicatedStorage>();
            DataModel.GetService<ServerStorage>();
            DataModel.GetService<ServerScriptService>();
            DataModel.GetService<ReplayService>();
            DataModel.GetService<HistoryService>();
            DataModel.GetService<SocialService>();
            DataModel.GetService<ContextActionService>();
            DataModel.GetService<CubemapFiltering>();

            Analytics = DataModel.GetService<AnalyticsService>();
            Players = DataModel.GetService<Players>();
            Lighting = DataModel.GetService<Lighting>();
            Selection = DataModel.GetService<SelectionService>();
            Selection.Name = "Selection";
            CoreEnvironment = new CoreEnvironment();
            StarterGui = new StarterGui {Parent = DataModel, ParentLocked = true};

            DebuggerManager = new DebuggerManager();


            DataModel.IsLoaded = true;
            DataModel.Loaded.Fire();
            IsInitialized = true;
            Initialized?.Invoke(null, null);
            Logger.Info("Game initialized.");
        }

        internal static void InvokeInstanceAdded(Instance instance)
        {
            InstanceAdded?.Invoke(instance);
        }

        internal static void InvokeInstanceRemoved(Instance instance)
        {
            InstanceRemoved?.Invoke(instance);
        }

        /// <summary>
        /// Creates a new instance of the given type.
        /// </summary>
        /// <param name="type">A type inherriting from Instance</param>
        /// <param name="parent">The object to parent the new instance to.</param>
        public static Instance CreateInstance(Type type, Instance parent = null)
        {
            Instance instance = Dynamic.InvokeConstructor(type);
            if (!instance.ParentLocked)
                instance.Parent = parent;
            return instance;
        }

        internal static void RegisterInitializeCallback(Action callback)
        {
            if (IsInitialized)
                callback();
            else
                Initialized += (s, e) => callback();
        }

        internal static readonly ILogger Logger = LogService.GetLogger();

        internal static readonly object Locker = new object();

#pragma warning disable 1591
        public static DataModel DataModel;
        public static Workspace Workspace;
        public static Lighting Lighting;
        public static Players Players;
        public static SelectionService Selection;
        public static AnalyticsService Analytics;
        internal static CoreEnvironment CoreEnvironment;
        public static NetworkClient NetworkClient;
        public static NetworkServer NetworkServer;
        public static CoreGui CoreGui => CoreEnvironment.CoreGui;
        public static DebuggerManager DebuggerManager;

        public static Camera FocusedCamera;

        internal static DebugStats Stats;
        public static StarterGui StarterGui;
#pragma warning restore 1591
    }
}
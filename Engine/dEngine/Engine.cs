﻿// Engine.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using dEngine.Graphics;
using dEngine.Serializer.V1;
using dEngine.Services;
using dEngine.Settings;
using dEngine.Settings.Global;
using dEngine.Settings.User;
using dEngine.Utility;
using dEngine.Utility.Extensions;
using dEngine.Utility.Native;
using JetBrains.Annotations;
using Microsoft.VisualBasic.Devices;

// ReSharper disable UnusedVariable

#pragma warning disable 1591

namespace dEngine
{
    /// <summary>
    /// The main engine class.
    /// </summary>
    public static class Engine
    {
        private static bool _isViewportFocused;
        internal static CancellationTokenSource CancelTokenSource;
        private static bool _isWindowFocused;
        private static bool _isViewportActive;

        [CanBeNull]
        public static string GameName { get; private set; }

        static Engine()
        {
            Logger = LogService.GetLogger();

            // ReSharper disable once AssignNullToNotNullAttribute
            UnpackNativeLibraries();

            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Commit = Assembly.GetExecutingAssembly().GetCustomAttribute<CommitIdAttribute>().CommitId;
            BuildType = Assembly.GetExecutingAssembly().GetCustomAttribute<BuildTypeAttribute>().BuildType;
            VersionWithMeta =
                Assembly.GetExecutingAssembly()
                    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                    .InformationalVersion;
        }

        private static void UnpackNativeLibraries()
        {
            var asm = Assembly.GetExecutingAssembly();
            var names = asm.GetManifestResourceNames();
            foreach (var name in names)
            {
                if (!name.Contains(".NativeLibraries.")) continue;
                var fileName = name.Substring(name.LastIndexOf(".", name.LastIndexOf(".", StringComparison.Ordinal) - 1, StringComparison.Ordinal)+1);

                using (var dll = File.Create(fileName))
                {
                    var stream = asm.GetManifestResourceStream(name);
                    stream?.CopyTo(dll);
                }
            }
        }

        internal static ILogger Logger { get; }

        /// <summary>
        /// The short ID of the current commit.
        /// </summary>
        public static string Commit { get; }

        /// <summary>
        /// The build type or branch, of the current commit.
        /// </summary>
        public static string BuildType { get; }

        /// <summary>
        /// The version of the engine.
        /// </summary>
        public static string Version { get; }

        /// <summary>
        /// The version of the engine with metadata.
        /// </summary>
        public static string VersionWithMeta { get; }

        /// <summary>
        /// If true, the application is currently closing.
        /// </summary>
        public static bool IsExiting { get; set; }

        /// <summary>
        /// The platform the process is running on.
        /// </summary>
        public static PlatformId PlatformId { get; private set; }

        /// <summary>
        /// The type of platform the process is running on.
        /// </summary>
        public static PlatformType PlatformType { get; private set; }

        /// <summary>
        /// The current process.
        /// </summary>
        internal static Process Process { get; set; }

        /// <summary>
        /// Gets/sets whether or not the render control is focused.
        /// </summary>
        public static bool IsViewportFocused
        {
            get { return _isViewportFocused; }
            set
            {
                if (value == _isViewportFocused) return;

                _isViewportFocused = value;

                if (value)
                    InputService.Service?.ViewportFocused.Fire();
                else
                    InputService.Service?.ViewportFocusReleased.Fire();

                ContextActionService.SetState("viewportFocus", value);
            }
        }

        /// <summary>
        /// Gets/sets whether or not the render control is active.
        /// </summary>
        public static bool IsViewportActive
        {
            get { return _isViewportActive; }
            set
            {
                if (value == _isViewportActive) return;

                _isViewportActive = value;

                ContextActionService.SetState("viewportActive", value);
            }
        }

        public static bool IsWindowFocused
        {
            get { return _isWindowFocused; }
            set
            {
                _isWindowFocused = value;

                if (value)
                    InputService.Service?.WindowFocused.Fire();
                else
                    InputService.Service?.WindowFocusReleased.Fire();
            }
        }
        
        internal static IntPtr Handle { get; private set; }

        /// <summary>
        /// The control of the <see cref="Handle" />.
        /// </summary>
        public static Control Control { get; set; }

        /// <summary>
        /// Gets the path to a temporary directory, which is deleted on engine shutdown.
        /// </summary>
        public static string TempPath { get; private set; }

        /// <summary>
        /// Gets the path to a game directory in the user's documents folder.
        /// </summary>
        public static string DocumentsPath { get; private set; }

        /// <summary>
        /// The user settings container.
        /// </summary>
        public static UserSettings UserSettings { get; private set; }

        /// <summary>
        /// The global settings container.
        /// </summary>
        public static GlobalSettings GlobalSettings { get; private set; }

        /// <summary>
        /// The mode to run the engine }
        /// </summary>
        public static EngineMode Mode { get; private set; }

        /// <summary>
        /// The Steam App ID.
        /// </summary>
        public static uint AppId { get; set; }

        /// <summary>
        /// A callback which is invoked when the place is saved.
        /// </summary>
        public static Action<string, Stream> SavePlace { get; set; }

        /// <summary>
        /// A callback which is invoked when the game is saved.
        /// </summary>
        public static Action<Stream> SaveGame { get; set; }

        /// <summary>
        /// Fired when the application is closing.
        /// </summary>
        public static event Action Exiting;

        /// <summary>
        /// Fires up the engine. And then it makes noise.
        /// </summary>
        public static void Start(EngineMode engineMode, string gameName)
        {
            var startupStopwatch = Stopwatch.StartNew();

            GameName = gameName;
            AppId = 480;
            Mode = engineMode;

            LogService.ConfigureLoggers();
            Logger.Info("Log opened.");

            Process = Process.GetCurrentProcess();
            PlatformId = PlatformId.Windows;
            PlatformType = PlatformType.Desktop;
            
            DocumentsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), gameName);
            Logger.Info($"Documents path: {DocumentsPath}");
            Directory.CreateDirectory(DocumentsPath);

            TempPath = Path.Combine(Path.GetTempPath(), "dEngine");
            Logger.Info($"Temp path: {TempPath}");
            if (Directory.Exists(TempPath))
                ContentProvider.DeleteDirectory(TempPath);
            Directory.CreateDirectory(TempPath);
            
            UserSettings = new UserSettings();
            UserSettings.Load();
            GlobalSettings = new GlobalSettings();
            GlobalSettings.Load();
            UserAnalyticsSettings.SessionCount++;

            Logger.Info($"Command line args: {string.Join(" ", Environment.GetCommandLineArgs().Skip(1))})");
            Logger.Info($"Base directory: {Environment.CurrentDirectory}");

            Logger.Info($"Engine mode: {engineMode}");
            Logger.Info($"Graphics mode: {RenderSettings.GraphicsMode}");

            Logger.Info($"User: {Environment.UserName} on {Environment.MachineName}");
            Logger.Info($"CPU: {DebugSettings.CpuName} ({Environment.ProcessorCount} processors)");
            Logger.Info($"Memory: {((long)new ComputerInfo().TotalPhysicalMemory).ToPrettySize()}");

            CancelTokenSource = new CancellationTokenSource();

            Inst.Init();

            var graphicsStopwatch = Stopwatch.StartNew();
            Logger.Info("Starting GraphicsThread...");
            StartThread(nameof(GraphicsThread), GraphicsThread.Start);
            GraphicsThread.Wait();
            Logger.Info($"GraphicsThread started, took {graphicsStopwatch:mm\\:ss}.");

            var audioStopwatch = Stopwatch.StartNew();
            Logger.Info("Starting AudioThread...");
            StartThread(nameof(AudioThread), AudioThread.Start);
            AudioThread.Wait();
            Logger.Info($"AudioThread started, took {audioStopwatch:mm\\:ss}.");

            var gameStopwatch = Stopwatch.StartNew();
            Logger.Info("Starting GameThread...");
            StartThread(nameof(GameThread), GameThread.Start);
            GameThread.Wait();
            Logger.Info("GameThread started. (took {gameThreadStopwatch:mm\\:ss})");
            Logger.Info($"Engine started, took {startupStopwatch:mm\\:ss}.");

            AppDomain.CurrentDomain.ProcessExit += (s, e) => Shutdown(0);
        }

        /// <summary>
        /// Shuts down the engine, cleaning up resources.
        /// </summary>
        public static void Shutdown(int exitCode = 1)
        {
            if (IsExiting)
                return;

            Logger.Warn("Shutting down...");

            AnalyticsService.EndSession();

            ContentProvider.DeleteDirectory(TempPath);

            IsExiting = true;
            
            Game.DataModel.OnClose();
            GlobalSettings.Save();
            UserSettings.Save();

            CancelTokenSource.Cancel();
            
            Logger.Info("Engine has shutdown.");
            Exiting?.Invoke();
        }

        /// <summary>
        /// Sets the handle for rendering and input.
        /// </summary>
        public static void SetHandle(IntPtr handle)
        {
            var lastControl = Control;

            Handle = handle;
            Control = Control.FromHandle(handle);
            HandleSet?.Invoke(handle);

            LayoutEventHandler sizeChanged = (s, e) =>
            {
                Renderer.ResizeNextFrame = true;
                Renderer.ControlSize = new Vector2(Control.Width, Control.Height);
            };

            if (lastControl != null)
                lastControl.Layout -= sizeChanged;

            if (handle != IntPtr.Zero)
                Control.Layout += sizeChanged;
        }

        private static void StartThread(string name, Action<object> startMethod)
        {
            var thread = new Thread(o =>
                {
                    try
                    {
                        startMethod(o);
                    }
                    catch (Exception e)
                    {
                        OnThreadFailed(name, e);
                    }
                })
                {Name = name};
            thread.Start();
            Game.DataModel.BindToClose((Action)thread.Abort, int.MaxValue);
        }

        private static void OnThreadFailed(string threadName, Exception exception)
        {
            if (IsExiting)
                return;

            Logger.Fatal(exception,
                $"{threadName} exited with unhandled exception ({exception.GetType().Name}) - application must close.");

            Shutdown();
        }

        /// <summary>
        /// Fired when <see cref="SetHandle" /> is called.
        /// </summary>
        public static event Action<IntPtr> HandleSet;

        internal static class GameThread
        {
            internal const float FixedStep = 1/50f;

            private static ConcurrentWorkQueue<Action> _queue;
            internal static ManualResetEventSlim _resetter = new ManualResetEventSlim(false);

            internal static void Start(object obj)
            {
                _queue = new ConcurrentWorkQueue<Action>(action => action());

                Game.Init();
                RunService.Init();
                SocialService.Init();
                HttpService.Init();
                ScriptService.Init();
                //AnalyticsService.Init();

                _resetter.Set();
                var stopwatch = Stopwatch.StartNew();
                var physicsStopwatch = Stopwatch.StartNew();

                while (!CancelTokenSource.IsCancellationRequested)
                {
                    var step = stopwatch.Elapsed.TotalSeconds;
                    stopwatch.Restart();

                    // User Input
                    InputService.Step();

                    Game.FocusedCamera?.UpdateCamera(step);

                    // Network Replication
                    Game.NetworkServer?.Update(step);
                    Game.NetworkClient?.Update(step);

                    // wait() resume
                    ScriptService.ResumeWaitingScripts();

                    // Stepped
                    RunService.Update(step);
                    _queue.Work();

                    // Physics
                    var physicsStep = (float)physicsStopwatch.Elapsed.TotalSeconds;
                    foreach (var world in Game.Worlds)
                        world.Key.Physics?.Step(physicsStep);
                    physicsStopwatch.Restart();

                    // Heartbeat
                    RunService.Service.Heartbeat.Fire(step);
                }
            }

            public static void Wait()
            {
                _resetter.Wait(CancelTokenSource.Token);
            }

            public static void Enqueue(Action action)
            {
                _queue.Enqueue(action);
            }
        }

        internal static class GraphicsThread
        {
            private static ConcurrentWorkQueue<Action> _queue;
            internal static ManualResetEventSlim _resetter = new ManualResetEventSlim(false);

            internal static void Start(object obj)
            {
                _queue = new ConcurrentWorkQueue<Action>(action => action());

                switch (RenderSettings.GraphicsMode)
                {
                    case GraphicsMode.Direct3D11:
                        Renderer.Init();
                        break;
                    case GraphicsMode.NoGraphics:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(RenderSettings.GraphicsMode),
                            $"Unsupported graphics mode \"{RenderSettings.GraphicsMode}\"");
                }
                Logger.Info("Loading primitives...");
                Primitives.Load();
                Logger.Info("Primitives loaded.");

                if (RenderSettings.GraphicsMode == GraphicsMode.NoGraphics)
                    return;

                _resetter.Set();
                var _stopwatch = Stopwatch.StartNew();

                Logger.Info("Entering render loop...");
                while (!CancelTokenSource.IsCancellationRequested)
                {
                    Renderer.Update(_stopwatch.Elapsed.TotalSeconds);
                    _queue.Work();
                    _stopwatch.Restart();
                }

                Renderer.Shutdown();
            }

            public static void Wait()
            {
                _resetter.Wait(CancelTokenSource.Token);
            }

            public static void Enqueue(Action action)
            {
                _queue.Enqueue(action);
            }
        }

        internal static class AudioThread
        {
            public static IntPtr XAudioDLL;

            private static ConcurrentWorkQueue<Action> _queue;
            internal static ManualResetEventSlim _resetter = new ManualResetEventSlim(false);

            internal static void Start(object obj)
            {
                _queue = new ConcurrentWorkQueue<Action>(action => action());

                SoundService.Init();

                XAudioDLL = Kernel32.LoadLibraryEx("XAudio2_7.DLL", IntPtr.Zero, (Kernel32.LoadLibraryFlags)0x00000800);

                _resetter.Set();
                //var _stopwatch = Stopwatch.StartNew();

                while (!CancelTokenSource.IsCancellationRequested)
                {
                    if (!SoundService.Update())
                        if (SoundService.IsCriticalError)
                            SoundService.Reset();
                    _queue.Work();
                    //_stopwatch.Restart();
                }
            }

            public static void Wait()
            {
                _resetter.Wait();
            }

            public static void Enqueue(Action action)
            {
                _queue.Enqueue(action);
            }
        }
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class BuildTypeAttribute : Attribute
    {
        public BuildTypeAttribute(string buildType)
        {
            BuildType = buildType;
        }

        public string BuildType { get; }
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class BuildNumberAttribute : Attribute
    {
        public BuildNumberAttribute(string buildNumber)
        {
            BuildNumber = buildNumber;
        }

        public string BuildNumber { get; }
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public sealed class CommitIdAttribute : Attribute
    {
        public CommitIdAttribute(string commitId)
        {
            CommitId = commitId;
        }

        public string CommitId { get; }
    }
}
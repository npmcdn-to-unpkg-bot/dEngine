// Editor.xaml.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using dEditor.Framework;
using dEditor.Framework.Plugins;
using dEditor.Instances;
using dEditor.Shell;
using dEditor.Utility;
using dEngine;
using dEngine.Instances;
using dEngine.Services;
using Gma.System.MouseKeyHook;

namespace dEditor
{
    /// <summary>
    /// Interaction logic for Editor.xaml
    /// </summary>
    public partial class Editor
    {
        public new static Editor Current;
        private Project _project;

        public Editor()
        {
            Current = this;

            Logger = LogService.GetLogger();

            Plugins = new List<Plugin>();

            EditorDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory);
            var documents = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            EditorDocumentsPath = Path.Combine(documents, "dEditor");
            PluginsPath = Path.Combine(EditorDocumentsPath, "Plugins");

            WindowManager = new WindowManager();

            LayoutFileName = Path.Combine(Current.EditorDirectory, "Layout.xaml");

            DispatcherUnhandledException += CurrentOnDispatcherUnhandledException;

            ProjectChanged += OnProjectChanged;
        }

        internal EditorSettings Settings { get; set; }

        public ShellViewModel Shell { get; internal set; }

        public WindowManager WindowManager { get; internal set; }

        public static ILogger Logger { get; internal set; }

        public string EditorDirectory { get; }
        public string EditorDocumentsPath { get; }
        public string PluginsPath { get; }

        public string LayoutFileName { get; }

        public static IMouseEvents MouseHook { get; set; }

        public List<Plugin> Plugins { get; }

        /// <summary>
        /// The currently loaded project.
        /// </summary>
        public Project Project
        {
            get { return _project; }
            internal set
            {
                if (value == _project) return;
                _project = value;
                ProjectChanged?.Invoke(value);
            }
        }

        public AppArgs Args { get; set; }

        /// <summary>
        /// Fired when <see cref="Project" /> is changed.
        /// </summary>
        public event Action<Project> ProjectChanged;

        private void OnProjectChanged(Project project)
        {
            if (project == null)
            {
                UnloadPlugins();
                Logger.Info("Project closed.");
            }
            else
            {
                LoadPlugins();
                Logger.Info($"Project \"{project.Name}\" opened.");
            }
        }

        public void LoadPlugins()
        {
            UnloadPlugins();

            var pluginsPath = Path.Combine(EditorDocumentsPath, "Plugins");

            if (!Directory.Exists(pluginsPath))
                Directory.CreateDirectory(pluginsPath);

            var pluginsFiles =
                Directory.GetFiles(pluginsPath).Where(f => f.EndsWith(".plugin"));

            var insertService = DataModel.GetService<InsertService>();
            var totalPlugins = 0;
            var pluginsLoaded = 0;
            foreach (var pluginFile in pluginsFiles)
            {
                totalPlugins++;

                try
                {
                    var container = insertService.LoadAsset(pluginFile);
                    var script = container as Script;

                    if (script != null)
                    {
                        container = new Folder();
                        script.Parent = container;
                    }

                    if (!(container is Model || container is Folder))
                        Engine.Logger.Warn(
                            $"Plugin file {Path.GetFileName(pluginFile)} must be a Folder, Model or Script. (was a {container.ClassName})");

                    Plugin.Load(container);
                }
                catch (Exception e)
                {
                    Logger.Error(e, $"Could not load plugin \"{Path.GetFileName(pluginFile)}\"");
                    continue;
                }

                pluginsLoaded++;
            }

            if (totalPlugins == 0)
                Logger.Info("No plugins found.");
            else if (pluginsLoaded == 0)
                Logger.Info($"Could not load any plugins. ({totalPlugins} failed)");
            else
                Logger.Info($"Plugins loaded. ({pluginsLoaded}/{totalPlugins})");
        }

        public void UnloadPlugins()
        {
            foreach (var plugin in Plugins.ToList())
                plugin.Destroy();
        }

        private void CurrentOnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Exception.ShowCrashDialog();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            // todo: save recent projects list

            var shell = Current.Shell;
            shell.LayourItemStatePersister.SaveState(shell, shell.View, Current.LayoutFileName);

            Engine.Shutdown();
            //Environment.Exit(0);
        }
    }
}
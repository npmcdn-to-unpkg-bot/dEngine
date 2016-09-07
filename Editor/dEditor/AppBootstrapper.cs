// AppBootstrapper.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.ReflectionModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using dEditor.Framework;
using dEditor.Framework.Services;
using dEditor.Instances;
using dEditor.Shell;
using dEditor.Shell.Client;
using dEditor.Tools;
using dEditor.Widgets.StartPage;
using dEngine;
using dEngine.Instances;
using dEngine.Services;
using dEngine.Utility.Extensions;
using DateTime = System.DateTime;
using TimeSpan = System.TimeSpan;

namespace dEditor
{
    public class AppBootstrapper : BootstrapperBase
    {
        internal DispatcherTimer AutoSaveTimer;
        private List<Assembly> _priorityAssemblies;

        protected CompositionContainer Container { get; set; }

        internal IList<Assembly> PriorityAssemblies => _priorityAssemblies;

        public AppBootstrapper()
        {
            PreInitialize();
            Initialize();

            Engine.SaveGame = data =>
            {
                var project = Project.Current;
                Debug.Assert(project != null);
                var path = Path.Combine(project.ProjectPath, $"{project.Name}.game");
                using (var file = File.Create(path))
                {
                    data.CopyTo(file);
                }
            };

            Engine.SavePlace = (place, data) =>
            {
                var project = Project.Current;
                Debug.Assert(project != null);
                var path = Path.Combine(project.ProjectPath, "Places", $"{place}.place");
                using (var file = File.Create(path))
                {
                    data.CopyTo(file);
                }
            };

            ContentManager.RegisterProtocols();
            Engine.Start(EngineMode.LevelEditor, "dEditor");
            Engine.Exiting += () => Editor.Current.Dispatcher.Invoke(() => Editor.Current.Shutdown());

            DataModel.SetStartupArguments(new Dictionary<string, string> {{"IsEditor", "true"}});
            InputService.MouseInputApi = InputApi.Windows;
            Editor.Current.Settings = new EditorSettings {Name = "Editor", Parent = Engine.Settings};
            Engine.Settings.Load();

            KeyBindings.Init();
            FindService.Init();

            ToolManager.SelectTool.IsEquipped = true;

            AutoSaveTimer = new DispatcherTimer(TimeSpan.FromMinutes(EditorSettings.AutosaveInterval),
                DispatcherPriority.Background, PerformAutoSave, Editor.Current.Dispatcher);
        }
        
        protected void PreInitialize()
        {
            // converts fbx to engine binary format
            //ContentManager.ConvertPrimitivesToGeometry();
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return new[] { Assembly.GetEntryAssembly() };
        }

        protected override void Configure()
        {
            // Add all assemblies to AssemblySource (using a temporary DirectoryCatalog).
            var directoryCatalog = new DirectoryCatalog(@"./");
            AssemblySource.Instance.AddRange(
                directoryCatalog.Parts
                    .Select(part => ReflectionModelServices.GetPartType(part).Value.Assembly)
                    .Where(assembly => !AssemblySource.Instance.Contains(assembly)));

            // Prioritise the executable assembly. This allows the client project to override exports, including IShell.
            // The client project can override SelectAssemblies to choose which assemblies are prioritised.
            _priorityAssemblies = SelectAssemblies().ToList();
            var priorityCatalog = new AggregateCatalog(_priorityAssemblies.Select(x => new AssemblyCatalog(x)));
            var priorityProvider = new CatalogExportProvider(priorityCatalog);

            // Now get all other assemblies (excluding the priority assemblies).
            var mainCatalog = new AggregateCatalog(
                AssemblySource.Instance
                    .Where(assembly => !_priorityAssemblies.Contains(assembly))
                    .Select(x => new AssemblyCatalog(x)));
            var mainProvider = new CatalogExportProvider(mainCatalog);

            Container = new CompositionContainer(priorityProvider, mainProvider);
            priorityProvider.SourceProvider = Container;
            mainProvider.SourceProvider = Container;

            var batch = new CompositionBatch();

            BindServices(batch);
            batch.AddExportedValue(mainCatalog);

            Container.Compose(batch);
        }

        protected virtual void BindServices(CompositionBatch batch)
        {
            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(Container);
            batch.AddExportedValue(this);
        }

        protected override object GetInstance(Type serviceType, string key)
        {
            var contract = string.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(serviceType) : key;
            var exports = Container.GetExports<object>(contract);

            var export = exports.FirstOrDefault();

            if (export != null)
                return export.Value;

            throw new Exception($"Could not locate any instances of contract {contract}.");
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            if (typeof(Document).IsAssignableFrom(service))
                return Editor.Current.Shell?.Items.Where(w => w.GetType() == service) ?? Enumerable.Empty<object>();
            if (typeof(Widget).IsAssignableFrom(service))
                return Editor.Current.Shell?.Widgets.Where(w => w.GetType() == service) ?? Enumerable.Empty<object>();
            return new[] {service.FastConstruct()};
        }

        private static void PerformAutoSave(object sender, EventArgs e)
        {
            if ((RunService.SimulationState != SimulationState.Stopped) || // do not auto-save during sessions
                (Editor.Current.MainWindow.WindowState == WindowState.Minimized) || EditorSettings.AutosaveEnabled)
                // do not auto-save if window is minimized
                return;

            var proj = Editor.Current.Project;

            if (proj == null)
                return;

            Engine.Logger.Trace("Autosaving...");

            var archiveFileName = Path.Combine(EditorSettings.AutosavePath,
                $"{proj.Name}_{DateTime.Now:yyyy-MM-dd-HH-mm-ss}.zip");

            try
            {
                var dir = Path.Combine(proj.ProjectPath, "Bin");

                if (Directory.Exists(dir))
                    Directory.Delete(dir, true);
            }
            catch
            {
                // ignored
            }

            ZipFile.CreateFromDirectory(proj.ProjectPath, archiveFileName, CompressionLevel.Optimal, false);

            //Engine.Logger.Trace($"Autosave complete - {archiveFileName}");
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            var args = AppArgs.Parse(e.Args);
            Editor.Current.Args = args;

            if (!string.IsNullOrEmpty(args.API))
                File.WriteAllText(args.API, API.Dump());

            var settings = new Dictionary<string, object>
            {
                {"SizeToContent", SizeToContent.Manual},
                {"MinWidth", 1024},
                {"MinHeight", 700}
            };

            if (args.UseClientShell)
            {
                DisplayRootViewFor<ClientShellViewModel>(settings);
            }
            else
            {
                DisplayRootViewFor<IShell>(settings);

                var shell = Editor.Current.Shell;

                if (!File.Exists(Editor.Current.LayoutFileName))
                {
                    var defaultLayoutStream = Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("dEditor.Content.DefaultLayout.xaml");

                    Debug.Assert(defaultLayoutStream != null);

                    using (var layoutFile = File.Create(Editor.Current.LayoutFileName))
                    {
                        defaultLayoutStream.CopyTo(layoutFile);
                    }
                }

                shell.LayourItemStatePersister.LoadState(shell, shell.View, Editor.Current.LayoutFileName);

                shell.OpenDocument(new StartPageViewModel());

                // TODO: remove test autoload
                //var p = Project.Load(@"C:\Users\Dan\Documents\dEditor\Projects\MyGame\MyGame.dproj");
                //p.Open();

                /*
                var mat = new Material();
                var tex = new TextureParamaterNode();
                mat.AddNode(tex);
                tex.RGB.ConnectTo(((FinalNode)mat.FinalNode).BaseColour);
                shell.OpenDocument(new MaterialEditorViewModel(mat));
                */
            }
        }

        protected override void OnExit(object sender, EventArgs e)
        {
            base.OnExit(sender, e);

            Editor.Current.Shell.Viewport?.TryClose();
        }
    }
}
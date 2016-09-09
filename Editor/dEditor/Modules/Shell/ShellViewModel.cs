// ShellViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms.VisualStyles;
using Caliburn.Micro;
using dEditor.Framework;
using dEditor.Framework.Commands;
using dEditor.Framework.Services;
using dEditor.Modules.Dialogs.MeshImport;
using dEditor.Modules.Dialogs.Settings;
using dEditor.Modules.Shell.CommandBar;
using dEditor.Modules.Shell.Commands;
using dEditor.Modules.Shell.StatusBar;
using dEditor.Modules.Widgets.CodeEditor;
using dEditor.Modules.Widgets.ProjectEditor;
using dEditor.Modules.Widgets.Viewport;
using dEditor.Tools;
using dEditor.Tools.Building;
using dEngine.Services;

namespace dEditor.Modules.Shell
{
    [Export(typeof(IShell))]
    public class ShellViewModel : Conductor<IDocument>.Collection.OneActive, IShell
    {
        private ILayoutItem _activeLayoutItem;

        public ShellViewModel()
        {
            Widgets = new ObservableCollection<IWidget>();
            RecentCommands = new ObservableCollection<string>();
            LayourItemStatePersister = new LayourItemStatePersister();

            ToolManager.SelectedToolChanged += tool =>
            {
                NotifyOfPropertyChange(nameof(ToolIncrement));
                NotifyOfPropertyChange(nameof(IsBuildTool));
                IncrementOptions = (tool as StudioBuildTool)?.IncrementOptions;
                NotifyOfPropertyChange(nameof(IncrementOptions));
            };

            SaveProjectCommand = new SaveProjectCommand(this);

            ActiveDocumentChanging += OnActiveDocumentChanging;

            Editor.Current.ProjectChanged += project => NotifyOfPropertyChange(() => DisplayName);

            Editor.Current.Shell = this;
        }

        public bool IsBuildTool => ToolManager.SelectedTool is StudioBuildTool;

        public object ToolIncrement
        {
            get { return (ToolManager.SelectedTool as StudioBuildTool)?.Increment ?? 1; }
            set
            {
                var btool = ToolManager.SelectedTool as StudioBuildTool;
                var dbl = value as double? ?? -1;
                if ((btool != null) && (dbl >= 0))
                    btool.Increment = dbl;
            }
        }

        public IEnumerable<double> IncrementOptions { get; private set; }

        public IDocument ActiveDocument
        {
            get { return ActiveItem; }
            set { ActiveItem = value; }
        }

        public ObservableCollection<IWidget> Widgets { get; }
        public ObservableCollection<string> RecentCommands { get; }
        public bool ShowFloatingWindowsInTaskbar { get; set; }
        public IEnumerable<object> FloatingWindows { get; internal set; }
        public LayourItemStatePersister LayourItemStatePersister { get; }
        public ShellView View { get; private set; }
        public ViewportViewModel Viewport { get; set; }

        public override string DisplayName
        {
            get
            {
                return Editor.Current.Project != null
                    ? $"{Editor.Current.Project.Name} - dEditor"
                    : "dEditor";
            }
            set { }
        }

        public ILayoutItem ActiveLayoutItem
        {
            get { return _activeLayoutItem; }
            set
            {
                if (ReferenceEquals(_activeLayoutItem, value))
                    return;

                _activeLayoutItem = value;

                var doc = value as Document;
                if (doc != null)
                    ActivateItem(doc);

                NotifyOfPropertyChange(() => ActiveLayoutItem);
            }
        }

        private void OnActiveDocumentChanging(IDocument document)
        {
            ContextActionService.SetState("viewportActive", document is ViewportViewModel);
            ContextActionService.SetState("scriptEditorActive", document is CodeEditorViewModel);
        }

        public event Action<IDocument> ActiveDocumentChanging;
        public event Action<IDocument> ActiveDocumentChanged;

        public override void ActivateItem(IDocument item)
        {
            ActiveDocumentChanging?.Invoke(item);

            var lastItem = ActiveItem;

            // BUG: InvalidOperationException
            // Cannot change ObservableCollection during a CollectionChanged event.
            // when pressing ctrl+o
            base.ActivateItem(item);

            if (!ReferenceEquals(item, lastItem))
                ActiveDocumentChanged?.Invoke(item);
        }

        protected override void OnViewLoaded(object view)
        {
            View = (ShellView)view;
        }

        internal void SaveLayout(Stream stream)
        {
            LayoutUtility.SaveLayout(View.DockingManager, stream);
        }

        internal void LoadLayout(Stream stream, Action<IWidget> addToolCallback, Action<IDocument> addDocumentCallback,
            Dictionary<string, ILayoutItem> itemsState)
        {
            LayoutUtility.LoadLayout(View.DockingManager, stream, addDocumentCallback, addToolCallback, itemsState);
        }

        public void ShowProjectProperties()
        {
            if (Project.Current != null)
            {
                var doc =
                Editor.Current.Shell.Items.OfType<ProjectEditorViewModel>().FirstOrDefault() ?? new ProjectEditorViewModel();
                doc.Activate();
            }
        }

        /// <summary>
        /// Shows an instance of a dialog.
        /// </summary>
        public void ShowDialog(IDialog dialog)
        {
            Editor.Current.WindowManager.ShowDialog(dialog, null, new Dictionary<string, object>
            {
                {"MinWidth", dialog.MinWidth},
                {"MinHeight", dialog.MinHeight},
                {"MaxWidth", dialog.MaxWidth},
                {"MaxHeight", dialog.MaxHeight},
                {"Width", dialog.StartingWidth},
                {"Height", dialog.StartingHeight},
                {"ShowInTaskbar", false},
                {"WindowStyle", WindowStyle.ToolWindow}
            });

            dialog.IsVisible = true;
        }

        /// <summary>
        /// Shows an instance of a tool.
        /// </summary>
        public void ShowTool(IWidget tool)
        {
            if (tool.IsVisible)
                throw new InvalidOperationException("Tool already shown.");

            Widgets.Add(tool);
            tool.IsVisible = true;
        }

        /// <summary>
        /// Hides an instance of a tool.
        /// </summary>
        public void HideTool(Widget tool)
        {
            Widgets.Remove(tool);
            tool.IsVisible = false;
        }

        /// <summary>
        /// Opens an instance of a document.
        /// </summary>
        public void OpenDocument(IDocument document)
        {
            if (document.IsVisible)
                throw new InvalidOperationException("Document already open.");

            Items.Add(document);

            ActiveLayoutItem = (ILayoutItem)document;

            document.IsVisible = true;
        }

        /// <summary>
        /// Closes an instance of a document.
        /// </summary>
        public void CloseDocument(Document document)
        {
            if (document == Viewport)
                Viewport = null;

            DeactivateItem(document, true);
        }

        public void ClosePlace()
        {
            CloseDocument(Viewport);
        }

        public override void DeactivateItem(IDocument item, bool close)
        {
            ActiveDocumentChanging?.Invoke(item);

            base.DeactivateItem(item, close);

            ActiveDocumentChanged?.Invoke(item);
        }

        public void OpenSettingsView()
        {
            var w = 860;
            var h = 400;

            var settings = new SettingsViewModel();
            Editor.Current.WindowManager.ShowDialog(settings, null,
                new Dictionary<string, object>
                {
                    {"MinWidth", w},
                    {"MinHeight", h},
                    {"MaxWidth", w},
                    {"MaxHeight", h},
                    {"Title", "Settings"},
                    {"WindowStyle", WindowStyle.ToolWindow}
                });
        }

        public void OpenView(Type viewModelType)
        {
            if (viewModelType == null) return;

            IWidget tool;
            IDocument doc;

            // if the document/tool is already open, highlight it.
            if (typeof(IDocument).IsAssignableFrom(viewModelType))
            {
                if ((doc = Items.FirstOrDefault(x => x.GetType() == viewModelType)) != null)
                {
                    ActivateItem(doc);
                    return;
                }
            }
            else if (typeof(Widget).IsAssignableFrom(viewModelType))
            {
                if ((tool = Widgets.FirstOrDefault(x => x.GetType() == viewModelType)) != null)
                {
                    tool.IsVisible = true;
                    return;
                }
            }

            var item = Activator.CreateInstance(viewModelType);
            tool = item as IWidget;
            doc = item as IDocument;
            var dialog = item as IDialog;

            if (tool != null)
                ShowTool(tool);
            else if (doc != null)
                OpenDocument(doc);
            else if (dialog != null)
                ShowDialog(dialog);
            else
                throw new ArgumentOutOfRangeException();
        }

        #region Commands

        public UnionCommand UnionCommand { get; set; } = new UnionCommand();
        public NegateCommand NegateCommand { get; set; } = new NegateCommand();
        public SeparateCommand SeparateCommand { get; set; } = new SeparateCommand();

        public CutCommand CutCommand { get; set; } = new CutCommand();
        public CopyCommand CopyCommand { get; set; } = new CopyCommand();
        public PasteCommand PasteCommand { get; set; } = new PasteCommand();
        public PasteIntoCommand PasteIntoCommand { get; } = new PasteIntoCommand();
        public DuplicateCommand DuplicateCommand { get; } = new DuplicateCommand();
        public DeleteCommand DeleteCommand { get; } = new DeleteCommand();

        public SaveProjectCommand SaveProjectCommand { get; }
        public OpenProjectCommand OpenProjectCommand { get; } = new OpenProjectCommand();
        public NewProjectCommand NewProjectCommand { get; } = new NewProjectCommand();
        public NewPlaceCommand NewPlaceCommand { get; } = new NewPlaceCommand();
        public GroupCommand GroupCommand { get; } = new GroupCommand();
        public UngroupCommand UngroupCommand { get; } = new UngroupCommand();

        public PlayCommand PlayCommand { get; } = new PlayCommand();
        public RunCommand RunCommand { get; } = new RunCommand();
        public PauseCommand PauseCommand { get; } = new PauseCommand();
        public StopCommand StopCommand { get; } = new StopCommand();

        public UndoCommand UndoCommand { get; } = new UndoCommand();
        public RedoCommand RedoCommand { get; } = new RedoCommand();

        public ExecuteScriptCommand ExecuteScriptCommand { get; } = new ExecuteScriptCommand();

        public CommandBarViewModel CommandBar { get; set; } = new CommandBarViewModel();
        public StatusBarViewModel StatusBar { get; set; } = new StatusBarViewModel();

        #endregion
    }
}
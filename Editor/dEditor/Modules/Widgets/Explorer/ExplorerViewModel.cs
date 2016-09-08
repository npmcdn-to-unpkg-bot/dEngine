// ExplorerViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Collections.Concurrent;
using System.ComponentModel.Composition;
using dEditor.Framework;
using dEditor.Framework.Commands;
using dEngine;
using dEngine.Instances;
using dEngine.Utility;

// ReSharper disable VirtualMemberCallInConstructor

namespace dEditor.Modules.Widgets.Explorer
{
    [Export(typeof(IExplorer))]
    public class ExplorerViewModel : Widget, IExplorer
    {
        private ExplorerItem _rootItem;

        static ExplorerViewModel()
        {
            ExplorerItems = new ConcurrentDictionary<Instance, WeakReference<ExplorerItem>>();

            Game.InstanceAdded += GameOnInstanceAdded;

            foreach (var i in Game.Instances)
                GameOnInstanceAdded(i.Value);

            Game.InstanceRemoved +=
                i => Editor.Current.Dispatcher.InvokeAsync(() => ExplorerItems.TryRemove(i));
        }

        public ExplorerViewModel()
        {
            DisplayName = "Explorer";
        }

        public bool IsFolderSelected => LastClickedInstance is Folder;

        public CopyCommand CopyCommand { get; } = new CopyCommand();
        public PasteCommand PasteCommand { get; } = new PasteCommand();
        public PasteIntoCommand PasteIntoCommand { get; } = new PasteIntoCommand();
        public DuplicateCommand DuplicateCommand { get; } = new DuplicateCommand();
        public MakePluginCommand MakePluginCommand { get; } = new MakePluginCommand();
        public DeleteCommand DeleteCommand { get; } = new DeleteCommand();
        public GroupCommand GroupCommand { get; } = new GroupCommand();
        public UngroupCommand UngroupCommand { get; } = new UngroupCommand();
        public SelectChildrenCommand SelectChildrenCommand { get; } = new SelectChildrenCommand();
        public ZoomToCommand ZoomToCommand { get; } = new ZoomToCommand();
        public InsertPartCommand InsertPartCommand { get; } = new InsertPartCommand();

        public static ConcurrentDictionary<Instance, WeakReference<ExplorerItem>> ExplorerItems { get; }

        public override PaneLocation PreferredLocation => PaneLocation.Right;

        public Instance LastClickedInstance { get; set; }

        public ExplorerItem RootItem
        {
            get { return _rootItem; }
            set
            {
                if (Equals(value, _rootItem)) return;
                _rootItem = value;
                value.IsExpanded = true;
                NotifyOfPropertyChange();
            }
        }

        private static void GameOnInstanceAdded(Instance i)
        {
            ExplorerItem.TryCreate(i);
        }
    }
}
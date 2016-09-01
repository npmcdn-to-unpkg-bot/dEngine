// DiagnosticsViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.ComponentModel.Composition;
using System.Linq;
using dEditor.Framework;
using dEditor.Widgets.Diagnostics.SharpTreeView;
using dEngine;
using dEngine.Instances.Diagnostics;

namespace dEditor.Widgets.Diagnostics
{
    [Export(typeof(IDiagnostics))]
    public class DiagnosticsViewModel : Widget, IDiagnostics
    {
        private DiagnosticsRootNode _rootNode;

        public DiagnosticsViewModel()
        {
            DisplayName = "Diagnostics";

            Game.RegisterInitializeCallback(OnGameInitialize);
        }

        public DiagnosticsRootNode RootNode
        {
            get { return _rootNode; }
            private set
            {
                if (Equals(value, _rootNode)) return;
                _rootNode = value;
                NotifyOfPropertyChange();
            }
        }

        public override PaneLocation PreferredLocation => PaneLocation.Left;

        private void OnGameInitialize()
        {
            RootNode = new DiagnosticsRootNode();

            Game.Stats.ChildAdded.Connect(c =>
            {
                RootNode.LazyLoading = true;
                RootNode.EnsureLazyChildren();
            });

            RootNode.LazyLoading = true;
            RootNode.EnsureLazyChildren();
        }

        public class DiagnosticsRootNode : SharpTreeNode
        {
            protected override void LoadChildren()
            {
                Children.AddRange(Game.Stats.Children.OfType<StatsItem>().Select(item => new StatsItemTreeNode(item)));
            }
        }
    }
}
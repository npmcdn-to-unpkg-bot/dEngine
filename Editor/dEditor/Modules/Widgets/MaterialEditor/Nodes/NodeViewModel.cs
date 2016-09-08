// NodeViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Windows;
using Caliburn.Micro;
using dEditor.Modules.Widgets.MaterialEditor.Nodes.Views;
using dEngine.Instances.Materials;
using dEngine.Instances.Materials.Nodes;

namespace dEditor.Modules.Widgets.MaterialEditor.Nodes
{
    public class NodeViewModel : Screen
    {
        private bool _isFolded;
        private string _subName;
        private Point _position;
        private NodeContent _nodeContent;

        public NodeViewModel(Node node)
        {
            Node = node;
            Position = new Point(node.Position.X, node.Position.Y);
            node.Changed.Connect(NodeChanged);

            switch (node.GetType().Name)
            {
                case nameof(TextureParameterNode):
                    NodeContent = new TextureNodeView();
                    break;
                case nameof(FinalNode):
                    NodeContent = new MainNodeView();
                    break;
            }
        }

        public Node Node { get; }

        public override string DisplayName => Node.Name;

        public string SubName
        {
            get { return _subName; }
            set
            {
                if (value == _subName) return;
                _subName = value;
                NotifyOfPropertyChange();
                NotifyOfPropertyChange(nameof(SubNameVisible));
            }
        }

        public bool IsFolded
        {
            get { return _isFolded; }
            set
            {
                if (value == _isFolded) return;
                _isFolded = value;
                NotifyOfPropertyChange();
            }
        }

        public Point Position
        {
            get { return _position; }
            set
            {
                if (value.Equals(_position)) return;
                _position = value;
                NotifyOfPropertyChange();
            }
        }

        public NodeContent NodeContent
        {
            get { return _nodeContent; }
            set
            {
                if (Equals(value, _nodeContent)) return;
                _nodeContent = value;
                NotifyOfPropertyChange();
            }
        }

        public bool SubNameVisible => !string.IsNullOrEmpty(_subName);

        private void NodeChanged(string prop)
        {
            switch (prop)
            {
                case nameof(Node.Name):
                    NotifyOfPropertyChange(nameof(DisplayName));
                    break;
                case nameof(Node.Position):
                    Position = new Point(Node.Position.X, Node.Position.Y);
                    break;
            }
        }
    }
}
// NodePin.xaml.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using dEngine;

namespace dEditor.Modules.Widgets.MaterialEditor.Nodes
{
    /// <summary>
    /// Interaction logic for InputPin.xaml
    /// </summary>
    public partial class NodePin
    {
        private const int Offset = 10;

        public NodePin()
        {
            InitializeComponent();
        }

        public Brush PinBrush
        {
            get { return (Brush)GetValue(PinBrushProperty); }
            set { SetValue(PinBrushProperty, value); }
        }

        public InOut? Mode
        {
            get { return (InOut?)GetValue(ModeProperty); }
            set { SetValue(ModeProperty, value); }
        }

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        private static void PropertyChangedCallback(DependencyObject obj, DependencyPropertyChangedEventArgs args)
        {
            var value = (InOut)obj.GetValue(args.Property);
            var nodePin = (NodePin)obj;

            if (value == InOut.In)
            {
                nodePin.PinCircle.RenderTransform = new TranslateTransform(-Offset, 0);
                DockPanel.SetDock(nodePin.PinCircle, System.Windows.Controls.Dock.Left);
                DockPanel.SetDock(nodePin.NodeLabel, System.Windows.Controls.Dock.Right);
            }
            else if (value == InOut.Out)
            {
                nodePin.PinCircle.RenderTransform = new TranslateTransform(Offset, 0);
                DockPanel.SetDock(nodePin.PinCircle, System.Windows.Controls.Dock.Right);
                DockPanel.SetDock(nodePin.NodeLabel, System.Windows.Controls.Dock.Left);
            }
        }

        public static readonly DependencyProperty PinBrushProperty = DependencyProperty.Register("PinBrush",
            typeof(Brush), typeof(NodePin), new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty ModeProperty = DependencyProperty.Register("Mode", typeof(InOut?),
            typeof(NodePin), new PropertyMetadata(default(InOut?), PropertyChangedCallback));

        public static readonly DependencyProperty HeaderProperty = DependencyProperty.Register("Header", typeof(string),
            typeof(NodePin), new PropertyMetadata(""));
    }
}
// DiagnosticsView.xaml.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Windows.Controls;
using dEditor.Modules.Widgets.Diagnostics.SharpTreeView;

namespace dEditor.Modules.Widgets.Diagnostics
{
    /// <summary>
    /// Interaction logic for DiagnosticsView.xaml
    /// </summary>
    public partial class DiagnosticsView : UserControl
    {
        public DiagnosticsView()
        {
            InitializeComponent();
        }

        public class DiagnosticsRootNode : SharpTreeNode
        {
        }
    }
}
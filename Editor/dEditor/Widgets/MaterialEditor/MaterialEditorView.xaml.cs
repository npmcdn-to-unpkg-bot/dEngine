// MaterialEditorView.xaml.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Windows;
using System.Windows.Controls;

namespace dEditor.Widgets.MaterialEditor
{
    /// <summary>
    /// Interaction logic for MaterialEditorView.xaml
    /// </summary>
    public partial class MaterialEditorView : UserControl
    {
        public MaterialEditorView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var vm = (MaterialEditorViewModel)DataContext;

            if (MaterialPreviewPanel.IsHandleCreated)
                vm.OnHandleSet(MaterialPreviewPanel.Handle);
            else
                MaterialPreviewPanel.HandleCreated += (s, e) => vm.OnHandleSet(MaterialPreviewPanel.Handle);
        }

        private void GraphCanvas_OnInitialized(object sender, EventArgs e)
        {
            var vm = (MaterialEditorViewModel)DataContext;
            vm.Canvas = sender as Canvas;
        }
    }
}
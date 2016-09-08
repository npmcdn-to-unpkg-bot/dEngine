// CodeEditorView.xaml.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Windows;
using System.Windows.Input;

// ReSharper disable UnusedMember.Local
// ReSharper disable PossibleNullReferenceException

namespace dEditor.Modules.Widgets.CodeEditor
{
    /// <summary>
    /// Interaction logic for CodeEditorView.xaml
    /// </summary>
    public partial class CodeEditorView
    {
        public CodeEditorView()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        private void OnDataContextChanged(object sender,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((CodeEditorViewModel)DataContext).TextEditor = TextEditor;
        }

        private void CanZoom(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OnZoomIn(object sender, ExecutedRoutedEventArgs e)
        {
            (DataContext as CodeEditorViewModel).ZoomScale += 0.1f;
        }

        private void OnZoomOut(object sender, ExecutedRoutedEventArgs e)
        {
            (DataContext as CodeEditorViewModel).ZoomScale -= 0.1f;
        }

        private void BreakpointContainer_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //var position = CodeEditor.GetPositionFromPoint(e.GetPosition(CodeEditor));

            //Debug.WriteLine(position?.Line ?? -1);
        }
    }
}
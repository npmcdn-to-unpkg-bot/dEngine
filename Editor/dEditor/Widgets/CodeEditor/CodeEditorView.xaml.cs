// CodeEditorView.xaml.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using dEditor.Shell.StatusBar;

// ReSharper disable UnusedMember.Local
// ReSharper disable PossibleNullReferenceException

namespace dEditor.Widgets.CodeEditor
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
            TextEditor.TextArea.Caret.PositionChanged += CaretOnPositionChanged;
        }

        private void CaretOnPositionChanged(object sender, EventArgs args)
        {
            var caretPos = TextEditor.TextArea.Caret;
            var statusBar = IoC.Get<IStatusBar>();
            statusBar.SetLineChar(caretPos.Line, caretPos.Column);
        }

        private void OnDataContextChanged(object sender,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((CodeEditorViewModel)DataContext).TextEditor = TextEditor;
        }

        private void CanZoom(CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        public CodeEditorViewModel ViewModel => (CodeEditorViewModel)DataContext;

        private void OnZoomIn(ExecutedRoutedEventArgs e)
        {
            ViewModel.ZoomScale += 0.1f;
        }

        private void OnZoomOut(object sender, ExecutedRoutedEventArgs e)
        {
            ViewModel.ZoomScale -= 0.1f;
        }

        private void BreakpointContainer_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //var position = CodeEditor.GetPositionFromPoint(e.GetPosition(CodeEditor));

            //Debug.WriteLine(position?.Line ?? -1);
        }
    }
}
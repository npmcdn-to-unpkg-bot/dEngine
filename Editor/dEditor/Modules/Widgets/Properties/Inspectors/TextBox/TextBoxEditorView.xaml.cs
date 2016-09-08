// TextBoxEditorView.xaml.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace dEditor.Modules.Widgets.Properties.Inspectors.TextBox
{
    /// <summary>
    /// Interaction logic for TextBoxEditorView.xaml
    /// </summary>
    public partial class TextBoxEditorView : UserControl
    {
        public TextBoxEditorView()
        {
            InitializeComponent();
        }

        private void UIElement_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            ((dynamic)DataContext).ApplyValueWithHistory();
        }

        private void UIElement_OnLostFocus(object sender, RoutedEventArgs e)
        {
            ((dynamic)DataContext).ApplyValueWithHistory();
        }
    }
}
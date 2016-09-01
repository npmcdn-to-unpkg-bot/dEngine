// SplitButton.xaml.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Xceed.Wpf.Toolkit;

namespace dEditor.Framework.Controls.SplitButton
{
    /// <summary>
    /// Interaction logic for SplitButton.xaml
    /// </summary>
    [ContentProperty("CustomContent")]
    public partial class SplitButton : UserControl
    {
        public SplitButton()
        {
            InitializeComponent();
            DataContext = this;
        }

        public object CustomContent
        {
            get { return GetValue(CustomContentProperty); }
            set { SetValue(CustomContentProperty, value); }
        }

        public object DropDownContent
        {
            get { return GetValue(DropDownContentProperty); }
            set { SetValue(DropDownContentProperty, value); }
        }

        public static readonly DependencyProperty CustomContentProperty = DependencyProperty.Register("CustomContent",
            typeof(object), typeof(DropDownButton), null);

        public static readonly DependencyProperty DropDownContentProperty =
            DependencyProperty.Register("DropDownContent",
                typeof(object), typeof(DropDownButton), null);
    }
}
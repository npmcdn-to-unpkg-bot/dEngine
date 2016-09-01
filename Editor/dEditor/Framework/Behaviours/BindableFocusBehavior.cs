// BindableFocusBehavior.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace dEditor.Framework.Behaviours
{
    public class BindableFocusBehavior : Behavior<Control>
    {
        public bool HasFocus
        {
            get { return (bool)GetValue(HasFocusProperty); }
            set
            {
                SetValue(HasFocusProperty, value);
                AssociatedObject.Focus();
            }
        }

        private static void HasFocusUpdated(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BindableFocusBehavior)d).SetFocus();
        }

        private void SetFocus()
        {
            if (HasFocus)
                AssociatedObject.Focus();
        }

        public static readonly DependencyProperty HasFocusProperty =
            DependencyProperty.Register("HasFocus", typeof(bool), typeof(BindableFocusBehavior),
                new PropertyMetadata(default(bool), HasFocusUpdated));
    }
}
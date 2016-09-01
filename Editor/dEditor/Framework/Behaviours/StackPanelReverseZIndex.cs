// StackPanelReverseZIndex.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace dEditor.Framework.Behaviours
{
    public class StackPanelReverseZIndex : Behavior<StackPanel>
    {
        /// <summary>
        /// Handles the onAttached event
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.LayoutUpdated += AssociatedObject_LayoutUpdated;
        }

        protected void AssociatedObject_LayoutUpdated(object sender, EventArgs e)
        {
            var childCount = AssociatedObject.Children.Count;
            foreach (FrameworkElement element in AssociatedObject.Children)
            {
                element.SetValue(Panel.ZIndexProperty, childCount);
                childCount--;
            }
        }

        /// <summary>
        /// Handles the OnDetaching event
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.LayoutUpdated -= AssociatedObject_LayoutUpdated;
        }
    }
}
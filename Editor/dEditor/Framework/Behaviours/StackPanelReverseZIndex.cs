﻿// StackPanelReverseZIndex.cs - dEditor
// Copyright (C) 2016-2016 DanDev (dandev.sco@gmail.com)
// 
// This library is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// You should have received a copy of the GNU General Public
// along with this program. If not, see <http://www.gnu.org/licenses/>.

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
// RowExpander.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows;
using System.Windows.Controls;

namespace dEditor.Framework.Controls.TreeListView
{
    public class RowExpander : Control
    {
        static RowExpander()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RowExpander),
                new FrameworkPropertyMetadata(typeof(RowExpander)));
        }
    }
}
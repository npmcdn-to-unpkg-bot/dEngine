// InsertMarker.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows;
using System.Windows.Controls;

namespace dEditor.Widgets.Diagnostics.SharpTreeView
{
    public class InsertMarker : Control
    {
        static InsertMarker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InsertMarker),
                new FrameworkPropertyMetadata(typeof(InsertMarker)));
        }
    }
}
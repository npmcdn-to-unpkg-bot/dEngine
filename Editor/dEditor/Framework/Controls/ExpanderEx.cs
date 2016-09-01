// ExpanderEx.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows;
using System.Windows.Controls;

namespace dEditor.Framework.Controls
{
    public class ExpanderEx : Expander
    {
        static ExpanderEx()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExpanderEx),
                new FrameworkPropertyMetadata(typeof(ExpanderEx)));
        }
    }
}
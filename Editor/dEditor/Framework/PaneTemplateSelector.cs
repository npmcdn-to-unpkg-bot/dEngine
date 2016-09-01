// PaneTemplateSelector.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows;
using System.Windows.Controls;

namespace dEditor.Framework
{
    public class PaneTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Template { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is LayoutItem)
                return Template;
            return base.SelectTemplate(item, container);
        }
    }
}
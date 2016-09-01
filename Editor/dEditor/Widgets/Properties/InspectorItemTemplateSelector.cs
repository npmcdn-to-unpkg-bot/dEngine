// InspectorItemTemplateSelector.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows;
using System.Windows.Controls;

namespace dEditor.Widgets.Properties
{
    public class InspectorItemTemplateSelector : DataTemplateSelector
    {
        public DataTemplate LabelledTemplate { get; set; }
        public DataTemplate DefaultTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item is ILabelled)
                return LabelledTemplate;
            return DefaultTemplate;
        }
    }
}
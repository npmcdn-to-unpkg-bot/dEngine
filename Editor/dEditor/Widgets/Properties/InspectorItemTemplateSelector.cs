﻿// InspectorItemTemplateSelector.cs - dEditor
// Copyright (C) 2016-2016 DanDev (dandev.sco@gmail.com)
// 
// This library is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// You should have received a copy of the GNU General Public
// along with this program. If not, see <http://www.gnu.org/licenses/>.

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
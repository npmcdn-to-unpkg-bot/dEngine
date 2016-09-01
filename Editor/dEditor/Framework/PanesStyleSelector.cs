// PanesStyleSelector.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows;
using System.Windows.Controls;

namespace dEditor.Framework
{
    public class PanesStyleSelector : StyleSelector
    {
        public Style ToolStyle { get; set; }

        public Style DocumentStyle { get; set; }

        public override Style SelectStyle(object item, DependencyObject container)
        {
            if (item is Widget)
                return ToolStyle;

            if (item is Document)
                return DocumentStyle;

            return base.SelectStyle(item, container);
        }
    }
}
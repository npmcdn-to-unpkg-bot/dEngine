// InOutToAlignmentConverter.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using dEngine;

namespace dEditor.Modules.Widgets.MaterialEditor
{
    public class InOutToAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var mode = (InOut?)value;
            switch (mode)
            {
                case InOut.In:
                    return HorizontalAlignment.Left;
                case InOut.Out:
                    return HorizontalAlignment.Right;
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var alignment = (HorizontalAlignment)value;
            switch (alignment)
            {
                case HorizontalAlignment.Left:
                    return InOut.In;
                case HorizontalAlignment.Right:
                    return InOut.Out;
            }
            return null;
        }
    }
}
﻿// InOutToAlignmentConverter.cs - dEditor
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
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using dEngine;

namespace dEditor.Widgets.MaterialEditor
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
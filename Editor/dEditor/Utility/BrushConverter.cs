// BrushConverter.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using dEngine;

namespace dEditor.Utility
{
    public class BrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            try
            {
                var c = (Colour)value;
                var color = Color.FromArgb((byte)(c.a*255), (byte)(c.r*255), (byte)(c.g*255), (byte)(c.b*255));
                var brush = new SolidColorBrush(color);
                return brush;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            try
            {
                var brush = (SolidColorBrush)value;
                var color = brush.Color;
                return Colour.fromRGBA(color.R, color.G, color.B, color.A);
            }
            catch
            {
                return null;
            }
        }
    }
}
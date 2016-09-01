// IgnoreLinebreaksConverter.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace dEditor.Framework.Converters
{
    public class IgnoreLinebreaksConverter : IValueConverter
    {
        private const string Sub = " \u200B";
        private const string Lb = "\r\n";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = (string)value;
            return string.IsNullOrEmpty(s) ? s : Regex.Replace(s, Lb, Sub);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = (string)value;
            return string.IsNullOrEmpty(s) ? s : Regex.Replace(s, Sub, Lb);
        }
    }
}
// NumberTextConverter.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Globalization;
using System.Windows.Data;

namespace dEditor.Framework.Converters
{
    public class NumberTextConverter : IValueConverter
    {
        public int DecimalCount { get; set; } = 32;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((double)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var s = value as string;
            double result;
            var didParse = double.TryParse(s, out result);
            return string.IsNullOrEmpty(s) || !didParse ? 0 : result;
        }
    }
}
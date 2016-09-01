// ColourEditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using dEngine.Serializer.V1;

namespace dEditor.Widgets.Properties.Inspectors.Colour
{
    public class ColourEditorViewModel : EditorBase<dEngine.Colour>, ILabelled
    {
        private double _alpha;
        private SolidColorBrush _brush;
        private double _hue;
        private double _luminance;
        private double _saturation;

        public ColourEditorViewModel(object obj, Inst.CachedProperty propDesc) : base(obj, propDesc)
        {
            NotifyOfPropertyChange(nameof(StringValue));
        }

        public string StringValue
        {
            get
            {
                if (AreMultipleValuesSame)
                    return
                        $"{Math.Round(255*Value.r)}, {Math.Round(255*Value.g)}, {Math.Round(255*Value.b)}, {Math.Round(Value.a, 3)}";
                return null;
            }
            set { Value = dEngine.Colour.parseRGBA(value); }
        }
    }

    public class ColourConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter,
            CultureInfo culture)
        {
            try
            {
                var c = (dEngine.Colour)value;
                var color = Color.FromArgb((byte)(c.a*255), (byte)(c.r*255), (byte)(c.g*255), (byte)(c.b*255));
                return color;
            }
            catch
            {
                return null;
            }
        }

        public object ConvertBack(object value, System.Type targetType, object parameter,
            CultureInfo culture)
        {
            try
            {
                var color = (Color)value;
                return dEngine.Colour.fromRGBA(color.R, color.G, color.B, color.A);
            }
            catch
            {
                return null;
            }
        }
    }
}
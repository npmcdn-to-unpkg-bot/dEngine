// ColourPicker.xaml.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using dEditor.Modules.Widgets.Properties.Inspectors.Colour;
using dEditor.Properties;
using dEngine;

namespace dEditor.Framework.Controls.ColourPicker
{
    /// <summary>
    /// Interaction logic for ColourPicker.xaml
    /// </summary>
    public partial class ColourPicker : INotifyPropertyChanged
    {
        private double _left;

        private bool _mouseDown;
        private double _top;
        private bool _updating;

        public ColourPicker()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public ColourEditorViewModel Editor { get; private set; }

        public Colour ColourOnlyHue => new Colour(Value.hue*255, 1, 1);

        public string Hex
        {
            get { return Value.ToHexString(); }
            set { Value = Colour.fromHex(value); }
        }

        public double Top
        {
            get { return _top; }
            set
            {
                _top = value;
                NotifyPropertyChanged();
            }
        }

        public double Left
        {
            get { return _left; }
            set
            {
                if (value.Equals(_left)) return;
                _left = value;
                NotifyPropertyChanged();
            }
        }

        public double Hue
        {
            get { return Value.hue; }
            set
            {
                Tuple<float, float, float> hsl = Value.toHSV();
                Value = Colour.fromHSV((float)(value/360), hsl.Item2, hsl.Item3);
            }
        }

        public double Red
        {
            get { return Value.r*255; }
            set
            {
                if (value == Red) return;
                Editor.Value = new Colour((float)value/255, Value.g, Value.b, Value.a);
                NotifyPropertyChanged();
            }
        }

        public double Green
        {
            get { return Math.Round(Value.g*255); }
            set
            {
                if (value == Green) return;
                Editor.Value = new Colour(Value.r, (float)value/255, Value.b, Value.a);
                NotifyPropertyChanged();
            }
        }

        public double Blue
        {
            get { return Math.Round(Value.b*255); }
            set
            {
                if (value == Blue) return;
                Editor.Value = new Colour(Value.r, Value.g, (float)value/255, Value.a);
                NotifyPropertyChanged();
            }
        }

        public double Alpha
        {
            get { return Math.Round(Value.a*255); }
            set
            {
                Editor.Value = new Colour(Value.r, Value.g, Value.b, (float)value/255);
                NotifyPropertyChanged();
            }
        }

        public Colour Value
        {
            get { return Editor?.Value ?? Colour.Zero; }
            set { Editor.Value = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            Editor = (ColourEditorViewModel)DataContext;
            Editor.PropertyChanged += EditorOnPropertyChanged;
            EditorOnPropertyChanged(this, new PropertyChangedEventArgs(nameof(Value)));
        }

        private void EditorOnPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName != nameof(ColourEditorViewModel.Value))
                return;

            if (!_updating)
                SetPositionToColour(Editor.Value);

            NotifyPropertyChanged(nameof(Hex));
            NotifyPropertyChanged(nameof(ColourOnlyHue));

            _updating = false;
        }

        private void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDown)
                SetColourToPosition(e.GetPosition(GradientCanvas));
        }

        private void SetColourToPosition(Point point)
        {
            _updating = true;
            var s = (float)(point.X/GradientCanvas.ActualWidth);
            var v = (float)(1 - point.Y/GradientCanvas.ActualHeight);
            Editor.Value = Colour.fromHSVA(Value.hue, s, v, Value.a);
            Left = point.X - 5;
            Top = point.Y - 5;
        }

        private void SetPositionToColour(Colour colour)
        {
            Tuple<float, float, float> hsv = colour.toHSV();
            var s = hsv.Item2;
            var v = hsv.Item3;
            Left = s*GradientCanvas.ActualWidth;
            Top = Math.Abs(v - 1)*GradientCanvas.ActualHeight;
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            _mouseDown = true;
            SetColourToPosition(e.GetPosition(GradientCanvas));
        }

        private void GradientCanvas_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            _mouseDown = false;
        }
        
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
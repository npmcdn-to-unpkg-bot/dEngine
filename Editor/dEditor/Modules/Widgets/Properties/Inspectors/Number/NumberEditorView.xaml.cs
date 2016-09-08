// NumberEditorView.xaml.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using dEditor.Framework.Converters;

namespace dEditor.Modules.Widgets.Properties.Inspectors.Number
{
    /// <summary>
    /// Interaction logic for NumberEditorView.xaml
    /// </summary>
    public partial class NumberEditorView
    {
        public static DependencyProperty CurrentConverterProperty = DependencyProperty.Register(
            nameof(CurrentConverter), typeof(NumberTextConverter), typeof(NumberEditorView), null);

        public NumberEditorView()
        {
            DataContextChanged += OnDataContextChanged;
            InitializeComponent();
            var textBinding = new Binding("DoubleValue") {Converter = CurrentConverter = new NumberTextConverter()};
            NumberTextBox.SetBinding(System.Windows.Controls.TextBox.TextProperty, textBinding);
        }

        public NumberTextConverter CurrentConverter
        {
            get { return (NumberTextConverter)GetValue(CurrentConverterProperty); }
            set { SetValue(CurrentConverterProperty, value); }
        }

        private void OnDataContextChanged(object sender,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            UIElement_OnLostFocus(null, null);
        }

        private void UIElement_OnGotFocus(object sender, RoutedEventArgs e)
        {
            dynamic vm = DataContext;
            CurrentConverter.DecimalCount = vm.IsRanged ? 4 : 18;
            vm.NotifyOfPropertyChange("DoubleValue");
        }

        private void UIElement_OnLostFocus(object sender, RoutedEventArgs e)
        {
            CurrentConverter.DecimalCount = 4;
            dynamic vm = DataContext;
            vm.NotifyOfPropertyChange("DoubleValue");
            if (sender != null)
                ((dynamic)DataContext).ApplyValueWithHistory();
        }

        private void Thumb_OnDragCompleted(object sender, DragCompletedEventArgs e)
        {
            ((dynamic)DataContext).ApplyValueWithHistory();
        }

        private void NumberTextBox_OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter)
                return;

            ((dynamic)DataContext).ApplyValueWithHistory();
        }
    }
}
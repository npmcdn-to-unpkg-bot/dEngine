﻿// Button.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using dEditor.Modules.Widgets.Viewport;
using dEngine;
using dEngine.Data;
using dEngine.Instances.Attributes;
using dEngine.Services;
using WpfToggleButton = System.Windows.Controls.Primitives.ToggleButton;

namespace dEditor.Framework.Plugins
{
    public class Button : PluginItemBase
    {
        private WpfToggleButton _button;
        private Toolbar _toolbar;
        private bool _isChecked;

        public Button(string text, string tooltip, string iconId, Toolbar toolbar)
        {
            Clicked = new Signal(this);

            Editor.Current.Dispatcher.InvokeAsync(() =>
            {
                _button = new WpfToggleButton
                {
                    Content = text,
                    ToolTip = tooltip,
                    DataContext = this,
                    Style = Application.Current.Resources["ToggleButtonStyle"] as Style
                };
                _button.Checked += ButtonOnChecked;
                _button.Unchecked += ButtonOnChecked;
                _button.Click += (s, e) => Clicked.Fire();

                var binding = new Binding {Path = new PropertyPath("IsChecked"), Mode = BindingMode.OneWay};
                BindingOperations.SetBinding(_button, WpfToggleButton.IsCheckedProperty, binding);

                UpdateButtonEnabled(Editor.Current.Shell.ActiveDocument);
                Editor.Current.Shell.ActiveDocumentChanged += UpdateButtonEnabled;

                var _imageContent = new Content<Texture>(iconId);
                _imageContent.Subscribe(this, (owner, texture) =>
                {
                    Editor.Current.Dispatcher.InvokeAsync(() =>
                    {
                        _button.Content = new Image
                        {
                            Source = BitmapSource.Create(texture.Width, texture.Height, 96, 96, PixelFormats.Pbgra32,
                                BitmapPalettes.WebPalette,
                                texture.GetBytesPBGRA(), texture.Width*4)
                        };
                    });
                });

                if (toolbar.IsLoaded)
                    toolbar.WpfToolBar.Items.Add(_button);
                else
                    toolbar.Loaded += tb => tb.Items.Add(_button);

                _toolbar = toolbar;
            });
        }

        public bool IsChecked
        {
            get { return _isChecked; }
            private set
            {
                _isChecked = value;
                NotifyChanged();
                NotifyOfPropertyChange();
            }
        }

        private void ButtonOnChecked(object sender, RoutedEventArgs routedEventArgs)
        {
            if (_button.IsChecked != _isChecked)
                NotifyOfPropertyChange(nameof(IsChecked));
        }

        private void UpdateButtonEnabled(IDocument doc)
        {
            if ((doc?.IsActive == true) && doc is ViewportViewModel)
                _button.IsEnabled = true;
            else
                _button.IsEnabled = false;
        }

        [ScriptSecurity(ScriptIdentity.Plugin)]
        public void SetActive(bool active)
        {
            ScriptService.AssertIdentity(ScriptIdentity.Plugin);
            Editor.Current.Dispatcher.InvokeAsync(() => { IsChecked = active; });
        }

        public override void Destroy()
        {
            ParentLocked = false;
            base.Destroy();
            Editor.Current.Dispatcher.InvokeAsync(() => { _toolbar?.WpfToolBar.Items.Remove(_button); });
        }

        public readonly Signal Clicked;
    }
}
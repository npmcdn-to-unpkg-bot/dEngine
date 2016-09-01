// TreeViewDragAdornerViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Screen = Caliburn.Micro.Screen;

namespace dEditor.Framework.Controls.CustomTreeView
{
    public class TreeViewDragAdornerViewModel : Screen
    {
        private ImageSource _image;
        private bool _isDragging;
        private Window _window;

        public bool IsDragging
        {
            get { return _isDragging; }
            set
            {
                if (value == _isDragging) return;
                _isDragging = value;

                _window.Visibility = value ? Visibility.Visible : Visibility.Collapsed;

                NotifyOfPropertyChange();
            }
        }

        public ImageSource Image
        {
            get { return _image; }
            set
            {
                if (Equals(value, _image)) return;
                _image = value;
                NotifyOfPropertyChange();
            }
        }

        public Point Offset { get; set; }
        public DependencyObject Source { get; set; }

        protected override void OnViewAttached(object view, object context)
        {
            base.OnViewAttached(view, context);
            _window = (Window)GetView();
        }

        public void UpdatePosition(object sender, EventArgs e)
        {
            var mousePos = Control.MousePosition;
            _window.Top = mousePos.Y - Offset.Y;
            _window.Left = mousePos.X - Offset.X;

            /*
            _window.RaiseEvent(new MouseEventArgs(Mouse.PrimaryDevice, 0)
            {
                RoutedEvent = Mouse.MouseMoveEvent,
                Source = Source,
            });
            */
        }
    }
}
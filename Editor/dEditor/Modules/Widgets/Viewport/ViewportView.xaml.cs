// ViewportView.xaml.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Reflection;
using System.Windows.Forms;
using dEngine;
using DataObject = System.Windows.DataObject;
using DragDropEffects = System.Windows.DragDropEffects;
using UserControl = System.Windows.Controls.UserControl;

namespace dEditor.Modules.Widgets.Viewport
{
    /// <summary>
    /// Interaction logic for SceneView.xaml
    /// </summary>
    public partial class ViewportView : UserControl
    {
        public ViewportView()
        {
            InitializeComponent();

            RenderPane.MouseDown += RenderPaneOnMouseDown;
            RenderPane.MouseUp += RenderPaneOnMouseUp;

            RenderPane.GotFocus += (s, e) => { Engine.IsViewportFocused = true; };

            RenderPane.LostFocus += (s, e) => { Engine.IsViewportFocused = false; };
        }

        public ViewportViewModel SceneViewModel => DataContext as ViewportViewModel;

        private void RenderPaneOnMouseDown(object sender, MouseEventArgs args)
        {
            if (args.Button == MouseButtons.Right)
            {
                /*
                var point = Point.Empty;
                User32.GetCursorPos(ref point);
                var rect = new Rectangle(point.X, point.Y, 1, 1);
                User32.ClipCursor(ref rect);
                */
                //RenderPane.Cursor = new Cursor(System.Windows.Forms.Cursor.Current.Handle);
                //System.Windows.Forms.Cursor.Position = System.Windows.Forms.Cursor.Position;
                //System.Windows.Forms.Cursor.Clip = new Rectangle(System.Windows.Forms.Cursor.Position, new Size(0, 0));
            }
        }

        private void RenderPaneOnMouseUp(object sender, MouseEventArgs args)
        {
            if (args.Button == MouseButtons.Right)
            {
                //User32.ClipCursor(IntPtr.Zero);
            }
        }

        private void RenderPane_HandleCreated(object sender, EventArgs e)
        {
            Engine.SetHandle(RenderPane.Handle);
        }

        private DataObject GetDataObject(DragEventArgs e)
        {
            var info = e.Data.GetType().GetField("innerData", BindingFlags.NonPublic | BindingFlags.Instance);
            var obj = info?.GetValue(e.Data);
            info = obj?.GetType().GetField("innerData", BindingFlags.NonPublic | BindingFlags.Instance);
            return info?.GetValue(obj) as DataObject;
        }

        private void RenderPane_OnDragOver(object sender, DragEventArgs e)
        {
            var dataObj = GetDataObject(e);
            var viewModel = (ViewportViewModel)DataContext;
            var effect = (DragDropEffects)e.Effect;
            viewModel.OnPreviewDragOver(sender, dataObj, ref effect);
            e.Effect = (System.Windows.Forms.DragDropEffects)effect;
        }

        private void RenderPane_OnDragDrop(object sender, DragEventArgs e)
        {
            var dataObj = GetDataObject(e);
            var viewModel = (ViewportViewModel)DataContext;
            viewModel.OnPreviewDragDrop(sender, dataObj);
        }

        private void UIElement_OnPreviewDragOver(object sender, System.Windows.DragEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
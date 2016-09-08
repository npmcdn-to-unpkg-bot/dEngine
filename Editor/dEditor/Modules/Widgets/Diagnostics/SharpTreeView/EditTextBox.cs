// EditTextBox.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace dEditor.Modules.Widgets.Diagnostics.SharpTreeView
{
    public class EditTextBox : TextBox
    {
        private bool commiting;

        static EditTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EditTextBox),
                new FrameworkPropertyMetadata(typeof(EditTextBox)));
        }

        public EditTextBox()
        {
            Loaded += delegate { Init(); };
        }

        public SharpTreeViewItem Item { get; set; }

        public SharpTreeNode Node
        {
            get { return Item.Node; }
        }

        private void Init()
        {
            Text = Node.LoadEditText();
            Focus();
            SelectAll();
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Commit();
            else if (e.Key == Key.Escape)
                Node.IsEditing = false;
        }

        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            if (Node.IsEditing)
                Commit();
        }

        private void Commit()
        {
            if (!commiting)
            {
                commiting = true;

                Node.IsEditing = false;
                if (!Node.SaveEditText(Text))
                    Item.Focus();
                Node.RaisePropertyChanged("Text");

                //if (Node.SaveEditText(Text)) {
                //    Node.IsEditing = false;
                //    Node.RaisePropertyChanged("Text");
                //}
                //else {
                //    Init();
                //}

                commiting = false;
            }
        }
    }
}
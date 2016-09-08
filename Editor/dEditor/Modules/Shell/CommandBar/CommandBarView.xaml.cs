// CommandBarView.xaml.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Caliburn.Micro;

namespace dEditor.Modules.Shell.CommandBar
{
    /// <summary>
    /// Interaction logic for CommandBarView.xaml
    /// </summary>
    public partial class CommandBarView
    {
        private string _replacementChar;

        private bool _return;

        public CommandBarView()
        {
            InitializeComponent();
            DataObject.AddPastingHandler(ComboBox, OnPaste);
        }

        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            var isText = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (!isText) return;
            _replacementChar = " ";
        }

        private void ComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter) && !string.IsNullOrWhiteSpace(ComboBox.Text))
            {
                _replacementChar = "";
                IoC.Get<ICommandBar>().Execute(ComboBox.Text);
                _return = true;
            }
        }

        private void ComboBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var text = ComboBox.Text.Replace(Environment.NewLine, _replacementChar);
            ComboBox.Text = text;

            if (_return)
            {
                ComboBox.SetCaret(text.Length);
                _return = false;
            }
        }
    }
}
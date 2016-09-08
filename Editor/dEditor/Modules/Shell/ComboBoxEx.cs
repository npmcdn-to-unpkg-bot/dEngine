// ComboBoxEx.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Windows;
using System.Windows.Controls;

namespace dEditor.Modules.Shell
{
    public class ComboBoxEx : ComboBox
    {
        private TextBox _textBox;

        public void SetCaret(int position)
        {
            _textBox.SelectionStart = position;
            _textBox.SelectionLength = 0;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var textBox = GetTemplateChild("PART_EditableTextBox") as TextBox;
            if (textBox != null)
                _textBox = textBox;

            _textBox.Padding = new Thickness(2);
        }
    }
}
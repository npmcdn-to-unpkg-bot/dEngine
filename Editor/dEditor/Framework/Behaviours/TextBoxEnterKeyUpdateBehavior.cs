﻿// TextBoxEnterKeyUpdateBehavior.cs - dEditor
// Copyright (C) 2016-2016 DanDev (dandev.sco@gmail.com)
// 
// This library is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// You should have received a copy of the GNU General Public
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace dEditor.Framework.Behaviours
{
    public class TextBoxEnterKeyUpdateBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            if (AssociatedObject != null)
            {
                base.OnAttached();
                AssociatedObject.KeyDown += AssociatedObject_KeyDown;
            }
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject != null)
            {
                AssociatedObject.KeyDown -= AssociatedObject_KeyDown;
                base.OnDetaching();
            }
        }

        private void AssociatedObject_KeyDown(object sender, KeyEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
                if (e.Key == Key.Return)
                    if (e.Key == Key.Enter)
                        Keyboard.ClearFocus();
        }
    }
}
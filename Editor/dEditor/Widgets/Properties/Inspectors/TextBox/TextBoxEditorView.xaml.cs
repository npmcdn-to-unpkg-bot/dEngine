﻿// TextBoxEditorView.xaml.cs - dEditor
// Copyright (C) 2016-2016 DanDev (dandev.sco@gmail.com)
// 
// This library is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// You should have received a copy of the GNU General Public
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace dEditor.Widgets.Properties.Inspectors.TextBox
{
	/// <summary>
	/// Interaction logic for TextBoxEditorView.xaml
	/// </summary>
	public partial class TextBoxEditorView : UserControl
	{
		public TextBoxEditorView()
		{
			InitializeComponent();
		}

	    private void UIElement_OnKeyUp(object sender, KeyEventArgs e)
	    {
	        if (e.Key != Key.Enter) return;
            ((dynamic)DataContext).ApplyValueWithHistory();
        }

	    private void UIElement_OnLostFocus(object sender, RoutedEventArgs e)
        {
            ((dynamic)DataContext).ApplyValueWithHistory();
        }
	}
}
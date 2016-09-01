// SharpGridView.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows;
using System.Windows.Controls;

namespace dEditor.Widgets.Diagnostics.SharpTreeView
{
    public class SharpGridView : GridView
    {
        static SharpGridView()
        {
            ItemContainerStyleKey =
                new ComponentResourceKey(typeof(SharpTreeView), "GridViewItemContainerStyleKey");
        }

        public static ResourceKey ItemContainerStyleKey { get; }

        protected override object ItemContainerDefaultStyleKey
        {
            get { return ItemContainerStyleKey; }
        }
    }
}
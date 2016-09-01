// AdvancedObjectsView.xaml.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace dEditor.Widgets.AdvancedObjects
{
    /// <summary>
    /// Interaction logic for AdvancedObjectView.xaml
    /// </summary>
    public partial class AdvancedObjectsView
    {
        public AdvancedObjectsView()
        {
            InitializeComponent();
        }

        private void ListBox_OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(ListBox, (DependencyObject)e.OriginalSource) as ListBoxItem;
            if (item != null)
                ((AdvancedObjectsViewModel)DataContext).OnObjectMouseDown((ObjectEntry)item.DataContext, e);
        }

        private void ListBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            var selectedIndex = ListBox.SelectedIndex;
            var count = ListBox.Items.Count;
            for (var i = selectedIndex+1; i < count; i++)
            {
                var item = (ObjectEntry)ListBox.Items[i];
                var chr = new KeyConverter().ConvertToString(e.Key).ToUpper()[0];
                if (item.Name[0] == chr)
                {
                    ListBox.SelectedIndex = i;
                    ListBox.ScrollIntoView(item);
                    break;
                }
            }
        }
    }
}
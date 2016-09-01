// OutputView.xaml.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows.Controls;

namespace dEditor.Widgets.Output
{
    /// <summary>
    /// Interaction logic for OutputView.xaml
    /// </summary>
    public partial class OutputView
    {
        private bool _autoScroll;

        public OutputView()
        {
            InitializeComponent();
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = (ScrollViewer)e.Source;

            if (e.ExtentHeightChange == 0)
                if (scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight)
                    _autoScroll = true;
                else
                    _autoScroll = false;

            if (_autoScroll && (e.ExtentHeightChange != 0))
                scrollViewer.ScrollToVerticalOffset((e.Source as ScrollViewer).ExtentHeight);
        }
    }
}
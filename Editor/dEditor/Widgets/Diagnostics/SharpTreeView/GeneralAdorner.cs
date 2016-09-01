// GeneralAdorner.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace dEditor.Widgets.Diagnostics.SharpTreeView
{
    public class GeneralAdorner : Adorner
    {
        private FrameworkElement child;

        public GeneralAdorner(UIElement target)
            : base(target)
        {
        }

        public FrameworkElement Child
        {
            get { return child; }
            set
            {
                if (child != value)
                {
                    RemoveVisualChild(child);
                    RemoveLogicalChild(child);
                    child = value;
                    AddLogicalChild(value);
                    AddVisualChild(value);
                    InvalidateMeasure();
                }
            }
        }

        protected override int VisualChildrenCount
        {
            get { return child == null ? 0 : 1; }
        }

        protected override Visual GetVisualChild(int index)
        {
            return child;
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (child != null)
            {
                child.Measure(constraint);
                return child.DesiredSize;
            }
            return new Size();
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            if (child != null)
            {
                child.Arrange(new Rect(finalSize));
                return finalSize;
            }
            return new Size();
        }
    }
}
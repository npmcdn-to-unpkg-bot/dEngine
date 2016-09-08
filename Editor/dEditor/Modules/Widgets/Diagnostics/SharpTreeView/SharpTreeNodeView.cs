﻿// SharpTreeNodeView.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace dEditor.Modules.Widgets.Diagnostics.SharpTreeView
{
    public class SharpTreeNodeView : Control
    {
        static SharpTreeNodeView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(SharpTreeNodeView),
                new FrameworkPropertyMetadata(typeof(SharpTreeNodeView)));
        }

        public Brush TextBackground
        {
            get { return (Brush)GetValue(TextBackgroundProperty); }
            set { SetValue(TextBackgroundProperty, value); }
        }

        public SharpTreeNode Node
        {
            get { return DataContext as SharpTreeNode; }
        }

        public SharpTreeViewItem ParentItem { get; private set; }

        public Control CellEditor
        {
            get { return (Control)GetValue(CellEditorProperty); }
            set { SetValue(CellEditorProperty, value); }
        }

        public SharpTreeView ParentTreeView
        {
            get { return ParentItem.ParentTreeView; }
        }

        internal LinesRenderer LinesRenderer { get; private set; }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            LinesRenderer = Template.FindName("linesRenderer", this) as LinesRenderer;
            UpdateTemplate();
        }

        protected override void OnVisualParentChanged(DependencyObject oldParent)
        {
            base.OnVisualParentChanged(oldParent);
            ParentItem = FindAncestor<SharpTreeViewItem>();
            ParentItem.NodeView = this;
        }

        public T FindAncestor<T>() where T : class
        {
            return AncestorsAndSelf().OfType<T>().FirstOrDefault();
        }

        public IEnumerable<DependencyObject> AncestorsAndSelf()
        {
            DependencyObject d = this;
            while (d != null)
            {
                yield return d;
                d = VisualTreeHelper.GetParent(d);
            }
        }

        public static void AddOnce(IList list, object item)
        {
            if (!list.Contains(item))
                list.Add(item);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == DataContextProperty)
                UpdateDataContext(e.OldValue as SharpTreeNode, e.NewValue as SharpTreeNode);
        }

        private void UpdateDataContext(SharpTreeNode oldNode, SharpTreeNode newNode)
        {
            if (newNode != null)
            {
                newNode.PropertyChanged += Node_PropertyChanged;
                if (Template != null)
                    UpdateTemplate();
            }
            if (oldNode != null)
                oldNode.PropertyChanged -= Node_PropertyChanged;
        }

        private void Node_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsEditing")
            {
                OnIsEditingChanged();
            }
            else if (e.PropertyName == "IsLast")
            {
                if (ParentTreeView.ShowLines)
                    foreach (var child in Node.VisibleDescendantsAndSelf())
                    {
                        var container =
                            ParentTreeView.ItemContainerGenerator.ContainerFromItem(child) as SharpTreeViewItem;
                        if (container != null)
                            container.NodeView.LinesRenderer.InvalidateVisual();
                    }
            }
            else if (e.PropertyName == "IsExpanded")
            {
                if (Node.IsExpanded)
                    ParentTreeView.HandleExpanding(Node);
            }
        }

        private void OnIsEditingChanged()
        {
            var textEditorContainer = Template.FindName("textEditorContainer", this) as Border;
            if (Node.IsEditing)
                if (CellEditor == null)
                    textEditorContainer.Child = new EditTextBox {Item = ParentItem};
                else
                    textEditorContainer.Child = CellEditor;
            else
                textEditorContainer.Child = null;
        }

        private void UpdateTemplate()
        {
            var spacer = Template.FindName("spacer", this) as FrameworkElement;
            spacer.Width = CalculateIndent();

            var expander = Template.FindName("expander", this) as ToggleButton;
            if ((ParentTreeView.Root == Node) && !ParentTreeView.ShowRootExpander)
                expander.Visibility = Visibility.Collapsed;
            else
                expander.ClearValue(VisibilityProperty);
        }

        internal double CalculateIndent()
        {
            var result = 19*Node.Level;
            if (ParentTreeView.ShowRoot)
            {
                if (!ParentTreeView.ShowRootExpander)
                    if (ParentTreeView.Root != Node)
                        result -= 15;
            }
            else
            {
                result -= 19;
            }
            if (result < 0)
                throw new InvalidOperationException();
            return result;
        }

        public static readonly DependencyProperty TextBackgroundProperty =
            DependencyProperty.Register("TextBackground", typeof(Brush), typeof(SharpTreeNodeView));

        public static readonly DependencyProperty CellEditorProperty =
            DependencyProperty.Register("CellEditor", typeof(Control), typeof(SharpTreeNodeView),
                new FrameworkPropertyMetadata());
    }
}
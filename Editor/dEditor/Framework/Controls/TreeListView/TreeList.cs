using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace dEditor.Framework.Controls.TreeListView
{
    public class TreeList : ListView
    {
        public TreeList()
        {
            Rows = new ObservableCollectionAdv<TreeNode>();
            Root = new TreeNode(this, null);
            Root.IsExpanded = true;
            ItemsSource = Rows;
            ItemContainerGenerator.StatusChanged += ItemContainerGeneratorStatusChanged;
        }

        private void ItemContainerGeneratorStatusChanged(object sender, EventArgs e)
        {
            if ((ItemContainerGenerator.Status == GeneratorStatus.ContainersGenerated) && (PendingFocusNode != null))
            {
                var item = ItemContainerGenerator.ContainerFromItem(PendingFocusNode) as TreeListItem;
                if (item != null)
                    item.Focus();
                PendingFocusNode = null;
            }
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TreeListItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TreeListItem;
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var ti = element as TreeListItem;
            var node = item as TreeNode;
            if ((ti != null) && (node != null))
            {
                ti.Node = item as TreeNode;
                base.PrepareContainerForItemOverride(element, node.Tag);
            }
        }

        internal void SetIsExpanded(TreeNode node, bool value)
        {
            if (value)
            {
                if (!node.IsExpandedOnce)
                {
                    node.IsExpandedOnce = true;
                    node.AssignIsExpanded(value);
                    CreateChildrenNodes(node);
                }
                else
                {
                    node.AssignIsExpanded(value);
                    CreateChildrenRows(node);
                }
            }
            else
            {
                DropChildrenRows(node, false);
                node.AssignIsExpanded(value);
            }
        }

        internal void CreateChildrenNodes(TreeNode node)
        {
            var children = GetChildren(node);
            if (children != null)
            {
                var rowIndex = Rows.IndexOf(node);
                node.ChildrenSource = children as INotifyCollectionChanged;
                foreach (var obj in children)
                {
                    var child = new TreeNode(this, obj);
                    child.HasChildren = HasChildren(child);
                    node.Children.Add(child);
                }
                Rows.InsertRange(rowIndex + 1, node.Children.ToArray());
            }
        }

        private void CreateChildrenRows(TreeNode node)
        {
            var index = Rows.IndexOf(node);
            if ((index >= 0) || (node == Root)) // ignore invisible nodes
            {
                var nodes = node.AllVisibleChildren.ToArray();
                Rows.InsertRange(index + 1, nodes);
            }
        }

        internal void DropChildrenRows(TreeNode node, bool removeParent)
        {
            var start = Rows.IndexOf(node);
            if ((start >= 0) || (node == Root)) // ignore invisible nodes
            {
                var count = node.VisibleChildrenCount;
                if (removeParent)
                    count++;
                else
                    start++;
                Rows.RemoveRange(start, count);
            }
        }

        private IEnumerable GetChildren(TreeNode parent)
        {
            if (Model != null)
                return Model.GetChildren(parent.Tag);
            return null;
        }

        private bool HasChildren(TreeNode parent)
        {
            if (parent == Root)
                return true;
            if (Model != null)
                return Model.HasChildren(parent.Tag);
            return false;
        }

        internal void InsertNewNode(TreeNode parent, object tag, int rowIndex, int index)
        {
            var node = new TreeNode(this, tag);
            if ((index >= 0) && (index < parent.Children.Count))
                parent.Children.Insert(index, node);
            else
            {
                index = parent.Children.Count;
                parent.Children.Add(node);
            }
            Rows.Insert(rowIndex + index + 1, node);
        }

        #region Properties

        /// <summary>
        /// Internal collection of rows representing visible nodes, actually displayed in the ListView
        /// </summary>
        internal ObservableCollectionAdv<TreeNode> Rows { get; }


        private ITreeModel _model;

        public ITreeModel Model
        {
            get { return _model; }
            set
            {
                if (_model != value)
                {
                    _model = value;
                    Root.Children.Clear();
                    Rows.Clear();
                    CreateChildrenNodes(Root);
                }
            }
        }

        internal TreeNode Root { get; }

        public ReadOnlyCollection<TreeNode> Nodes
        {
            get { return Root.Nodes; }
        }

        internal TreeNode PendingFocusNode { get; set; }

        public ICollection<TreeNode> SelectedNodes
        {
            get { return SelectedItems.Cast<TreeNode>().ToArray(); }
        }

        public TreeNode SelectedNode
        {
            get
            {
                if (SelectedItems.Count > 0)
                    return SelectedItems[0] as TreeNode;
                return null;
            }
        }

        #endregion
    }
}
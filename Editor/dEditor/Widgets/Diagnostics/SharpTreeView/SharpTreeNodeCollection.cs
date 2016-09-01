// SharpTreeNodeCollection.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;

namespace dEditor.Widgets.Diagnostics.SharpTreeView
{
    public sealed class SharpTreeNodeCollection : IList<SharpTreeNode>, INotifyCollectionChanged
    {
        private readonly SharpTreeNode parent;
        private List<SharpTreeNode> list = new List<SharpTreeNode>();
        private bool isRaisingEvent;

        public SharpTreeNodeCollection(SharpTreeNode parent)
        {
            this.parent = parent;
        }

        public SharpTreeNode this[int index]
        {
            get { return list[index]; }
            set
            {
                ThrowOnReentrancy();
                var oldItem = list[index];
                if (oldItem == value)
                    return;
                ThrowIfValueIsNullOrHasParent(value);
                list[index] = value;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, value,
                    oldItem, index));
            }
        }

        public int Count
        {
            get { return list.Count; }
        }

        bool ICollection<SharpTreeNode>.IsReadOnly
        {
            get { return false; }
        }

        public int IndexOf(SharpTreeNode node)
        {
            if ((node == null) || (node.modelParent != parent))
                return -1;
            return list.IndexOf(node);
        }

        public void Insert(int index, SharpTreeNode node)
        {
            ThrowOnReentrancy();
            ThrowIfValueIsNullOrHasParent(node);
            list.Insert(index, node);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, node, index));
        }

        public void RemoveAt(int index)
        {
            ThrowOnReentrancy();
            var oldItem = list[index];
            list.RemoveAt(index);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItem,
                index));
        }

        public void Add(SharpTreeNode node)
        {
            ThrowOnReentrancy();
            ThrowIfValueIsNullOrHasParent(node);
            list.Add(node);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, node,
                list.Count - 1));
        }

        public void Clear()
        {
            ThrowOnReentrancy();
            var oldList = list;
            list = new List<SharpTreeNode>();
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldList, 0));
        }

        public bool Contains(SharpTreeNode node)
        {
            return IndexOf(node) >= 0;
        }

        public void CopyTo(SharpTreeNode[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public bool Remove(SharpTreeNode item)
        {
            var pos = IndexOf(item);
            if (pos >= 0)
            {
                RemoveAt(pos);
                return true;
            }
            return false;
        }

        public IEnumerator<SharpTreeNode> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            Debug.Assert(!isRaisingEvent);
            isRaisingEvent = true;
            try
            {
                parent.OnChildrenChanged(e);
                if (CollectionChanged != null)
                    CollectionChanged(this, e);
            }
            finally
            {
                isRaisingEvent = false;
            }
        }

        private void ThrowOnReentrancy()
        {
            if (isRaisingEvent)
                throw new InvalidOperationException();
        }

        private void ThrowIfValueIsNullOrHasParent(SharpTreeNode node)
        {
            if (node == null)
                throw new ArgumentNullException("node");
            if (node.modelParent != null)
                throw new ArgumentException("The node already has a parent", "node");
        }

        public void InsertRange(int index, IEnumerable<SharpTreeNode> nodes)
        {
            if (nodes == null)
                throw new ArgumentNullException("nodes");
            ThrowOnReentrancy();
            var newNodes = nodes.ToList();
            if (newNodes.Count == 0)
                return;
            foreach (var node in newNodes)
                ThrowIfValueIsNullOrHasParent(node);
            list.InsertRange(index, newNodes);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, newNodes, index));
        }

        public void RemoveRange(int index, int count)
        {
            ThrowOnReentrancy();
            if (count == 0)
                return;
            var oldItems = list.GetRange(index, count);
            list.RemoveRange(index, count);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, oldItems,
                index));
        }

        public void AddRange(IEnumerable<SharpTreeNode> nodes)
        {
            InsertRange(Count, nodes);
        }

        public void RemoveAll(Predicate<SharpTreeNode> match)
        {
            if (match == null)
                throw new ArgumentNullException("match");
            ThrowOnReentrancy();
            var firstToRemove = 0;
            for (var i = 0; i < list.Count; i++)
            {
                bool removeNode;
                isRaisingEvent = true;
                try
                {
                    removeNode = match(list[i]);
                }
                finally
                {
                    isRaisingEvent = false;
                }
                if (!removeNode)
                {
                    if (firstToRemove < i)
                    {
                        RemoveRange(firstToRemove, i - firstToRemove);
                        i = firstToRemove - 1;
                    }
                    else
                    {
                        firstToRemove = i + 1;
                    }
                    Debug.Assert(firstToRemove == i + 1);
                }
            }
            if (firstToRemove < list.Count)
                RemoveRange(firstToRemove, list.Count - firstToRemove);
        }
    }
}
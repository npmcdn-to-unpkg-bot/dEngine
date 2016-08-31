﻿// TreeFlattener.cs - dEditor
// Copyright (C) 2016-2016 DanDev (dandev.sco@gmail.com)
// 
// This library is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// You should have received a copy of the GNU General Public
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;

namespace dEditor.Widgets.Diagnostics.SharpTreeView
{
    public sealed class TreeFlattener : IList, INotifyCollectionChanged
    {
        private readonly bool includeRoot;

        /// <summary>
        /// The root node of the flat list tree.
        /// Tjis is not necessarily the root of the model!
        /// </summary>
        internal SharpTreeNode root;

        public TreeFlattener(SharpTreeNode modelRoot, bool includeRoot)
        {
            root = modelRoot;
            while (root.listParent != null)
                root = root.listParent;
            root.treeFlattener = this;
            this.includeRoot = includeRoot;
        }

        public object this[int index]
        {
            get
            {
                if ((index < 0) || (index >= Count))
                    throw new ArgumentOutOfRangeException();
                return SharpTreeNode.GetNodeByVisibleIndex(root, includeRoot ? index : index + 1);
            }
            set { throw new NotSupportedException(); }
        }

        public int Count
        {
            get { return includeRoot ? root.GetTotalListLength() : root.GetTotalListLength() - 1; }
        }

        public int IndexOf(object item)
        {
            var node = item as SharpTreeNode;
            if ((node != null) && node.IsVisible && (node.GetListRoot() == root))
                if (includeRoot)
                    return SharpTreeNode.GetVisibleIndexForNode(node);
                else
                    return SharpTreeNode.GetVisibleIndexForNode(node) - 1;
            return -1;
        }

        bool IList.IsReadOnly
        {
            get { return true; }
        }

        bool IList.IsFixedSize
        {
            get { return false; }
        }

        bool ICollection.IsSynchronized
        {
            get { return false; }
        }

        object ICollection.SyncRoot { get; } = new object();

        void IList.Insert(int index, object item)
        {
            throw new NotSupportedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        int IList.Add(object item)
        {
            throw new NotSupportedException();
        }

        void IList.Clear()
        {
            throw new NotSupportedException();
        }

        public bool Contains(object item)
        {
            return IndexOf(item) >= 0;
        }

        public void CopyTo(Array array, int arrayIndex)
        {
            foreach (var item in this)
                array.SetValue(item, arrayIndex++);
        }

        void IList.Remove(object item)
        {
            throw new NotSupportedException();
        }

        public IEnumerator GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
                yield return this[i];
        }

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public void RaiseCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (CollectionChanged != null)
                CollectionChanged(this, e);
        }

        public void NodesInserted(int index, IEnumerable<SharpTreeNode> nodes)
        {
            if (!includeRoot) index--;
            foreach (var node in nodes)
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, node,
                    index++));
        }

        public void NodesRemoved(int index, IEnumerable<SharpTreeNode> nodes)
        {
            if (!includeRoot) index--;
            foreach (var node in nodes)
                RaiseCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, node,
                    index));
        }

        public void Stop()
        {
            Debug.Assert(root.treeFlattener == this);
            root.treeFlattener = null;
        }
    }
}
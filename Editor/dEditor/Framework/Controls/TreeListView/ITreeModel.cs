﻿// ITreeModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Collections;

namespace dEditor.Framework.Controls.TreeListView
{
    public interface ITreeModel
    {
        /// <summary>
        /// Get list of children of the specified parent
        /// </summary>
        IEnumerable GetChildren(object parent);

        /// <summary>
        /// returns wheather specified parent has any children or not.
        /// </summary>
        bool HasChildren(object parent);
    }
}
// ILayoutItem.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.IO;

namespace dEditor.Framework
{
    public interface ILayoutItem
    {
        bool ShouldReopenOnStart { get; set; }
        string ContentId { get; }
        void LoadState(BinaryReader reader);
        void SaveState(BinaryWriter writer);
    }
}
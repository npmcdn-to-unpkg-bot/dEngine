// IDocument.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEditor.Framework
{
    public interface IDocument : ILayoutItem
    {
        bool IsSelected { get; set; }
        bool IsVisible { get; set; }
        bool IsActive { get; }
    }
}
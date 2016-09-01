// IWidget.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEditor.Framework
{
    public interface IWidget : ILayoutItem
    {
        bool IsVisible { get; set; }
        bool IsSelected { get; set; }
        void Activate();
    }
}
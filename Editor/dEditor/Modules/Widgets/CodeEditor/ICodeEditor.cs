// ICodeEditor.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

namespace dEditor.Modules.Widgets.CodeEditor
{
    public interface ICodeEditor
    {
        string CurrentSelection { get; set; }
        void UpdateTheme();
    }
}
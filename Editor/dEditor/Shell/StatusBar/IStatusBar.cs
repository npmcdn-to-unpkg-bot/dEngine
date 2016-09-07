// IStatusBar.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
namespace dEditor.Shell.StatusBar
{
    public interface IStatusBar
    {
        bool IsFrozen { get; set; }
        string Text { get; set; }
        int Line { get; set; }
        int Char { get; set; }
        void FreezeOutput(bool freeze);
        void SetLineChar(int line, int @char);
    }
}
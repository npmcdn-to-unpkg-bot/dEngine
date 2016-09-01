// IOutput.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEditor.Framework;

namespace dEditor.Widgets.Output
{
    public interface IOutput : IWidget
    {
        bool WordWrap { get; set; }
    }
}
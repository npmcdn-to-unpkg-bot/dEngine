// StatusBarViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.ComponentModel.Composition;

namespace dEditor.Shell.StatusBar
{
    [Export(typeof(IStatusBar))]
    public class StatusBarViewModel : IStatusBar
    {
    }
}
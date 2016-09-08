// ICommandBar.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Collections.Generic;

namespace dEditor.Modules.Shell.CommandBar
{
    public interface ICommandBar
    {
        IList<string> RecentCommands { get; }

        void Execute(string code, bool mute = false);
    }
}
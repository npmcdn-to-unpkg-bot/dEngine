// ChatViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.ComponentModel.Composition;
using dEditor.Framework;

namespace dEditor.Modules.Widgets.Chat
{
    [Export(typeof(IChat))]
    public class ChatViewModel : Widget, IChat
    {
        public ChatViewModel()
        {
            DisplayName = "Team Chat";
        }

        public override PaneLocation PreferredLocation => PaneLocation.Left;
    }
}
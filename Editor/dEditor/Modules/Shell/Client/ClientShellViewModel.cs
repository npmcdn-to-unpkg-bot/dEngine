// ClientShellViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using Caliburn.Micro;
using dEditor.Modules.Widgets.Viewport;

namespace dEditor.Modules.Shell.Client
{
    public class ClientShellViewModel : Conductor<ViewportViewModel>
    {
        public ClientShellViewModel()
        {
            ActiveItem = new ViewportViewModel(false);
        }
    }
}
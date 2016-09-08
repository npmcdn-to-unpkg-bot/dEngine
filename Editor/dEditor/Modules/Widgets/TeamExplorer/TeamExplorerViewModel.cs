// TeamExplorerViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.ComponentModel.Composition;
using dEditor.Framework;

namespace dEditor.Modules.Widgets.TeamExplorer
{
    [Export(typeof(ITeamExplorer))]
    public class TeamExplorerViewModel : Widget, ITeamExplorer
    {
        public override PaneLocation PreferredLocation { get; } = PaneLocation.Right;
    }
}
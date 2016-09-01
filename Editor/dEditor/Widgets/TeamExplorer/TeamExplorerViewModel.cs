﻿// TeamExplorerViewModel.cs - dEditor
// Copyright (C) 2016-2016 DanDevPC (dandev.sco@gmail.com)
// 
// This library is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// You should have received a copy of the GNU General Public
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using System.ComponentModel.Composition;
using dEditor.Framework;

namespace dEditor.Widgets.TeamExplorer
{
    [Export(typeof(ITeamExplorer))]
    public class TeamExplorerViewModel : Widget, ITeamExplorer
    {
        public override PaneLocation PreferredLocation { get; } = PaneLocation.Right;
    }
}
// IExplorer.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEditor.Framework;
using dEngine.Instances;

namespace dEditor.Widgets.Explorer
{
    public interface IExplorer : IWidget
    {
        Instance LastClickedInstance { get; set; }
        ExplorerItem RootItem { get; set; }
    }
}
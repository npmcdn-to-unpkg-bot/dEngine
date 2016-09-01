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
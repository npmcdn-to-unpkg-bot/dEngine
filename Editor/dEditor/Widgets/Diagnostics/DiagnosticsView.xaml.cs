using System.Windows.Controls;
using dEditor.Widgets.Diagnostics.SharpTreeView;

namespace dEditor.Widgets.Diagnostics
{
    /// <summary>
    /// Interaction logic for DiagnosticsView.xaml
    /// </summary>
    public partial class DiagnosticsView : UserControl
    {
        public DiagnosticsView()
        {
            InitializeComponent();
        }

        public class DiagnosticsRootNode : SharpTreeNode
        {
        }
    }
}
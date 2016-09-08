using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using dEditor.Framework;
using dEditor.Framework.Commands;

namespace dEditor.Widgets.ContentBrowser.ContextMenus
{
    /// <summary>
    /// Interaction logic for FileContextMenu.xaml
    /// </summary>
    public partial class FileContextMenu : ContextMenu
    {
        public FileContextMenu()
        {
            InitializeComponent();
        }

        public OpenCommand Open;
        public Command Cut;
        public Command Copy;
        public Command Paste;

        public class OpenCommand : Command
        {
            public override string Name => "Open";
            public override string Text => "Opens the file.";

            public override bool CanExecute(object parameter)
            {
                return true;
            }

            public override void Execute(object parameter)
            {

            }
        }
    }
}

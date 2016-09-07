// Document.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Threading.Tasks;
using System.Windows.Input;
using dEngine.Instances;

namespace dEditor.Framework
{
    /// <summary>
    /// A document is a type of module which is docked in the middle of the window.
    /// </summary>
    public abstract class Document : LayoutItem, IDocument
    {
        protected Document()
        {
            ShouldReopenOnStart = false;
        }

        public override ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(p => TryClose(null), p => true)); }
        }

        public virtual void OnHide()
        {
        }

        public virtual Task OnSave(LuaSourceContainer container)
        {
            return Task.FromResult(true);
        }

        public void Activate()
        {
            Editor.Current.Shell.ActiveLayoutItem = this;
        }
    }
}
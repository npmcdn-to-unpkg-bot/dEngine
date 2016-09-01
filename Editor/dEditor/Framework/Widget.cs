// Widget.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows.Input;

namespace dEditor.Framework
{
    /// <summary>
    /// A widget which can be docked to the shell.
    /// </summary>
    public abstract class Widget : LayoutItem, IWidget
    {
        public abstract PaneLocation PreferredLocation { get; }

        public virtual double PreferredWidth => 200;

        public virtual double PreferredHeight => 200;

        public override ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(p => IsVisible = false, p => true)); }
        }

        public override void TryClose(bool? dialogResult = null)
        {
            base.TryClose(dialogResult);

            Editor.Current.Shell.Widgets.Remove(this);
        }

        public void Activate()
        {
            Editor.Current.Shell.ActiveLayoutItem = this;
        }
    }
}
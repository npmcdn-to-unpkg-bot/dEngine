// Dialog.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Windows.Input;

namespace dEditor.Framework
{
    public abstract class Dialog : LayoutItem
    {
        public float MinWidth { get; protected set; }
        public float MinHeight { get; protected set; }
        public float MaxWidth { get; protected set; }
        public float MaxHeight { get; protected set; }
        public float Width { get; protected set; }
        public float Height { get; protected set; }

        public override ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new RelayCommand(p => IsVisible = false, p => true)); }
        }

        public override void TryClose(bool? dialogResult = null)
        {
            base.TryClose(dialogResult);
        }
    }
}
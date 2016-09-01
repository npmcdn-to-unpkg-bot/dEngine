// Command.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Windows.Input;

namespace dEditor.Framework
{
    public abstract class Command : ICommand
    {
        public abstract string Name { get; }
        public abstract string Text { get; }
        public virtual Uri IconSource => null;
        public abstract KeyGesture KeyGesture { get; }

        public event EventHandler CanExecuteChanged;

        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter);

        public void UpdateCanExecute()
        {
            Editor.Current.Dispatcher.InvokeAsync(() => { CanExecuteChanged?.Invoke(this, EventArgs.Empty); });
        }
    }
}
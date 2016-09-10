// GarbageCollectCommand.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using Caliburn.Micro;
using dEditor.Framework;
using dEditor.Modules.Shell.CommandBar;

namespace dEditor.Modules.Shell.Commands
{
    public class GarbageCollectCommand : Command
    {
        public override string Name { get; } = "GC";
        public override string Text { get; } = "Executes a script from a file.";

        public override bool CanExecute(object parameter)
        {
            return true;
        }

        public override void Execute(object parameter)
        {
            GC.Collect();
        }
    }
}
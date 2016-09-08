// CollapsibleOutputItem.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Collections.ObjectModel;
using dEngine;

namespace dEditor.Modules.Widgets.Output
{
    public class CollapsibleOutputItem : OutputItem
    {
        public CollapsibleOutputItem(ref string msg, ref LogLevel level, ref string logger)
            : base(ref msg, ref level, ref logger)
        {
            Contents = new ObservableCollection<OutputItem>();
        }

        public ObservableCollection<OutputItem> Contents { get; set; }
    }
}
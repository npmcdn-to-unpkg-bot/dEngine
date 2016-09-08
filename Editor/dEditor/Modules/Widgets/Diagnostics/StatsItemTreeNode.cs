// StatsItemTreeNode.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Linq;
using dEditor.Modules.Widgets.Diagnostics.SharpTreeView;
using dEngine.Instances.Diagnostics;

namespace dEditor.Modules.Widgets.Diagnostics
{
    public class StatsItemTreeNode : SharpTreeNode
    {
        public StatsItemTreeNode(StatsItem item)
        {
            Item = item;
        }

        public StatsItem Item { get; }

        public string Value => Item.ValueString;

        public override object Text => Item.Name;
        public override object Icon => null;
        public override bool ShowExpander => Item.Children.Count > 0;

        public override void Delete()
        {
            base.Delete();
            Parent.Children.Remove(this);
        }

        protected override void LoadChildren()
        {
            Children.AddRange(Item.Children.OfType<StatsItem>().Select(item => new StatsItemTreeNode(item)));
        }
    }
}
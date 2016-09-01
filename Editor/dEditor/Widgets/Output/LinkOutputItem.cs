// LinkOutputItem.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine;

namespace dEditor.Widgets.Output
{
    public class LinkOutputItem : OutputItem
    {
        public LinkOutputItem(ref string msg, ref LogLevel level, ref string logger, ref string url)
            : base(ref msg, ref level, ref logger)
        {
            Url = url;
        }

        public string Url { get; }
    }
}
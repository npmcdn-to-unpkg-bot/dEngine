// FindAndReplaceResult.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Collections.Generic;
using dEngine.Instances;

namespace dEditor.Modules.Dialogs.FindAndReplace
{
    public class FindAndReplaceResult
    {
        public string MatchText { get; }
        public string Mode { get; }
        public bool Regex { get; }
        public IEnumerable<string> Results { get; set; }

        public class Line
        {
            public LuaSourceContainer Source { get; }
            public string Name { get; }
            public int LineNumber { get; }
        }
    }
}
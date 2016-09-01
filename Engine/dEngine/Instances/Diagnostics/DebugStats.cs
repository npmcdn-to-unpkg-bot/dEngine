// DebugStats.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances.Diagnostics
{
    [Uncreatable]
    [ExplorerOrder(-1)]
    internal sealed class DebugStats : Instance
    {
        public DebugStats()
        {
            Archivable = false;
        }
    }
}
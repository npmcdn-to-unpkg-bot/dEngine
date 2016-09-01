// FileSystem.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using dEngine.Instances;

namespace dEditor.Widgets.ContentBrowser.Instances
{
    public abstract class FileSystem : Instance
    {
        public string Path { get; set; }
    }
}
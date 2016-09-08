// ProjectEditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using dEditor.Framework;

namespace dEditor.Modules.Widgets.ProjectEditor
{
    public class ProjectEditorViewModel : Document, IProjectEditor
    {
        public override string DisplayName => Project.Name;
        public Project Project => Project.Current;
    }
}
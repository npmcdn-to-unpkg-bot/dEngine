// TemplateItem.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;

namespace dEditor.Dialogs.ProjectProperties
{
    public class TemplateItem
    {
        public TemplateItem(string name, string description, Uri icon, Action templateLoader)
        {
            Name = name;
            Description = description;
            Icon = icon;
            LoadTemplate = templateLoader;
        }

        public string Name { get; }
        public string Description { get; }
        public Uri Icon { get; }
        public Action LoadTemplate { get; }
    }
}
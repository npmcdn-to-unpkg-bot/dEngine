// ProjectTemplate.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace dEditor.Modules.Widgets.StartPage
{
    public class ProjectTemplate
    {
        public ProjectTemplate(string name, Action loadTemplate)
        {
            Name = name;

            var thumbnail =
                new BitmapImage(new Uri($"/dEditor;component/Content/Templates/{name}/thumbnail.png", UriKind.Relative));
            thumbnail.Freeze();
            Thumbnail = thumbnail;
            Load = loadTemplate;
        }

        public string Name { get; }
        public Action Load { get; }
        public ImageSource Thumbnail { get; }
    }
}
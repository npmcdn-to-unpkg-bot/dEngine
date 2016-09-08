// PlacesProtocol.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.IO;
using dEngine;

namespace dEditor.Framework.Content.Protocols
{
    public class PlaceProtocol : IContentProtocol
    {
        public Stream Fetch(Uri uri)
        {
            return File.OpenRead(Path.Combine(Project.Current.ProjectPath, "Places", uri.Host + ".place"));
        }
    }
}
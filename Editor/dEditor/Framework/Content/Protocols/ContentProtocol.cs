// ContentProtocol.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.IO;
using dEngine;
using dEngine.Services;

namespace dEditor.Framework.Content.Protocols
{
    public class ContentProtocol : IContentProtocol
    {
        public Stream Fetch(Uri uri)
        {
            var localFile = Path.Combine(Project.Current.ContentPath, ContentProvider.GetAbsolutePath(uri));

            if (File.Exists(localFile))
                return File.OpenRead(localFile);

            return File.OpenRead(localFile);
        }
    }
}
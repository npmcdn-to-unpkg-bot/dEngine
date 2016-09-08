// EditorProtocol.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Resources;
using dEngine;
using dEngine.Services;

namespace dEditor.Framework.Content.Protocols
{
    public class EditorProtocol : IContentProtocol
    {
        private static readonly Assembly _externalBaml =
            Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "dEditor.exe"));

        public Stream Fetch(Uri uri)
        {
            var editorPath = $"content/{ContentProvider.GetAbsolutePath(uri).TrimEnd('/').ToLower()}";
            var name = _externalBaml.GetName().Name + ".g.resources";
            var resourceStream = _externalBaml.GetManifestResourceStream(name);
            if (resourceStream == null)
                throw new FileNotFoundException($"Could not find mainifest resource \"{name}\".");
            using (var reader = new ResourceReader(resourceStream))
            {
                foreach (DictionaryEntry entry in reader)
                {
                    if (entry.Key.ToString() == editorPath)
                        return (Stream)entry.Value;
                }
            }
            throw new NotSupportedException($"No file found in editor manifest resource.");
        }
    }
}
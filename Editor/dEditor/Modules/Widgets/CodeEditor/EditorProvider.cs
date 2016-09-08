// EditorProvider.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Linq;
using dEngine.Instances;

namespace dEditor.Modules.Widgets.CodeEditor
{
    public static class EditorProvider
    {
        public static CodeEditorViewModel Open(LuaSourceContainer script, int lineNumber = 0)
        {
            var doc =
                Editor.Current.Shell.Items.OfType<CodeEditorViewModel>()
                    .FirstOrDefault(editor => editor.LuaSourceContainer == script) ?? new CodeEditorViewModel(script);
            doc.Activate();
            return doc;
        }
    }
}
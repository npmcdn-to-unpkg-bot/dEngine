// CustomCursors.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Reflection;
using System.Windows.Input;

namespace dEditor.Framework
{
    public static class CustomCursors
    {
        public static Cursor ClosedHand =
            new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("dEditor.Content.Icons.closedhand.cur"));

        public static Cursor OpenHand =
            new Cursor(Assembly.GetExecutingAssembly().GetManifestResourceStream("dEditor.Content.Icons.openhand.cur"));
    }
}
// DirectoryExtensions.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.IO;

namespace dEditor.Utility
{
    public static class DirectoryExtensions
    {
        public static bool EqualsDir(this DirectoryInfo dirA, DirectoryInfo dirB)
        {
            return
                0 == string.Compare(
                    Path.GetFullPath(dirA.FullName).TrimEnd('\\'),
                    Path.GetFullPath(dirB.FullName).TrimEnd('\\'),
                    StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
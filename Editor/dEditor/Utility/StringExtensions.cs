// StringExtensions.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Text.RegularExpressions;
using dEditor.Framework;

namespace dEditor.Utility
{
    public static class StringExtensions
    {
        private static readonly int _contentLength = "Content/".Length;

        public static string ToContentId(this string path, Project project = null)
        {
            project = project ?? Project.Current;

            return !path.StartsWith(project.ContentPath)
                ? $"file://{path}"
                : $"content://{path.Substring(project.ContentPath.Length + 1)}";

            /*
            var contentUri = new Uri(project.ContentPath, UriKind.Absolute);
            var uri = new Uri(url, UriKind.Absolute);
            var relUri = contentUri.MakeRelativeUri(uri).ToString().Substring(_contentLength);

            return $"content://{relUri}";
            */
        }

        public static bool ValidateFileName(this string fileName)
        {
            var unsupported =
                new Regex(
                    "(^(PRN|AUX|NUL|CON|COM[1-9]|LPT[1-9]|(\\.+)$)(\\..*)?$)|(([\\x00-\\x1f\\\\?*:\";‌​|/<>])+)|([\\. ]+)",
                    RegexOptions.IgnoreCase);
            return !unsupported.IsMatch(fileName);
        }
    }
}
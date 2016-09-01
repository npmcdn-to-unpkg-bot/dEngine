// FindService.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Services;

namespace dEditor.Framework.Services
{
    public class FindService
    {
        public static void Init()
        {
            ContextActionService.Register("findInAllScripts", OpenFindAllWindow);
        }

        private static void OpenFindAllWindow()
        {
        }
    }
}
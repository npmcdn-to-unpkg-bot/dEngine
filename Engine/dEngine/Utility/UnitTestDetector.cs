// UnitTestDetector.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Linq;

namespace dEngine.Utility
{
    /// <summary>
    /// Detects if we are running inside a unit test.
    /// </summary>
    public static class UnitTestDetector
    {
        static UnitTestDetector()
        {
            var testAssemblyName = "Microsoft.VisualStudio.QualityTools.UnitTestFramework";
            IsInUnitTest = AppDomain.CurrentDomain.GetAssemblies()
                .Any(a => a.FullName.StartsWith(testAssemblyName));
        }

        /// <summary/>
        public static bool IsInUnitTest { get; private set; }
    }
}
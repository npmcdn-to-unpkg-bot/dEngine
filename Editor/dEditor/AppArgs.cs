// AppArgs.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Diagnostics;

namespace dEditor
{
    public class AppArgs
    {
        public bool UseClientShell { get; set; }
        public string API { get; set; }

        public static AppArgs Parse(string[] args)
        {
            if (args.Length%2 != 0)
                throw new ArgumentOutOfRangeException(nameof(args), "Args must be in key-value pairs. (divisible by 2)");

            var type = typeof(AppArgs);
            var argsObj = new AppArgs();

            for (var i = 0; i < args.Length; i += 2)
            {
                var name = args[i].TrimStart('/', '-');
                var value = args[i + 1];

                var property = type.GetProperty(name);

                if (property == null)
                {
                    Debug.WriteLine($"No argument defined in AppArgs for \"{name}\"");
                    continue;
                }

                property.SetValue(argsObj, Convert.ChangeType(value, property.PropertyType));
            }

            return argsObj;
        }
    }
}
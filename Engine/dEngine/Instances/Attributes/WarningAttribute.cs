﻿// WarningAttribute.cs - dEngine
// Copyright (C) 2016-2016 DanDevPC (dandev.sco@gmail.com)
// 
// This library is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//  
// You should have received a copy of the GNU General Public
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using System;

namespace dEngine.Instances.Attributes
{
    /// <summary>
    /// An attribute which defines a warning in the documentation.
    /// </summary>
    [AttributeUsage(
         AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Struct,
         AllowMultiple = true, Inherited = false)]
    public class WarningAttribute : Attribute
    {
        /// <summary />
        public WarningAttribute(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
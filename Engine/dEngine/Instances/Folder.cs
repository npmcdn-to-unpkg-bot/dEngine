// Folder.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A container for organizing objects.
    /// </summary>
    /// <seealso cref="Model" />
    [TypeId(14)]
    [ToolboxGroup("Containers")]
    [ExplorerOrder(2)]
    public class Folder : Instance
    {
        private int _hue;

        /// <summary>
        /// The hue of the folder.
        /// </summary>
        [InstMember(1)]
        [EditorVisible("Hue")]
        [Range(0, 360)]
        public int Hue
        {
            get { return _hue; }
            set
            {
                if (value == _hue) return;
                value = Math.Max(0, Math.Min(value, 360));
                _hue = value;
                NotifyChanged();
            }
        }
    }
}
// StringValue.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.IO;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A <see cref="ValueContainer" /> that holds a <see cref="string" />.
    /// </summary>
    [TypeId(88)]
    [ExplorerOrder(3)]
    [ToolboxGroup("Values")]
    public sealed class StringValue : ValueContainer
    {
        /// <summary>
        /// The maximum length of <see cref="Value" />.
        /// </summary>
        public const int MaxLength = 300000;

        private string _value;

        /// <inheritdoc />
        public StringValue()
        {
            _value = "Value";
        }

        /// <summary>
        /// The value that this container holds.
        /// </summary>
        [InstMember(1)]
        [EditorVisible]
        public string Value
        {
            get { return _value; }
            set
            {
                value = value ?? string.Empty;

                if (value.Length > MaxLength)
                    throw new InvalidDataException("String value was too long.");

                _value = value;

                NotifyChanged(nameof(Value));
            }
        }
    }
}
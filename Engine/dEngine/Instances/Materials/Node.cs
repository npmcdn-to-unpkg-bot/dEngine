// Node.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.Collections.Generic;
using System.Reflection;
using dEngine.Instances.Attributes;

namespace dEngine.Instances.Materials
{
    /// <summary>
    /// Base class for material nodes.
    /// </summary>
    [Uncreatable]
    [TypeId(211)]
    public abstract class Node : Instance
    {
        private readonly List<Slot> _inputSlots;
        private readonly List<Slot> _outputSlots;

        private Vector2 _position;
        private Vector2 _size;
        private string _comment;

        internal int InSlotCount;
        internal int OutSlotCount;

        /// <summary />
        protected Node()
        {
            _inputSlots = new List<Slot>();
            _outputSlots = new List<Slot>();

            SlotConnected = new Signal<Slot>(this);

            var properties = GetType().GetProperties();
            foreach (var prop in properties)
            {
                var slotAttr = prop.GetCustomAttribute<NodeSlotAttribute>();
                var slotColourAttr = prop.GetCustomAttribute<NodeSlotColourAttribute>();
                if (slotAttr != null)
                {
                    var slot = new Slot(this, slotAttr.DisplayName, slotAttr.Type, slotAttr.Mode, slotColourAttr?.Colour);
                    prop.SetValue(this, slot);
                    if (slotAttr.Mode == InOut.In)
                        _inputSlots.Add(slot);
                    else
                        _outputSlots.Add(slot);
                }
            }
        }

        /// <summary>
        /// The position of the node.
        /// </summary>
        [EditorVisible]
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                if (value == _position) return;
                _position = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// The size of the node.
        /// </summary>
        [EditorVisible]
        public Vector2 Size
        {
            get { return _size; }
            set
            {
                if (value == _size) return;
                _size = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// The comment attached to the node.
        /// </summary>
        [EditorVisible]
        public string Comment
        {
            get { return _comment; }
            set
            {
                if (value == _comment) return;
                _comment = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Gets the slots of the given mode.
        /// </summary>
        public IEnumerable<Slot> GetSlots(InOut mode)
        {
            return mode == InOut.In ? _inputSlots : _outputSlots;
        }

        /// <summary>
        /// Fired when a slot is connected.
        /// </summary>
        public readonly Signal<Slot> SlotConnected;
    }
}
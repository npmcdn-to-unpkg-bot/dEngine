// Seat.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A brick which can be occupied by a <see cref="Character" />.
    /// </summary>
    [TypeId(16)]
    [ExplorerOrder(3)]
    [ToolboxGroup("Bricks")]
    public class Seat : Part
    {
        private readonly Attachment _attachment;
        /// <summary>
        /// Determines if the seat is disabled.
        /// </summary>
        protected bool _disabled;
        private bool _occupyOnTouch;
        private Character _occupant;

        /// <summary />
        public Seat()
        {
            _attachment = new Attachment
            {
                Name = "SeatAttachment",
                Parent = this,
                Archivable = false,
                ParentLocked = false
            };
        }

        /// <summary>
        /// The character that currently occupies this seat.
        /// </summary>
        /// <remarks>
        /// This property can be manually set, which will eject the current occupant if there is one,
        /// and force the new character to sit on it.
        /// </remarks>
        [InstMember(1)]
        public Character Occupant
        {
            get { return _occupant; }
            set
            {
                if (value == _occupant) return;
                _occupant = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// Determines if this object can be occupied by touching it.
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public bool OccupyOnTouch
        {
            get { return _occupyOnTouch; }
            set
            {
                if (value == _occupyOnTouch) return;
                _occupyOnTouch = value;
                NotifyChanged();
            }
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            base.Destroy();

            Occupant = null;
            _attachment.ParentLocked = false;
        }
    }
}
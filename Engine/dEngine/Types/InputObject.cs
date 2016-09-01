// InputObject.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System.IO;

namespace dEngine
{
    /// <summary>
    /// An object which describes an input action.
    /// </summary>
    public class InputObject : IDataType
    {
        internal InputObject(Key key, InputState inputState, InputType inputType)
        {
            Key = key;
            InputState = inputState;
            InputType = inputType;
            Position = Vector3.Zero;
            Delta = Vector3.Zero;
        }

        internal InputObject(Key key, InputState inputState, InputType inputType, Vector3 position, Vector3 delta)
        {
            Key = key;
            InputState = inputState;
            InputType = inputType;
            Position = position;
            Delta = delta;
        }

        /// <summary />
        public InputObject()
        {
        }

        /// <summary>
        /// Determines if this input has been handled.
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        /// The pressed key.
        /// </summary>
        [InstMember(1)]
        public Key Key { get; private set; }

        /// <summary>
        /// The state of the input.
        /// </summary>
        [InstMember(2)]
        public InputState InputState { get; private set; }

        /// <summary>
        /// The type of input.
        /// </summary>
        [InstMember(3)]
        public InputType InputType { get; private set; }

        /// <summary>
        /// Describes the positional value.
        /// </summary>
        [InstMember(4)]
        public Vector3 Position { get; private set; }

        /// <summary>
        /// Describes the delta for mouse/stick movements.
        /// </summary>
        [InstMember(5)]
        public Vector3 Delta { get; private set; }

        /// <summary />
        public void Load(BinaryReader reader)
        {
            Key = (Key)reader.ReadInt32();
            InputState = (InputState)reader.ReadByte();
            InputType = (InputType)reader.ReadByte();
            var position = new Vector3();
            var delta = new Vector3();
            position.Load(reader);
            delta.Load(reader);
            Position = position;
            Delta = delta;
        }

        /// <summary />
        public void Save(BinaryWriter writer)
        {
            writer.Write((int)Key);
            writer.Write((byte)InputState);
            writer.Write((byte)InputType);
            Position.Save(writer);
            Delta.Save(writer);
        }
    }
}
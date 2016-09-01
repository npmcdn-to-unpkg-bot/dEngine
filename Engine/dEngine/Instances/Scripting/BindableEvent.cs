// BindableEvent.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using Neo.IronLua;

namespace dEngine.Instances
{
    /// <summary>
    /// Acts as a custom event to allow communication between scripts.
    /// </summary>
    [TypeId(55)]
    [ToolboxGroup("Scripting")]
    [Warning(
         "BindableEvents do not allows for communication between the server and client. If you are looking for this functionality use [RemoteEvents](index?.htmltitle=RemoteEvent)."
     )]
    public class BindableEvent : Instance
    {
        /// <summary />
        public BindableEvent()
        {
            Event = new Signal<LuaResult>(this);
        }

        /// <summary>
        /// Fires the <see cref="Event" />.
        /// </summary>
        /// <param name="arguments">The arguments to pass to the event.</param>
        public void Fire(params object[] arguments)
        {
            Event.Fire(new LuaResult(arguments));
        }

        /// <summary>
        /// An event which is fired by <see cref="Fire" />.
        /// </summary>
        /// <eventParam name="arguments" />
        public readonly Signal<LuaResult> Event;
    }
}
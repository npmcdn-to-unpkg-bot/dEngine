// RemoteEvent.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// A networked event.
    /// </summary>
    [TypeId(46)]
    [ToolboxGroup("Scripting")]
    public sealed class RemoteEvent : Instance
    {
        /// <inheritdoc />
        public RemoteEvent()
        {
            DeliveryMethod = DeliveryMethod.ReliableOrdered;
            OnClientEvent = new Signal<dynamic[]>(this);
            OnServerEvent = new Signal<dynamic[]>(this);
        }

        /// <summary>
        /// The delivery method to use.
        /// </summary>
        [InstMember(1)]
        public DeliveryMethod DeliveryMethod { get; set; }

        /// <summary>
        /// Fires <see cref="OnClientEvent" /> for the provided player.
        /// </summary>
        /// <param name="player">The player to fire the event for.</param>
        /// <param name="args">The arguments to fire the event with.</param>
        public void FireClient(Player player, params dynamic[] args)
        {
        }

        /// <summary>
        /// Fires <see cref="OnServerEvent" /> for the server.
        /// </summary>
        /// <param name="args">The arguments to fire the event with.</param>
        public void FireServer(params dynamic[] args)
        {
        }

        /// <summary>
        /// Fired on the client when the server invokes this event.
        /// </summary>
        public readonly Signal<dynamic[]> OnClientEvent;

        /// <summary>
        /// Fired on the server when the client invokes this event.
        /// </summary>
        public readonly Signal<dynamic[]> OnServerEvent;
    }
}
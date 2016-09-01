// ServerReplicator.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using Lidgren.Network;
using Steamworks;

namespace dEngine.Instances
{
    /// <summary>
    /// Represents the server's connection to a client.
    /// </summary>
    /// <remarks>
    /// The server creates this when a client connects.
    /// </remarks>
    [Uncreatable]
    [TypeId(47)]
    public sealed class ServerReplicator : NetworkReplicator
    {
        internal ServerReplicator(NetConnection connection, Player player) : base(connection, player)
        {
            TicketProcessed = new Signal<uint, bool, int>(this);

            Address = connection.RemoteEndPoint.Address.ToString();
            Port = connection.RemoteEndPoint.Port;
        }

        internal CSteamID SteamId { get; private set; }

        /// <inheritdoc />
        public override void CloseConnection(string goodbye = "")
        {
            Player.Disconnected.Fire(goodbye);
            Player.Destroy();
            SteamGameServer.EndAuthSession(SteamId);
            base.CloseConnection(goodbye);
        }

        /// <summary>
        /// Fired when the player's authentication ticket is processed.
        /// </summary>
        public readonly Signal<uint, bool, int> TicketProcessed;
    }
}
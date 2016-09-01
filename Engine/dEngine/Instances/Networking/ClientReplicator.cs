// ClientReplicator.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using Lidgren.Network;
using Steamworks;

namespace dEngine.Instances
{
    /// <summary>
    /// Represents the client's connection to a server.
    /// </summary>
    /// <remarks>
    /// The client creates this when it connects to a server.
    /// </remarks>
    [TypeId(43)]
    [Uncreatable]
    public sealed class ClientReplicator : NetworkReplicator
    {
        internal ClientReplicator(NetConnection connection, HAuthTicket steamTicket, Player player)
            : base(connection, player)
        {
            AuthTicket = steamTicket;

            Address = connection.RemoteEndPoint.Address.ToString();
            Port = connection.RemoteEndPoint.Port;
        }

        internal HAuthTicket AuthTicket { get; }

        /// <inheritdoc />
        public override void Destroy()
        {
            base.Destroy();
            CloseConnection("Replicator was destroyed.");
        }
    }
}
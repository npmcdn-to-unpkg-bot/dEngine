// NetworkReplicator.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using Lidgren.Network;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace dEngine.Instances
{
    /// <summary>
    /// Base class for the client and server replicator classes.
    /// </summary>
    [TypeId(44)]
    [Uncreatable]
    public abstract class NetworkReplicator : Instance
    {
        private readonly NetConnection _connection;
        protected bool _replicationEnabled;

        /// <inheritdoc />
        protected NetworkReplicator(NetConnection connection, Player player)
        {
            _connection = connection;
            Player = player;
        }

        /// <summary>
        /// Gets whether or not the connection is connected.
        /// </summary>
        public bool IsConnected => _connection.Status == NetConnectionStatus.Connected;

        /// <summary>
        /// The player that this replicator is connected to.
        /// </summary>
        [EditorVisible]
        public Player Player { get; }

        /// <summary>
        /// Determines if replication packets should be sent/received.
        /// </summary>
        public bool ReplicationEnabled
        {
            get { return _replicationEnabled; }
            set
            {
                if (value == _replicationEnabled) return;
                _replicationEnabled = value;
                NotifyChanged();
            }
        }

        /// <summary>
        /// The address of this connection.
        /// </summary>
        [EditorVisible]
        public string Address { get; protected set; }

        /// <summary>
        /// The port this connection is running on.
        /// </summary>
        [EditorVisible]
        public int Port { get; protected set; }

        /// <summary>
        /// Returns the player which is associated with this replicator.
        /// </summary>
        public Player GetPlayer()
        {
            return Player;
        }

        /// <summary>
        /// Returns a string representation of network statistics.
        /// </summary>
        /// <returns></returns>
        public string GetLidgrenStatsString()
        {
            return _connection.Statistics.ToString();
        }

        /// <summary>
        /// Disconnects the replicator, ending the connection.
        /// </summary>
        public virtual void CloseConnection(string goodbye = "")
        {
            _connection.Disconnect(goodbye);
            if (!IsDestroyed)
                Destroy();
        }

        /// <inheritdoc />
        public override void Destroy()
        {
            base.Destroy();
            if (IsConnected)
                CloseConnection("Replicator was destroyed.");
        }
    }
}
﻿// NetworkServer.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Collections.Concurrent;
using System.Net.Sockets;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using dEngine.Instances.Messages;
using dEngine.Settings.Global;
using Lidgren.Network;
using Steamworks;

namespace dEngine.Services.Networking
{
    /// <summary>
    /// Service for handling networking and replication on the server.
    /// </summary>
    [TypeId(77)]
    [ExplorerOrder(6)]
    public sealed class NetworkServer : NetworkPeer
    {
        internal static NetworkServer Service;
        private NetServer _netServer;
        private bool _authenticationRequired;

        /// <inheritdoc />
        public NetworkServer()
        {
            Service = this;
            Replicators = new ConcurrentDictionary<Player, ServerReplicator>();
        }

        internal static bool IsHost => Service.IsRunning;

        /// <summary>
        /// The port the server is hosted on.
        /// </summary>
        [EditorVisible]
        public int Port { get; private set; }

        /// <summary>
        /// The address the server is hosted on.
        /// </summary>
        [EditorVisible]
        public string Address { get; private set; }

        /// <summary>
        /// The number of clients connected to the server.
        /// </summary>
        [EditorVisible]
        public int ClientCount => _netServer?.ConnectionsCount ?? 0;

        /// <summary>
        /// A dictionary of replicators indexed by player.
        /// </summary>
        public ConcurrentDictionary<Player, ServerReplicator> Replicators { get; }

        /// <summary>
        /// A custom handler for messages that are not handled in the base implementation.
        /// </summary>
        [LevelEditorRelated]
        public Func<byte, NetIncomingMessage, bool> CustomMessageHandler { get; set; }

        internal static object GetExisting()
        {
            return DataModel.GetService<NetworkServer>();
        }

        /// <summary>
        /// Starts the server.
        /// </summary>
        /// <param name="port">The port number to host on.</param>
        public void Start(int port = 0)
        {
            _peerConfig = NetworkSettings.GetNetPeerConfig(this);
            _peerConfig.Port = port > 0 ? port : _peerConfig.Port;

            if (_authenticationRequired)
                _peerConfig.EnableMessageType(NetIncomingMessageType.ConnectionApproval);

            Port = port;
            Address = _peerConfig.BroadcastAddress.ToString();

            _netServer = new NetServer(_peerConfig);
            _peer = _netServer;

            Logger.Trace($"NetworkServer starting at {Address}:{Port}");

            try
            {
                // TODO: init steamworks game server
                _netServer.Start();
                IsRunning = true;
                Logger.Trace("NetworkServer started.");
            }
            catch (SocketException e)
            {
                Logger.Warn("NetworkServer threw an exception while starting.");
                Logger.Warn(e.Message);
            }
        }

        /// <summary>
        /// Sets whether or not authentication is required.
        /// </summary>
        /// <param name="required"></param>
        [ScriptSecurity(ScriptIdentity.CoreScript | ScriptIdentity.Server | ScriptIdentity.Editor)]
        public void SetIsPlayerAuthenticationRequired(bool required)
        {
            ScriptService.AssertIdentity(ScriptIdentity.CoreScript | ScriptIdentity.Server | ScriptIdentity.Editor);
            _authenticationRequired = true;
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop()
        {
            if (!IsRunning)
                return;

            ClearChildren();

            IsRunning = false;

            _netServer?.Shutdown("NetworkServer.Stop()");
        }

        internal override void ProcessMessages()
        {
            if (_netServer == null)
                return;

            NetIncomingMessage msg;

            while ((msg = _netServer.ReadMessage()) != null)
            {
                switch (msg.MessageType)
                {
                    case NetIncomingMessageType.ConnectionApproval:
                        AuthorizeConnection(msg);
                        break;
                    case NetIncomingMessageType.Data:
                        var customId = msg.ReadByte();
                        break;
                    default:
                        Logger.Warn($"Unhandled MessageType \"{msg.MessageType}\"");
                        break;
                }

                _netServer.Recycle(msg);
            }
        }
        
        /// <summary/>
        private void SendMessage(IMessageHandler messageHandler, DeliveryMethod deliveryMethod, Player player = null)
        {
            var output = _netServer.CreateMessage();
            output.Write(messageHandler.MessageId);
            messageHandler.ServerWrite(output);

            if (player != null)
            {
                _netServer.SendMessage(output, player.Replicator.Connection, (NetDeliveryMethod)deliveryMethod);
            }
            else
            {
                _netServer.SendToAll(output, (NetDeliveryMethod)deliveryMethod);
            }
        }

        /// <summary>
        /// Checks if the given <see cref="NetIncomingMessageType.ConnectionApproval" /> request is authorized.
        /// </summary>
        /// <param name="msg"></param>
        private void AuthorizeConnection(NetIncomingMessage msg)
        {
            var authTicket = msg.ReadBytes(1024);
            var pcbTicket = msg.ReadUInt32();
            var steamId = new CSteamID(msg.ReadUInt64());

            var result = SteamGameServer.BeginAuthSession(authTicket, (int)pcbTicket, steamId);

            var connection = msg.SenderConnection;

            if (result == EBeginAuthSessionResult.k_EBeginAuthSessionResultOK)
            {
                connection.Approve();
                Logger.Info($"Connection {connection.RemoteEndPoint} approved.");
            }
            else
            {
                connection.Deny(result.ToString());
                Logger.Warn($"Connection {connection.RemoteEndPoint} was not approved: {result}");
            }
        }

        /// <summary>
        /// Creates a <see cref="Player" /> and <see cref="ServerReplicator" /> for the client.
        /// </summary>
        private void AddClient(string persona, NetConnection connection)
        {
            var player = new Player(LoginService.SteamId.GetAccountID().m_AccountID) {Name = persona};

            var replicator = new ServerReplicator(connection, player);
            replicator.Parent = this;

            player.Parent = Game.Players;
            Game.Players.PlayerAdded.Fire(player);
        }

        internal void ConfigureAsTeamBuildServer(bool b)
        {
        }
    }
}
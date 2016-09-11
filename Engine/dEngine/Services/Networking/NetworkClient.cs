// NetworkClient.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using dEngine.Instances.Messages;
using dEngine.Settings.Global;
using Lidgren.Network;
using Steamworks;

namespace dEngine.Services.Networking
{
    /// <summary>
    /// Service for handling networking and replication on the client.
    /// </summary>
    [TypeId(76)]
    [ExplorerOrder(6)]
    public sealed class NetworkClient : NetworkPeer
    {
        private HAuthTicket _authTicket;
        private NetConnection _connection;
        private ClientReplicator _replicator;
        private NetClient _netClient;

        /// <inheritdoc />
        public NetworkClient()
        {
            Connected = new Signal<ClientReplicator>(this);
            Disconnected = new Signal<string>(this);
            Rejected = new Signal<string>(this);
        }

        /// <summary>
        /// The authorization ticket.
        /// </summary>
        [EditorVisible]
        public string Ticket => _authTicket.m_HAuthTicket.ToString();

        internal static object GetExisting()
        {
            return DataModel.GetService<NetworkClient>();
        }

        /// <summary>
        /// Sets up internal network client.
        /// </summary>
        public void Start(int port = 0)
        {
            _peerConfig = NetworkSettings.GetNetPeerConfig(this);
            _peerConfig.Port = port > 0 ? port : _peerConfig.Port;

            _netClient = new NetClient(_peerConfig);
            _peer = _netClient;

            _netClient.Start();

            IsRunning = true;
        }

        /// <summary>
        /// Creates a <see cref="Player" /> object and attempts to connect to the server.
        /// </summary>
        public void Connect(string serverIp, int serverPort)
        {
            if (NetworkServer.IsHost)
                throw new InvalidOperationException("Cannot connect to a server while hosting one.");

            if (_netClient == null)
                throw new InvalidOperationException("Start() must be called before Connect()");

            Logger.Trace($"NetworkClient conecting to {serverIp}:{serverPort}");

            // get an auth ticket from steam.
            var ticket = new byte[1024];
            uint pcbTicket;
            _authTicket = SteamUser.GetAuthSessionTicket(ticket, 1024, out pcbTicket);

            var approvalRequest = _netClient.CreateMessage();
            approvalRequest.Write(ticket);
            approvalRequest.Write(pcbTicket);
            approvalRequest.Write(SteamUser.GetSteamID().m_SteamID);

            _connection = _netClient.Connect(serverIp, serverPort, approvalRequest);
        }

        /// <summary>
        /// Disconnect this client from the server.
        /// </summary>
        public void Disconnect(string message = "Client was manually disconnected by server.")
        {
            _connection?.Disconnect(message);
        }

        internal override void ProcessMessages()
        {
            NetIncomingMessage msg;
            while ((msg = _netClient.ReadMessage()) != null)
                switch ((NetIncomingMessageType)msg.ReadByte())
                {
                    case NetIncomingMessageType.StatusChanged:
                        OnStatusChanged(msg);
                        break;
                }
        }

        private void SendMessage(IMessageHandler messageHandler, DeliveryMethod deliveryMethod)
        {
            var output = _netClient.CreateMessage();
            output.Write(messageHandler.MessageId);
            messageHandler.ServerWrite(output);
            _netClient.SendMessage(output, (NetDeliveryMethod)deliveryMethod);
        }

        private void OnStatusChanged(NetIncomingMessage msg)
        {
            switch ((NetConnectionStatus)msg.ReadByte())
            {
                case NetConnectionStatus.Connected:
                    Logger.Trace("Connected");
                    break;
                case NetConnectionStatus.Disconnected:
                    var reason = msg.ReadString();
                    Logger.Warn(string.IsNullOrEmpty(reason)
                        ? "Disconnected: no reason given."
                        : $"Disconnected, Reason: {reason}");
                    break;
            }
        }

        /// <summary>
        /// Fired when <see cref="Connect(string, int)" />successfully connects to a server. Returns the ClientReplicator.
        /// </summary>
        public readonly Signal<ClientReplicator> Connected;

        /// <summary>
        /// Fired if <see cref="Connect(string, int)" /> fails to connect to a server.
        /// </summary>
        public readonly Signal<string> Disconnected;

        /// <summary>
        /// Fired if <see cref="Connect(string, int)" /> connects to a server, but the request to join is rejected.
        /// </summary>
        /// <remarks>
        /// This can happen if the client and server versions are mismatched, the player is banned from server, or ticket
        /// authorization failed.
        /// </remarks>
        public readonly Signal<string> Rejected;
    }
}
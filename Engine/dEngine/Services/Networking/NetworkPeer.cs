// NetworkPeer.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using dEngine.Instances.Messages;
using dEngine.Serializer.V1;
using Lidgren.Network;

namespace dEngine.Services.Networking
{
    /// <summary>
    /// Base class for <see cref="NetworkClient" /> and <see cref="NetworkServer" />
    /// </summary>
    [TypeId(74)]
    public abstract class NetworkPeer : Service
    {
        internal static Dictionary<byte, IMessageHandler> _messageHandlers;
        internal NetPeer _peer;
        internal NetPeerConfiguration _peerConfig;

        static NetworkPeer()
        {
            _messageHandlers = new Dictionary<byte, IMessageHandler>();

            RegisterHandler<WorldUpdate>();
        }

        internal static void RegisterHandler<TMessageHandler>() where TMessageHandler : IMessageHandler, new()
        {
            var handler = new TMessageHandler();
            _messageHandlers.Add(handler.MessageId, handler);
        }

        /// <summary/>
        protected double _accumulator;

        /// <summary>
        /// If true, the peer has been started.
        /// </summary>
        [EditorVisible]
        public bool IsRunning { get; protected set; }

        internal static bool IsClient()
        {
            return (Engine.Mode == EngineMode.Game) || (Engine.Mode == EngineMode.LevelEditor);
        }

        internal static bool IsServer()
        {
            return (Engine.Mode == EngineMode.Server) || (Engine.Mode == EngineMode.LevelEditor);
        }

        internal abstract void ProcessMessages();

        /// <summary>
        /// Performs a network update.
        /// </summary>
        /// <param name="step">The time since the last step.</param>
        public void Update(double step)
        {
            if (!IsRunning)
                return;

            _accumulator += step;
            if (_accumulator >= step)
                if (IsRunning)
                    ProcessMessages();
        }
    }
}
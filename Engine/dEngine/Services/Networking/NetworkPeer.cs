// NetworkPeer.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;
using Lidgren.Network;

namespace dEngine.Services.Networking
{
    /// <summary>
    /// Base class for <see cref="NetworkClient" /> and <see cref="NetworkServer" />
    /// </summary>
    [TypeId(74)]
    public abstract class NetworkPeer : Service
    {
        internal NetPeer _peer;
        internal NetPeerConfiguration _peerConfig;

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
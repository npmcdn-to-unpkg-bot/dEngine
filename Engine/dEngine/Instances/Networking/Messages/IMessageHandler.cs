// IMessage.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using Lidgren.Network;

namespace dEngine.Instances.Messages
{
    /// <summary>
    /// Interface for network messages.
    /// </summary>
    public interface IMessageHandler
    {
        byte MessageId { get; }
        /// <summary>
        /// A method invoked on the client which writes data to a message which will be send to the server.
        /// </summary>
        void ClientWrite(NetOutgoingMessage msg);
        /// <summary>
        /// A method invoked on the server which writes data to a message which will be send to the client.
        /// </summary>
        void ServerWrite(NetOutgoingMessage msg);
        /// <summary>
        /// A method invoked on the client which reads data from a message which was send from the server.
        /// </summary>
        void ClientRead(NetOutgoingMessage msg);
        /// <summary>
        /// A method invoked on the server which reads data from a message which was send from the client.
        /// </summary>
        void ServerRead(NetOutgoingMessage msg);
    }
}
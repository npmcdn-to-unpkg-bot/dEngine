// RemoteFunctionReturn.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using Lidgren.Network;

namespace dEngine.Instances.Messages
{
    internal class RemoteFunctionReturn : IMessageHandler
    {
        public byte MessageId => 7;

        public void ClientWrite(NetOutgoingMessage msg)
        {
            throw new System.NotImplementedException();
        }

        public void ServerWrite(NetOutgoingMessage msg)
        {
            throw new System.NotImplementedException();
        }

        public void ClientRead(NetOutgoingMessage msg)
        {
            throw new System.NotImplementedException();
        }

        public void ServerRead(NetOutgoingMessage msg)
        {
            throw new System.NotImplementedException();
        }
    }
}
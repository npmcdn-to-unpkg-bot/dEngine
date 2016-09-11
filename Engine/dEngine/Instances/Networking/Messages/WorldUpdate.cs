// WorldUpdate.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using dEngine.Instances.Attributes;
using Lidgren.Network;

namespace dEngine.Instances.Messages
{
    internal class WorldUpdate : IMessageHandler
    {
        public byte MessageId => 1;

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
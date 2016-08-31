﻿// InstanceReference.cs - dEngine
// Copyright (C) 2016-2016 DanDev (dandev.sco@gmail.com)
// 
// This library is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// You should have received a copy of the GNU Lesser General Public
// along with this program. If not, see <http://www.gnu.org/licenses/>.

using System.IO;
using dEngine.Instances;

namespace dEngine
{
    /// <summary>
    /// A reference to an instance for networking.
    /// </summary>
    public struct InstanceReference : IDataType
    {
        /// <summary>
        /// The id of the instance to reference.
        /// </summary>
        [InstMember(1)] public string InstanceId;

        internal InstanceReference(Instance instance)
        {
            InstanceId = instance.InstanceId;
        }

        public void Load(BinaryReader reader)
        {
            InstanceId = reader.ReadString();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(InstanceId);
        }
    }
}
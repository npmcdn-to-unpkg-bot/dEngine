// DataStoreService.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using dEngine.Instances.DataStores;

#pragma warning disable 1591

namespace dEngine.Services
{
    /// <summary>
    /// A service for managing DataStores.
    /// </summary>
    [TypeId(57)]
    public class DataStoreService : Service
    {
        internal static DataStoreService Service;

        /// <inheritdoc />
        public DataStoreService()
        {
            Service = this;
        }

        public GlobalDataStore GetDataStore(string name, string scope = "global")
        {
            throw new NotImplementedException();
        }

        public GlobalDataStore GetGlobalDataStore()
        {
            throw new NotImplementedException();
        }

        public OrderedDataStore GetOrderedDataStore()
        {
            throw new NotImplementedException();
        }

        internal static object GetExisting()
        {
            return DataModel.GetService<DataStoreService>();
        }
    }
}
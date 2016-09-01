// GlobalDataStore.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances.Attributes;
using dEngine.Services;

#pragma warning disable 1591

namespace dEngine.Instances
{
    /// <summary>
    /// A GlobalDataStore allows manipulation of a data base.
    /// </summary>
    [Uncreatable]
    [TypeId(71)]
    public class GlobalDataStore : Instance
    {
        /// <inheritdoc />
        public GlobalDataStore()
        {
            Parent = DataStoreService.Service;
            ParentLocked = true;
            Archivable = false;
        }

        public object GetAsync(string key)
        {
            throw new NotImplementedException("DataStores not implemented.");
        }

        public object IncrementAsync(string key, int delta = 1)
        {
            throw new NotImplementedException("DataStores not implemented.");
        }

        public void SetAsync(string key, object value)
        {
            throw new NotImplementedException("DataStores not implemented.");
        }

        public void UpdateAsync(string key, Func<object, object> transformer)
        {
            throw new NotImplementedException("DataStores not implemented.");
        }
    }
}
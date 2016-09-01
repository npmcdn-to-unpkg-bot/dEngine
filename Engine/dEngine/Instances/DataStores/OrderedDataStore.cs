// OrderedDataStore.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using dEngine.Instances.Attributes;

namespace dEngine.Instances.DataStores
{
    /// <summary>
    /// A type of DataStore where the value must be a positive integer.
    /// </summary>
    [TypeId(73)]
    public class OrderedDataStore : GlobalDataStore
    {
    }
}
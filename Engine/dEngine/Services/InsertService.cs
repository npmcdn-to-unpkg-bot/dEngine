// InsertService.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using dEngine.Serializer.V1;

namespace dEngine.Services
{
    /// <summary>
    /// Service for inserting assets into the game.
    /// </summary>
    [TypeId(170)]
    [ExplorerOrder(-1)]
    public partial class InsertService : Service
    {
        /// <inheritdoc />
        internal InsertService()
        {
            Service = this;
        }

        internal static object GetExisting()
        {
            return DataModel.GetService<InsertService>();
        }

        /// <summary>
        /// Loads an asset from a content url.
        /// </summary>
        public Instance LoadAsset(string contentId)
        {
            using (var stream = ContentProvider.DownloadStream(new Uri(contentId)).Result)
            {
                if (stream == null)
                    throw new InvalidOperationException($"Failed to load asset: could not fetch. ({contentId})");

                if (Inst.CheckHeader(stream))
                    return Inst.Deserialize(stream, skipMagic: true);

                throw new NotSupportedException("Downloaded data did not begin with \"INSTBIN\".");
            }
        }

        /// <summary>
        /// Loads an asset from the Steam Workshop.
        /// </summary>
        /// <param name="workshopId">The workshop ID.</param>
        public Instance LoadAsset(uint workshopId)
        {
            throw new NotImplementedException();
        }
    }

    public partial class InsertService
    {
        // static methods
        internal static InsertService Service;
    }
}
﻿// CacheableContentProvider.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using dEngine.Data;
using dEngine.Instances.Materials;
using dEngine.Settings.Global;
using dEngine.Utility;

namespace dEngine.Services
{
    /// <summary>
    /// A resource manager for asset types.
    /// </summary>
    public static class CacheableContentProvider<TAsset> where TAsset : AssetBase, new()
    {
        private static readonly ConcurrentDictionary<Uri, Resource> _resources;

        /// <summary>
        /// Determines if this content provider can download content.
        /// </summary>
        /// <remarks>
        /// If the engine is in server mode, or <see cref="RenderSettings.GraphicsMode" /> is set to
        /// <see cref="GraphicsMode.NoGraphics" />, it will not download certain types of assets.
        /// </remarks>
        /// <seealso cref="Engine.Mode" />
        // ReSharper disable once StaticMemberInGenericType
        internal static bool CanDownload = true;

        static CacheableContentProvider()
        {
            _resources = new ConcurrentDictionary<Uri, Resource>();

            var assetType = typeof(TAsset);

            // prevent non-graphics app from downloading unnecessary assets.
            if (RenderSettings.GraphicsMode == GraphicsMode.NoGraphics)
                if ((assetType == typeof(Texture)) || (assetType == typeof(Material)) || (assetType == typeof(Cubemap)) ||
                    (assetType == typeof(AnimationData)))
                    CanDownload = false;

            if (Engine.Mode == EngineMode.Server) // prevent server from downloading unnecessary assets.
                if (assetType == typeof(AudioData))
                    CanDownload = false;
        }

        /// <summary>
        /// Clears the resource cache.
        /// </summary>
        public static void ClearResources()
        {
            _resources.Clear();
        }

        private static void RemoveResource(Resource resource)
        {
            if (resource.Uri != null)
                _resources.TryRemove(resource.Uri);
        }

        /// <summary>
        /// Deserializes the given stream into an asset.
        /// </summary>
        private static TAsset GetAssetFromStream(Stream stream)
        {
            if (stream == null) return null;
            var asset = new TAsset();
            asset.Load(stream);
            return asset;
        }

        /// <summary>
        /// Redownloads the resource for the given content ID.
        /// </summary>
        public static void RefreshResource(Uri uri)
        {
            Resource resource;
            if (_resources.TryGetValue(uri, out resource))
                resource.Download();
        }

        internal static Reference<TAsset> EmptyReference()
        {
            var resource = Resource.Empty;
            return new Reference<TAsset>(resource);
        }

        /// <summary>
        /// Returns a reference without waiting for the resource to load.
        /// </summary>
        internal static Reference<TAsset> GetAsync(Uri contentId)
        {
            Resource resource;
            if (!_resources.TryGetValue(contentId, out resource))
                _resources[contentId] = resource = new Resource(contentId);
            return new Reference<TAsset>(resource);
        }

        /// <summary>
        /// Returns a reference after waiting for the resource to load.
        /// </summary>
        internal static Reference<TAsset> Get(Uri contentId)
        {
            var reference = GetAsync(contentId);
            reference.Resource.WaitAsync().Wait();
            return reference;
        }


        internal static void Cache(Uri uri, Stream stream)
        {
            _resources[uri] = new Resource(uri);
        }

        internal class Resource : IDisposable
        {
            private bool _disposed;
            private TaskCompletionSource<bool> _downloadCompletionSource;
            private int _referenceCount;

            private Resource()
            {
            }

            /// <summary>
            /// Creates a new resource.
            /// </summary>
            /// <param name="contentUri">The content id url.</param>
            public Resource(Uri contentUri)
            {
                Uri = contentUri;
                Download();
            }

            public Uri Uri { get; }
            public TAsset Content { get; private set; }

            public int ReferenceCount
            {
                get { return _referenceCount; }
                set
                {
                    if (value == _referenceCount) return;
                    _referenceCount = value;
                    if (value == 0)
                        Dispose(true);
                }
            }

            public bool IsDownloaded { get; private set; }

            public static Resource Empty => new Resource();

            /// <summary>
            /// Disposes of the resource.
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            public event Action Downloaded;

            ~Resource()
            {
                Dispose(false);
            }

            /// <summary>
            /// Waits for the resource to be downloaded.
            /// </summary>
            public async Task WaitAsync()
            {
                await _downloadCompletionSource.Task.ConfigureAwait(false);
            }

            public static implicit operator TAsset(Resource resource)
            {
                return resource?.Content;
            }

            /// <summary>
            /// Disposes of the object.
            /// </summary>
            /// <param name="disposing">True if Dispose() was invoked, False if destructor was invoked.</param>
            protected virtual void Dispose(bool disposing)
            {
                if (_disposed)
                    return;

                if (disposing)
                    Content?.Dispose();

                Content = null;
                ReferenceCount = 0;
                RemoveResource(this);

                _disposed = true;
            }

            public void Download()
            {
                if (!CanDownload)
                    return;

                IsDownloaded = false;

                _downloadCompletionSource = new TaskCompletionSource<bool>();
                ContentProvider.DownloadStream(Uri)
                    .ContinueWith(t =>
                    {
                        Content = GetAssetFromStream(t.Result);
                        _downloadCompletionSource.SetResult(true);
                        IsDownloaded = true;
                        Downloaded?.Invoke();
                    }, TaskContinuationOptions.None);
            }
        }
    }

    /// <summary>
    /// A reference to a resource.
    /// </summary>
    public class Reference<TAsset> : IDisposable where TAsset : AssetBase, new()
    {
        private bool _disposed;

        internal Reference(CacheableContentProvider<TAsset>.Resource resource)
        {
            Resource = resource;
            resource.ReferenceCount++;
        }

        internal CacheableContentProvider<TAsset>.Resource Resource { get; }

        /// <summary>
        /// Decrements the reference count.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary />
        ~Reference()
        {
            Dispose(false);
        }

        /// <summary>
        /// Implicitly returns the content object.
        /// </summary>
        public static implicit operator TAsset(Reference<TAsset> reference)
        {
            return reference?.Resource?.Content;
        }

        /// <summary>
        /// Disposes of the object.
        /// </summary>
        /// <param name="disposing">True if Dispose() was invoked, False if destructor was invoked.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            Resource.ReferenceCount--;

            _disposed = true;
        }
    }
}
﻿// Content.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Data;
using dEngine.Services;

namespace dEngine
{
    /// <summary>
    /// A reference to a content resource.
    /// </summary>
    public class Content<TAsset> : IEquatable<Content<TAsset>>, IEquatable<string>
        where TAsset : AssetBase, new()
    {
        private string _contentId = "";

        /// <summary>
        /// Fired when the new content is fetched.
        /// </summary>
        private Action<string, TAsset> _onGot;

        private Reference<TAsset> _reference;

        private Content()
        {
        }

        /// <summary>
        /// Creates a new content object with optional default uri.
        /// </summary>
        public Content(string contentId = "")
        {
            ContentId = contentId;
        }

        /// <summary>
        /// The asset.
        /// </summary>
        public TAsset Asset => _reference;

        /// <summary>
        /// The content URL to download from. When set will delete current data and start downloading.
        /// </summary>
        [InstMember(1)]
        public string ContentId
        {
            get { return _contentId; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    value = "";

                _contentId = value;

                if (string.IsNullOrWhiteSpace(value))
                {
                    _reference = CacheableContentProvider<TAsset>.EmptyReference();
                    OnResourceDownloaded();
                }
                else
                {
                    _reference = CacheableContentProvider<TAsset>.GetAsync(new Uri(value));

                    if (_reference.Resource.IsDownloaded)
                    {
                        OnResourceDownloaded();

                        if (_reference.Resource.Content == null)
                            _reference.Resource.Download();
                    }
                    else
                        _reference.Resource.Downloaded += OnResourceDownloaded;
                }
            }
        }

        /// <summary>
        /// If true, Data exists and no download is happening.
        /// </summary>
        public bool IsLoaded { get; set; }

        /// <summary>
        /// Determines if two Contents have equal content IDs.
        /// </summary>
        public bool Equals(Content<TAsset> other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(_contentId, other._contentId);
        }

        /// <summary>
        /// Determines if this Content has the given content id.
        /// </summary>
        public bool Equals(string contentId)
        {
            return string.Equals(_contentId, contentId);
        }

        /// <inheritdoc />
        ~Content()
        {
            _reference = null;
            Unsubscribe();
        }

        /// <summary>
        /// Sets the given object as the content owner, and subscribes with a callback.
        /// </summary>
        public void Subscribe(object owner, Action<string, TAsset> callback = null)
        {
            _onGot = callback;

            if (IsLoaded)
                callback?.Invoke(_contentId, Asset);
        }

        /// <inheritdoc />
        public void Unsubscribe()
        {
            _onGot = null;
        }

        private void OnResourceDownloaded()
        {
            _onGot?.Invoke(_reference.Resource.Uri.AbsoluteUri, _reference);
            IsLoaded = true;
        }

        /// <summary />
        public override string ToString()
        {
            return _contentId;
        }

        /// <summary>
        /// Returns <see cref="ContentId" />.
        /// </summary>
        public static implicit operator string(Content<TAsset> content)
        {
            return content?.ContentId;
        }

        /// <summary>
        /// Returns a <see cref="Content{TAsset}" /> using the given content id.
        /// </summary>
        public static implicit operator Content<TAsset>(string content)
        {
            return new Content<TAsset>(content);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Content<TAsset>)obj);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return _contentId.GetHashCode();
        }

        /// <summary/>
        public void RetryDownload()
        {
            ContentId = _contentId;
        }
    }
}
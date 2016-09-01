// SourceContainer.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using dEngine.Data;
using dEngine.Instances.Attributes;

namespace dEngine.Instances
{
    /// <summary>
    /// Base class for an object containing source code.
    /// </summary>
    [TypeId(59)]
    public abstract class SourceContainer : Instance
    {
        private Content<TextSource> _linkedSource;

        private string _source;

        /// <inheritdoc />
        protected SourceContainer()
        {
            _linkedSource = new Content<TextSource>();

            Downloaded = new Signal<string>(this);
        }

        /// <summary>
        /// The source code.
        /// </summary>
        [InstMember(1)]
        public string Source
        {
            get { return _source; }
            set
            {
                if (!string.IsNullOrEmpty(LinkedSource) && !_deserializing)
                    Logger.Warn($"{GetFullName()}.SourceCode could not be set because LinkedSource is being used.");
                else
                {
                    FilterString(ref value);
                    _source = value;
                    LastModified = DateTime.UtcNow();
                }
            }
        }

        /// <summary>
        /// When set, will download and overwrite source code.
        /// </summary>
        [InstMember(2)]
        [EditorVisible]
        public Content<TextSource> LinkedSource
        {
            get { return _linkedSource.ContentId; }
            set
            {
                _linkedSource = value;
                value.Subscribe(this, OnLinkedSourceFetched);
                NotifyChanged();
            }
        }

        /// <summary>
        /// Gets the time the source was last modified.
        /// </summary>
        [InstMember(3)]
        public DateTime LastModified { get; internal set; }

        private static void FilterString(ref string source)
        {
            source = source.Trim('\uFEFF', '\u200B');
        }

        private void OnLinkedSourceFetched(string uri, TextSource sourceCode)
        {
            if (sourceCode == null)
                return;

            try
            {
                var source = sourceCode.Text;
                FilterString(ref source);
                _source = source;
                Downloaded.Fire(uri);
            }
            catch (Exception e)
            {
                var msg = $"{GetFullName()}.LinkedSource - error reading data from \"{uri}.\"";
                Logger.Warn(msg);
                _source = $"An exception occured when reading data from {uri}. \n{e.Message}.\n{e.Source}";
            }
        }

        /// <summary>
        /// Fired when the source code is downloaded from <see cref="LinkedSource" />.
        /// </summary>
        public readonly Signal<string> Downloaded;
    }
}
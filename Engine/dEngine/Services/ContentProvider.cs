// ContentProvider.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using dEngine.Data;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using dEngine.Utility.Extensions;
using Dynamitey;
using Neo.IronLua;

namespace dEngine.Services
{
    /// <summary>
    /// A service for managing content.
    /// </summary>
    [TypeId(8)]
    [ExplorerOrder(-1)]
    public partial class ContentProvider : Service
    {
        internal const int TimeoutMiliseconds = 30*1000;

        /// <summary>Image Format string for file dialogs.</summary>
        public const string SupportedImageFormats =
            "Supported Formats (*.png;*.tiff;*.bmp*.jpg;*.jpeg;*.gif)|*.png;*.tiff;*.bmp*.jpg;*.jpeg;*.gif|All files (*.*)|*.*";

        /// <summary>Audio Format string for file dialogs.</summary>
        public const string SupportedAudioFormats =
            "Supported Formats (*.mp3;*.mpeg3;*.wav;*.wave;*.flac;*.fla;*.aiff;*aif;*.aifc;*.aac;*.adt;*.adts;*.m2ts;*.mp2;*.3g2;*.3gp2;*.m4a;*.mp4v;*.mp4v;*.mp4;*.mov;*.asf;*.wm;*.wmv;*.wma;*.mp1;*.avi*.ac3;*.ec3;*.cue;*.m3u;*.pls)|*.mp3;*.mpeg3;*.wav;*.wave;*.flac;*.fla;*.aiff;*aif;*.aifc;*.aac;*.adt;*.adts;*.m2ts;*.mp2;*.3g2;*.3gp2;*.m4a;*.mp4v;*.mp4v;*.mp4;*.mov;*.asf;*.wm;*.wmv;*.wma;*.mp1;*.avi*.ac3;*.ec3;*.cue;*.m3u;*.pls;|All files (*.*)|*.*";

        /// <summary>RenderMesh Format string for file dialogs.</summary>
        public const string SupportedMeshFormats =
            "Supported Formats (*.3d;*.3ds;*.ac;*.ac3d;*.acc;*.ase;*.ask;*.b3d;*.blend;*.bvh;*.cob;*.csm;*.dae;*.dxf;*.enff;*.fbx;*.hmp;*.ifc;*.ifczip;*.irr;*.irrmesh;*.lwo;*.lws;*.lxo;*.md2;*.md3;*.md5anim;*.md5camera;*.md5mesh;*.mdc;*.mdl;*.mesh;*.RenderMesh.xml;*.mot;*.ms3d;*.ndo;*.nff;*.obj;*.off;*.pk3;*.ply;*.prj;*.q3o;*.q3s;*.raw;*.scn;*.smd;*.stl;*.ter;*.uc;*.vta;*.x;*.xgl;*.xml;*.zgl;)|*.3d;*.3ds;*.ac;*.ac3d;*.acc;*.ase;*.ask;*.b3d;*.blend;*.bvh;*.cob;*.csm;*.dae;*.dxf;*.enff;*.fbx;*.hmp;*.ifc;*.ifczip;*.irr;*.irrmesh;*.lwo;*.lws;*.lxo;*.md2;*.md3;*.md5anim;*.md5camera;*.md5mesh;*.mdc;*.mdl;*.RenderMesh;*.RenderMesh.xml;*.mot;*.ms3d;*.ndo;*.nff;*.obj;*.off;*.pk3;*.ply;*.prj;*.q3o;*.q3s;*.raw;*.scn;*.smd;*.stl;*.ter;*.uc;*.vta;*.task;*.xgl;*.xml;*.zgl;|All files (*.*)|*.*";

        internal static ContentProvider Service;

        private static readonly IEnumerable<Type> _assetTypes = typeof(AssetBase).GetDescendantTypes();
        private static long _cacheMaxItemSize;
        private static Dictionary<string, IContentProtocol> _protocols;

        static ContentProvider()
        {
            LoggerInternal = LogService.GetLogger();
            RegisterProtocols();
        }

        /// <summary />
        public ContentProvider()
        {
            Service = this;
        }

        /// <summary>
        /// If an item exceeds this size, it will not be cached.
        /// </summary>
        [InstMember(1)]
        public int CacheMaxItemSize
        {
            get { return (int)_cacheMaxItemSize; }
            set { _cacheMaxItemSize = value; }
        }

        internal static ILogger LoggerInternal { get; }

        internal static string SoundcloudClientId { get; set; }

        internal static Dictionary<string, IContentProtocol> Protocols
            => _protocols ?? (_protocols = new Dictionary<string, IContentProtocol>());

        /// <summary>
        /// Sets the client ID for the Soundcloud API.
        /// </summary>
        [ScriptSecurity(ScriptIdentity.CoreScript | ScriptIdentity.Editor)]
        public void SetSoundcloudClientId(string clientId)
        {
            ScriptService.AssertIdentity(ScriptIdentity.CoreScript | ScriptIdentity.Editor);
            SoundcloudClientId = clientId;
        }

        /// <summary>
        /// Yields untill all the given content urls have been downloaded.
        /// </summary>
        /// <param name="contentUrls">A table of content URLs to download.</param>
        [YieldFunction]
        public void PreloadAsync(LuaTable contentUrls)
        {
            var thread = ScriptService.CurrentThread;
            ScriptService.YieldThread();

            var count = contentUrls.Length;
            var tasks = new Task[count];

            for (var i = 0; i < count; i++)
            {
                var contentUrl = new Uri((string)contentUrls[count]);
                tasks[i] = DownloadStream(contentUrl).ContinueWith(t => Cache(contentUrl, t));
            }

            Task.WhenAll(tasks).ContinueWith(t => ScriptService.ResumeThread(thread));
        }


        /// <summary>
        /// Preloads content from the given url.
        /// </summary>
        public void Preload(string contentId)
        {
            var uri = new Uri(contentId);
            DownloadStream(uri).ContinueWith(x => Cache(uri, x));
        }

        internal static void DeleteDirectory(string path)
        {
            foreach (var directory in Directory.GetDirectories(path))
                DeleteDirectory(directory);

            try
            {
                Directory.Delete(path, true);
            }
            catch (IOException)
            {
                Directory.Delete(path, true);
            }
            catch (UnauthorizedAccessException)
            {
                Directory.Delete(path, true);
            }
        }

        private static void Cache(Uri uri, Task<Stream> task)
        {
            var stream = task.Result;
            var contentType = AssetBase.PeekContent(task.Result);

            switch (contentType)
            {
                case ContentType.StaticMesh:
                case ContentType.SkeletalMesh:
                    CacheableContentProvider<Geometry>.Cache(uri, stream);
                    break;
                case ContentType.Video:
                case ContentType.Texture:
                    CacheableContentProvider<Texture>.Cache(uri, stream);
                    break;
                case ContentType.Sound:
                    CacheableContentProvider<AudioData>.Cache(uri, stream);
                    break;
                case ContentType.Animation:
                    CacheableContentProvider<AnimationData>.Cache(uri, stream);
                    break;
                case ContentType.Cubemap:
                    CacheableContentProvider<Cubemap>.Cache(uri, stream);
                    break;
            }
        }

        /// <summary>
        /// Returns a stream of data from the given content id.
        /// </summary>
        /// <param name="contentId">The content id url.</param>
        internal static string DownloadString(Uri contentId)
        {
            return DownloadStream(contentId).Result.ReadString();
        }

        /// <summary>
        /// Returns a stream of data from the given content id.
        /// </summary>
        /// <param name="contentId">The content URI.</param>
        internal static async Task<Stream> DownloadStream(Uri contentId)
        {
            return await Task.Factory.StartNew(() =>
            {
                Stream data;

                try
                {
                    IContentProtocol handler;
                    if (!Protocols.TryGetValue(contentId.Scheme, out handler))
                        throw new NotSupportedException($"No handler registered for protocol \"{contentId.Scheme}\"");

                    data = handler.Fetch(contentId);

                    if (data != null)
                    {
                        var mem = data as MemoryStream;

                        if (mem == null) // if returned data was not a MemoryStream, replace it with one.
                        {
                            using (data)
                            {
                                mem = new MemoryStream((int)data.Length);
                                data.Position = 0;
                                data.CopyTo(mem);
                            }
                            data = mem;
                        }

                        data.Position = 0;
                    }
                }
                catch (Exception e)
                {
                    LoggerInternal.Error($"Content fetch failed for \"{contentId}\": {e.Message}");
                    return null;
                }

                return data;
            }).ConfigureAwait(false);
        }

        internal static object GetExisting()
        {
            return DataModel.GetService<ContentProvider>();
        }

        /// <summary>
        /// Forces a cached resource to be redownloaded.
        /// </summary>
        /// <param name="resourceName"></param>
        public void RefreshResource(string resourceName)
        {
            var genericType = typeof(CacheableContentProvider<>);
            foreach (var del in from assetType in _assetTypes
                where !assetType.IsAbstract
                select genericType.MakeGenericType(assetType)
                into type
                select type.GetMethod("RefreshResource")
                into method
                select method.CreateDelegate(typeof(Action<string>)))
                del.FastDynamicInvoke(resourceName);
        }

        private static Stream GetEmbeddedFile(string resource)
        {
            resource = resource.Replace("/", "."); // remove leading slash if exists
            resource = resource.Replace("content.", "");
            resource = resource.ToLower();
            var assembly = Assembly.GetExecutingAssembly();

            var resourceNames = assembly.GetManifestResourceNames();

            return (from name in resourceNames
                let matchName = name.ToLower().Replace("dengine.content.", "")
                where matchName == resource
                select assembly.GetManifestResourceStream(name)).FirstOrDefault();
        }

        internal static bool ValidateUri(string uri)
        {
            Uri _;
            return Uri.TryCreate(uri, UriKind.Absolute, out _);
        }
    }
}
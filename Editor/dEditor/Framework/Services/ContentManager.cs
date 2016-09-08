// ContentManager.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using dEditor.Framework.Content;
using dEditor.Framework.Content.Protocols;
using dEngine.Data;
using dEngine.Services;
using MessageBox = System.Windows.MessageBox;

namespace dEditor.Framework.Services
{
    public static class ContentManager
    {
        public static Dictionary<string, string[]> Formats = new Dictionary<string, string[]>
        {
            {"Mesh", new[] {".fbx", ".open", ".dae", ".3ds", ".obj"}},
            {"Sound", new[] {".mp3", ".wav", ".ogg", ".opus", ".flac", ".m4a"}},
            {"Texture", new[] {".png", ".dds", ".bmp", ".gif", ".webp", ".pdn"}},
            {"Video", new[] {".mp4", ".wmv", ".webm"}},
            {"Lua Script", new[] {".lua"}},
        };
        
        public static void ConvertPrimitivesToGeometry()
        {
            const string path = "D:\\Cloud Storage\\Google Drive\\dEngine\\Primitives";
            var files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                Import(file, $@"C:\Users\Dan\Source\Repos\dEngine\Engine\dEngine\Content\Meshes\primitives\", true);
            }
        }

        public static ImportContext Import(string file, string directory, bool skipDialog = false)
        {
            if (!File.Exists(file))
                throw new ArgumentNullException($"File \"{file}\" does not exist.");
            var context = new ImportContext(file, directory, skipDialog);
            return context;
        }

        public static void Import(string[] files, string directory)
        {
            foreach (var file in files)
                Import(file, directory);
        }

        public class ImportContext
        {
            public readonly bool SkipDialog;
            public readonly string OutputDirectory;
            public readonly string Name;
            public readonly string Extension;
            public readonly Stream Stream;
            public readonly List<string> Results;

            public ImportContext(string file, string outputDirectory, bool skipDialog = false)
            {
                OutputDirectory = outputDirectory;
                Name = Path.GetFileNameWithoutExtension(file);
                Extension = Path.GetExtension(file);
                Stream = File.OpenRead(file);
                SkipDialog = skipDialog;
                Results = new List<string>();

                try
                {
                    switch (Extension)
                    {
                        case ".opex":
                            throw new NotImplementedException("OPEX mesh format not implemented.");
                        case ".fbx":
                        case ".dae":
                        case ".3ds":
                        case ".obj":
                            MeshImporter.Import(this);
                            break;
                        case ".mp3":
                        case ".wav":
                        case ".ogg":
                        case ".opus":
                        case ".flac":
                        case ".m4a":
                            AudioImporter.Import(this);
                            break;
                        case ".png":
                        case ".dds":
                        case ".bmp":
                        case ".gif":
                        case ".webp":
                            TextureImporter.Import(this);
                            break;
                        case ".pdn":
                            throw new NotImplementedException("Paint.NET texture format not implemented.");
                        case ".mp4":
                        case ".wmv":
                        case ".webm":
                            VideoImporter.Import(this);
                            break;
                        case ".lua":
                            ImportScript();
                            break;
                        default:
                            throw new NotSupportedException($"The format {Extension} is not supported.");
                    }
                }
#if !DEBUG
                catch (Exception e)
                {
                    var msg = $"Could not import asset \"{file}\":\n {e.Message}";
                    IoC.Get<IStatusBar>().Text = "Asset import failed.";
                    Editor.Logger.Error(e, msg);
                    MessageBox.Show(msg, "dEditor", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw;
                }
#endif
                finally
                {
                    Stream.Dispose();
                    Stream = null;
                }
            }

            private Stream CreateFile()
            {
                var file = File.Create(Path.Combine(OutputDirectory, Name + Extension));
                return file;
            }

            private void ImportScript()
            {
                using (var output = CreateFile())
                {
                    var data = new TextSource();
                    data.Load(Stream);
                    data.Save(output);
                }
            }

            private void ImportVideo()
            {
                throw new NotImplementedException();
            }

            private void ImportTexture()
            {
                using (var output = CreateFile())
                {
                    var data = new Texture();
                    data.Load(Stream);
                    data.Save(output);
                }
            }

            private void ImportAudio()
            {
                using (var output = CreateFile())
                {
                    var data = new AudioData();
                    data.Load(Stream);
                    data.Save(output);
                }
            }
        }

        public static bool CreateFile(ImportContext context, string fileName, string extension, out Stream stream)
        {
            stream = null;
            var path = $"{context.OutputDirectory}/{fileName}.{extension}";

            if (File.Exists(path))
            {
                var result =
                    System.Windows.Forms.MessageBox.Show(
                        $"An asset already exists at the import location: {path}\nOverwrite?",
                        "Overwrite File", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);

                if (result != DialogResult.Yes)
                {
                    return false;
                }
            }

            context.Results.Add(path);
            stream = File.Create(path);
            return true;
        }

        public static void RegisterProtocols()
        {
            ContentProvider.Protocols["content"] = new ContentProtocol();
            ContentProvider.Protocols["editor"] = new EditorProtocol();
            ContentProvider.Protocols["place"] = new PlaceProtocol();
        }
    }
}
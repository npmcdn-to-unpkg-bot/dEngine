// Project.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Xml.Serialization;
using Caliburn.Micro;
using dEditor.Modules.Shell.StatusBar;
using dEditor.Modules.Widgets.Viewport;
using dEngine;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using dEngine.Serializer.V1;
using dEngine.Services;
using DateTime = System.DateTime;
using MessageBox = System.Windows.Forms.MessageBox;

// ReSharper disable UnusedVariable

namespace dEditor.Framework
{
    [Serializable]
    [TypeId(2144)]
    public class TeamProject : Project
    {
    }

    [TypeId(2143)]
    public class Place
    {
        public string Name { get; set; }
        public string File { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public ImageSource Thumbnail { get; set; }
        public bool IsStartupPlace => Project.Current.StartupPlace == Name;
        public bool IsOpen => Game.Workspace.PlaceId == Name;
    }

    [Serializable]
    [TypeId(2142)]
    public class Project : PropertyChangedBase
    {
        private string _startupPlace;
        private ViewportViewModel _viewportVm;
        private List<Place> _places;

        public Project()
        {
            AppId = 480;
        }

        public List<Place> Places
        {
            get { return _places; }
            set
            {
                if (Equals(value, _places)) return;
                _places = value;
                NotifyOfPropertyChange();
            }
        }

        [XmlIgnore]
        public string ProjectPath { get; private set; }

        [XmlIgnore]
        public string ProjectFile { get; private set; }

        /// <summary>
        /// Determines if the project is currently loaded.
        /// </summary>
        [XmlIgnore]
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Determines if the project was created this session.
        /// </summary>
        [XmlIgnore]
        public bool IsNew { get; set; }

        /// <summary>
        /// Gets the currently loaded project. (null if no project is loaded)
        /// </summary>
        public static Project Current => Editor.Current.Project;

        /// <summary>
        /// Gets the path to the project's content folder.
        /// </summary>
        public string ContentPath => Path.Combine(ProjectPath, "Content");

        /// <summary>
        /// Fired when <see cref="StartupPlace" /> is changed.
        /// </summary>
        public event Action<string> StartupPlaceChanged;

        // TODO: add a file watcher
        private void UpdatePlaces()
        {
            Places.Clear();

            foreach (var file in Directory.GetFiles(Path.Combine(ProjectPath, "Places"), "*.place"))
            {
                Places.Add(LoadPlaceItem(file));
            }
        }

        private static Place LoadPlaceItem(string placeFile)
        {
            using (var stream = File.OpenRead(placeFile))
            {
                var meta = Inst.ReadMeta(stream);

                var place = new Place
                {
                    Name = Path.GetFileNameWithoutExtension(placeFile),
                    Description = meta["Description"],
                    File = placeFile,
                    Created = File.GetCreationTimeUtc(placeFile),
                    Modified = File.GetLastWriteTimeUtc(placeFile)
                };

                return place;
            }
        }

        /// <summary>
        /// Initializes a project object from the given project file.
        /// </summary>
        /// <seealso cref="Open"/>
        public static Project Load(string projFile)
        {
            if (!File.Exists(projFile))
                throw new FileNotFoundException("Project file not found.", projFile);

            using (var reader = new StreamReader(projFile))
            {
                var serializer = new XmlSerializer(typeof(Project));

                var project = (Project)serializer.Deserialize(reader);
                project.ProjectFile = projFile;
                project.ProjectPath = Path.GetDirectoryName(projFile);
                project.UpdatePlaces();

                return project;
            }
        }

        /// <summary>
        /// Creates a new project from scratch and opens it.
        /// </summary>
        /// <remarks>
        /// A project directory should look like the following:
        /// MyProject
        /// --Content
        /// -----Animations
        /// -----Audios
        /// -----Models
        /// -----Images
        /// --Places
        /// ------default.place
        /// ------place0.place
        /// ------place1.place
        /// --MyProject.game
        /// --MyProject.dproj
        /// --MyProject.ico (optional)
        /// </remarks>
        public static Project Create(string name, string directory)
        {
            var rootPath = Path.Combine(directory, name);
            var contentPath = Path.Combine(rootPath, "Content");
            var placePath = Path.Combine(rootPath, "Places");

            if (Directory.Exists(rootPath))
                throw new InvalidOperationException($"The directory \"{directory}/{name}\" already exists.");

            Directory.CreateDirectory(rootPath);
            Directory.CreateDirectory(contentPath);
            Directory.CreateDirectory(placePath);

            var projFile = Path.Combine(rootPath, $"{name}.dproj");
            File.Create(projFile).Close();

            var gameFile = Path.Combine(rootPath, $"{name}.game");
            File.Create(gameFile).Close();

            var proj = new Project
            {
                Name = name,
                CompanyName = Environment.UserName,
                ProjectPath = rootPath,
                ProjectFile = projFile,
                StartupPlace = "Frontend",
                IsNew = true
            };

            Editor.Current.Project = proj;
            Game.Workspace.PlaceId = proj.StartupPlace;

            GenerateFrontend();
            
            proj.Save();
            proj.Open();


            return proj;
        }

        private static void GenerateFrontend()
        {
            var baseplate = new Part
            {
                Anchored = true,
                Size = new Vector3(512, 10, 512),
                Position = new Vector3(0, -10, 0),
                Parent = Game.Workspace,
                BrickColour = new Colour(0.388235f, 0.372549f, 0.384314f)
            };

            // TODO: generate a main menu
        }

        /// <summary>
        /// Saves the loaded project.
        /// </summary>
        public void Save(bool saveGame = true)
        {
            if (RunService.SimulationState != SimulationState.Stopped)
                throw new InvalidOperationException("Cannot save while simulation is running.");

            if (!IsOpen && !IsNew)
                throw new InvalidOperationException("Attempt to save unloaded project.");

            var projSerializer = new XmlSerializer(typeof(Project));
            using (var stream = File.Open(ProjectFile, FileMode.Truncate))
            {
                projSerializer.Serialize(stream, this);
            }

            if (saveGame)
            {
                DataModel.Save(SaveFilter.SaveGame);
                DataModel.Save(SaveFilter.SaveWorld);
            }
        }

        /// <summary>
        /// Loads the game and startup place.
        /// </summary>
        public void Open()
        {
            var statusBar = IoC.Get<IStatusBar>();
            statusBar.Text = $"Opening {Name}.dproj";
            statusBar.IsFrozen = true;

            IsOpen = true;

            Editor.Current.Shell.Viewport?.TryClose();
            Editor.Current.Project = this;

            try
            {
                var gameFile = Path.Combine(ProjectPath, $"{Name}.game");

                if (!IsNew)
                    using (var gameStream = File.OpenRead(gameFile))
                    {
                        Inst.Deserialize(gameStream, Game.DataModel);
                    }

                Game.Workspace.LoadPlace(StartupPlace);
            }
            catch (Exception e)
            {
                statusBar.Text = $"Project could not be opened. ({e.GetType().Name})";
                Editor.Logger.Error(e);
                MessageBox.Show($"Could not load project:\n\n{e.Message}", "Open Project", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
                return;
            }
            finally
            {
                statusBar.IsFrozen = false;
            }

            Editor.Current.Shell.Viewport = _viewportVm = new ViewportViewModel();
            Editor.Current.Shell.OpenDocument(_viewportVm);
            var loginService = DataModel.GetService<LoginService>();
            if (!loginService.IsLoggedIn())
                loginService.TryLogin();
        }

        /// <summary>
        /// Closes the project.
        /// </summary>
        public void Close()
        {
            if (!IsOpen) return;
            IsOpen = false;

            Game.DataModel.ClearChildren();
            Game.Workspace.LoadPlace(null);

            _viewportVm?.TryClose();

            Editor.Current.Project = null;

            LoginService.Service.Logout();
        }

        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Renames the project and its related files.
        /// </summary>
        /// <param name="newName"></param>
        public void Rename(string newName)
        {
            if (newName == Name)
                return;

            throw new NotImplementedException();
        }


        public static string ToContentPath(string folder)
        {
            var pathUri = new Uri(Current.ContentPath);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
                folder += Path.DirectorySeparatorChar;
            var folderUri = new Uri(folder);
            var uri =
                Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri)
                    .ToString()
                    .Replace('/', Path.DirectorySeparatorChar));
            return $"content://{uri}";
        }

        #region XML Properties

        /// <summary>
        /// The author of the project.
        /// </summary>
        [XmlElement("CompanyName")]
        [EditorVisible("Publisher")]
        public string CompanyName { get; set; }

        /// <summary>
        /// The project homepage.
        /// </summary>
        [XmlElement("Homepage")]
        [EditorVisible("Publisher")]
        public string Homepage { get; set; }

        /// <summary>
        /// The project contact information.
        /// </summary>
        [XmlElement("SupportContact")]
        [EditorVisible("Publisher")]
        public string SupportContact { get; set; }

        /// <summary>
        /// The project thumbnail.
        /// </summary>
        [XmlElement("ProjectThumbnail")]
        [EditorVisible("About")]
        [ContentId(ContentType.Texture)]
        public string Thumbnail { get; set; }

        /// <summary>
        /// The project description.
        /// </summary>
        [XmlElement("Description")]
        [EditorVisible("About")]
        public string Description { get; set; }

        /// <summary>
        /// The project name.
        /// </summary>
        [XmlElement("Name")]
        [EditorVisible("About")]
        public string Name { get; set; }

        /// <summary>
        /// The project version.
        /// </summary>
        [XmlElement("Version")]
        [EditorVisible("About")]
        public string Version { get; set; }

        /// <summary>
        /// The copyright/trademark notices.
        /// </summary>
        [XmlElement("CopyrightNotice")]
        [EditorVisible("Legal")]
        public string CopyrightNotice { get; set; }

        /// <summary>
        /// The licensing terms.
        /// </summary>
        [XmlElement("LicenseTerms")]
        [EditorVisible("Legal")]
        public string LicenseTerms { get; set; }

        /// <summary>
        /// A relative path to startup place.
        /// </summary>
        [XmlElement("StartupPlace")]
        public string StartupPlace
        {
            get { return _startupPlace; }
            set
            {
                _startupPlace = value;
                StartupPlaceChanged?.Invoke(value);
            }
        }

        /// <summary>
        /// The Steam App ID for this project.
        /// </summary>
        [XmlElement("AppId")]
        [EditorVisible("Steam")]
        public uint AppId { get; set; }

        #endregion
    }
}
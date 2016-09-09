// GenericSettings.cs - dEngine
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using dEngine.Instances;
using dEngine.Instances.Attributes;
using Dynamitey;
using IniParser;
using IniParser.Model;
using IniParser.Model.Configuration;
using IniParser.Parser;

namespace dEngine.Settings
{
    /// <summary>
    /// Base class for settings containers.
    /// </summary>
    [TypeId(183)]
    [Uncreatable]
    public abstract class GenericSettings : Instance // TODO: save INIs to AppData/Local/dE/(GameName)
    {
        private static readonly IniParserConfiguration IniParserConfig = new IniParserConfiguration
        {
            CaseInsensitive = true,
            CommentString = "--",
            KeyValueAssigmentChar = '=',
            SkipInvalidLines = true,
            AssigmentSpacer = " "
        };

        /// <inheritdoc />
        protected GenericSettings()
        {
            ParentLocked = true;
            ChildAdded.Event += ChildAddedOnEvent;
        }

        private void ChildAddedOnEvent(Instance instance)
        {
            if (instance is Settings)
                Load();
        }

        /// <inheritdoc />
        protected override void OnChildAdded(Instance child)
        {
            base.OnChildAdded(child);

            Settings settings;
            if ((settings = child as Settings) != null)
                settings.ParentLocked = true;
        }

        /// <summary>
        /// Creates a new <see cref="Settings" /> object and adds it to the settings container.
        /// </summary>
        protected T CreateSettings<T>(string name) where T : Settings, new()
        {
            return new T {Name = name, Parent = this, ParentLocked = true, Archivable = false};
        }

        /// <summary>
        /// Restores all settings to their default values.
        /// </summary>
        public void RestoreDefaults()
        {
            foreach (var settings in Children.OfType<Settings>())
            {
                settings.RestoreDefaults();
                Save();
            }
        }

        /// <summary>
        /// Saves the settings to an ini file.
        /// </summary>
        public void Save()
        {
            try
            {
                var data = new IniData {Configuration = IniParserConfig};

                foreach (var setting in Children.OfType<Settings>())
                    data.Merge(setting.GetIniData());

                var settingsPath = Path.Combine(Engine.DocumentsPath, "Settings");
                Directory.CreateDirectory(settingsPath);
                var file = Path.Combine(settingsPath, $"{Name}.ini");
                using (var stream = File.Create(file))
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        var ini = new StreamIniDataParser();
                        ini.WriteData(writer, data);
                    }
                }
            }
            catch (Exception e)
            {
                Engine.Logger.Error(e);
                throw new InvalidOperationException($"Could not save ini for ${Name}. (${ClassName}): {e.Message}", e);
            }
        }

        /// <summary>
        /// Loads the settings from an ini file.
        /// </summary>
        public void Load()
        {
            var settingsPath = Path.Combine(Engine.DocumentsPath, "Settings");
            var file = Path.Combine(settingsPath, $"{Name}.ini");
            if (!File.Exists(file))
            {
                Logger.Warn($"Settings/{Name}.ini not found - creating it.");
                Save();
                return;
            }

            using (var stream = File.OpenRead(file))
            {
                using (var reader = new StreamReader(stream))
                {
                    var parser = new StreamIniDataParser(new IniDataParser(IniParserConfig));
                    var data = parser.ReadData(reader);

                    foreach (var section in data.Sections)
                    {
                        var settings = FindFirstChild(section.SectionName);

                        if (settings != null)
                        {
                            var settingsType = settings.GetType();
                            foreach (var kv in section.Keys)
                            {
                                var property = settingsType.GetProperty(kv.KeyName);

                                if (property == null)
                                {
                                    Logger.Warn(
                                        $"Ignoring INI property \"{kv.KeyName}\" (not defined in {settings.ClassName})");
                                    continue;
                                }

                                if (property.SetMethod?.IsPublic != true)
                                    continue;

                                dynamic value;

                                if (property.PropertyType == typeof(Font))
                                {
                                    var trimmed = kv.Value.Remove(0, 7);
                                    trimmed = trimmed.Remove(trimmed.Length - 1);
                                    var parts = trimmed.Split(new[] {", "}, StringSplitOptions.None)
                                        .ToDictionary(x => x.Split('=')[0], x => x.Split('=')[1]);

                                    value = new Font(parts["Name"], float.Parse(parts["Size"]),
                                        (GraphicsUnit)int.Parse(parts["Units"]));
                                }
                                else
                                {
                                    var propType = property.PropertyType;
                                    if (propType == typeof(int))
                                        value = int.Parse(kv.Value);
                                    else if (propType == typeof(string))
                                        value = kv.Value;
                                    else if (propType == typeof(float))
                                        value = float.Parse(kv.Value);
                                    else if (propType == typeof(double))
                                        value = double.Parse(kv.Value);
                                    else
                                        value = Dynamic.CoerceConvert(kv.Value, propType);
                                }

                                Dynamic.InvokeSet(settings, kv.KeyName, value);
                            }
                        }
                        else
                        {
                            Logger.Warn($"No settings object found for INI section: {section.SectionName}");
                        }
                    }
                }
            }
        }
    }
}
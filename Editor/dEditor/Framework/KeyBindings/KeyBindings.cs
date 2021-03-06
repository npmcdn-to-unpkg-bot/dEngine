﻿// KeyBindings.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using System.IO;
using dEngine;
using dEngine.Instances;
using dEngine.Services;
using Expressions;
using MoreLinq;
using Newtonsoft.Json;

// ReSharper disable once LocalizableElement

namespace dEditor.Framework
{
    public static class KeyBindings
    {
        public static string CustomShortcutsPath = Path.Combine(Editor.Current.EditorDocumentsPath,
            "KeyboardShortcuts.json");

        private static readonly BoundExpressionOptions WhenExpressionOptions = new BoundExpressionOptions
        {
            AllowPrivateAccess = false,
            ResultType = typeof(bool)
        };

        private static FileSystemWatcher _watcher;
        private static string _defaultShortcutsJson;
        private static readonly List<ContextActionService.ContextBinding> _bindings;

        static KeyBindings()
        {
            _bindings = new List<ContextActionService.ContextBinding>();

            DataModel.GetService<ContextActionService>();
            ContextActionService.DefineState("viewportFocus");
            ContextActionService.DefineState("scriptEditorFocus");
            ContextActionService.DefineState("dragging");
            ContextActionService.DefineState("selectionEmpty");
            ContextActionService.DefineState("textSelectionEmpty");
        }

        public static void Init()
        {
            // load custom shortcuts first
            if (!File.Exists(CustomShortcutsPath))
                File.WriteAllText(CustomShortcutsPath, "[\n]");

            _watcher = new FileSystemWatcher
            {
                Path = Editor.Current.EditorDocumentsPath,
                Filter = "KeyboardShortcuts.json",
                NotifyFilter = NotifyFilters.LastWrite,
                EnableRaisingEvents = true
            };
            _watcher.Changed += (s, e) => ReloadKeymap();

            _defaultShortcutsJson = ContentProvider.DownloadString(new Uri("editor://KeyboardShortcuts.json"));
            ReloadKeymap();
        }

        private static void ReloadKeymap()
        {
            foreach (var binding in _bindings)
                ContextActionService.Unbind(binding);

            LoadKeymap(_defaultShortcutsJson);
            LoadKeymap(File.ReadAllText(CustomShortcutsPath));
        }

        private static void LoadKeymap(string keymap)
        {
            var items = JsonConvert.DeserializeObject<KeymapItem[]>(keymap);
            foreach (var item in items)
            {
                Key key;
                ContextActionService.KeyCollection modifiers;
                Func<bool> when;

                ParseKey(item.Key, out key, out modifiers);
                ParseWhen(item.When, out when);

                try
                {
                    _bindings.Add(ContextActionService.Bind(item.Command, key, modifiers, when));
                }
                catch (ExpressionsException e)
                {
                    Editor.Logger.Error(e, "An exception was thrown when parsing a keybinding expression.");
                }
            }
        }

        private static Key KeyFromString(string key)
        {
            switch (key)
            {
                case "alt":
                    return Key.LeftAlt;
                case "shift":
                    return Key.LeftShift;
                case "ctrl":
                    return Key.LeftControl;
                case ".":
                    return Key.FullStop;
                case ",":
                    return Key.Comma;
                case ";":
                    return Key.Semicolon;
                case "'":
                    return Key.Apostrophe;
                case "`":
                    return Key.Grave;
                case "minus":
                case "-":
                    return Key.Minus;
                case "plus":
                case "+":
                    return Key.Equals;
                case "[":
                    return Key.LeftBracket;
                case "]":
                    return Key.RightBracket;
                case "#":
                    return Key.Hash;
                case "/":
                    return Key.Slash;
                case "\\":
                    return Key.Backslash;
                case "win":
                    return Key.Win;
                case "1":
                    return Key.Num1;
                case "2":
                    return Key.Num2;
                case "3":
                    return Key.Num3;
                case "4":
                    return Key.Num4;
                case "5":
                    return Key.Num5;
                case "6":
                    return Key.Num6;
                case "7":
                    return Key.Num7;
                case "8":
                    return Key.Num8;
                case "9":
                    return Key.Num9;
                case "0":
                    return Key.Num0;
                default:
                    return (Key)Enum.Parse(typeof(Key), key, true);
            }
        }

        private static void ParseKey(string str, out Key key, out ContextActionService.KeyCollection modifiers)
        {
            var keys = str.Split('+');
            key = KeyFromString(keys[keys.Length - 1]);
            modifiers = new ContextActionService.KeyCollection();

            for (var i = 0; i < keys.Length - 1; i++)
                modifiers.Add(KeyFromString(keys[i]));
        }

        private static void ParseWhen(string when, out Func<bool> predicate)
        {
            if (string.IsNullOrEmpty(when))
            {
                predicate = () => true;
                return;
            }
            var expression = new DynamicExpression(when, ExpressionLanguage.Csharp);


            predicate = () =>
            {
                var context = new ExpressionContext();
                ContextActionService.StateVariables.ForEach(v => context.Variables.Add(v.Key, v.Value));
                var result = expression.Invoke(context, WhenExpressionOptions);
                return (bool)result;
            };
        }

        [JsonObject]
        public class KeymapItem
        {
            [JsonProperty("key")] public string Key;

            [JsonProperty("command")] public string Command;

            [JsonProperty("when")] public string When;
        }
    }
}
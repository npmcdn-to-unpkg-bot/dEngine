// EditorSettings.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using Caliburn.Micro;
using dEditor.Modules.Widgets.CodeEditor;
using dEditor.Styles;
using dEngine;
using dEngine.Instances.Attributes;
using dEngine.Settings;

namespace dEditor
{
    [TypeId(301)]
    internal class EditorSettings : Settings
    {
        private static Colour _backgroundColour;
        private static Colour _commentColour;
        private static Colour _errorColour;
        private static Colour _keywordColour;
        private static Colour _numberColour;
        private static Colour _operatorColour;
        private static Colour _selectionBackgroundColour;
        private static Colour _selectionTextColour;
        private static Colour _stringColour;
        private static Colour _textColour;
        private static int _maxOutputLines;
        private static float _tabWidth;
        private static Font _font;
        private static string _autosavePath;
        private static int _autosaveInterval;
        private static bool _autosaveEnabled;
        private static bool _turnTabsIntoSpaces;
        private static bool _showDeprecated;
        private static Theme _theme;

        #region Output

        /// <summary>
        /// The number of outlines.
        /// </summary>
        [EditorVisible("Output", "Max Output Lines")]
        public static int MaxOutputLines
        {
            get { return _maxOutputLines; }
            set
            {
                _maxOutputLines = value;
                NotifyChangedStatic();
            }
        }

        #endregion

        #region Properties

        [EditorVisible("Appearance")]
        public static Theme Theme
        {
            get
            {
                return _theme;
            }
            set
            {
                _theme = value;
                NotifyChangedStatic();
            }
        }

        /// <summary>
        /// Shows deprecated/obsolete properties.
        /// </summary>
        [EditorVisible("Properties", "Show Deprecated")]
        public static bool ShowDeprecated
        {
            get { return _showDeprecated; }
            set
            {
                _showDeprecated = value;
                NotifyChangedStatic();
            }
        }

        #endregion

        private static void NotifyChangedStatic([CallerMemberName] string propertyName = null)
        {
            Editor.Current.Dispatcher.InvokeAsync(() => Editor.Current.Settings?.NotifyChanged(propertyName));
        }

        private static void NotifyScriptThemeChanged()
        {
            Editor.Current.Dispatcher.InvokeAsync(() =>
            {
                foreach (var editor in IoC.GetAll<CodeEditorViewModel>())
                    editor.UpdateTheme();
            });
        }

        public override void RestoreDefaults()
        {
            Theme = Theme.Light;

            MaxOutputLines = 200;

            ShowDeprecated = true;

            AutosaveEnabled = true;
            AutosaveInterval = 5;
            AutosavePath = Path.Combine(Editor.Current.EditorDocumentsPath, "Autosaves");

            Font = new Font("Roboto Mono", 14.0f);
            TabWidth = 4;

            BackgroundColour = Colour.fromRGB(255, 255, 255);
            CommentColour = Colour.fromRGB(0, 127, 0);
            ErrorColour = Colour.fromRGB(255, 0, 0);
            KeywordColour = Colour.fromRGB(0, 0, 127);
            NumberColour = Colour.fromRGB(0, 127, 127);
            OperatorColour = Colour.fromRGB(127, 127, 0);
            SelectionBackgroundColour = Colour.fromRGB(110, 161, 241);
            SelectionTextColour = Colour.fromRGB(255, 255, 255);
            StringColour = Colour.fromRGB(127, 0, 127);
            TextColour = Colour.fromRGB(0, 0, 0);
        }

        #region Camera

        #endregion

        #region Autosave

        /// <summary>
        /// Determines if autosave is enabled.
        /// </summary>
        [EditorVisible("Autosave", "Autosave Enabled")]
        public static bool AutosaveEnabled
        {
            get { return _autosaveEnabled; }
            set
            {
                _autosaveEnabled = value;
                NotifyChangedStatic();
            }
        }

        /// <summary>
        /// The time between autosaves in minutes.
        /// </summary>
        [EditorVisible("Autosave", "Autosave Interval")]
        public static int AutosaveInterval
        {
            get { return _autosaveInterval; }
            set
            {
                _autosaveInterval = value;
                NotifyChangedStatic();
            }
        }

        /// <summary>
        /// The time between autosaves in minutes.
        /// </summary>
        [EditorVisible("Autosave", "Autosave Path")]
        public static string AutosavePath
        {
            get { return _autosavePath; }
            set
            {
                _autosavePath = value;
                NotifyChangedStatic();
            }
        }

        #endregion

        #region Script Editor

        /// <summary>
        /// The script editor font.
        /// </summary>
        [EditorVisible("Script Editor", "Font")]
        public static Font Font
        {
            get { return _font; }
            set
            {
                _font = value;
                NotifyChangedStatic();
            }
        }

        /// <summary>
        /// The length for tabs.
        /// </summary>
        [EditorVisible("Script Editor", "Tab Width")]
        public static float TabWidth
        {
            get { return _tabWidth; }
            set
            {
                _tabWidth = value;
                NotifyChangedStatic();
            }
        }

        /// <summary>
        /// Pressing tab will insert spaces instead.
        /// </summary>
        [EditorVisible("Script Editor", "Tab Width")]
        public static bool TurnTabsIntoSpaces
        {
            get { return _turnTabsIntoSpaces; }
            set
            {
                _turnTabsIntoSpaces = value;
                NotifyChangedStatic();
            }
        }

        #endregion

        #region Script Editor Colours

        /// <summary>
        /// The colour of the background.
        /// </summary>
        [EditorVisible("Script Editor", "Background Colour")]
        public static Colour BackgroundColour
        {
            get { return _backgroundColour; }
            set
            {
                _backgroundColour = value;
                NotifyChangedStatic();
                NotifyScriptThemeChanged();
            }
        }

        /// <summary>
        /// The colour of comments.
        /// </summary>
        [EditorVisible("Script Editor", "Comment Colour")]
        public static Colour CommentColour
        {
            get { return _commentColour; }
            set
            {
                _commentColour = value;
                NotifyChangedStatic();
                NotifyScriptThemeChanged();
            }
        }

        /// <summary>
        /// The colour of errors.
        /// </summary>
        [EditorVisible("Script Editor", "Error Colour")]
        public static Colour ErrorColour
        {
            get { return _errorColour; }
            set
            {
                _errorColour = value;
                NotifyChangedStatic();
                NotifyScriptThemeChanged();
            }
        }

        /// <summary>
        /// The colour of keywords.
        /// </summary>
        [EditorVisible("Script Editor", "Keyword Colour")]
        public static Colour KeywordColour
        {
            get { return _keywordColour; }
            set
            {
                _keywordColour = value;
                NotifyChangedStatic();
                NotifyScriptThemeChanged();
            }
        }

        /// <summary>
        /// The colour of operators.
        /// </summary>
        [EditorVisible("Script Editor", "Number Colour")]
        public static Colour NumberColour
        {
            get { return _numberColour; }
            set
            {
                _numberColour = value;
                NotifyChangedStatic();
                NotifyScriptThemeChanged();
            }
        }

        /// <summary>
        /// The colour of operators.
        /// </summary>
        [EditorVisible("Script Editor", "Operator Colour")]
        public static Colour OperatorColour
        {
            get { return _operatorColour; }
            set
            {
                _operatorColour = value;
                NotifyChangedStatic();
                NotifyScriptThemeChanged();
            }
        }

        /// <summary>
        /// The colour of the selection background.
        /// </summary>
        [EditorVisible("Script Editor", "Selection Background Colour")]
        public static Colour SelectionBackgroundColour
        {
            get { return _selectionBackgroundColour; }
            set
            {
                _selectionBackgroundColour = value;
                NotifyChangedStatic();
                NotifyScriptThemeChanged();
            }
        }

        /// <summary>
        /// The colour of the selection text.
        /// </summary>
        [EditorVisible("Script Editor", "Selection Text Colour")]
        public static Colour SelectionTextColour
        {
            get { return _selectionTextColour; }
            set
            {
                _selectionTextColour = value;
                NotifyChangedStatic();
                NotifyScriptThemeChanged();
            }
        }

        /// <summary>
        /// The colour of strings.
        /// </summary>
        [EditorVisible("Script Editor", "String Colour")]
        public static Colour StringColour
        {
            get { return _stringColour; }
            set
            {
                _stringColour = value;
                NotifyChangedStatic();
                NotifyScriptThemeChanged();
            }
        }

        /// <summary>
        /// The colour of the text.
        /// </summary>
        [EditorVisible("Script Editor", "Text Colour")]
        public static Colour TextColour
        {
            get { return _textColour; }
            set
            {
                _textColour = value;
                NotifyChangedStatic();
                NotifyScriptThemeChanged();
            }
        }

        #endregion
    }
}
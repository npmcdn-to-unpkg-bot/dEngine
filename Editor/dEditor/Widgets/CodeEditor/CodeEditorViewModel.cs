// CodeEditorViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using Caliburn.Micro;
using dEditor.Framework;
using dEditor.Instances;
using dEditor.Widgets.CodeEditor.Commands;
using dEngine.Instances;
using dEngine.Services;
using dEngine.Utility.Extensions;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;

namespace dEditor.Widgets.CodeEditor
{
    [Export(typeof(CodeEditorViewModel))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class CodeEditorViewModel : Document
    {
        private FontFamily _fontFamily;
        private float _fontSize;
        private SolidColorBrush _foregroundBrush;
        private float _zoomScale = 1.0f;

        public CodeEditorViewModel()
        {
            AvalonEditCommands.IndentSelection.InputGestures.Clear();
            AvalonEditCommands.DeleteLine.InputGestures.Clear();

            ContextActionService.Register("scriptEditor.indent", () =>
            {
                var test = new TextEditor();
                AvalonEditCommands.IndentSelection.Execute(null, test);
            });

            ContextActionService.Register("scriptEditor.deleteLine", () =>
            {
                var test = new TextEditor();
                AvalonEditCommands.DeleteLine.Execute(null, test);
            });
        }

        public CodeEditorViewModel(LuaSourceContainer luaSourceContainer)
        {
            LuaSourceContainer = luaSourceContainer;
            NotifyOfPropertyChange(() => LuaSourceContainer);
            NotifyOfPropertyChange(() => DisplayName);

            UpdateTheme();

            InsertBreakpointCommand = new InsertBreakpointCommand(this);
        }

        public bool IsReadOnly => !string.IsNullOrEmpty(LuaSourceContainer?.LinkedSource);

        public static RoutedUICommand ZoomInCommand { get; }
        public static RoutedUICommand ZoomOutCommand { get; }
        public InsertBreakpointCommand InsertBreakpointCommand { get; }

        public float ZoomScale
        {
            get { return _zoomScale; }
            set
            {
                if (value.Equals(_zoomScale)) return;
                _zoomScale = Math.Min(4.0f, Math.Max(0.2f, value));
                NotifyOfPropertyChange();
            }
        }

        public float FontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value;
                NotifyOfPropertyChange();
            }
        }

        public FontFamily FontFamily
        {
            get { return _fontFamily; }
            set
            {
                _fontFamily = value;
                NotifyOfPropertyChange();
            }
        }

        public LuaSourceContainer LuaSourceContainer { get; }

        public override string DisplayName => LuaSourceContainer?.Name ?? "";

        public IHighlightingDefinition SyntaxHighlighting { get; set; }

        public SolidColorBrush ForegroundBrush
        {
            get { return _foregroundBrush; }
            set
            {
                if (Equals(value, _foregroundBrush)) return;
                _foregroundBrush = value;
                NotifyOfPropertyChange();
            }
        }

        public TextEditor TextEditor { get; set; }

        public void UpdateTheme()
        {
            using (var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("dEditor.Content.Syntax.Lua.xshd"))
            {
                var xml = stream.ReadString();
                xml = xml.Replace("#Number", EditorSettings.NumberColour.ToHexString());
                xml = xml.Replace("#Operator", EditorSettings.OperatorColour.ToHexString());
                xml = xml.Replace("#String", EditorSettings.StringColour.ToHexString());
                xml = xml.Replace("#Keyword", EditorSettings.KeywordColour.ToHexString());
                xml = xml.Replace("#Comment", EditorSettings.CommentColour.ToHexString());
                xml = xml.Replace("#Comment", EditorSettings.CommentColour.ToHexString());

                var tc = EditorSettings.TextColour;
                ForegroundBrush =
                    new SolidColorBrush(Color.FromRgb((byte)(tc.r*255), (byte)(tc.g*255), (byte)(tc.b*255)));

                var reader = new XmlTextReader(new StringReader(xml));

                SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                NotifyOfPropertyChange(() => SyntaxHighlighting);
            }

            FontSize = EditorSettings.Font.Size;
            FontFamily = new FontFamily(EditorSettings.Font.FontFamily.Name);
        }

        public string CurrentSelection { get; set; }

        protected override void OnActivate()
        {
            ContextActionService.SetState("scriptEditorFocus", true);
        }

        protected override void OnDeactivate(bool close)
        {
            ContextActionService.SetState("scriptEditorFocus", false);
        }

        public override int GetHashCode()
        {
            return LuaSourceContainer.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as CodeEditorViewModel;
            return (other != null)
                   && Equals(LuaSourceContainer, other.LuaSourceContainer);
        }

        public override void CanClose(Action<bool> callback)
        {
            callback(true);
        }

        public static readonly DependencyProperty ZoomInCommandProperty = DependencyProperty.Register("ZoomInCommand",
            typeof(ICommand), typeof(CodeEditorViewModel));

        public static readonly DependencyProperty ZoomOutCommandProperty = DependencyProperty.Register("ZoomOutCommand",
            typeof(ICommand), typeof(CodeEditorViewModel));

        public void ScrollTo(int lineNumber)
        {
            var editor = TextEditor;
            if (editor != null)
            {
                editor.ScrollToLine(lineNumber);
            }
            else
            {
                ViewAttached += (s, e) => editor.ScrollToLine(lineNumber);
            }
        }
    }
}
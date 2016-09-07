﻿// FindAndReplaceViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Windows;
using Caliburn.Micro;
using dEditor.Widgets.CodeEditor;

namespace dEditor.Dialogs.FindAndReplace
{
    [Export(typeof(IFindAndReplace))]
    public class FindAndReplaceViewModel : Screen, IFindAndReplace
    {
        private readonly bool _selectionModeAvailable;

        public FindAndReplaceViewModel()
        {
            var currentEditor = GetCurrentDocumentAsCodeEditor();
            var selection = GetSelection(currentEditor);
            _selectionModeAvailable = !string.IsNullOrEmpty(selection);

            var options = new List<string>
            {
                "Selection",
                "Current Script",
                "Entire Game",
                "Entire Project (includes content folder)"
            };
            if ((currentEditor != null) && _selectionModeAvailable)
                options.Insert(0, selection);

            LookInModes = options;
            LookIn = LookInMode.EntireGame;
        }


        public IEnumerable<string> LookInModes { get; set; }
        public LookInMode LookIn { get; set; }
        public string MatchText { get; set; }

        public int LookInOptionIndex
        {
            get
            {
                var option = (int)LookIn;
                return _selectionModeAvailable ? option : option + 1;
            }
            set { LookIn = _selectionModeAvailable ? (LookInMode)value : (LookInMode)value + 1; }
        }

        public bool MatchCase { get; set; }
        public bool MatchWholeWord { get; set; }
        public bool UseRegex { get; set; }
        public bool AppendResults { get; set; }

        private CodeEditorViewModel GetCurrentDocumentAsCodeEditor()
        {
            var codeEditor = Editor.Current.Shell.ActiveDocument as CodeEditorViewModel;
            return codeEditor;
        }

        private string GetSelection(CodeEditorViewModel editor)
        {
            return editor?.CurrentSelection;
        }

        public void FindAll()
        {
            switch (LookIn)
            {
                case LookInMode.Selection:
                    break;
                case LookInMode.CurrentScript:
                    break;
                case LookInMode.ContentFolder:
                    break;
                case LookInMode.EntireGame:
                    break;
                case LookInMode.EntireProject:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public IDictionary<string, object> GetDialogSettings()
        {
            return new Dictionary<string, object>
            {
                {"SizeToContent", SizeToContent.Manual},
                {"MinWidth", 266},
                {"MinHeight", 253},
                {"Width", 266},
                {"Height", 253},
                {"ShowInTaskbar", false},
                {"WindowStyle", WindowStyle.ToolWindow}
            };
        }

        public enum LookInMode
        {
            Selection,
            CurrentScript,
            ContentFolder,
            EntireGame,
            EntireProject
        }

        public enum Window
        {
            Results1,
            Results2
        }
    }
}
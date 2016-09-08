// StatusBarViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.

using System.ComponentModel.Composition;
using System.Threading;
using Caliburn.Micro;

namespace dEditor.Modules.Shell.StatusBar
{
    [Export(typeof(IStatusBar))]
    public class StatusBarViewModel : PropertyChangedBase, IStatusBar
    {
        private bool _frozen;
        private string _text;
        private int _line;
        private int _char;
        private string _lineStr;
        private string _charStr;
        private Timer _timer;
        private static readonly string _ready = string.Intern("Ready");
        private const int resetTextRate = 3 * 1000;

        public StatusBarViewModel()
        {
            _timer = new Timer(o =>
            {
                if (!_frozen && _text != _ready)
                    Text = _ready;
            }, null, 0, resetTextRate);
        }

        public string LineStr
        {
            get { return _lineStr; }
            set
            {
                if (value == _lineStr) return;
                _lineStr = value;
                NotifyOfPropertyChange();
            }
        }

        public string CharStr
        {
            get { return _charStr; }
            set
            {
                if (value == _charStr) return;
                _charStr = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsFrozen
        {
            get { return _frozen; }
            set
            {
                if (value == _frozen) return;
                _frozen = value;
                NotifyOfPropertyChange();
            }
        }

        public string Text
        {
            get { return _text; }
            set
            {
                if (value == _text) return;
                _text = value;
                NotifyOfPropertyChange();
            }
        }

        public int Line
        {
            get { return _line; }
            set
            {
                if (value == _line) return;
                _line = value;
                NotifyOfPropertyChange();
                LineStr = $"Ln {value}";
            }
        }

        public int Char
        {
            get { return _char; }
            set
            {
                if (value == _char) return;
                _char = value;
                NotifyOfPropertyChange();
                CharStr = $"Ch {value}";
            }
        }

        public void FreezeOutput(bool freeze)
        {
            IsFrozen = true;
        }

        public void SetLineChar(int line, int @char)
        {
            Line = line;
            Char = @char;
        }
    }
}
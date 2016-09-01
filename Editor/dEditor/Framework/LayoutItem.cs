// LayoutItem.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using Caliburn.Micro;

namespace dEditor.Framework
{
    public abstract class LayoutItem : Screen, ILayoutItem
    {
        protected RelayCommand _closeCommand;
        private Guid _guid;
        private bool _isVisible;
        private bool _selected;

        protected LayoutItem()
        {
            _guid = Guid.NewGuid();
        }

        public Guid Id => _guid;

        public string ContentId => _guid.ToString();

        public virtual BitmapSource IconSource { get; }

        public bool ShouldReopenOnStart { get; set; } = true;

        public abstract ICommand CloseCommand { get; }

        public bool IsSelected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                NotifyOfPropertyChange();
            }
        }

        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                _isVisible = value;
                NotifyOfPropertyChange();
            }
        }

        public virtual void LoadState(BinaryReader reader)
        {
        }

        public virtual void SaveState(BinaryWriter writer)
        {
        }
    }
}
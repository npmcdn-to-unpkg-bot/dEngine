// AdvancedObjectsViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using dEditor.Framework;
using dEditor.Widgets.CodeEditor;
using dEditor.Widgets.Explorer;
using dEditor.Widgets.MaterialEditor;
using dEditor.Widgets.Viewport;
using dEngine;
using dEngine.Instances.Attributes;
using dEngine.Serializer.V1;
using dEngine.Services;

namespace dEditor.Widgets.AdvancedObjects
{
    [Export(typeof(IAdvancedObjects))]
    public class AdvancedObjectsViewModel : Widget, IAdvancedObjects
    {
        private string _filter;
        private ICollectionView _items;
        private Mode _mode;

        private MaterialEditorViewModel _materialEditor;
        private ViewportViewModel _viewportEditor;
        private CodeEditorViewModel _scriptEditor;
        private bool _selectInsertedObject;

        public AdvancedObjectsViewModel()
        {
            SelectInsertedObject = true;
            UpdateItems();
            Editor.Current.Shell.ActiveDocumentChanged += doc => UpdateItems();
            DisplayName = "Advanced Objects";
        }

        public string Filter
        {
            get { return _filter; }
            set
            {
                if (value == _filter) return;
                _filter = value.ToLower();
                Items.Refresh();
                NotifyOfPropertyChange();
            }
        }

        public override PaneLocation PreferredLocation => PaneLocation.Bottom;

        public ICollectionView Items
        {
            get { return _items; }
            set
            {
                if (Equals(value, _items)) return;
                _items = value;
                NotifyOfPropertyChange();
            }
        }

        [Export]
        public bool SelectInsertedObject
        {
            get { return _selectInsertedObject; }
            set
            {
                if (value == _selectInsertedObject) return;
                _selectInsertedObject = value;
                NotifyOfPropertyChange();
            }
        }

        public void UpdateItems()
        {
            Items = null;
            var doc = Editor.Current.Shell.ActiveDocument;
            IEnumerable<Type> types;

            if ((_materialEditor = doc as MaterialEditorViewModel) != null)
            {
                types = new Type[0];
                _mode = Mode.Expressions;
                DisplayName = "Material Expressions";
            }
            else if ((_scriptEditor = doc as CodeEditorViewModel) != null)
            {
                // TODO: code snippets.
                types = new Type[0];
                _mode = Mode.Snippets;
                DisplayName = "Code Snippets";
            }
            else if ((_viewportEditor = doc as ViewportViewModel) != null)
            {
                types = Inst.TypeDictionary.Values.Where(t =>
                    !t.IsAbstract && t.IsPublic && (t.GetCustomAttribute<UncreatableAttribute>() == null) &&
                    !typeof(Service).IsAssignableFrom(t.Type)).Select(t => t.Type);
                _mode = Mode.Objects;
                DisplayName = "Advanced Objects";
            }
            else
            {
                types = new Type[0];
            }

            var entries = types.Select(t => new ObjectEntry(t));
            var items = CollectionViewSource.GetDefaultView(entries);
            items.Filter = FilterFunc;
            items.SortDescriptions.Add(new SortDescription(nameof(ObjectEntry.Name), ListSortDirection.Ascending));
            Items = items;
        }

        private bool FilterFunc(object o)
        {
            var item = (ObjectEntry)o;
            return string.IsNullOrEmpty(_filter) || item.Name.ToLower().Contains(_filter);
        }

        public void OnObjectMouseDown(ObjectEntry entry, MouseButtonEventArgs args)
        {
            try
            {
                if (args.ClickCount == 2)
                    switch (_mode)
                    {
                        case Mode.None:
                            break;
                        case Mode.Objects:
                            var inst = ScriptService.NewInstance(entry.Name, SelectionService.First() ?? Game.Workspace);
                            if (SelectInsertedObject)
                            {
                                SelectionService.Service.Select(inst, true);

                                ExplorerItem explorerItem; // expand parent
                                if (ExplorerViewModel.ExplorerItems[inst.Parent].TryGetTarget(out explorerItem))
                                    explorerItem.IsExpanded = true;
                            }
                            break;
                        case Mode.Expressions:
                            break;
                        case Mode.Snippets:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, DisplayName);
            }
        }

        public enum Mode
        {
            None,
            Objects,
            Expressions,
            Snippets
        }
    }
}
// ViewportViewModel.cs - dEditor
// Copyright © https://github.com/DanDevPC/
// This file is subject to the terms and conditions defined in the 'LICENSE' file.
using System;
using System.Diagnostics;
using System.Windows;
using dEditor.Framework;
using dEngine;
using dEngine.Instances;
using GongSolutions.Wpf.DragDrop;

namespace dEditor.Widgets.Viewport
{
    public class ViewportViewModel : Document, IDropTarget
    {
        public ViewportViewModel(bool createGui = true)
        {
            Editor.Current.ProjectChanged += p => NotifyOfPropertyChange(() => DisplayName);
            Game.Workspace.PlaceLoaded.Connect(p => NotifyOfPropertyChange(() => DisplayName));

            var viewportScript = new LocalScript
            {
                Name = "ViewportScript",
                LinkedSource = "editor://Scripts/Viewport.lua",
                Identity = ScriptIdentity.Editor,
                Parent = Game.CoreEnvironment
            };

            viewportScript.Run(this);
        }

        public ScreenGui ViewportGui { get; set; }

        public override string DisplayName => Game.Workspace?.PlaceId ?? "???";

        public void DragOver(IDropInfo dropInfo)
        {
            // TODO: content drag to viewport
            /*
            var fileItem = dropInfo.Data as FileItem;
            var data = dropInfo.Data as DataObject;

            if (fileItem != null)
            {
                dropInfo.Effects = DragDropEffects.Copy;
            }
            else if (data?.ContainsFileDropList() == true)
            {
                var test = data.GetFormats();
            }
            */
        }

        public void Drop(IDropInfo dropInfo)
        {
            throw new NotImplementedException();
        }

        public override void TryClose(bool? dialogResult = null)
        {
            Engine.SetHandle(IntPtr.Zero);
            Editor.Current.Project?.Close(); // try to close project.
            ViewportGui?.Destroy();

            base.TryClose(dialogResult);
        }

        public void OnPreviewDragOver(object sender, DataObject dataObject, ref DragDropEffects effects)
        {
            if (dataObject == null)
            {
                Debug.WriteLine("DragObject was null for Viewport drag");
                return;
            }

            if (dataObject.ContainsFileDropList() || dataObject.GetDataPresent("GongSolutions.Wpf.DragDrop"))
                effects = DragDropEffects.Copy;
        }

        public void OnPreviewDragDrop(object sender, DataObject dataObject)
        {
            /*
            if (dataObject.GetDataPresent("GongSolutions.Wpf.DragDrop"))
            {
                var obj = dataObject.GetData("GongSolutions.Wpf.DragDrop");
                var fileItem = obj as FileItem;

                if (fileItem != null)
                {
                    try
                    {
                        var instance = InsertService.Service.LoadAsset(fileItem.FilePath);
                        Game.Selection.ClearSelection();

                        if (instance is Mesh)
                        {
                            var parent = new Part {Parent = Game.Workspace, Size = new Vector3(1, 1, 1)};
                            instance.Parent = parent;
                            Game.Selection.Select(parent);
                        }
                        else
                        {
                            instance.Parent = Game.Workspace;
                            Game.Selection.Select(instance);
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message, "Insert Asset", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            */
        }
    }
}
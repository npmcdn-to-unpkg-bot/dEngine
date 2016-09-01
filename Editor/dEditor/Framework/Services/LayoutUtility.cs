// LayoutUtility.cs - dEditor
// Copyright (C) 2016-2016 DanDevPC (dandev.sco@gmail.com)
//  
// This library is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// You should have received a copy of the GNU General Public
// along with this program. If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.IO;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;
using Xceed.Wpf.AvalonDock.Layout.Serialization;

namespace dEditor.Framework.Services
{
    internal static class LayoutUtility
    {
        public static void SaveLayout(DockingManager manager, Stream stream)
        {
            var layoutSerializer = new XmlLayoutSerializer(manager);
            layoutSerializer.Serialize(stream);
        }

        public static void LoadLayout(DockingManager manager, Stream stream, Action<IDocument> addDocumentCallback,
                                      Action<IWidget> addToolCallback, Dictionary<string, ILayoutItem> items)
        {
            var layoutSerializer = new XmlLayoutSerializer(manager);

            layoutSerializer.LayoutSerializationCallback += (s, e) =>
            {
                ILayoutItem item;
                if (items.TryGetValue(e.Model.ContentId, out item))
                {
                    e.Content = item;

                    var tool = item as IWidget;
                    var anchorable = e.Model as LayoutAnchorable;

                    var document = item as IDocument;
                    var layoutDocument = e.Model as LayoutDocument;

                    if (tool != null && anchorable != null)
                    {
                        addToolCallback(tool);
                        tool.IsVisible = anchorable.IsVisible;

                        if (anchorable.IsActive)
                            tool.Activate();

                        tool.IsSelected = e.Model.IsSelected;

                        return;
                    }

                    if (document != null && layoutDocument != null)
                    {
                        addDocumentCallback(document);

                        // Nasty hack to get around issue that occurs if documents are loaded from state,
                        // and more documents are opened programmatically.
                        layoutDocument.GetType().GetProperty("IsLastFocusedDocument").SetValue(layoutDocument, false, null);

                        document.IsSelected = layoutDocument.IsSelected;
                        return;
                    }
                }

                // Don't create any panels if something went wrong.
                e.Cancel = true;
            };

            try
            {
                layoutSerializer.Deserialize(stream);
            }
            catch
            {
            }
        }
    }
}
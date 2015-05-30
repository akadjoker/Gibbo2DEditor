﻿#region Copyrights
/*
Gibbo2D License - Version 1.0
Copyright (c) 2013 - Gibbo2D Team
Founders Joao Alves <joao.cpp.sca@gmail.com> & Luis Fernandes <luisapidcloud@hotmail.com>

Permission is granted to use this software and associated documentation files (the "Software") free of charge, 
to any person or company. The code can be used, modified and merged without restrictions, but you cannot sell 
the software itself and parts where this license applies. Still, permission is granted for anyone to sell 
applications made using this software (for example, a game). This software cannot be claimed as your own, 
except for copyright holders. This license notes should also be available on any of the changed or added files.

The software is provided "as is", without warranty of any kind, express or implied, including but not limited to 
the warranties of merchantability, fitness for a particular purpose and non-infrigement. In no event shall the 
authors or copyright holders be liable for any claim, damages or other liability.

The license applies to all versions of the software, both newer and older than the one listed, unless a newer copy 
of the license is available, in which case the most recent copy of the license supercedes all others.

*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gibbo.Library;
using System.Windows.Forms;
using System.IO;
using Gibbo.Editor.Model;
using System.Windows;
using System.Windows.Controls;
using Gibbo.Editor.WPF.Controls;
using System.Collections.ObjectModel;

namespace Gibbo.Editor.WPF
{
    static class EditorHandler
    {
        internal static Xceed.Wpf.Toolkit.PropertyGrid.PropertyGrid SelectedObjectPG = null;

        private static ObservableCollection<OutputMessage> outputMessages = new ObservableCollection<OutputMessage>();
        public static ObservableCollection<OutputMessage> OutputMessages
        {
            get 
            {
                return outputMessages; 
            }
        }

        private static PicturePreview picturePreview;
        internal static PicturePreview PicturePreview
        {
            get
            {
                if (picturePreview == null)
                {
                    picturePreview = new PicturePreview();                   
                    picturePreview.ShowInTaskbar = false;
                    //picturePreview.Topmost = true;
                }

                return picturePreview;
            }
        }

        //internal static OutputWindow OutputWindow = new OutputWindow();

        /// <summary>
        /// Changes the property grid and property label to the input value
        /// </summary>
        /// <param name="value">The object</param>
        public static void ChangeSelectedObject(object value)
        {
            if (value != null)
            {
                EditorHandler.PropertyGridContainer.Children.Clear();
                PropertyBox properties;
                properties = new PropertyBox();
                properties.SelectedObject = value;
                properties.ToggleExpand();

                EditorHandler.PropertyGridContainer.Children.Add(properties);
            }
        }

        /// <summary>
        /// Changes the property grid and property label to the selected objects
        /// </summary>
        public static void ChangeSelectedObjects()
        {
            EditorHandler.TilesetMenuItems.IsEnabled = false;
            if (SelectedGameObjects != null)
            {
                if (SelectedGameObjects.Count == 1)
                {
                    ChangeSelectedObject(SelectedGameObjects[0]);

                    if (SelectedGameObjects[0] is Tileset)
                    {
                        EditorHandler.TilesetBrushControl.ChangeSelectionSize((SelectedGameObjects[0] as Tileset).TileWidth, (SelectedGameObjects[0] as Tileset).TileHeight);
                        EditorHandler.TilesetBrushControl.ChangeImageSource((SelectedGameObjects[0] as Tileset).ImageName);

                        SceneManager.ActiveTileset = (Tileset)SelectedGameObjects[0];
                        EditorHandler.TilesetMenuItems.IsEnabled = true;
                    }
                    else
                    {
                        SceneManager.ActiveTileset = null;
                    }

                    EditorCommands.CreatePropertyGridView();
                }
                else if (SelectedGameObjects.Count == 0)
                {
                    SceneManager.ActiveTileset = null;

                    if(EditorHandler.SceneTreeView.SelectedItem  != null)
                        (EditorHandler.SceneTreeView.SelectedItem as DragDropTreeViewItem).IsSelected = false;

                    EditorCommands.CreatePropertyGridView();
                }
                else
                {
                    EditorCommands.CreatePropertyGridView();
                }

                if (EditorHandler.TilesetBrushControl != null)
                    EditorHandler.TilesetBrushControl.InvalidateVisual();
            }
        }

        internal static UserPreferences UserPreferences { get; set; }

        internal static ProjectExplorerTreeView ProjectTreeView { get; set; }

        internal static StackPanel PropertyGridContainer { get; set; }

        internal static StackPanel TilesetMenuItems { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal static SceneViewGameControl SceneViewControl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal static SplitContainer EditorSplitterContainer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public static TilesetBrushControl TilesetBrushControl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal static IniFile Settings { get; set; }

        /// <summary>
        /// 
        /// </summary>
        internal static ToolStripStatusLabel StatusLabel { get; set; }

        /// <summary>
        /// Gets or sets the active PropertyGrid.
        /// </summary>
        internal static PropertyGrid PropertyGrid { get; set; }

        /// <summary>
        /// Gets or sets the active PropertyLabel.
        /// </summary>
        internal static TabPage PropertyPage { get; set; }

        /// <summary>
        /// Gets or sets the active SceneTreeView.
        /// </summary>
        public static SceneHierarchyTreeView SceneTreeView { get; set; }

        /// <summary>
        /// Gets or sets the selected GameObjects.
        /// </summary>
        internal static List<GameObject> SelectedGameObjects { get; set; }

        /// <summary>
        /// Gets or sets the active UnDoRedo.
        /// </summary>
        internal static UndoRedo UnDoRedo { get; set; }
    }
}

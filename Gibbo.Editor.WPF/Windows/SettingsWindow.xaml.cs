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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.ComponentModel;
using Gibbo.Library;
using System.Diagnostics;

namespace Gibbo.Editor.WPF
{
    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private IniFile iniSettings = new IniFile(SceneManager.GameProject.ProjectPath + "\\settings.ini");

        public SettingsWindow()
        {
            InitializeComponent();
            propertyGrid.Tag = "gibbo_general";
            propertyGrid.SelectedObject = LoadProperties("gibbo_general");
        }

        private ISettingsChannelA LoadProperties(string _ref)
        {
            ISettingsChannelA settings = null;

            switch (_ref)
            {
                case "gibbo_general":
                    settings = new GibboGeneralSettingsDynamic();
                    (settings as GibboGeneralSettingsDynamic).AutomaticProjectLoad = Properties.Settings.Default.LoadLastProject;
                    try
                    {
                        (settings as GibboGeneralSettingsDynamic).ScriptEditors = (GibboGeneralSettingsDynamic.ScriptingEditors)Enum.Parse(typeof(GibboGeneralSettingsDynamic.ScriptingEditors), Properties.Settings.Default.DefaultScriptEditor, true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    (settings as GibboGeneralSettingsDynamic).StartOnFullScreen = Properties.Settings.Default.StartOnFullScreen;
                    (settings as GibboGeneralSettingsDynamic).ShowDebugView = Properties.Settings.Default.ShowDebugView;
                    (settings as GibboGeneralSettingsDynamic).ReduceConsumption = Properties.Settings.Default.ReduceConsumption;
                    break;

                case "gibbo_tileset":
                    settings = new GibboTilesetSettingsDynamic();
                    (settings as GibboTilesetSettingsDynamic).HighlightActiveTileset = Properties.Settings.Default.HighlightActiveTileset;
                    break;

                case "game_general":
                    settings = new GameGeneralSettingsDynamic();
                    (settings as GameGeneralSettingsDynamic).ProjectName = SceneManager.GameProject.ProjectName;
                    break;

                case "game_grid":
                    settings = new GameGridSettingsDynamic();
                    (settings as GameGridSettingsDynamic).GridSpacing = SceneManager.GameProject.EditorSettings.GridSpacing;
                    (settings as GameGridSettingsDynamic).GridThickness = SceneManager.GameProject.EditorSettings.GridThickness;
                    (settings as GameGridSettingsDynamic).GridColor = SceneManager.GameProject.EditorSettings.GridColor;
                    (settings as GameGridSettingsDynamic).DisplayLines = SceneManager.GameProject.EditorSettings.GridNumberOfLines;
                    break;

                case "game_debug":
                    settings = new GameDebugDynamic();
                    (settings as GameDebugDynamic).ShowConsole = iniSettings.IniReadValue("Console", "Visible").ToLower().Trim().Equals("true") ? true : false;
                    (settings as GameDebugDynamic).Attach = Properties.Settings.Default.AttachVisualStudio;

                    try
                    {
                        (settings as GameDebugDynamic).DebugMode = (GameDebugDynamic.DebugModes)Enum.Parse(typeof(GameDebugDynamic.DebugModes), (SceneManager.GameProject.Debug ? "Debug" : "Release"), true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    break;

                case "game_screen":
                    settings = new GameScreenDynamic();
                    (settings as GameScreenDynamic).MouseVisible = iniSettings.IniReadValue("Mouse", "Visible").ToLower().Trim().Equals("true") ? true : false;
                    (settings as GameScreenDynamic).StartOnFullScreen = iniSettings.IniReadValue("Window", "StartFullScreen").ToLower().Trim().Equals("true") ? true : false;
                    (settings as GameScreenDynamic).ScreenWidth = SceneManager.GameProject.Settings.ScreenWidth;
                    (settings as GameScreenDynamic).ScreenHeight = SceneManager.GameProject.Settings.ScreenHeight;
                    //(settings as GameScreenDynamic).VSync = SceneManager.GameProject.ProjectSettings.VSyncEnabled;
                    break;
            }

            return settings;
        }

        private void SaveCurrent()
        {
            if (propertyGrid.Tag == null) return;

            switch (propertyGrid.Tag.ToString())
            {
                case "gibbo_general":
                    Properties.Settings.Default.LoadLastProject = (propertyGrid.SelectedObject as GibboGeneralSettingsDynamic).AutomaticProjectLoad;
                    Properties.Settings.Default.StartOnFullScreen = (propertyGrid.SelectedObject as GibboGeneralSettingsDynamic).StartOnFullScreen;
                    Properties.Settings.Default.ShowDebugView = (propertyGrid.SelectedObject as GibboGeneralSettingsDynamic).ShowDebugView;
                    Properties.Settings.Default.ReduceConsumption = (propertyGrid.SelectedObject as GibboGeneralSettingsDynamic).ReduceConsumption;
                    string appName = (propertyGrid.SelectedObject as GibboGeneralSettingsDynamic).ScriptEditors.ToString();
                    
                    // colocar no startup do gibbo
                    EditorUtils.StoreInstalledApplications();

                    //bool isInstalled = false;
                    //foreach (string name in EditorUtils.InstalledApps.Keys)
                    //{
                    //    if (name.Replace(" ", "").ToLower().Contains(appName.ToLower()))
                    //    {
                    //        isInstalled = true;
                    //        MessageBox.Show("Found::::" + EditorUtils.InstalledApps[name]);
                    //        //break;
                    //    }
                    //}

                    //C:\Program Files (x86)\SharpDevelop\4.3\bin\SharpDevelop.exe    
                    // C:\Program Files (x86)\Xamarin Studio\bin\XamarinStudio.exe

                    //Process ide;
                    //    ProcessStartInfo pinfo = new ProcessStartInfo();
                    //    pinfo.FileName = "SharpDevelop.exe";
                    //    pinfo.WorkingDirectory = SceneManager.GameProject.ProjectPath;
                    //    pinfo.UseShellExecute = true;
                    //    ide = Process.Start(pinfo);

                    if (appName.ToLower().Equals("lime"))
                    {
                        Properties.Settings.Default.DefaultScriptEditor = (propertyGrid.SelectedObject as GibboGeneralSettingsDynamic).ScriptEditors.ToString();
                    }

                    if ((propertyGrid.SelectedObject as GibboGeneralSettingsDynamic).ScriptEditors.ToString().ToLower() != "lime")
                        if (EditorUtils.CheckVisualStudioExistance((propertyGrid.SelectedObject as GibboGeneralSettingsDynamic).ScriptEditors.ToString()))
                            Properties.Settings.Default.DefaultScriptEditor = (propertyGrid.SelectedObject as GibboGeneralSettingsDynamic).ScriptEditors.ToString();
                        else
                            MessageBox.Show("You don't have the selected visual studio IDE");
                    else
                        Properties.Settings.Default.DefaultScriptEditor = (propertyGrid.SelectedObject as GibboGeneralSettingsDynamic).ScriptEditors.ToString();
                    
                    
                    Properties.Settings.Default.Save();
                    break;

                case "gibbo_tileset":
                    Properties.Settings.Default.HighlightActiveTileset = (propertyGrid.SelectedObject as GibboTilesetSettingsDynamic).HighlightActiveTileset;
                    break;

                case "game_general":
                    SceneManager.GameProject.ProjectName = (propertyGrid.SelectedObject as GameGeneralSettingsDynamic).ProjectName;
                    break;

                case "game_grid":
                    SceneManager.GameProject.EditorSettings.GridSpacing = (propertyGrid.SelectedObject as GameGridSettingsDynamic).GridSpacing;
                    SceneManager.GameProject.EditorSettings.GridThickness = (propertyGrid.SelectedObject as GameGridSettingsDynamic).GridThickness;
                    SceneManager.GameProject.EditorSettings.GridColor = (propertyGrid.SelectedObject as GameGridSettingsDynamic).GridColor;
                    SceneManager.GameProject.EditorSettings.GridNumberOfLines = (propertyGrid.SelectedObject as GameGridSettingsDynamic).DisplayLines;
                    break;

                case "game_debug":
                    iniSettings.IniWriteValue("Console", "Visible", (propertyGrid.SelectedObject as GameDebugDynamic).ShowConsole.ToString());
                    Properties.Settings.Default.AttachVisualStudio = (propertyGrid.SelectedObject as GameDebugDynamic).Attach;
                    Properties.Settings.Default.Save();
                    SceneManager.GameProject.Debug = (propertyGrid.SelectedObject as GameDebugDynamic).DebugMode == GameDebugDynamic.DebugModes.Debug ? true : false;
                    break;

                case "game_screen":
                    iniSettings.IniWriteValue("Mouse", "Visible",  (propertyGrid.SelectedObject as GameScreenDynamic).MouseVisible.ToString());
                    iniSettings.IniWriteValue("Window", "StartFullScreen",  (propertyGrid.SelectedObject as GameScreenDynamic).StartOnFullScreen.ToString());
                    SceneManager.GameProject.Settings.ScreenWidth = (propertyGrid.SelectedObject as GameScreenDynamic).ScreenWidth;
                    SceneManager.GameProject.Settings.ScreenHeight = (propertyGrid.SelectedObject as GameScreenDynamic).ScreenHeight;
                    //SceneManager.GameProject.ProjectSettings.VSyncEnabled = (propertyGrid.SelectedObject as GameScreenDynamic).VSync;
                    break;
            }
        } 

        private void ProjectsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProjectsListBox.SelectedItem == null || (ProjectsListBox.SelectedItem as ListBoxItem).Tag == null) return;

            SaveCurrent();

            propertyGrid.Tag = (ProjectsListBox.SelectedItem as ListBoxItem).Tag.ToString();
            propertyGrid.SelectedObject = LoadProperties(propertyGrid.Tag.ToString());
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            SaveCurrent();
        }
    }

    interface ISettingsChannelA { }

    class GibboGeneralSettingsDynamic : ISettingsChannelA
    {
        public enum ScriptingEditors { None, Lime, VisualStudio2013, VisualStudio2012, VisualStudio2010 } //CSExpress2010

        private bool automaticProjectLoad;
        private bool startOnFullScreen;
        private bool showDebugView;
        private bool reduceConsumption;
        private ScriptingEditors scriptEditors;

        [Category("General")]
        [DisplayName("Show Debug View")]
        public bool ShowDebugView
        {
            get { return showDebugView; }
            set { showDebugView = value; }
        }

        [Category("General")]
        [DisplayName("Reduce CPU Consumption")]
        public bool ReduceConsumption
        {
            get { return reduceConsumption; }
            set { reduceConsumption = value; }
        }

        [Category("General")]
        [DisplayName("Start Gibbo in Full Screen")]
        public bool StartOnFullScreen
        {
            get { return startOnFullScreen; }
            set { startOnFullScreen = value; }
        }

        [Category("General")]
        [DisplayName("Script Editor")]
        public ScriptingEditors ScriptEditors
        {
            get { return scriptEditors; }
            set { scriptEditors = value; }
        }

        [Category("General")]
        [DisplayName("Load Last Project on Start")]
        public bool AutomaticProjectLoad
        {
            get { return automaticProjectLoad; }
            set { automaticProjectLoad = value; }
        }
    }

    class GibboTilesetSettingsDynamic : ISettingsChannelA
    {
        private bool highlightActiveTileset;

        [Category("Tileset")]
        [DisplayName("Highlight Active Tileset")]
        public bool HighlightActiveTileset
        {
            get { return highlightActiveTileset; }
            set { highlightActiveTileset = value; }
        }
    }

    class GameGeneralSettingsDynamic : ISettingsChannelA
    {
        private string projectName;

        [Category("General")]
        [DisplayName("Project Name")]
        public string ProjectName
        {
            get { return projectName; }
            set { projectName = value; }
        }
    }

    class GameGridSettingsDynamic : ISettingsChannelA
    {
        private int gridSpacing;
        private int gridThickness;
        private int displayLines;

        private Microsoft.Xna.Framework.Color gridColor;

        [Category("Grid")]
        [DisplayName("Grid Spacing")]
        public int GridSpacing
        {
            get { return gridSpacing; }
            set { gridSpacing = value; }
        }

        [Category("Grid")]
        [DisplayName("Grid Thickness")]
        public int GridThickness
        {
            get { return gridThickness; }
            set { gridThickness = value; }
        }

        [Category("Grid")]
        [DisplayName("Display Lines")]
        public int DisplayLines
        {
            get { return displayLines; }
            set { displayLines = value; }
        }

        [Category("Grid")]
        [DisplayName("Grid Color")]
        public Microsoft.Xna.Framework.Color GridColor
        {
            get { return gridColor; }
            set { gridColor = value; }
        }
    }

    class GameDebugDynamic : ISettingsChannelA
    {
        public enum DebugModes { Debug, Release }

        private DebugModes _debugMode;
        private bool _showConsole;
        private bool _attach;

        [Category("Debug")]
        [DisplayName("Debug Mode")]
        public DebugModes DebugMode
        {
            get { return _debugMode; }
            set { _debugMode = value; }
        }

        [Category("Debug")]
        [DisplayName("Show Console")]
        public bool ShowConsole
        {
            get { return _showConsole; }
            set { _showConsole = value; }
        }

        [Category("Debug")]
        [DisplayName("Debug with Visual Studio")]
        [Description("Will atempt to attach solution if the corresponding visual studio instance is found")]
        public bool Attach
        {
            get { return _attach; }
            set { _attach = value; }
        }
    }

    class GameScreenDynamic : ISettingsChannelA
    {
        private bool startOnFullScreen;
        private bool mouseVisible;
        private int screenWidth;
        private int screenHeight;
        //private bool vsync;

        //[Category("Screen")]
        //[DisplayName("Vertical Sync (VSync)")]
        //public bool VSync
        //{
        //    get { return vsync; }
        //    set { vsync = value; }
        //}

        [Category("Screen")]
        [DisplayName("Start on Full Screen")]
        public bool StartOnFullScreen
        {
            get { return startOnFullScreen; }
            set { startOnFullScreen = value; }
        }

        [Category("Screen")]
        [DisplayName("Mouse Visible")]
        public bool MouseVisible
        {
            get { return mouseVisible; }
            set { mouseVisible = value; }
        }

        [Category("Screen")]
        [DisplayName("Screen Width")]
        public int ScreenWidth
        {
            get { return screenWidth; }
            set { screenWidth = value; }
        }

        [Category("Screen")]
        [DisplayName("Screen Height")]
        public int ScreenHeight
        {
            get { return screenHeight; }
            set { screenHeight = value; }
        }
    }
}

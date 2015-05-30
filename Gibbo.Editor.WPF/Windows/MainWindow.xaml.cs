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

using Gibbo.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes; 
using WinInterop = System.Windows.Interop;
using System.Deployment.Application;
using Gibbo.Editor.Model;
using System.Windows.Controls;
using Xceed.Wpf.AvalonDock;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Gibbo.Editor.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region fullScreen
        static bool fullscreen = false;

        public void SetFullScreen(bool setFullscreen)
        {
            fullscreen = setFullscreen;
            setFullScreenName(fullscreen);

            if (fullscreen)
                this.WindowStyle = WindowStyle.None;
            else
                this.WindowStyle = WindowStyle.SingleBorderWindow;

            this.WindowState = WindowState.Minimized;
            this.WindowState = WindowState.Maximized;


        }

        public void setFullScreenName(bool setFullScreen)
        {
            System.Windows.Controls.Grid grid = (this.MainGrid.FindName("TopRightButtonsGrid") as System.Windows.Controls.Grid);

            if (setFullScreen)
                (grid.FindName("fullScreenBtn") as RoundedButton).Content = "Exit FullScreen";
            else
                (grid.FindName("fullScreenBtn") as RoundedButton).Content = "Switch to FullScreen";
        }

        private void fullScreenBtn_Click(object sender, RoutedEventArgs e)
        {
            SetFullScreen(!fullscreen);
        }

        //#region Maximize Handler - testing

        //#region events
        //void win_SourceInitialized(object sender, EventArgs e)
        //{
        //    System.IntPtr handle = (new WinInterop.WindowInteropHelper(this)).Handle;
        //    WinInterop.HwndSource.FromHwnd(handle).AddHook(new WinInterop.HwndSourceHook(WindowProc));
        //}

        //void win_Loaded(object sender, RoutedEventArgs e)
        //{
        //    SetFullScreen(fullscreen);
        //}

        //#endregion

        //private static System.IntPtr WindowProc(System.IntPtr hwnd, int msg, System.IntPtr wParam, System.IntPtr lParam, ref bool handled)
        //{
        //    switch (msg)
        //    {
        //        case 0x0024:
        //            if (!fullscreen)
        //            {
        //                WmGetMinMaxInfo(hwnd, lParam);
        //                handled = true;
        //            }
        //            break;
        //    }

        //    return (System.IntPtr)0;
        //}

        //private static void WmGetMinMaxInfo(System.IntPtr hwnd, System.IntPtr lParam)
        //{

        //    MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));

        //    // Adjust the maximized size and position to fit the work area of the correct monitor
        //    int MONITOR_DEFAULTTONEAREST = 0x00000002;
        //    System.IntPtr monitor = MonitorFromWindow(hwnd, MONITOR_DEFAULTTONEAREST);

        //    if (monitor != System.IntPtr.Zero)
        //    {

        //        MONITORINFO monitorInfo = new MONITORINFO();
        //        GetMonitorInfo(monitor, monitorInfo);
        //        RECT rcWorkArea = monitorInfo.rcWork;
        //        RECT rcMonitorArea = monitorInfo.rcMonitor;
        //        mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left)-1;
        //        mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top)-1;
        //        mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left)+2;
        //        mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top)+2;
        //    }

        //    Marshal.StructureToPtr(mmi, lParam, true);
        //}

        ///// <summary>
        ///// POINT aka POINTAPI
        ///// </summary>
        //[StructLayout(LayoutKind.Sequential)]
        //public struct POINT
        //{
        //    /// <summary>
        //    /// x coordinate of point.
        //    /// </summary>
        //    public int x;
        //    /// <summary>
        //    /// y coordinate of point.
        //    /// </summary>
        //    public int y;

        //    /// <summary>
        //    /// Construct a point of coordinates (x,y).
        //    /// </summary>
        //    public POINT(int x, int y)
        //    {
        //        this.x = x;
        //        this.y = y;
        //    }
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //public struct MINMAXINFO
        //{
        //    public POINT ptReserved;
        //    public POINT ptMaxSize;
        //    public POINT ptMaxPosition;
        //    public POINT ptMinTrackSize;
        //    public POINT ptMaxTrackSize;
        //};

        ///// <summary>
        ///// </summary>
        //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        //public class MONITORINFO
        //{
        //    /// <summary>
        //    /// </summary>            
        //    public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));

        //    /// <summary>
        //    /// </summary>            
        //    public RECT rcMonitor = new RECT();

        //    /// <summary>
        //    /// </summary>            
        //    public RECT rcWork = new RECT();

        //    /// <summary>
        //    /// </summary>            
        //    public int dwFlags = 0;
        //}

        ///// <summary> Win32 </summary>
        //[StructLayout(LayoutKind.Sequential, Pack = 0)]
        //public struct RECT
        //{
        //    /// <summary> Win32 </summary>
        //    public int left;
        //    /// <summary> Win32 </summary>
        //    public int top;
        //    /// <summary> Win32 </summary>
        //    public int right;
        //    /// <summary> Win32 </summary>
        //    public int bottom;

        //    /// <summary> Win32 </summary>
        //    public static readonly RECT Empty = new RECT();

        //    /// <summary> Win32 </summary>
        //    public int Width
        //    {
        //        get { return Math.Abs(right - left); }  // Abs needed for BIDI OS
        //    }
        //    /// <summary> Win32 </summary>
        //    public int Height
        //    {
        //        get { return bottom - top; }
        //    }

        //    /// <summary> Win32 </summary>
        //    public RECT(int left, int top, int right, int bottom)
        //    {
        //        this.left = left;
        //        this.top = top;
        //        this.right = right;
        //        this.bottom = bottom;
        //    }


        //    /// <summary> Win32 </summary>
        //    public RECT(RECT rcSrc)
        //    {
        //        this.left = rcSrc.left;
        //        this.top = rcSrc.top;
        //        this.right = rcSrc.right;
        //        this.bottom = rcSrc.bottom;
        //    }

        //    /// <summary> Win32 </summary>
        //    public bool IsEmpty
        //    {
        //        get
        //        {
        //            // BUGBUG : On Bidi OS (hebrew arabic) left > right
        //            return left >= right || top >= bottom;
        //        }
        //    }
        //    /// <summary> Return a user friendly representation of this struct </summary>
        //    public override string ToString()
        //    {
        //        if (this == RECT.Empty) { return "RECT {Empty}"; }
        //        return "RECT { left : " + left + " / top : " + top + " / right : " + right + " / bottom : " + bottom + " }";
        //    }

        //    /// <summary> Determine if 2 RECT are equal (deep compare) </summary>
        //    public override bool Equals(object obj)
        //    {
        //        if (!(obj is Rect)) { return false; }
        //        return (this == (RECT)obj);
        //    }

        //    /// <summary>Return the HashCode for this struct (not garanteed to be unique)</summary>
        //    public override int GetHashCode()
        //    {
        //        return left.GetHashCode() + top.GetHashCode() + right.GetHashCode() + bottom.GetHashCode();
        //    }


        //    /// <summary> Determine if 2 RECT are equal (deep compare)</summary>
        //    public static bool operator ==(RECT rect1, RECT rect2)
        //    {
        //        return (rect1.left == rect2.left && rect1.top == rect2.top && rect1.right == rect2.right && rect1.bottom == rect2.bottom);
        //    }

        //    /// <summary> Determine if 2 RECT are different(deep compare)</summary>
        //    public static bool operator !=(RECT rect1, RECT rect2)
        //    {
        //        return !(rect1 == rect2);
        //    }


        //}

        //[DllImport("user32")]
        //internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        ///// <summary>
        ///// 
        ///// </summary>
        //[DllImport("User32")]
        //internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
        //#endregion

        #endregion

        #region fields
        string lastLatestProjects = string.Empty;
        string projectPathToLoad = string.Empty;
        SceneViewGameControl sceneViewGameControl;
        GameViewGameControl gameViewGameControl;
        System.Windows.Forms.Timer timer;
        System.Windows.Forms.Timer timerUI;
        EditorModes lastEditorMode;
        WindowsFormsHostOverlay overlay;
        ApplicationDeployment deployment;
        string versionInfo;
        bool gameVideoMode = false;
        System.Windows.Controls.ContextMenu gameViewContextMenu;
        #endregion
        
        #region constructors

        public MainWindow()
        {
            InitializeComponent();

            //this.Loaded += new RoutedEventHandler(win_Loaded);
            //this.SourceInitialized += new EventHandler(win_SourceInitialized);
            SetFullScreen(Properties.Settings.Default.StartOnFullScreen);

            Initialize();

        }

        public MainWindow(string projectPath)
        {
            InitializeComponent();

            SetFullScreen(Properties.Settings.Default.StartOnFullScreen);

            Initialize();

            projectPathToLoad = projectPath;
        }

        #endregion

        #region methods

        private void Initialize()
        {
            this.OutputDataGrid.ItemsSource = EditorHandler.OutputMessages;
            (this.OutputDataGrid.ItemsSource as System.Collections.ObjectModel.ObservableCollection<OutputMessage>).CollectionChanged += MainWindow_CollectionChanged;

            sceneViewGameControl = new SceneViewGameControl();

            sceneViewGameControl.MouseMove += sceneViewGameControl_MouseMove;
            sceneViewGameControl.MouseDown += sceneViewGameControl_MouseDown;
            sceneViewGameControl.MouseUp += sceneViewGameControl_MouseUp;

            SceneViewFormContainer.Child = sceneViewGameControl;

            //gameViewGameControl = new GameViewGameControl();
            //SceneViewGameFormContainer.Child = gameViewGameControl;

            EditorHandler.SceneViewControl = sceneViewGameControl;
            EditorHandler.PropertyGridContainer = PropertyGridContainer;
            EditorHandler.SceneTreeView = sceneTreeView;
            EditorHandler.ProjectTreeView = projectTreeView;
            EditorHandler.UnDoRedo = new Model.UndoRedo();
            EditorHandler.TilesetMenuItems = tilesetMenuItems;
            // Expander's Context Menu
            this.OutputExpander.ContextMenu = new System.Windows.Controls.ContextMenu();

            // TODO change image source
            System.Windows.Controls.MenuItem ClearItem = EditorUtils.CreateMenuItem("Clear", null);

            this.OutputExpander.ContextMenu.Items.Add(ClearItem);

            // event handlers
            ClearItem.Click += ClearItem_Click;
            Closed += MainWindow_Closed;
            ContentRendered += MainWindow_ContentRendered;

            sceneViewGameControl.AllowDrop = true;
            sceneViewGameControl.DragEnter += sceneViewGameControl_DragEnter;
            sceneViewGameControl.DragDrop += sceneViewGameControl_DragDrop;
            
        }

        void sceneViewGameControl_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (SceneManager.ActiveScene == null) return;

            if (e.Data.GetDataPresent(typeof(ExplorerTreeViewItem)))
            {
                FieldInfo info;
                object obj;
                info = e.Data.GetType().GetField("innerData", BindingFlags.NonPublic | BindingFlags.Instance);
                obj = info.GetValue(e.Data);
                info = obj.GetType().GetField("innerData", BindingFlags.NonPublic | BindingFlags.Instance);
                System.Windows.DataObject dataObj = info.GetValue(obj) as System.Windows.DataObject;
                ExplorerTreeViewItem item = dataObj.GetData(typeof(ExplorerTreeViewItem)) as ExplorerTreeViewItem;

                if (EditorUtils.isDirectory(item.FullPath)) return;

                string relativePath = item.FullPath.Replace(SceneManager.GameProject.ProjectPath + "\\", string.Empty);
                string name = System.IO.Path.GetFileNameWithoutExtension(relativePath);

                
                string[] extensions = item.Text.ToLower().Split('.');


                string secundaryExtension = string.Empty;
                if (extensions.Length > 2)
                    secundaryExtension = extensions[extensions.Length - 2];

                GameObject objToAdd = null;

                if (secundaryExtension.ToLower().Equals(Properties.Settings.Default.secundaryExtension))
                {
                    objToAdd = new AnimatedSprite() { ImageName = relativePath, Name = name.ToLower().Replace("." + Properties.Settings.Default.secundaryExtension, ""), TotalFramesPerRow = 1, TotalRows = 1 };
                }
                else
                {
                    string extension = extensions.Last();

                    switch (extension)
                    {
                        case "png":
                        case "jpeg":
                        case "jpg":
                        case "gif":
                        case "bmp":
                            objToAdd = new Sprite() { ImageName = relativePath, Name = name };
                            break;
                        case "mp3":
                        case "wav":
                            objToAdd = new AudioObject() { FilePath = relativePath, Name = name };
                            break;
                    }
                }

                if (objToAdd != null)
                {
                    EditorHandler.SceneTreeView.AddGameObject(objToAdd, "", true);
                    EditorHandler.ChangeSelectedObjects();
                }
                
                
            }
        }

        void sceneViewGameControl_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (SceneManager.ActiveScene == null) return;

            if (e.Data.GetDataPresent(typeof(ExplorerTreeViewItem)))
            {
                e.Effect = System.Windows.Forms.DragDropEffects.Move;
            }
        }

        private void UpdateTilesetModes()
        {
            pencilTilesetBtn.IsChecked = false;
            rectangleTileset.IsChecked = false;
            rectangleRemoveTileset.IsChecked = false;
            addColumnTileset.IsChecked = false;
            addRowTileset.IsChecked = false;
            removeColumnTileset.IsChecked = false;
            removeRowTileset.IsChecked = false;

            switch (sceneViewGameControl.TilesetMode)
            {
                case TilesetModes.Pencil:
                    pencilTilesetBtn.IsChecked = true;
                    break;
                case TilesetModes.Rectangle:
                    rectangleTileset.IsChecked = true;
                    break;
                case TilesetModes.Eraser:
                    rectangleRemoveTileset.IsChecked = true;
                    break;
                case TilesetModes.AddColumn:
                    addColumnTileset.IsChecked = true;
                    break;
                case TilesetModes.RemoveColumn:
                    removeColumnTileset.IsChecked = true;
                    break;
                case TilesetModes.AddRow:
                    addRowTileset.IsChecked = true;
                    break;
                case TilesetModes.RemoveRow:
                    removeRowTileset.IsChecked = true;
                    break;
            }
        }

        private bool compileProject()
        {
            EditorCommands.ApplyBlurEffect(this);

            CompilerWindow cw = new CompilerWindow();

            cw.ShowDialog();
            EditorCommands.ClearEffect(this);
            EditorCommands.CreatePropertyGridView();

            return cw.Result;
        }

        private void debugProject()
        {
            EditorCommands.ApplyBlurEffect(this);
            EditorCommands.DebugGame();
            EditorCommands.ClearEffect(this);
        }

        private void fullDebugProject()
        {
            if (compileProject())
                debugProject();
        }

        #endregion

        #region events

        // TODO: add invalidate at the end so the inspector refreshes
        private void manageTagsBtn_Click(object sender, RoutedEventArgs e)
        {
            ManageTagsWindow window = new ManageTagsWindow();

            window.ShowDialog();
        }

        // Attemps to Reset the Layout by setting Gibbo's default layout as current
        private void ResetLayoutClick(object sender, RoutedEventArgs e)
        {
            EditorCommands.ShowOutputMessage("Attempting to reset the layout");
            LayoutHelper.LoadGibbsoDefaultLayout();

        }

        private void DebugResetClick(object sender, RoutedEventArgs e)
        {
            var serializer = new Xceed.Wpf.AvalonDock.Layout.Serialization.XmlLayoutSerializer(dockManager);
            using (var stream = new StreamWriter(@".\Layout\Default.layout"))
                serializer.Serialize(stream);
        }

        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            LayoutHelper.DockManager = dockManager;

            // Attempts to load the latest used layout
            //if (Properties.Settings.Default.Layout.Equals("Default"))
            //{
            //    // attempts to load Gibbo's default layout
            //    LayoutHelper.LoadGibbsoDefaultLayout();
            //    return;
            //}

            //if (LayoutHelper.LayoutExists(Properties.Settings.Default.Layout))
            //{
            EditorCommands.ShowOutputMessage("Attempting to load User's saved layout");
            if (LayoutHelper.LoadLayout(Properties.Settings.Default.Layout))
            {
                EditorCommands.ShowOutputMessage("User's saved layout has been loaded successfully");
                return;
            }
            else
                EditorCommands.ShowOutputMessage("User's saved layout load attempt has failed");
            //}
        }

        private void LayoutMenuItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            this.CurrentLayoutMenuItem.Header = "Currently Set: " + Properties.Settings.Default.Layout;


        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            LayoutHelper.CreateNewLayout(Properties.Settings.Default.Layout);
            Properties.Settings.Default.Save();

            if (SceneManager.ActiveScene != null)
            {
                MessageBoxResult r = System.Windows.MessageBox.Show("Do you want to save the current scene?", "Warning", MessageBoxButton.YesNoCancel);

                if (r == MessageBoxResult.Yes)
                {
                    SceneManager.SaveActiveScene();
                }
                else if (r == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
            }

            if (!e.Cancel)
            {
                LimeScriptEditor.Instance.Close();
                Application.Current.Shutdown();
            }
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void loadMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EditorCommands.LoadProject();
        }

        void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            //overlay = new WindowsFormsHostOverlay(SceneViewFormContainer, sceneViewGameControl);

            //this.DataContext = new ButtonVisibilityViewModel(this);

            //System.Windows.Controls.Canvas.SetZIndex(overlay, 1);

            if (projectPathToLoad != string.Empty)
            {
                EditorCommands.LoadProject(projectPathToLoad);
                projectPathToLoad = string.Empty;
            }

            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(CurrentDomain_AssemblyResolve);

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1;
            timer.Tick += timer_Tick;
            timer.Enabled = true;

            timerUI = new System.Windows.Forms.Timer();
            timerUI.Interval = 100;
            timerUI.Tick += timerUI_Tick;
            timerUI.Enabled = true;

            EditorHandler.TilesetBrushControl = TilesetControl;

            //propertyGrid.SelectedObject = new Gibbo.Library.AnimatedSprite();

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                deployment = ApplicationDeployment.CurrentDeployment;
                versionInfo = deployment.CurrentVersion.ToString();
            }

            gameViewContextMenu = new System.Windows.Controls.ContextMenu();

            GibboMenuItem panelCreateObjectItem = EditorUtils.CreateMenuItem("Create New Object...", (ImageSource)FindResource("GameObjectIcon_Sprite"));
            GibboMenuItem panelAddFromStateItem = EditorUtils.CreateMenuItem("Add From State...", null);
            GibboMenuItem panelPasteItem = EditorUtils.CreateMenuItem("Paste", (ImageSource)FindResource("PasteIcon"));

            gameViewContextMenu.Items.Add(panelCreateObjectItem);
            gameViewContextMenu.Items.Add(new System.Windows.Controls.Separator());
            gameViewContextMenu.Items.Add(panelAddFromStateItem);
            gameViewContextMenu.Items.Add(new System.Windows.Controls.Separator());
            gameViewContextMenu.Items.Add(panelPasteItem);

            SceneViewFormContainer.ContextMenu = gameViewContextMenu;

            panelCreateObjectItem.Click += createObjectItem_Click;
            panelAddFromStateItem.Click += addFromStateItem_Click;
            panelPasteItem.Click += panelPasteItem_Click;
        }

        void createObjectItem_Click(object sender, RoutedEventArgs e)
        {
            new AddNewItemWindow(null).ShowDialog();
        }

        void addFromStateItem_Click(object sender, RoutedEventArgs e)
        {
            sceneTreeView.AddFromState();
        }

        void panelPasteItem_Click(object sender, RoutedEventArgs e)
        {
            sceneTreeView.Paste();
        }

        void timerUI_Tick(object sender, EventArgs e)
        {
            if (SceneManager.GameProject != null)
            {
                string sceneName = (SceneManager.ActiveScene != null) ? sceneName = System.IO.Path.GetFileNameWithoutExtension(SceneManager.ActiveScenePath) : "";

                this.Title = string.Format("{0} - {1} - {2}", "Gibbo 2D [" + versionInfo + "]",
                    SceneManager.GameProject.ProjectName, sceneName);

                showGridBtn.IsChecked = SceneManager.GameProject.EditorSettings.ShowGrid;
                gridSnappingBtn.IsChecked = SceneManager.GameProject.EditorSettings.SnapToGrid;
                showCollisionsBtn.IsChecked = SceneManager.GameProject.EditorSettings.ShowCollisions;
                debugViewBtn.IsChecked = Properties.Settings.Default.ShowDebugView;
                // Console.WriteLine(sceneEditorControl1.EditorMode + ":" + lastEditorMode);

                if (sceneViewGameControl.EditorMode != lastEditorMode)
                {
                    selectBtn.IsChecked = false;
                    translateBtn.IsChecked = false;
                    rotateBtn.IsChecked = false;
                    scaleBtn.IsChecked = false;

                    if (sceneViewGameControl.EditorMode == EditorModes.Select)
                    {
                        selectBtn.IsChecked = true;
                    }
                    else if (sceneViewGameControl.EditorMode == EditorModes.Move)
                    {
                        translateBtn.IsChecked = true;
                    }
                    else if (sceneViewGameControl.EditorMode == EditorModes.Rotate)
                    {
                        rotateBtn.IsChecked = true;
                    }
                    else if (sceneViewGameControl.EditorMode == EditorModes.Scale)
                    {
                        scaleBtn.IsChecked = true;
                    }

                    lastEditorMode = sceneViewGameControl.EditorMode;
                }
            }
        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //This handler is called only when the common language runtime tries to bind to the assembly and fails.

            //Retrieve the list of referenced assemblies in an array of AssemblyName.
            Assembly objExecutingAssemblies;
            string strTempAssmbPath = "";

            objExecutingAssemblies = Assembly.GetExecutingAssembly();
            AssemblyName[] arrReferencedAssmbNames = objExecutingAssemblies.GetReferencedAssemblies();

            //Loop through the array of referenced assembly names.
            foreach (AssemblyName strAssmbName in arrReferencedAssmbNames)
            {
                //Console.WriteLine("search: " + SceneManager.GameProject.ProjectPath + "\\" + args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll");
                //Console.WriteLine("aa: " + args.Name.Substring(0, args.Name.IndexOf(",")));
                //Check for the assembly names that have raised the "AssemblyResolve" event.
                if (strAssmbName.FullName.Substring(0, strAssmbName.FullName.IndexOf(",")) == args.Name.Substring(0, args.Name.IndexOf(",")))
                {
                    //Build the path of the assembly from where it has to be loaded.				
                    //strTempAssmbPath = "C:\\Myassemblies\\" + args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
                    strTempAssmbPath = SceneManager.GameProject.ProjectPath + "\\" + args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
                    break;
                }
            }

            if (strTempAssmbPath == "")
            {
                foreach (string fileName in Directory.GetFiles(SceneManager.GameProject.ProjectPath + "\\libs\\"))
                {
                    string asmName = System.IO.Path.GetFileName(fileName);
                    //Console.WriteLine("search: " + asmName.Replace(".dll", "") + " == " + args.Name.Substring(0, args.Name.IndexOf(",")));
                    if (asmName.Replace(".dll", "") == args.Name.Substring(0, args.Name.IndexOf(",")))
                    {
                        // Console.WriteLine("entrei");
                        strTempAssmbPath = SceneManager.GameProject.ProjectPath + "\\libs\\" + asmName;
                        break;
                    }
                }
            }

            if (strTempAssmbPath == "")
            {
                return SceneManager.ScriptsAssembly;
            }

            //Load and return the loaded assembly.
            return Assembly.LoadFrom(strTempAssmbPath);
        }

        void MainWindow_Closed(object sender, EventArgs e)
        {
            System.Environment.Exit(0);
        }

        private void newProjectMenuItem_Click(object sender, RoutedEventArgs e)
        {
            NewProjectWindow np = new NewProjectWindow();
            np.ShowDialog();

            // A project was created?
            if (np.DialogResult.Value)
            {
                //Reload();
                EditorCommands.LoadProject(np.ProjectPath);
                EditorCommands.AddToProjectHistory(np.ProjectPath);
            }
        }

        private void saveProjectBtn_Click(object sender, RoutedEventArgs e)
        {
            EditorCommands.SaveProject();
            EditorCommands.SaveScene(false);
        }

        private void exportSceneXMLBtn_Click(object sender, RoutedEventArgs e)
        {
            EditorCommands.SaveProject();

            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog();
            sfd.InitialDirectory = SceneManager.GameProject.ProjectPath;
            sfd.Filter = "(*.xml)|*.xml";
            System.Windows.Forms.DialogResult dr = sfd.ShowDialog();

            if (dr == System.Windows.Forms.DialogResult.Yes|| dr == System.Windows.Forms.DialogResult.OK)
            {
                SceneManager.SaveActiveScene(sfd.FileName, true);
            }
        }

        private void saveSceneBtn_Click(object sender, RoutedEventArgs e)
        {
            SceneManager.SaveActiveScene();
            EditorCommands.ShowOutputMessage("Game Scene Saved");
        }

        private void deployBtn_Click(object sender, RoutedEventArgs e)
        {
            //foreach (string dirPath in Directory.GetDirectories(SceneManager.GameProject.ProjectPath, "*",
            //    SearchOption.AllDirectories))
            //    Directory.CreateDirectory(dirPath.Replace(SceneManager.GameProject.ProjectPath, @"C:\Users\John\Desktop\output\"));

            ////Copy all the files
            //foreach (string newPath in Directory.GetFiles(SceneManager.GameProject.ProjectPath, "*.*",
            //    SearchOption.AllDirectories))
            //{
            //    if (newPath.ToLower().EndsWith(".scene"))
            //    {
            //        GameScene scene = (GameScene)GibboHelper.DeserializeObject(newPath);
            //        GibboHelper.SerializeObjectXML(newPath.Replace(SceneManager.GameProject.ProjectPath, @"C:\Users\John\Desktop\output\"), scene);
            //    }
            //    else if (newPath.ToLower().EndsWith(".gibbo"))
            //    {
            //        GibboProject gp = (GibboProject)GibboHelper.DeserializeObject(newPath);
            //        GibboHelper.SerializeObjectXML(newPath.Replace(SceneManager.GameProject.ProjectPath, @"C:\Users\John\Desktop\output\"), gp);
            //    }
            //    else
            //    {
            //        File.Copy(newPath, newPath.Replace(SceneManager.GameProject.ProjectPath, @"C:\Users\John\Desktop\output\"), true);
            //    }
            //}

            new DeploymentWindow().ShowDialog();
        }

        private void undoBtn_Click(object sender, RoutedEventArgs e)
        {
            EditorHandler.UnDoRedo.Undo(1);
        }

        private void redoBtn_Click(object sender, RoutedEventArgs e)
        {
            EditorHandler.UnDoRedo.Redo(1);
        }

        private void debugBtn_Click(object sender, RoutedEventArgs e)
        {
            debugProject();
        }

        private void compileBtn_Click(object sender, RoutedEventArgs e)
        {
            compileProject();
        }

        private void selectBtn_Click(object sender, RoutedEventArgs e)
        {
            sceneViewGameControl.EditorMode = EditorModes.Select;
        }

        private void translateBtn_Click(object sender, RoutedEventArgs e)
        {
            sceneViewGameControl.EditorMode = EditorModes.Move;
        }

        private void rotateBtn_Click(object sender, RoutedEventArgs e)
        {
            sceneViewGameControl.EditorMode = EditorModes.Rotate;
        }

        private void scaleBtn_Click(object sender, RoutedEventArgs e)
        {
            sceneViewGameControl.EditorMode = EditorModes.Scale;
        }


        private void gameViewBtn_Click(object sender, RoutedEventArgs e)
        {
            //if(!gameVideoMode)
            //    SceneManager.SaveActiveScene();

            //gameVideoMode = !gameVideoMode;
            //gameViewBtn.IsChecked = gameVideoMode;

            //if (!gameVideoMode)
            //{
            //    // Editor Mode
            //    SceneViewTabItem.IsSelected = true;
            //    SceneManager.IsEditor = true;

            //    //EditorCommands.CreatePropertyGridView();
            //}
            //else
            //{
            //    // Game Mode
            //    GameViewTabItem.IsSelected = true;
            //    SceneManager.IsEditor = false;
            //}
        }

        private void centerCameraObjectBtn_Click(object sender, RoutedEventArgs e)
        {
            sceneViewGameControl.CenterCameraObject();
        }

        private void centerCameraBtn_Click(object sender, RoutedEventArgs e)
        {
            sceneViewGameControl.CenterCamera();
        }

        private void showGridBtn_Click(object sender, RoutedEventArgs e)
        {
            SceneManager.GameProject.EditorSettings.ShowGrid = !SceneManager.GameProject.EditorSettings.ShowGrid;
        }

        private void gridSnappingBtn_Click(object sender, RoutedEventArgs e)
        {
            SceneManager.GameProject.EditorSettings.SnapToGrid = !SceneManager.GameProject.EditorSettings.SnapToGrid;
        }

        private void showCollisionsBtn_Click(object sender, RoutedEventArgs e)
        {
            SceneManager.GameProject.EditorSettings.ShowCollisions = !SceneManager.GameProject.EditorSettings.ShowCollisions;
        }

        private void MenuItem_tutorial_click(object sender, RoutedEventArgs e)
        {
            TutorialsListWindow tutoListWindow = new TutorialsListWindow();
            tutoListWindow.ShowDialog();
        }

        private void debugViewBtn_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.ShowDebugView = !Properties.Settings.Default.ShowDebugView;
            Properties.Settings.Default.Save();
        }

        private void refreshSceneBtn_Click(object sender, RoutedEventArgs e)
        {
            sceneTreeView.CreateView();
        }


        private void refreshProjectExplorerBtn_Click(object sender, RoutedEventArgs e)
        {
            projectTreeView.CreateView();
        }

        private void addNewItemSceneBtn_Click(object sender, RoutedEventArgs e)
        {
            if (SceneManager.ActiveScene == null)
            {
                System.Windows.MessageBox.Show("No scene loaded!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                AddNewItemWindow window = new AddNewItemWindow(sceneTreeView.SelectedItem);
                window.ShowDialog();
            }
        }

        private void GridSplitter_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {

        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void sceneSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            EditorHandler.ChangeSelectedObject(SceneManager.ActiveScene);
        }

        private void clearTextureBuffBtn_Click(object sender, RoutedEventArgs e)
        {
            TextureLoader.Clear();
        }

        private void aboutBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://gibbo2d.anlagehub.com/index.php/main/display/about");
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Whitebeard86/Gibbo2D");           
        }

        private void websiteBtn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://anlagehub.com/gibbo2d/");
        }

        private void ToCollisionBlock()
        {
            if (sceneViewGameControl.SelectionArea != Microsoft.Xna.Framework.Rectangle.Empty)
            {
                GameObject gameObject = new GameObject();
                gameObject.Name = "Collision Block";

                gameObject.Transform.Position = new Microsoft.Xna.Framework.Vector2(sceneViewGameControl.SelectionArea.X + sceneViewGameControl.SelectionArea.Width / 2, sceneViewGameControl.SelectionArea.Y + sceneViewGameControl.SelectionArea.Height / 2);

                SceneManager.ActiveScene.GameObjects.Add(gameObject);

                RectangleBody body = new RectangleBody();
                body.EditorExpanded = true;

                gameObject.AddComponent(body);

                body.Width = sceneViewGameControl.SelectionArea.Width;
                body.Height = sceneViewGameControl.SelectionArea.Height;
                body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;

                sceneTreeView.CreateView();

                EditorHandler.SelectedGameObjects = new List<GameObject>();
                EditorHandler.SelectedGameObjects.Add(gameObject);
                EditorHandler.ChangeSelectedObjects();
            }
            else
                System.Windows.MessageBox.Show("No valid selection provided, please make a screen selection before executing this command", "Error");
        }

        private void convertToCollision_Click(object sender, RoutedEventArgs e)
        {
            ToCollisionBlock();
        }

        private void selectAll_Click(object sender, RoutedEventArgs e)
        {
            EditorHandler.SelectedGameObjects = GameObject.GetAllGameObjects();
            EditorHandler.ChangeSelectedObjects();
        }

        private void clearSelection_Click(object sender, RoutedEventArgs e)
        {
            EditorHandler.SelectedGameObjects.Clear();
            EditorHandler.ChangeSelectedObjects();
        }

        private void rotate90º_Click(object sender, RoutedEventArgs e)
        {
            foreach (GameObject gameObject in EditorHandler.SelectedGameObjects)
            {
                EditorHandler.UnDoRedo.InsertInUnDoRedoForRotate(gameObject.Transform.Rotation + (float)Math.PI / 2,
                    gameObject.Transform.Rotation, gameObject);

                gameObject.Transform.Rotation += (float)Math.PI / 2;
            }
        }

        private void rotate180º_Click(object sender, RoutedEventArgs e)
        {
            foreach (GameObject gameObject in EditorHandler.SelectedGameObjects)
            {
                EditorHandler.UnDoRedo.InsertInUnDoRedoForRotate(gameObject.Transform.Rotation + (float)Math.PI,
                            gameObject.Transform.Rotation, gameObject);

                gameObject.Transform.Rotation += (float)Math.PI;
            }
        }

        private void rotate90ºInverse_Click(object sender, RoutedEventArgs e)
        {
            foreach (GameObject gameObject in EditorHandler.SelectedGameObjects)
            {
                EditorHandler.UnDoRedo.InsertInUnDoRedoForRotate(gameObject.Transform.Rotation + (float)Math.PI / 2,
                    gameObject.Transform.Rotation, gameObject);

                gameObject.Transform.Rotation -= (float)Math.PI / 2;
            }
        }

        private void rotate45º_Click(object sender, RoutedEventArgs e)
        {
            foreach (GameObject gameObject in EditorHandler.SelectedGameObjects)
            {
                EditorHandler.UnDoRedo.InsertInUnDoRedoForRotate(gameObject.Transform.Rotation + (float)Math.PI / 4,
                    gameObject.Transform.Rotation, gameObject);

                gameObject.Transform.Rotation += (float)Math.PI / 4;
            }
        }

        private void rotate45ºInverse_Click(object sender, RoutedEventArgs e)
        {
            foreach (GameObject gameObject in EditorHandler.SelectedGameObjects)
            {
                EditorHandler.UnDoRedo.InsertInUnDoRedoForRotate(gameObject.Transform.Rotation - (float)Math.PI / 4,
                    gameObject.Transform.Rotation, gameObject);

                gameObject.Transform.Rotation -= (float)Math.PI / 4;
            }
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            foreach (GameObject gameObject in EditorHandler.SelectedGameObjects)
            {
                EditorHandler.UnDoRedo.InsertInUnDoRedoForRotate(0,
                    gameObject.Transform.Rotation, gameObject);

                gameObject.Transform.Rotation = 0;
            }
        }

        private void numericScale_Click(object sender, RoutedEventArgs e)
        {
            if (EditorHandler.SelectedGameObjects.Count > 0)
            {
                // CHECK: o numericScale ainda não foi feito, penso
                //new NumericScale().ShowDialog();
            }
            else
            {
                System.Windows.MessageBox.Show("No object selected", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void bmFont_Click(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "bmfont\\bmfont.exe";
            startInfo.Arguments = "bmfont\\bmsettings.bmfc";
            Process.Start(startInfo);
        }

        private void checkForUpdatesBtn_Click(object sender, RoutedEventArgs e)
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment updateCheck = ApplicationDeployment.CurrentDeployment;
                UpdateCheckInfo info = updateCheck.CheckForDetailedUpdate();

                if (info.UpdateAvailable)
                {
                    if (System.Windows.Forms.MessageBox.Show("There is a new build available [" + info.AvailableVersion.ToString() + "]\nCurrent Version [" + updateCheck.CurrentVersion.ToString() + "]\n\nDo you want download?", "Gibbo 2D Software", System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        updateCheck.Update();
                        System.Windows.MessageBox.Show("The application has been updated, and will now restart.", "Gibbo 2D Software");
                        System.Windows.Forms.Application.Restart();
                        System.Windows.Application.Current.Shutdown();
                        //System.Diagnostics.Process.Start("http://www.dragon-scale-studios.com/downloads/gibbo2d/setup.exe");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("There is no update available.", "Gibbo 2D Software");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("This option is not available for DEBUG builds.", "Gibbo 2D Software");
            }
        }

        private void projectSettingsBtn_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow().ShowDialog();
        }


        void sceneViewGameControl_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                sceneViewGameControl.LeftMouseKeyDown = false;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                sceneViewGameControl.MiddleMouseKeyDown = false;
            }
        }

        void sceneViewGameControl_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            sceneViewGameControl.SetMousePosition(new Microsoft.Xna.Framework.Vector2(e.X, e.Y));
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (gameVideoMode)
            {
                gameViewGameControl.Invalidate();
            }
            else
            {
                sceneViewGameControl.Invalidate();
            }
        }

        void sceneViewGameControl_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            sceneViewGameControl.Focus();

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                sceneViewGameControl.LeftMouseKeyDown = true;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Middle)
            {
                sceneViewGameControl.MiddleMouseKeyDown = true;
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                SceneViewFormContainer.ContextMenu.IsOpen = true;
            }
        }

        private void Window_KeyDown_1(object sender, System.Windows.Input.KeyEventArgs e)
        {

            // 
            // shortcuts
            switch (e.Key)
            {
                case Key.F11:
                    SetFullScreen(!fullscreen);
                    break;
                case Key.F5:
                    debugProject();
                    break;
                case Key.F6:
                    compileProject();
                    break;
                default:
                    break;
            }

        }

        private void SaveProject(object sender, ExecutedRoutedEventArgs e)
        {
            EditorCommands.SaveProject();
            EditorCommands.SaveScene(false);
        }

        private void UndoKeyBinding(object sender, ExecutedRoutedEventArgs e)
        {
            EditorHandler.UnDoRedo.Undo(1);
        }

        private void RedoKeyBinding(object sender, ExecutedRoutedEventArgs e)
        {
            EditorHandler.UnDoRedo.Redo(1);
        }

        private void CopyKeyBinding(object sender, ExecutedRoutedEventArgs e)
        {
            if (SceneViewFormContainer.IsFocused || sceneTreeView.canCopyPaste)
            {
                EditorCommands.CopySelectedObjects();
            }
        }

        private void PasteKeyBinding(object sender, ExecutedRoutedEventArgs e)
        {
            if (SceneViewFormContainer.IsFocused || sceneTreeView.canCopyPaste)
            {
                EditorCommands.PasteSelectedObjects();
            }
        }

        private void refreshTileset_Click(object sender, RoutedEventArgs e)
        {
            if (EditorHandler.SelectedGameObjects.Count > 0 && EditorHandler.SelectedGameObjects[0] is Tileset)
            {
                EditorHandler.TilesetBrushControl.ChangeImageSource((EditorHandler.SelectedGameObjects[0] as Tileset).ImageName);
            }
        }



        private void pencilTilesetBtn_Click(object sender, RoutedEventArgs e)
        {
            sceneViewGameControl.TilesetMode = TilesetModes.Pencil;
            UpdateTilesetModes();
        }

        private void rectangleTileset_Click(object sender, RoutedEventArgs e)
        {
            sceneViewGameControl.TilesetMode = TilesetModes.Rectangle;
            UpdateTilesetModes();
        }

        private void rectangleRemoveTileset_Click(object sender, RoutedEventArgs e)
        {
            sceneViewGameControl.TilesetMode = TilesetModes.Eraser;
            UpdateTilesetModes();
        }

        private void addColumnTileset_Click(object sender, RoutedEventArgs e)
        {
            sceneViewGameControl.TilesetMode = TilesetModes.AddColumn;
            UpdateTilesetModes();
        }

        private void addRowTileset_Click(object sender, RoutedEventArgs e)
        {
            sceneViewGameControl.TilesetMode = TilesetModes.AddRow;
            UpdateTilesetModes();
        }

        private void removeColumnTileset_Click(object sender, RoutedEventArgs e)
        {
            sceneViewGameControl.TilesetMode = TilesetModes.RemoveColumn;
            UpdateTilesetModes();
        }

        private void removeRowTileset_Click(object sender, RoutedEventArgs e)
        {
            sceneViewGameControl.TilesetMode = TilesetModes.RemoveRow;
            UpdateTilesetModes();
        }

        private void shiftLeftTileset_Click(object sender, RoutedEventArgs e)
        {
            if (EditorHandler.SelectedGameObjects.Count == 0 || !(EditorHandler.SelectedGameObjects[0] is Tileset)) return;

            Tile[,] _tiles = (EditorHandler.SelectedGameObjects[0] as Tileset).DeepCopy();

            (EditorHandler.SelectedGameObjects[0] as Tileset).ShiftLeft(1);

            TilesetCommand tc = new TilesetCommand((EditorHandler.SelectedGameObjects[0] as Tileset).Tiles, _tiles, (EditorHandler.SelectedGameObjects[0] as Tileset));
            EditorHandler.UnDoRedo.InsertUndoRedo(tc);
        }

        private void shiftUpTileset_Click(object sender, RoutedEventArgs e)
        {
            if (EditorHandler.SelectedGameObjects.Count == 0 || !(EditorHandler.SelectedGameObjects[0] is Tileset)) return;

            Tile[,] _tiles = (EditorHandler.SelectedGameObjects[0] as Tileset).DeepCopy();

            (EditorHandler.SelectedGameObjects[0] as Tileset).ShiftUp(1);

            TilesetCommand tc = new TilesetCommand((EditorHandler.SelectedGameObjects[0] as Tileset).Tiles, _tiles, (EditorHandler.SelectedGameObjects[0] as Tileset));
            EditorHandler.UnDoRedo.InsertUndoRedo(tc);
        }

        private void shiftRightTileset_Click(object sender, RoutedEventArgs e)
        {
            if (EditorHandler.SelectedGameObjects.Count == 0 || !(EditorHandler.SelectedGameObjects[0] is Tileset)) return;

            Tile[,] _tiles = (EditorHandler.SelectedGameObjects[0] as Tileset).DeepCopy();

            (EditorHandler.SelectedGameObjects[0] as Tileset).ShiftRight(1);

            TilesetCommand tc = new TilesetCommand((EditorHandler.SelectedGameObjects[0] as Tileset).Tiles, _tiles, (EditorHandler.SelectedGameObjects[0] as Tileset));
            EditorHandler.UnDoRedo.InsertUndoRedo(tc);
        }

        private void shiftDownTileset_Click(object sender, RoutedEventArgs e)
        {
            if (EditorHandler.SelectedGameObjects.Count == 0 || !(EditorHandler.SelectedGameObjects[0] is Tileset)) return;

            Tile[,] _tiles = (EditorHandler.SelectedGameObjects[0] as Tileset).DeepCopy();

            (EditorHandler.SelectedGameObjects[0] as Tileset).ShiftDown(1);

            TilesetCommand tc = new TilesetCommand((EditorHandler.SelectedGameObjects[0] as Tileset).Tiles, _tiles, (EditorHandler.SelectedGameObjects[0] as Tileset));
            EditorHandler.UnDoRedo.InsertUndoRedo(tc);
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("documentation.chm");
        }

        void MainWindow_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            this.OutputDataGrid.Items.Refresh();
            if (OutputDataGrid.Items.Count > 0)
                this.OutputDataGrid.ScrollIntoView(OutputDataGrid.Items[OutputDataGrid.Items.Count - 1]);
        }

        void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            //if (sender != null)
            //{

            //    System.Windows.Controls.DataGridRow row = sender as System.Windows.Controls.DataGridRow;
            //    if (row != null)
            //    {
            //        OutputMessage item = row.Item as OutputMessage;
            //        string time = item.Time;
            //        string message = item.Message;
            //    }
            //}
        }

        void ClearItem_Click(object sender, RoutedEventArgs e)
        {
            if (EditorHandler.OutputMessages.Count > 0)
                EditorHandler.OutputMessages.Clear();
        }

        private void OutputExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            SceneViewContainer.RowDefinitions[3].Height = new GridLength(20);
        }

        private void OutputExpander_Expanded(object sender, RoutedEventArgs e)
        {
            SceneViewContainer.RowDefinitions[3].Height = new GridLength(100);
        }

        private void fullDebug_Click_1(object sender, RoutedEventArgs e)
        {
            fullDebugProject();
        }

        private void CommandBinding_Executed_1(object sender, ExecutedRoutedEventArgs e)
        {
            fullDebugProject();
        }

        private void CommandBinding_Executed_2(object sender, ExecutedRoutedEventArgs e)
        {
            ToCollisionBlock();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            fullDebugProject();
        }

        private void MenuItem_PreviewMouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            if (lastLatestProjects == Properties.Settings.Default.LastLoadedProjects)
                return;

            lastLatestProjects = Properties.Settings.Default.LastLoadedProjects;

            loadRecentMenuItem.Items.Clear();

            if (Properties.Settings.Default.LastLoadedProjects.Trim() != string.Empty)
            {
                string[] splitted = Properties.Settings.Default.LastLoadedProjects.Split('|');
                int c = 0;
                foreach (string split in splitted)
                {
                    if (split.Trim() != string.Empty && File.Exists(split))
                    {
                        MenuItem item = new MenuItem();

                        item.Header = GibboHelper.SplitCamelCase(System.IO.Path.GetDirectoryName(split).Split('\\').Last());
                        item.Width = 200;

                        item.Click += ((s, _e) =>
                        {
                            EditorCommands.AddToProjectHistory(split);
                            EditorCommands.LoadProject(split);
                        });

                        loadRecentMenuItem.Items.Add(item);
                    }

                    c++;
                    if (c == 8)
                        break;
                }
            }
            else
            {
                loadRecentMenuItem.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            EditorHandler.SceneViewControl.TakeScreenshot();
        }

        private void ThumbScreenshotButton_Click(object sender, EventArgs e)
        {
            EditorHandler.SceneViewControl.TakeScreenshot();
        }

        private void VisualScriptingMenuItem_Click(object sender, RoutedEventArgs e)
        {
            VisualScriptingAnchorable.Show();
        }

        #endregion

        
    }
}

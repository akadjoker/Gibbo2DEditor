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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Gibbo.Library;
using System.Globalization;
using System.Threading.Tasks;

namespace Gibbo.Editor.WPF.Controls
{
    /// <summary>
    /// Interaction logic for TilesetBrushControl.xaml
    /// </summary>
    public partial class TilesetBrushControl : UserControl
    {
        #region fields

        private string imagePath = string.Empty;
        private Point selectionStartPoint = new Point(0,0);
        private Rect selectionRectangle = Rect.Empty;
        private bool selectionStarted;
        private int brushSizeX;
        private int brushSizeY;
        private BitmapImage image;

        #endregion

        #region properties

        public int BrushSizeX
        {
            get { return brushSizeX; }
            set
            {
                brushSizeX = value;
                DrawingCanvas.BrushSizeX = value;
            }
        }

        public int BrushSizeY
        {
            get { return brushSizeY; }
            set
            {
                brushSizeY = value;
                DrawingCanvas.BrushSizeY = value;
            }
        }

        public Rect SelectionRectangle
        {
            get { return selectionRectangle; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Microsoft.Xna.Framework.Rectangle CurrentSelectionXNA
        {
            get
            {
                return new Microsoft.Xna.Framework.Rectangle()
                {
                    X = (int)selectionRectangle.X,
                    Y = (int)selectionRectangle.Y,
                    Width = (int)selectionRectangle.Width,
                    Height = (int)selectionRectangle.Height
                };
            }
        }

        #endregion

        public TilesetBrushControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">The path to the image</param>
        public void ChangeImageSource(string path)
        {
            this.imagePath = path;

            if (!File.Exists(SceneManager.GameProject.ProjectPath + "\\" + path))
            {
                image = new BitmapImage();
                ImageHolder.Source = null;
                return;
            }

            Task t = new Task(() =>
            {
                image = new BitmapImage(new Uri(SceneManager.GameProject.ProjectPath + "\\" + path));
                image = EditorUtils.ConvertBitmapToImage96DPI(image);
            });

            t.RunSynchronously();

            DrawingCanvas.Width = image.Width;
            DrawingCanvas.Height = image.Height;
            ImageHolder.Source = image;

            selectionRectangle = Rect.Empty;

            this.InvalidateVisual();
        }

        /// <summary>
        /// Changes the brush size of the brushControl
        /// </summary>
        /// <param name="brushSizeX">Width</param>
        /// <param name="brushSizeY">Height</param>
        public void ChangeSelectionSize(int brushSizeX, int brushSizeY)
        {
            BrushSizeX = brushSizeX;
            BrushSizeY = brushSizeY;
        }

        private void UpdateSelection(MouseButtonEventArgs e)
        {
            if (image == null) return;


            try
            {
                int px = (int)e.GetPosition(ImageHolder).X;
                int py = (int)e.GetPosition(ImageHolder).Y;

                if (px < this.image.Width - 1 && py < this.image.Height - 1 && px > 0 && py > 0)
                {
                    int x, y, width, height;

                    int ex = px / brushSizeX;
                    int ey = py / brushSizeY;

                    x = (int)selectionStartPoint.X;
                    y = (int)selectionStartPoint.Y;

                    width = 1;
                    height = 1;

                    if (ex > selectionStartPoint.X) width = ex - (int)selectionStartPoint.X + 1;
                    else if (ex < selectionStartPoint.X)
                    {
                        x = ex;
                        width = ((int)selectionStartPoint.X - x) + 1;
                    }
                    if (ey > selectionStartPoint.Y) height = ey - (int)selectionStartPoint.Y + 1;
                    else if (ey < selectionStartPoint.Y)
                    {
                        y = ey;
                        height = ((int)selectionStartPoint.Y - y) + 1;
                    }

                    selectionRectangle = new Rect(x * brushSizeX, y * brushSizeY, width * brushSizeX, height * brushSizeY);
                    DrawingCanvas.Selection = selectionRectangle;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void imageHolder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (brushSizeX == 0 || brushSizeY == 0) return;

            if (!this.selectionStarted)
            {
                selectionStartPoint.X = (int)e.GetPosition(ImageHolder).X / brushSizeX;
                selectionStartPoint.Y = (int)e.GetPosition(ImageHolder).Y / brushSizeY;

                this.selectionStarted = true;

                UpdateSelection(e);
            }
        }

        private void ImageHolder_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.selectionStarted)
            {
                UpdateSelection(new MouseButtonEventArgs(e.MouseDevice, e.Timestamp, MouseButton.Left));
            }
        }

        private void ImageHolder_MouseUp(object sender, MouseButtonEventArgs e)
        {
            selectionStarted = false;
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            if (EditorHandler.SelectedGameObjects.Count > 0 &&
                EditorHandler.SelectedGameObjects[0] is Tileset)
            {
                ImageHolder.Visibility = System.Windows.Visibility.Visible;
                DrawingCanvas.Visibility = System.Windows.Visibility.Visible;

                BrushSizeX = (EditorHandler.SelectedGameObjects[0] as Tileset).TileWidth;
                BrushSizeY = (EditorHandler.SelectedGameObjects[0] as Tileset).TileHeight;

                DrawingCanvas.InvalidateVisual();
            }
            else
            {
                ImageHolder.Visibility = System.Windows.Visibility.Collapsed;
                DrawingCanvas.Visibility = System.Windows.Visibility.Collapsed;

                dc.DrawText(new FormattedText("No tileset selected.", CultureInfo.CurrentCulture, System.Windows.FlowDirection.LeftToRight, new Typeface("Segoe UI"), 12, new System.Windows.Media.BrushConverter().ConvertFromString("#666") as Brush), new Point(15,10));
            }
        }

        private void DrawingCanvas_MouseEnter(object sender, MouseEventArgs e)
        {
            this.InvalidateVisual();
        }
    }
}

#region Copyrights
/*
Gibbo2D - Copyright - 2013 Gibbo2D Team
Founders - Joao Alves <joao.cpp.sca@gmail.com> and Luis Fernandes <luisapidcloud@hotmail.com>

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE. 
*/
#endregion
using System;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;

namespace Gibbo.Library
{
    /// <summary>
    /// The Scene Manager.
    /// This class is responsable for managing the active game scene.
    /// </summary>
    public static class SceneManager
    {
        #region fields

        private static Game gameWindow;
        private static GameScene activeScene;
        private static ContentManager content;
        private static SpriteBatch spriteBatch;
        private static GraphicsDeviceManager graphics;
        private static GraphicsDevice graphicsDevice;
        private static Assembly scriptsAssembly;
        private static Camera activeCamera;

        private static bool isEditor = true;
        private static Tileset activeTileset = null;

        internal static int drawPass = 0;
        private static int fpsCount = 0;
        private static int fps = 0;
        private static float deltaFPSTime = 0f;

        private static string[] gameArgs = new string[0];
        private static string[] gameExtra = new string[0];

        #endregion

        #region properties

        /// <summary>
        /// 
        /// </summary>
        public static string[] GameArgs
        {
            get { return SceneManager.gameArgs; }
            set { SceneManager.gameArgs = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string[] GameExtra
        {
            get { return SceneManager.gameExtra; }
            set { SceneManager.gameExtra = value; }
        }

        /// <summary>
        /// The current Draw Pass Index
        /// </summary>
        public static int DrawPass
        {
            get { return SceneManager.drawPass; }
        }

        /// <summary>
        /// The active game window.
        /// Only available in the game mode
        /// </summary>
        public static Game GameWindow
        {
            get { return SceneManager.gameWindow; }
            set { SceneManager.gameWindow = value; }
        }

        /// <summary>
        /// The current frame per second rate
        /// </summary>
        public static int FPS
        {
            get { return fps; }
            set { fps = value; }
        }

        /// <summary>
        /// The active tileset
        /// </summary>
        public static Tileset ActiveTileset
        {
            get { return activeTileset; }
            set { activeTileset = value; }
        }

        /// <summary>
        /// The active camera
        /// </summary>
        public static Camera ActiveCamera
        {
            get
            {
                if (!IsEditor && ActiveScene != null)
                {
                    return ActiveScene.Camera;
                }
                else
                {
                    return SceneManager.activeCamera;
                }
            }
            set { 
                SceneManager.activeCamera = value;

                if (!IsEditor && ActiveScene != null)
                {
                    ActiveScene.Camera = value;
                }
            }
        }

        /// <summary>
        /// Determines if we are running in editor mode
        /// </summary>
        public static bool IsEditor
        {
            get { return SceneManager.isEditor; }
            set { SceneManager.isEditor = value; }
        }

        /// <summary>
        /// Gets or sets the current game project.
        /// </summary>
        public static GibboProject GameProject { get; set; }

        /// <summary>
        /// Gets or Sets the active game scene
        /// </summary>
        public static GameScene ActiveScene
        {
            get { return activeScene; }
            set
            {
                if (value == null)
                {
                    activeScene = null;
                    return;
                }

                if (content == null)
                    throw new Exception("Cannot add scene, content manager is not assigned");

                if (spriteBatch == null)
                    throw new Exception("Cannot add scene, sprite batch is not assigned");

                if (activeScene != null)
                    activeScene.Dispose();

                activeScene = value;

                value.Content = content;
                value.SpriteBatch = spriteBatch;
                value.Graphics = graphics;

                value.Initialize();
                value.LoadContent();
     
                activeTileset = null;
            }
        }

        /// <summary>
        /// The active content manager
        /// </summary>
        public static ContentManager Content
        {
            get { return SceneManager.content; }
            set { SceneManager.content = value; }
        }

        /// <summary>
        /// The active spritebatch
        /// </summary>
        public static SpriteBatch SpriteBatch
        {
            get { return SceneManager.spriteBatch; }
            set { SceneManager.spriteBatch = value; }
        }

        /// <summary>
        /// The active graphics device manager
        /// </summary>
        public static GraphicsDeviceManager Graphics
        {
            get { return SceneManager.graphics; }
            set { SceneManager.graphics = value; }
        }

        /// <summary>
        /// The active graphics device
        /// </summary>
        public static GraphicsDevice GraphicsDevice
        {
            get { return SceneManager.graphicsDevice; }
            set { SceneManager.graphicsDevice = value; }
        }

        /// <summary>
        /// The current active scene path
        /// </summary>
        public static string ActiveScenePath { get; set; }

        /// <summary>
        /// The active scripts assembly reference
        /// </summary>
        public static Assembly ScriptsAssembly
        {
            get { return SceneManager.scriptsAssembly; }
            set { SceneManager.scriptsAssembly = value; }
        }

        #endregion

        #region methods

        /// <summary>
        /// Update of the active scene's logic.
        /// The collision engine and game input are also updated.
        /// </summary>
        /// <param name="gameTime">The gametime</param>
        public static void Update(GameTime gameTime)
        {
            GameInput.Update();
            Log.Update(gameTime);

            deltaFPSTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (deltaFPSTime >= 1)
            {
                fps = fpsCount;
                fpsCount = 0;
                deltaFPSTime = 0;
            }
            else
            {
                fpsCount++;
            }

            if (activeScene != null)
            {
                 activeScene.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw the active scene
        /// </summary>
        /// <param name="gameTime">The gameTime</param>
        public static void Draw(GameTime gameTime)
        {
            if (activeScene != null)
            {
                activeScene.Draw(gameTime);
            }
        }

        /// <summary>
        /// Create a scene at the input location
        /// </summary>
        /// <param name="filename">The filename of the scene to be created</param>
        /// <returns>True if successfully created</returns>
        public static bool CreateScene(string filename)
        {
            GameScene gameScene = new GameScene();
            GibboHelper.SerializeObject(filename, gameScene);

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scenePath"></param>
        /// <returns></returns>
        public static bool LoadScene(string scenePath)
        {
            return LoadScene(scenePath, false);
        }

        /// <summary>
        /// Loads a scene to memory from a file.
        /// The active scene is updated to this one if loaded with success.
        /// </summary>
        /// <param name="scenePath">The path of the scene to load</param>
        /// <returns>True if successfully loaded</returns>
        public static bool LoadScene(string scenePath, bool saveHistory)
        {
            try
            {
#if WINDOWS
                GameScene gameScene = (GameScene)GibboHelper.DeserializeObject(scenePath);
#elif WINRT
                GameScene gameScene = (GameScene)GibboHelper.DeserializeObject(typeof(GameScene), scenePath);
#endif
                ActiveScene = gameScene;
                ActiveScenePath = scenePath;

                // Update last saved scene:
                if (GameProject != null && !scenePath.StartsWith("_") && saveHistory)
                    GameProject.EditorSettings.LastOpenScenePath = GibboHelper.MakeExclusiveRelativePath(GameProject.ProjectPath, ActiveScenePath);

                // Load with success, notify:
                return true;
            }
            catch (Exception exception)
            {
                Console.WriteLine("Error loading scene: " + exception.Message + "\n>" + exception.ToString());
                // Not loaded, notify:
                return false;
            }
        }

        /// <summary>
        /// Saves the active scene at its location
        /// </summary>
        /// <returns>True if scene is saved</returns>
        public static bool SaveActiveScene()
        {
            if (ActiveScene == null) return false;

            activeScene.SaveComponentValues();
            GibboHelper.SerializeObject(ActiveScenePath, ActiveScene);

            return true;
        }

        public static XmlElement processNodeBody(XmlDocument xmlDoc, GameObject obj)
        {
            XmlElement NodeGameObject = xmlDoc.CreateElement("Body");
            

            return NodeGameObject;
        }
        public static XmlElement processNode(XmlDocument xmlDoc, GameObject obj)
        {
            XmlElement NodeGameObject = xmlDoc.CreateElement("GameObject");
            NodeGameObject.SetAttribute("Name", obj.Name);

            if (obj is Sprite)
            {
                NodeGameObject.SetAttribute("Type", "Sprite");
            }
            else
                if (obj is AnimatedSprite)
                {
                    NodeGameObject.SetAttribute("Type", "AnimatedSprite");
                }
                else
                    if (obj is Tileset)
                    {
                        NodeGameObject.SetAttribute("Type", "Tileset");
                    } else
               if (obj is BMFont)
            {
                NodeGameObject.SetAttribute("Type", "FontObject");
            }
               else
                   if (obj is Path)
                   {
                       NodeGameObject.SetAttribute("Type", "PathLine");
                   }
                   else
                   if (obj is AudioObject)
                   {
                       NodeGameObject.SetAttribute("Type", "AudioObject");
                   }
                   else
                       if (obj is ParticleEmitter)
                       {
                           NodeGameObject.SetAttribute("Type", "ParticleEmitter");
                       }
            else
            {
                NodeGameObject.SetAttribute("Type", "GameObject");
            }


            if (obj.Disabled)
            { NodeGameObject.SetAttribute("Enable", "false"); }
            else
            { NodeGameObject.SetAttribute("Enable", "true"); }
            if (obj.Visible)
            { NodeGameObject.SetAttribute("Visible", "true"); }
            else
            { NodeGameObject.SetAttribute("Visible", "false"); }

            if (obj.Transform.Parent != null)
            { NodeGameObject.SetAttribute("Parent", obj.Transform.Parent.GameObject.Name); }
            else
            { NodeGameObject.SetAttribute("Parent", "None"); }

            NodeGameObject.SetAttribute("Children", obj.Children.Count.ToString());

            int cm = obj.GetComponents().Count;
            NodeGameObject.SetAttribute("Components", cm.ToString());

            

            if (obj.Children.Count!=0)
            {
              foreach(GameObject child in obj.Children)
              {

                 XmlElement NodeProcess=processNode(xmlDoc, child);
                 XmlElement NodeChild = xmlDoc.CreateElement("Child");
                 NodeChild.AppendChild(NodeProcess);
                 NodeGameObject.AppendChild(NodeChild);
              }
            }




            XmlElement NodeTransform = xmlDoc.CreateElement("Transform");

            XmlElement NodePosition = xmlDoc.CreateElement("Position");
            NodePosition.SetAttribute("x", obj.Transform.Position.X.ToString());
            NodePosition.SetAttribute("y", obj.Transform.Position.Y.ToString());


            XmlElement NodeRelative = xmlDoc.CreateElement("Relative");
            NodeRelative.SetAttribute("x", obj.Transform.RelativePosition.X.ToString());
            NodeRelative.SetAttribute("y", obj.Transform.RelativePosition.Y.ToString());

            int w = SceneManager.GraphicsDevice.Viewport.Width;
            int h = SceneManager.GraphicsDevice.Viewport.Height;


            float x = 0;
            float y = 0;

            if (obj is Tileset)
            {
                Tileset tiles = obj as Tileset;


                int MapSizeX = (tiles.Width * tiles.TileWidth);
                int MapSizeY = (tiles.Height * tiles.TileHeight);

                x = ((w - MapSizeX) / 2);
                y = ((h - MapSizeY) / 2);


            }
            else
            {
                x = obj.Transform.Position.X + (w / 2);
                y = obj.Transform.Position.Y + (h / 2);
            }




            XmlElement NodeWorld = xmlDoc.CreateElement("World");
            NodeWorld.SetAttribute("x", x.ToString());
            NodeWorld.SetAttribute("y", y.ToString());


            XmlElement NodeScale = xmlDoc.CreateElement("Scale");

            NodeScale.SetAttribute("x", obj.Transform.Scale.X.ToString());
            NodeScale.SetAttribute("y", obj.Transform.Scale.Y.ToString());

            XmlElement NodeRotate = xmlDoc.CreateElement("Rotate");


            int angle = (int)Math.Round(MathHelper.ToDegrees(obj.Transform.Rotation));
            NodeRotate.SetAttribute("angle", obj.Transform.Rotation.ToString());
            NodeRotate.SetAttribute("angleDeg", angle.ToString());

            NodeTransform.AppendChild(NodePosition);
            NodeTransform.AppendChild(NodeRelative);
            NodeTransform.AppendChild(NodeWorld);
            NodeTransform.AppendChild(NodeScale);
            NodeTransform.AppendChild(NodeRotate);
            NodeGameObject.AppendChild(NodeTransform);


            if (obj is Sprite)
            {

                Sprite sprite = obj as Sprite;
                if (sprite != null)
                {

                    XmlElement BlendMode = xmlDoc.CreateElement("BlendMode");
                    BlendMode.SetAttribute("value", sprite.BlendMode.ToString());

                    XmlElement DisplayMode = xmlDoc.CreateElement("DisplayMode");
                    DisplayMode.SetAttribute("value", sprite.DisplayMode.ToString());

                    XmlElement Origin = xmlDoc.CreateElement("Origin");
                    Origin.SetAttribute("value", sprite.Origin.ToString());

                    XmlElement Image = xmlDoc.CreateElement("Image");
                    Image.SetAttribute("value", System.IO.Path.GetFileName(sprite.ImageName));


                    XmlElement effect = xmlDoc.CreateElement("Effect");
                    effect.SetAttribute("value", sprite.SpriteEffect.ToString());
                    XmlElement color = xmlDoc.CreateElement("Color");
                    color.SetAttribute("r", sprite.Color.R.ToString());
                    color.SetAttribute("g", sprite.Color.G.ToString());
                    color.SetAttribute("b", sprite.Color.B.ToString());
                    color.SetAttribute("a", sprite.Color.A.ToString());

                    XmlElement SourceRectangle = xmlDoc.CreateElement("SourceRectangle");
                    SourceRectangle.SetAttribute("x", sprite.SourceRectangle.X.ToString());
                    SourceRectangle.SetAttribute("y", sprite.SourceRectangle.Y.ToString());
                    SourceRectangle.SetAttribute("w", sprite.SourceRectangle.Width.ToString());
                    SourceRectangle.SetAttribute("h", sprite.SourceRectangle.Height.ToString());


                    XmlElement DimensionRectangle = xmlDoc.CreateElement("Dimension");

                    DimensionRectangle.SetAttribute("x", sprite.MeasureDimension().X.ToString());
                    DimensionRectangle.SetAttribute("y", sprite.MeasureDimension().Y.ToString());
                    DimensionRectangle.SetAttribute("w", sprite.MeasureDimension().Width.ToString());
                    DimensionRectangle.SetAttribute("h", sprite.MeasureDimension().Height.ToString());

                    // XmlElement pivot = xmlDoc.CreateElement("Pivot");
                    //  pivot.SetAttribute("x", sprite.orgx.X.ToString());
                    // pivot.SetAttribute("y", sprite.orgx.Y.ToString());


                    NodeGameObject.AppendChild(Image);
                    NodeGameObject.AppendChild(BlendMode);
                    NodeGameObject.AppendChild(DisplayMode);
                    NodeGameObject.AppendChild(Origin);
                    NodeGameObject.AppendChild(SourceRectangle);
                    NodeGameObject.AppendChild(effect);
                    NodeGameObject.AppendChild(color);

                }

            }
            else
                if (obj is AnimatedSprite)
                {

                    AnimatedSprite sprite = obj as AnimatedSprite;
                    if (sprite != null)
                    {

                        XmlElement BlendMode = xmlDoc.CreateElement("BlendMode");
                        BlendMode.SetAttribute("value", sprite.BlendMode.ToString());


                        XmlElement Image = xmlDoc.CreateElement("Image");
                        Image.SetAttribute("value", System.IO.Path.GetFileName(sprite.ImageName));

                        XmlElement effect = xmlDoc.CreateElement("Effect");
                        effect.SetAttribute("value", sprite.SpriteEffect.ToString());
                        XmlElement color = xmlDoc.CreateElement("Color");
                        color.SetAttribute("r", sprite.Color.R.ToString());
                        color.SetAttribute("g", sprite.Color.G.ToString());
                        color.SetAttribute("b", sprite.Color.B.ToString());
                        color.SetAttribute("a", sprite.Color.A.ToString());


                        XmlElement animLoop = xmlDoc.CreateElement("Loop");
                        animLoop.SetAttribute("value", sprite.Loop.ToString());
                        XmlElement animMode = xmlDoc.CreateElement("Mode");
                        animMode.SetAttribute("value", sprite.PlayMode.ToString());

                        XmlElement animation = xmlDoc.CreateElement("Animation");

                        animation.SetAttribute("column", sprite.CurrentColumn.ToString());
                        animation.SetAttribute("row", sprite.CurrentRow.ToString());
                        animation.SetAttribute("FramesPerRow", sprite.TotalFramesPerRow.ToString());



                        NodeGameObject.AppendChild(Image);
                        NodeGameObject.AppendChild(BlendMode);


                        NodeGameObject.AppendChild(effect);
                        NodeGameObject.AppendChild(color);


                        NodeGameObject.AppendChild(animLoop);
                        NodeGameObject.AppendChild(animMode);
                        NodeGameObject.AppendChild(animation);

                    }
                }
                else
                    if (obj is Tileset)
                    {

                        Tileset tiles = obj as Tileset;
                        if (tiles != null)
                        {
                            XmlElement tilesetup = xmlDoc.CreateElement("Map");
                            tilesetup.SetAttribute("MapWidth", tiles.Width.ToString());
                            tilesetup.SetAttribute("MapHeight", tiles.Height.ToString());
                            tilesetup.SetAttribute("TileWidth", tiles.TileWidth.ToString());
                            tilesetup.SetAttribute("TileHeight", tiles.TileHeight.ToString());
                            XmlElement Image = xmlDoc.CreateElement("Image");
                            Image.SetAttribute("value", System.IO.Path.GetFileName(tiles.ImageName));


                            XmlElement color = xmlDoc.CreateElement("Color");
                            color.SetAttribute("r", tiles.Color.R.ToString());
                            color.SetAttribute("g", tiles.Color.G.ToString());
                            color.SetAttribute("b", tiles.Color.B.ToString());
                            color.SetAttribute("a", tiles.Color.A.ToString());


                            XmlElement nodedata = xmlDoc.CreateElement("TileData");
                            nodedata.InnerText = tiles.GetCSV();




                            NodeGameObject.AppendChild(color);
                            NodeGameObject.AppendChild(Image);
                            NodeGameObject.AppendChild(tilesetup);
                            NodeGameObject.AppendChild(nodedata);

                        }
                    }
             else
            if (obj is BMFont)
            {

                BMFont font = obj as BMFont;
                if (font != null)
                {

                    XmlElement color = xmlDoc.CreateElement("Color");
                    color.SetAttribute("r", font.OverlayColor.R.ToString());
                    color.SetAttribute("g", font.OverlayColor.G.ToString());
                    color.SetAttribute("b", font.OverlayColor.B.ToString());
                    color.SetAttribute("a", font.OverlayColor.A.ToString());

                            XmlElement fontsetup = xmlDoc.CreateElement("Settings");
                            fontsetup.SetAttribute("AlignMode", font.AlignMode.ToString());
                            fontsetup.SetAttribute("Text", font.Text);
                            fontsetup.SetAttribute("LineSpacing", font.LineSpacing.ToString());
                            fontsetup.SetAttribute("File", System.IO.Path.GetFileName(font.TextureFilePath));
                            fontsetup.AppendChild(color);
                            NodeGameObject.AppendChild(fontsetup);
                }
            }
            else
                if (obj is Path)
                {
                     Path path = obj as Path;
                     if (path != null)
                     {
                         XmlElement nPoints = xmlDoc.CreateElement("Points");

                         foreach(Vector2  point in path.Points)
                         {
                               XmlElement nPoint = xmlDoc.CreateElement("Point");
                               nPoint.SetAttribute("x", point.X.ToString());
                               nPoint.SetAttribute("y", point.X.ToString());
                               nPoints.AppendChild(nPoint);
                         }
                         NodeGameObject.AppendChild(nPoints);

                     }
                }else

                if (obj is AudioObject)
                {

                    AudioObject audio = obj as AudioObject;
                    if (audio != null)
                    {

                        XmlElement fontsetup = xmlDoc.CreateElement("Settings");
                        fontsetup.SetAttribute("Volume", audio.Volume.ToString());
                        fontsetup.SetAttribute("PlayOnStart", audio.PlayOnStart.ToString());
                        fontsetup.SetAttribute("Loop", audio.Loop.ToString());
                        fontsetup.SetAttribute("File", System.IO.Path.GetFileName(audio.FilePath));
                        NodeGameObject.AppendChild(fontsetup);
                    }
                }
            if (obj is ParticleEmitter)
            {

                ParticleEmitter particles = obj as ParticleEmitter;
                if (particles != null)
                {
                    XmlElement config = xmlDoc.CreateElement("Basic");
                    config.SetAttribute("Burst",particles.Burst.ToString());
                    config.SetAttribute("Enable", particles.Enabled.ToString());
                    config.SetAttribute("File", System.IO.Path.GetFileName(particles.ImageName));
                    config.SetAttribute("LifespanMin", particles.LifespanMin.ToString());
                    config.SetAttribute("LifespanMax", particles.LifespanMax.ToString());
                    config.SetAttribute("MaxParticles", particles.MaxParticles.ToString());
                    config.SetAttribute("SecondsPerSpawnMin", particles.SecondsPerSpawnMin.ToString());
                    config.SetAttribute("SecondsPerSpawnMax", particles.SecondsPerSpawnMax.ToString());

                    XmlElement behavior = xmlDoc.CreateElement("Behaviour");
                    behavior.SetAttribute("Rotation", particles.RotationStrength.ToString());
                    //djoker... node or attribute 

                  //  XmlElement nrotate = xmlDoc.CreateElement("Rotation");
                   // nrotate.SetAttribute("value", particles.RotationStrength.ToString());
                    //behavior.AppendChild(nrotate);

                    XmlElement nangle = xmlDoc.CreateElement("SpawnAngle");
                    nangle.SetAttribute("x", particles.SpawnAngleNoise.X.ToString());
                    nangle.SetAttribute("y", particles.SpawnAngleNoise.Y.ToString());
                    XmlElement direction = xmlDoc.CreateElement("SpawnDirection");
                    direction.SetAttribute("x", particles.SpawnAngleNoise.X.ToString());
                    direction.SetAttribute("y", particles.SpawnAngleNoise.Y.ToString());
                   
                    behavior.AppendChild(nangle);
                    behavior.AppendChild(direction);

                    
                    XmlElement color = xmlDoc.CreateElement("Color");
                    color.SetAttribute("Blend",particles.BlendMode.ToString());
                    
                    XmlElement startColor1 = xmlDoc.CreateElement("InitialColor1");
                    startColor1.SetAttribute("r", particles.InitialColor1.R.ToString());
                    startColor1.SetAttribute("g", particles.InitialColor1.G.ToString());
                    startColor1.SetAttribute("b", particles.InitialColor1.B.ToString());
                    startColor1.SetAttribute("a", particles.InitialColor1.A.ToString());
                    color.AppendChild(startColor1);
                    XmlElement startColor2 = xmlDoc.CreateElement("InitialColor2");
                    startColor2.SetAttribute("r", particles.InitialColor2.R.ToString());
                    startColor2.SetAttribute("g", particles.InitialColor2.G.ToString());
                    startColor2.SetAttribute("b", particles.InitialColor2.B.ToString());
                    startColor2.SetAttribute("a", particles.InitialColor2.A.ToString());
                    color.AppendChild(startColor2);
                    XmlElement endColor1 = xmlDoc.CreateElement("FinalColor1");
                    endColor1.SetAttribute("r", particles.FinalColor1.R.ToString());
                    endColor1.SetAttribute("g", particles.FinalColor1.G.ToString());
                    endColor1.SetAttribute("b", particles.FinalColor1.B.ToString());
                    endColor1.SetAttribute("a", particles.FinalColor1.A.ToString());
                    color.AppendChild(endColor1);
                    XmlElement endColor2 = xmlDoc.CreateElement("FinalColor2");
                    endColor2.SetAttribute("r", particles.FinalColor2.R.ToString());
                    endColor2.SetAttribute("g", particles.FinalColor2.G.ToString());
                    endColor2.SetAttribute("b", particles.FinalColor2.B.ToString());
                    endColor2.SetAttribute("a", particles.FinalColor2.A.ToString());
                    color.AppendChild(endColor2);

                    XmlElement eTrasf = xmlDoc.CreateElement("EmitterTransform");
                    eTrasf.SetAttribute("InitialScaleMin", particles.InitialScaleMin.ToString());
                    eTrasf.SetAttribute("InitialScaleMax", particles.InitialScaleMax.ToString());
                    eTrasf.SetAttribute("InitialSpeedMin", particles.InitialSpeedMin.ToString());
                    eTrasf.SetAttribute("InitialSpeedMax", particles.InitialSpeedMax.ToString());
                    eTrasf.SetAttribute("FinalScaleMin", particles.FinalScaleMin.ToString());
                    eTrasf.SetAttribute("FinalScaleMax", particles.FinalScaleMax.ToString());
                    eTrasf.SetAttribute("FinalSpeedMin", particles.FinalSpeedMin.ToString());
                    eTrasf.SetAttribute("FinalSpeedMax", particles.FinalSpeedMax.ToString());
                    
                    NodeGameObject.AppendChild(behavior);
                    NodeGameObject.AppendChild(config);
                    NodeGameObject.AppendChild(color);
                    NodeGameObject.AppendChild(eTrasf);
                }
            }


          List<ObjectComponent> components=  obj.GetComponents();
          if (components!=null)
          {
              foreach(ObjectComponent component in components)
              {
                   if (component is PhysicalBody)
                  {
                      PhysicalBody pBody = (component as PhysicalBody);
                      XmlElement body = xmlDoc.CreateElement("PhysicalBody");
                      body.SetAttribute("Friction", pBody.Friction.ToString());
                      body.SetAttribute("Restitution", pBody.Restitution.ToString());
                      body.SetAttribute("BodyType", pBody.BodyType.ToString());
                      body.SetAttribute("AngularDamping", pBody.AngularDamping.ToString());
                      body.SetAttribute("LinearDamping", pBody.LinearDamping.ToString());
                      body.SetAttribute("AngularVelocity", pBody.AngularVelocity.ToString());
                      body.SetAttribute("IsBullet", pBody.IsBullet.ToString());
                      body.SetAttribute("IsSensor", pBody.IsSensor.ToString());
                      body.SetAttribute("FixedRotation", pBody.FixedRotation.ToString());
                      body.SetAttribute("IgnoreGravity", pBody.IgnoreGravity.ToString());

                      if (component is RectangleBody)
                      {
                          
                          RectangleBody sBody = (component as RectangleBody);
                          
                          XmlElement shape = xmlDoc.CreateElement("Shape");
                          shape.SetAttribute("Type", "Rectangle");
                          shape.SetAttribute("Density", sBody.Density.ToString());
                          shape.SetAttribute("Width", sBody.Width.ToString());
                          shape.SetAttribute("Height", sBody.Height.ToString());
                          body.AppendChild(shape);
                          NodeGameObject.AppendChild(body);

                      }else
                          if (component is CircleBody)
                          {
                              
                              CircleBody sBody = (component as CircleBody);
                              XmlElement shape = xmlDoc.CreateElement("Shape");
                              shape.SetAttribute("Type", "Circle");
                              shape.SetAttribute("Density", sBody.Density.ToString());
                              shape.SetAttribute("Radius", sBody.Radius.ToString());
                              body.AppendChild(shape);
                          } else
                       if (component is TextureBody)
                      {

                          TextureBody sBody = (component as TextureBody);
                          body.SetAttribute("Density", sBody.Density.ToString());
                          XmlElement shape = xmlDoc.CreateElement("Shape");
                          shape.SetAttribute("Type", "Polygon");
                          shape.SetAttribute("Density", sBody.Density.ToString());
                          shape.SetAttribute("Texture", System.IO.Path.GetFileName(sBody.ImageName));
                          

                           if(sBody.polygonos!=null)
                           {
                               XmlElement poly = xmlDoc.CreateElement("Polygons");
                               for (int i=0;i<sBody.polygonos.Count;i++)
                                {
                                  Vector2[] poli= sBody.polygonos[i].ToArray();
                                  if (poli != null)//não va o azar bater na tecla :P
                                  {
                                      XmlElement v = xmlDoc.CreateElement("Polygon");

                                      for (int j = 0; j < poli.Length; j++)
                                      {
                                          XmlElement face = xmlDoc.CreateElement("Face");
                                          face.SetAttribute("x", poli[j].X.ToString());
                                          face.SetAttribute("y", poli[j].Y.ToString());
                                          v.AppendChild(face);
                                      }
                                      poly.AppendChild(v);
                                  }
                               }

                               
                               shape.AppendChild(poly);

                           }

                          body.AppendChild(shape);
                      }


                      NodeGameObject.AppendChild(body);
                     

                  } else
                   {
                       ///que raio de merda se vai meter aqui .....
                   }

                  /*
                
                  if (component is RectangleBody)
                  {
                      RectangleBody pBody = (component as RectangleBody);
                      XmlElement body = xmlDoc.CreateElement("PhysicalBody");
                      body.SetAttribute("Name", "RectangleBody");
                      body.SetAttribute("Friction", pBody.Friction.ToString());
                      body.SetAttribute("Restitution", pBody.Restitution.ToString());
                      body.SetAttribute("BodyType", pBody.BodyType.ToString());
                      body.SetAttribute("AngularDamping", pBody.AngularDamping.ToString());
                      body.SetAttribute("LinearDamping", pBody.LinearDamping.ToString());
                      body.SetAttribute("AngularVelocity", pBody.AngularVelocity.ToString());
                      body.SetAttribute("IsBullet", pBody.IsBullet.ToString());
                      body.SetAttribute("IsSensor", pBody.IsSensor.ToString());
                      body.SetAttribute("FixedRotation", pBody.FixedRotation.ToString());
                      body.SetAttribute("IgnoreGravity", pBody.IgnoreGravity.ToString());
                      body.SetAttribute("Density", pBody.Density.ToString());
                      XmlElement shape = xmlDoc.CreateElement("Shape");
                      shape.SetAttribute("Width", pBody.Width.ToString());
                      shape.SetAttribute("Height", pBody.Height.ToString());
                      body.AppendChild(shape);
                      NodeGameObject.AppendChild(body);
                      return NodeGameObject;

                  } else
                        if (component is CircleBody)
                  {

                      CircleBody pBody = (component as CircleBody);



                      XmlElement body = xmlDoc.CreateElement("PhysicalBody");
                      body.SetAttribute("Name", "CircleBody");
                      body.SetAttribute("Friction", pBody.Friction.ToString());
                      body.SetAttribute("Restitution", pBody.Restitution.ToString());
                      body.SetAttribute("BodyType", pBody.BodyType.ToString());
                      body.SetAttribute("AngularDamping", pBody.AngularDamping.ToString());
                      body.SetAttribute("LinearDamping", pBody.LinearDamping.ToString());
                      body.SetAttribute("AngularVelocity", pBody.AngularVelocity.ToString());
                      body.SetAttribute("IsBullet", pBody.IsBullet.ToString());
                      body.SetAttribute("IsSensor", pBody.IsSensor.ToString());
                      body.SetAttribute("FixedRotation", pBody.FixedRotation.ToString());
                      body.SetAttribute("IgnoreGravity", pBody.IgnoreGravity.ToString());
                      body.SetAttribute("Density", pBody.Density.ToString());
                      XmlElement shape = xmlDoc.CreateElement("Shape");
                      shape.SetAttribute("Radius", pBody.Radius.ToString());
                      body.AppendChild(shape);


                      NodeGameObject.AppendChild(body);
                      return NodeGameObject;
                  } else
                  if (component is TextureBody)
                  {
                      TextureBody pBody = (component as TextureBody);



                      XmlElement body = xmlDoc.CreateElement("PhysicalBody");
                      body.SetAttribute("Name", "TextureBody");
                      body.SetAttribute("Friction", pBody.Friction.ToString());
                      body.SetAttribute("Restitution", pBody.Restitution.ToString());
                      body.SetAttribute("BodyType", pBody.BodyType.ToString());
                      body.SetAttribute("AngularDamping", pBody.AngularDamping.ToString());
                      body.SetAttribute("LinearDamping", pBody.LinearDamping.ToString());
                      body.SetAttribute("AngularVelocity", pBody.AngularVelocity.ToString());
                      body.SetAttribute("IsBullet", pBody.IsBullet.ToString());
                      body.SetAttribute("IsSensor", pBody.IsSensor.ToString());
                      body.SetAttribute("FixedRotation", pBody.FixedRotation.ToString());
                      body.SetAttribute("IgnoreGravity", pBody.IgnoreGravity.ToString());
                      body.SetAttribute("Density", pBody.Density.ToString());
                      XmlElement shape = xmlDoc.CreateElement("Shape");
                      shape.SetAttribute("Texture",  System.IO.Path.GetFileName(pBody.ImageName));
                      body.AppendChild(shape);

                      NodeGameObject.AppendChild(body);
                      return NodeGameObject;

                  } 
                  if (component is PhysicalBody)
                  {
                      PhysicalBody pBody = (component as PhysicalBody);
                      XmlElement body = xmlDoc.CreateElement("PhysicalBody");
                      body.SetAttribute("Name", "Empty");
                      body.SetAttribute("Friction", pBody.Friction.ToString());
                      body.SetAttribute("Restitution", pBody.Restitution.ToString());
                      body.SetAttribute("BodyType", pBody.BodyType.ToString());
                      body.SetAttribute("AngularDamping", pBody.AngularDamping.ToString());
                      body.SetAttribute("LinearDamping", pBody.LinearDamping.ToString());
                      body.SetAttribute("AngularVelocity", pBody.AngularVelocity.ToString());
                      body.SetAttribute("IsBullet", pBody.IsBullet.ToString());
                      body.SetAttribute("IsSensor", pBody.IsSensor.ToString());
                      body.SetAttribute("FixedRotation", pBody.FixedRotation.ToString());
                      body.SetAttribute("IgnoreGravity", pBody.IgnoreGravity.ToString());
                  


                      NodeGameObject.AppendChild(body);
                      return NodeGameObject;

                  } else
                  {
                      //ho my ho my!!!
                  }
                  */


              }

          }



            return NodeGameObject;
        }
        //djoker
        public static bool SaveXmlScene(string path,GameScene scene)
        {
            if (ActiveScene == null) return false;

             XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);

       
            XmlElement rootNode = xmlDoc.CreateElement("Scene");
            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
            xmlDoc.AppendChild(rootNode);

          
            XmlElement parentNode = xmlDoc.CreateElement("Layer");
            parentNode.SetAttribute("ID", "0");
           xmlDoc.DocumentElement.PrependChild(parentNode);


            XmlElement nodeCamera = xmlDoc.CreateElement("Camera");
            nodeCamera.SetAttribute("x", SceneManager.ActiveScene.Camera.Position.X.ToString());
            nodeCamera.SetAttribute("y", SceneManager.ActiveScene.Camera.Position.Y.ToString());
            parentNode.AppendChild(nodeCamera);
            

            XmlElement NodeGameObjects = xmlDoc.CreateElement("GameObjects");
            for (int i=0;i<SceneManager.ActiveScene.GameObjects.Count;i++)
            {

                  

             GameObject obj = SceneManager.ActiveScene.GameObjects[i];
             if (obj != null)
             {

                 XmlElement NodeGameObject = processNode(xmlDoc, obj);
                 NodeGameObjects.AppendChild(NodeGameObject);
             }
                  
            }

              parentNode.AppendChild(NodeGameObjects);
           
           
            xmlDoc.Save(path);
       System.Windows.MessageBox.Show("Scene Exported", "Export");
        

          
            return true;
        }
        /// <summary>
        /// Saves the active scene at the input location
        /// </summary>
        /// <param name="path">The target path</param>
        /// <returns></returns>
        public static bool SaveActiveScene(string path, bool xml)
        {
            if (ActiveScene == null) return false;

            activeScene.SaveComponentValues();

            if (!xml)
                GibboHelper.SerializeObject(path, ActiveScene);
            else
                SaveXmlScene(path, activeScene);

            return true;
        }

        #endregion
    }
}

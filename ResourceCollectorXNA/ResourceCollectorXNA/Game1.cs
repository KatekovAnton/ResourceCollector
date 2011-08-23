using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using ResourceCollectorXNA.Engine;
using ResourceCollectorXNA.Engine.DebugRender;
using ResourceCollectorXNA.Content;

namespace ResourceCollectorXNA
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MyGame : Microsoft.Xna.Framework.Game
    {
        
        public static RenderWindow renderWindow;
        public static ResourceCollector.FormMainPackExplorer packexplorer;
        public static MainWindow MDIParent;
        public static ResourceCollectorEditorWindows.Level levelform;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private SpriteFont Font1;
        public static GameEngine _engine;
        private Vector2 FontPos;
        private Log log;
        string outputstring = string.Empty;
        public static GraphicsProfile currentprofile;
        ConsoleWindow console;
        public MyGame()
        {
            log = new Log();
            _engine = new GameEngine(this);

            graphics = GameEngine.DeviceManager;
            if(Microsoft.Xna.Framework.Graphics.GraphicsAdapter.DefaultAdapter.IsProfileSupported(GraphicsProfile.HiDef))
                graphics.GraphicsProfile = GraphicsProfile.HiDef;
            Content.RootDirectory = "Content";

            graphics.SynchronizeWithVerticalRetrace = false;
            this.IsFixedTimeStep = false;


        }

        void gameWindowForm_Shown(object sender, EventArgs e)
        {
            ((System.Windows.Forms.Form)sender).Hide();
        }
        System.Drawing.Point p;
        protected override void Initialize()
        {
            base.Initialize();
            System.Windows.Forms.Form gameWindowForm = (System.Windows.Forms.Form)System.Windows.Forms.Form.FromHandle(this.Window.Handle);

            gameWindowForm.Shown += new EventHandler(gameWindowForm_Shown);
            p = gameWindowForm.Location;
            p.Y -= 20;
            renderWindow = new RenderWindow();
            MDIParent = new MainWindow();
            levelform = new ResourceCollectorEditorWindows.Level();
            console = new ConsoleWindow();
            packexplorer = new ResourceCollector.FormMainPackExplorer();
            MDIParent.HandleDestroyed += new EventHandler(MDIParent_HandleDestroyed);
            
            levelform.MdiParent = MDIParent;
            console.MdiParent = MDIParent;
            renderWindow.MdiParent = MDIParent;
            
            packexplorer.MdiParent = MDIParent;
            levelform.Show();
            renderWindow.Show();
            packexplorer.Show();
            console.Show();
            MDIParent.Show();
            _engine.Initalize();

            // TODO: Add your initialization logic here


            this.IsMouseVisible = true;

            _engine.Update();

        }

        void MDIParent_HandleDestroyed(object sender, EventArgs e)
        {
            Exit();
        }


        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            GameEngine.Device = GraphicsDevice;
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Font1 = Content.Load<SpriteFont>("Courier New");
            FontPos = new Vector2(10.0f, 10.0f);

            _engine.Font1 = Font1;
           
          //  outputstring = packs.packs.Length.ToString();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            _engine.UnLoad();
        }
        bool lfirst = false;


        private void HandleKeyboard(KeyboardState keyboardState)
        {
          /*  if (keyboardState.IsKeyDown(Keys.L) && !lfirst)
            {
                lfirst = true;

             //   _engine.WorldObjectCharacterBox.behaviourmodel.SetGlobalPose(Matrix.CreateRotationX(-MathHelper.PiOver2) * Matrix.CreateTranslation(0, 20, 0), null);
            }
            if (keyboardState.IsKeyUp(Keys.L) && lfirst)
                lfirst = false;

            */


          /*  if (keyboardState.IsKeyDown(Keys.K))
                _engine.WorldObjectCharacterBox.behaviourmodel.Move(new Vector3(5.0f, 0, 0));

            if (keyboardState.IsKeyDown(Keys.H))
                _engine.WorldObjectCharacterBox.behaviourmodel.Move(new Vector3(-5.0f, 0, 0));

            if (keyboardState.IsKeyDown(Keys.J))
                _engine.WorldObjectCharacterBox.behaviourmodel.Move(new Vector3(0, 0, 5.0f));

            if (keyboardState.IsKeyDown(Keys.U))
                _engine.WorldObjectCharacterBox.behaviourmodel.Move(new Vector3(0, 0, -5.0f));
            */


         /*   if (keyboardState.IsKeyDown(Keys.Escape))
                Exit();*/
            


           /* Vector3? point = _engine.WorldObjectTestSide.RCCM.IntersectionClosest(ray, _engine.WorldObjectTestSide.behaviourmodel.GetGlobalPose());
            if (point != null)
                _engine.WorldObjectCursorSphere.behaviourmodel.SetGlobalPose(Matrix.CreateTranslation(point.Value), null);
            */
        }
        public static bool NeedForceUpdate = false;
        public static void AddOject(Engine.Logic.PivotObject newobject)
        {
            _engine.AddObject(newobject);
            NeedForceUpdate = true;
        }
        protected override void Update(GameTime gameTime)
        {
            if (MDIParent.ActiveMdiChild == renderWindow)
            {
                HandleKeyboard(Keyboard.GetState());
                System.Drawing.Point formpos = renderWindow.DesktopLocation;
                //132,55
                //254,140
                MouseState m = Mouse.GetState();
                Vector2 mousepos = new Vector2(m.X + p.X - formpos.X, m.Y + p.Y - formpos.Y);
                var ray = Extensions.FromScreenPoint(
                    GraphicsDevice.Viewport,
                    mousepos,
                    _engine.Camera.View,
                    _engine.Camera.Projection);
                _engine.Update(gameTime, ray, mousepos);
            }
            else if (NeedForceUpdate)
            {
                _engine.Update();
            }
            base.Update(gameTime);
            NeedForceUpdate = false;

        }


        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _engine.Draw();

            drawstring();
            GraphicsDevice.Present(null, null, renderWindow.PanelHandle);
        }

        public void drawstring()
        {

            outputstring = _engine.Camera.View.Translation.ToString() + '\n' + _engine.Camera.View.Up.ToString(); ;
            spriteBatch.Begin();
           // if (_engine.BoxScreenPosition.Z < 1.0)
         //       spriteBatch.DrawString(Font1, "Box position = " + _engine.WorldObjectBox.behaviourmodel.GetGlobalPose().Translation.ToString(), new Vector2(_engine.BoxScreenPosition.X, _engine.BoxScreenPosition.Y), Color.Black, 0, new Vector2(), 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(Font1, DateTime.Now.ToString(), new Vector2(10, 170), Color.Black, 0, new Vector2(), 1.0f, SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(Font1, string.Format("FPS: {0} Frame time: {1}", _engine.FPSCounter.FramesPerSecond, _engine.FPSCounter.FrameTime), Vector2.Zero, Color.White);
            spriteBatch.DrawString(Font1, "Visible objects count: " + _engine.visibleobjectscount.ToString(), new Vector2(0, 15), Color.White);
            spriteBatch.DrawString(Font1, "Recalulcalated objects count: " + _engine.gameLevel.sceneGraph.recalulcalated().ToString(), new Vector2(0, 30), Color.White);

            spriteBatch.End();
        }
    }

   
}

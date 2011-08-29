using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;


//using StillDesign.PhysX;


using ResourceCollectorXNA.Content;
using ResourceCollectorXNA.Engine.Render;
using ResourceCollectorXNA.Engine.ContentLoader;
using ResourceCollectorXNA.Engine.Logic;
using ResourceCollectorXNA.Engine.Interface;

namespace ResourceCollectorXNA.Engine
{
    public class GameEngine
	{
        public static GameEngine Instance;
        public static GraphicsDeviceManager DeviceManager;
        public static GraphicsDevice Device;


        public static RCViewControllers.RenderWindowVC renderController;
        public static RCViewControllers.LevelWindowVC levelController;
        public static bool actionToInterface;
        public static bool actionFromInterface;
        public RenderPipeline GraphicPipeleine;

        public Helpers.FpsCounter FPSCounter;

        public SpriteBatch spriteBatch;
        public SpriteFont Font1;

        public GameEditor editor;

        public Vector3 lightDir = new Vector3(-1, -1, -1);


        private bool needclear;
        private bool neednewlevel;
        public int visibleobjectscount = 0;
        private MyContainer<PivotObject> objectstoadd;

        public Engine.Level.EngineLevel gameLevel;

        public Engine.Level.EngineLevel futureGameLevel;
        /// <summary>
        /// ПОЛЬЗУЙСЯ ИМ!!!!!!!!!!!!!!
        /// </summary>
        /// <param name="newobject"></param>
        public void AddObject(PivotObject newobject)
        {
            objectstoadd.Add(newobject);
        }


        public GameEngine(MyGame game)
		{
            lightDir.Normalize();
			DeviceManager = new GraphicsDeviceManager( game );
            //разме рэкрана1158; 708
            DeviceManager.PreferredBackBufferWidth = 1158;
            DeviceManager.PreferredBackBufferHeight = 708;
            objectstoadd = new MyContainer<PivotObject>();
            gameLevel = new Level.EngineLevel();
		}
        
        bool locked;
        
        public void clear()
        {
            needclear = true;
        }

        public void CreateNewLevel()
        {
            neednewlevel = true;
        }

        public void LoadNewLevel(Engine.Level.EngineLevel _futureGameLevel)
        {
            futureGameLevel = _futureGameLevel;
        }

        public void ResetDevice(System.Drawing.Size ClientSize)
        {
            locked = true;
            try
            {
                if (System.Threading.Monitor.TryEnter(this))
                {
                    Device.PresentationParameters.BackBufferWidth = ClientSize.Width;
                    Device.PresentationParameters.BackBufferHeight = ClientSize.Height;
                    DeviceManager.PreferredBackBufferWidth = (int)(ClientSize.Width);
                    DeviceManager.PreferredBackBufferHeight = (int)(ClientSize.Height);
                    Device.Reset();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.ToString());
            }
            finally
            {
                System.Threading.Monitor.Exit(this);
                if(Camera!=null)
                    Camera.ResetProjection((float)ClientSize.Width / (float)ClientSize.Height);
            }
            locked = false;
        }


        public void Initalize()
        {
            FPSCounter = new Helpers.FpsCounter();
            this.Camera = new Camera(this, new Vector3(20, 20, 10), new Vector3(0, 15, 0));
            spriteBatch = new SpriteBatch(DeviceManager.GraphicsDevice);
            #region create physic scene
            /*CoreDescription coreDesc = new CoreDescription();
            UserOutput output = new UserOutput();

            Core = new Core(coreDesc, output);
            Core.SetParameter(PhysicsParameter.ContinuousCollisionDetection, false);
            Core.SetParameter(PhysicsParameter.ContinuousCollisionDetectionEpsilon, 0.01f);

            SceneDescription sceneDesc = new SceneDescription()
            {
                SimulationType = SimulationType.Software,//Hardware,
                MaximumBounds = new Bounds3(-1000,-1000,-1000,1000,1000,1000),
                UpAxis = 2,
                Gravity = new Vector3(0.0f, -9.81f*1.3f, 0.0f),
                GroundPlaneEnabled = false
            };
            this.Scene = Core.CreateScene(sceneDesc);
            manager = Scene.CreateControllerManager();
           */
            #endregion
           // _sceneGraph = new Logic.SceneGraph.SceneGraph();
            editor = new GameEditor(gameLevel);
            GraphicPipeleine = new RenderPipeline(DeviceManager.GraphicsDevice, Camera);
            CreateShader();
            editor.SetDestEngine();
            editor.SetFont(Font1);
        }


        void CreateShader()
        {
            byte[] buffer;
            System.IO.BinaryReader br = new System.IO.BinaryReader(new System.IO.FileStream("Effect.shader", System.IO.FileMode.Open));
            buffer = br.ReadBytes(Convert.ToInt32(new System.IO.FileInfo("Effect.shader").Length));
            br.Close();
            ResourceCollectorXNA.Engine.Render.Materials.Material.ObjectRenderEffect = new Effect(Device, buffer);
        }


        public Vector3 BoxScreenPosition;
        public Vector2 mousePos;
        public void UpdateLevelPart()
        {
            if (needclear)
            {
                editor.Clear();
                if (objectstoadd.Count != 0)
                    foreach (PivotObject p in objectstoadd)
                        ContentLoader.ContentLoader.UnloadPivotObject(p);
                objectstoadd.Clear();
                gameLevel.unload();
                needclear = false;

                //GraphicPipeleine.NewFrame(lightDir);
                //   return;

            }

            if (neednewlevel)
            {
                if (objectstoadd.Count != 0)
                    foreach (PivotObject p in objectstoadd)
                        ContentLoader.ContentLoader.UnloadPivotObject(p);
                objectstoadd.Clear();
                gameLevel.unload();
                neednewlevel = false;

                GraphicPipeleine.NewFrame(lightDir);
                gameLevel = new Level.EngineLevel();
                editor.setNewLevel(gameLevel);
                // return;
            }

            if (futureGameLevel != null)
            {
                if (objectstoadd.Count != 0)
                    foreach (PivotObject p in objectstoadd)
                        ContentLoader.ContentLoader.UnloadPivotObject(p);
                objectstoadd.Clear();
                futureGameLevel.load();
                GraphicPipeleine.NewFrame(lightDir);
                gameLevel.unload();
                gameLevel = futureGameLevel;
                editor.setNewLevel(gameLevel);
                futureGameLevel = null;
            }
        }
		public void Update( GameTime gameTime ,Ray ray, Vector2 mouseScreenPos)
		{
            mousePos = mouseScreenPos;
            UpdateLevelPart();

            if (objectstoadd.Count != 0)
            {
                foreach (PivotObject lo in objectstoadd)
                {
                    lo.Update();
                    RenderObject ro = lo.HaveRenderAspect();
                    if (ro != null)
                        GraphicPipeleine.ProceedObject(ro);
                }
                editor.AddObjects(objectstoadd);
                objectstoadd.Clear();
            }


            foreach (PivotObject lo in gameLevel.objects)
                lo.BeginDoFrame();
            KeyboardManager.Manager.Update();
            editor.Update(ray, mouseScreenPos);

            //Updating camera
            Camera.Update(gameTime);
            //очищаем конвейер
            GraphicPipeleine.NewFrame(lightDir);


            //Garbage collection(nulling deleted objects)
            //------
            //вот так вот примерно будет происходить рисование
            //тут идёт обновление сценграфа = (фрустумкулинг=> получение списка видимых объектов) 
            //добавляем все нобходимые объекты на отрисовку
   
            gameLevel.UpdateScene();



            GraphicPipeleine.AddObjectToPipeline(gameLevel.VisibleObjects);
            GraphicPipeleine.AddObjectToShadow(gameLevel.ShadowObjects);
            visibleobjectscount = gameLevel.VisibleObjects.Count;

            FPSCounter.Update(gameTime);
		}
        public void Update()
        {
            if (objectstoadd.Count != 0)
            {
                foreach (PivotObject lo in objectstoadd)
                {
                    lo.Update();
                    RenderObject ro = lo.HaveRenderAspect();
                    if(ro!=null)
                        GraphicPipeleine.ProceedObject(ro);
                }
                editor.AddObjects(objectstoadd);
                objectstoadd.Clear();
               
            }

            //очищаем конвейер
            GraphicPipeleine.NewFrame(lightDir);
            gameLevel.UpdateScene();
            /*
            foreach (PivotObject lo in objects)
                lo.Update();

            //Garbage collection(nulling deleted objects)
            //------

            //вот так вот примерно будет происходить рисование
            //тут идёт обновление сценграфа = (фрустумкулинг=> получение списка видимых объектов) 
            _sceneGraph.NewFrame();
            _sceneGraph.calculateVisibleObjects(Camera.cameraFrustum);


            
            
            _sceneGraph.calculateShadowVisibleObjects(GraphicPipeleine.frustumForShadow);*/
            //добавляем все нобходимые объекты на отрисовку
            GraphicPipeleine.AddObjectToPipeline(gameLevel.VisibleObjects);
            GraphicPipeleine.AddObjectToShadow(gameLevel.ShadowObjects);
            visibleobjectscount = gameLevel.VisibleObjects.Count;
           // ma.UpdateData();
            editor.Update();
        }



        public void Draw()
        {
            //основной рендер. будет потом в колор рендертаргет. Внутри- дефферед шэйдинг и вся хрень
            GraphicPipeleine.RenderToPicture(Camera, lightDir);

            //потом пост-процессинг

            //потом ещё чонить
            spriteBatch.Begin();
            editor.Draw(spriteBatch);
            spriteBatch.End();
        }
       
		public static Color Int32ToColor( int color )
		{
			byte a = (byte)( ( color & 0xFF000000 ) >> 32 );
			byte r = (byte)( ( color & 0x00FF0000 ) >> 16 );
			byte g = (byte)( ( color & 0x0000FF00 ) >> 8 );
			byte b = (byte)( ( color & 0x000000FF ) >> 0 );

			return new Color( r, g, b, a );
		}
		public static int ColorToArgb( Color color )
		{
			int a = (int)( color.A );
			int r = (int)( color.R );
			int g = (int)( color.G );
			int b = (int)( color.B );

			return ( a << 24 ) | ( r << 16 ) | ( g << 8 ) | ( b << 0 );
		}
      
        public void UnLoad()
        {
        }

        public Camera Camera;


    }
}

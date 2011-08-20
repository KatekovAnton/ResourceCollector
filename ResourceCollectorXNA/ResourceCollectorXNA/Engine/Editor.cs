using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//using StillDesign.PhysX;


using ResourceCollectorXNA.Content;
using ResourceCollectorXNA.Engine;
using ResourceCollectorXNA.Engine.Render;
using ResourceCollectorXNA.Engine.ContentLoader;
using ResourceCollectorXNA.Engine.Logic;
using ResourceCollectorXNA.Engine.Interface;

namespace ResourceCollectorXNA.Engine
{
    
    public class GameEditor:IKeyboardUserInterface
    {
        GameScene gameScene;
        
        public static BasicEffect _visualizationEffect;
        public static SpriteFont Font1;

        private ObjectContainer activeObject;
        private bool needUpdate = true;
        public Actions.ActionStack actions;
        
        private bool lmbwasreleased = true;
        public TransformManager transformator;
        List<HotKey> hotkeys1;

        public GameEditor(GameScene scene)
        {
            actions = new Actions.ActionStack();
            gameScene = scene;

            hotkeys1 = new List<HotKey>();
            KeyboardManager.Manager.AddKeyboardUser(this);
            HotKey ctrlz = new HotKey(new Keys[] { Keys.LeftControl, Keys.Z }, revertAction);
            hotkeys1.Add(ctrlz);
            HotKey biggerarrows = new HotKey(new Keys[] { Keys.OemPlus }, biggerArrows);
            hotkeys1.Add(biggerarrows);
            HotKey smallerarrows = new HotKey(new Keys[] { Keys.OemMinus }, smallerArrows);
            hotkeys1.Add(smallerarrows);
            HotKey deleteobjects = new HotKey(new Keys[] { Keys.Delete }, DeleteSelectedObjects);
            hotkeys1.Add(deleteobjects);
            activeObject = new ObjectContainer();
        }
        public void DeleteSelectedObjects()
        {
            if (activeObject.Length != 0)
            {
                MyContainer<PivotObject> deletingObjects = new MyContainer<PivotObject>(activeObject.Length, 1);
                deletingObjects.AddRange(activeObject.objects.ToArray());
                SetActiveObjects(new ObjectContainer(), true);
                DeleteObjects(deletingObjects, false);
                
            }
        }
        public void AddObjects(MyContainer<PivotObject> objects, bool back = false)
        {
            gameScene.AddObjects(objects);
            if (!back)
            {
                Actions.AddObjectPivotAction action = new Actions.AddObjectPivotAction(new ObjectContainer(objects));
                actions.AddNewAction(action);
            }
        }
        public void DeleteObjects(MyContainer<PivotObject> objects, bool back = false)
        {
            gameScene.DeleteObjects(objects);
            if (!back)
            {
                Actions.DeleteObjectPivotAction action = new Actions.DeleteObjectPivotAction(new ObjectContainer(objects));
                actions.AddNewAction(action);
            }
        }
        public void clear()
        {
            actions.clear();

            SetActiveObjects(new ObjectContainer(), true);
        }
        public bool IsKeyboardCaptured()
        {
            return true;
        }
        public List<HotKey> hotkeys()
        {
            return hotkeys1;
        }
        public void SetDestEngine()
        {
            if (_visualizationEffect != null)
            {
                _visualizationEffect.Dispose();
                _visualizationEffect = null;
            }
            _visualizationEffect = new BasicEffect(GameEngine.Device)
            {
                VertexColorEnabled = true
            };
            transformator = new TransformManager(this);
        }

        public void SetFont(SpriteFont font)
        {
            Font1 = font;
        }

        public void SetActiveObjects(ObjectContainer lo, bool back)
        {
            if (!back && !lo.Same(activeObject))
            {
                NotificationCenter.postNotification("NOTIFICATION_ACTIVE_OBJECT_CHANGED", activeObject);
                Actions.ChangeActiveObject newaction = new Actions.ChangeActiveObject(activeObject);
                actions.AddNewAction(newaction);
            }
            activeObject = lo;
            transformator.SetActiveObject(lo);
        }
        public void biggerArrows()
        {
            ArrowSize *= 2;
        }
        public void smallerArrows()
        {
            ArrowSize *= 0.5f;
        }
        public void revertAction()
        {
            if (transformator.IsFree())
            {
                Actions.EditorAction laastaction = actions.RemoveLastAction();
                if (laastaction != null)
                {
                    laastaction.CancelAction(this);

                    transformator.UpdateView();
                    ConsoleWindow.TraceMessage("Reverting last action, action stack contains " + actions.Count + " elements");
                }
                else
                    ConsoleWindow.TraceMessage("Cannot to revert last action - stack is empty");
            }
        }

        public float ArrowSize
        {
            get { return transformator.ArrowSize; }
            set { transformator.ArrowSize = value; }
        }

        public void Update()
        {
            transformator.Update();
        }

        private PivotObject SearchClickedObject(Ray mr)
        {
            //ищем объект на кот тыкнули
            PivotObject clickedlo = null;
            float distance = 10000;


            Vector3 camerapos = mr.Position;
            foreach (PivotObject lo in gameScene.VisibleObjects)
            {
                Vector3? point = lo.raycastaspect.IntersectionClosest(mr, lo.transform);
                if (point != null)
                {
                    float range = (point.Value - camerapos).Length();
                    if (range < distance)
                    {
                        clickedlo = lo;
                        distance = range;
                    }
                }
            }
            return clickedlo;
        }



        public void Update(Ray mouseary, Vector2 mousepos)
        {
            MouseState mouseState = Mouse.GetState();
            if (GameEngine.actionToInterface)
            {
                if (GameEngine.actionFromInterface)
                {
                    GameEngine.actionFromInterface = false;
                    needUpdate = true;
                }
                else
                {
                    transformator.Update();
                    return;
                }
            }

            if (needUpdate)
            {
                needUpdate = false;
                transformator.Update(mouseary);
                if (!Interface.MouseManager.IsMouseCaptured)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed && lmbwasreleased)
                    {
                        PivotObject lo = SearchClickedObject(mouseary);
                        KeyboardState ks = Keyboard.GetState();
                        if (lo != null)
                        {

                            if (ks.IsKeyDown(Keys.LeftControl))
                            {
                                ObjectContainer newactiveobjects = new ObjectContainer(activeObject.objects.ToArray());
                                if (activeObject.Contains(lo))
                                {
                                    lo.SetActive(false);
                                    newactiveobjects.RemoveObject(lo);
                                }
                                else
                                {
                                    lo.SetActive(true);
                                    newactiveobjects.AddObject(lo);
                                }
                                SetActiveObjects(newactiveobjects, false);
                            }
                            else
                            {
                                ObjectContainer newactiveobjects = new ObjectContainer();
                                if (activeObject.Contains(lo))
                                {
                                    lo.SetActive(false);
                                    newactiveobjects.RemoveObject(lo);
                                }
                                else
                                {
                                    lo.SetActive(true);
                                    newactiveobjects.AddObject(lo);
                                }
                                SetActiveObjects(newactiveobjects, false);
                            }
                        }
                        else
                            if (!ks.IsKeyDown(Keys.LeftControl))
                                SetActiveObjects(new ObjectContainer(), false);
                    }
                }

            }
            if (mouseState.LeftButton == ButtonState.Pressed)
                lmbwasreleased = false;
            else
                lmbwasreleased = true;

        }
        
        public void Draw(SpriteBatch sb)
        {
            needUpdate = true;

            if (activeObject != null)
            {
                _visualizationEffect.View = GameEngine.Instance.Camera.View;
                _visualizationEffect.Projection = GameEngine.Instance.Camera.Projection;
                _visualizationEffect.CurrentTechnique.Passes[0].Apply();

                transformator.Draw(sb, GameEngine.Instance.mousePos);
            }
            
        }
        ~GameEditor()
        {
            KeyboardManager.Manager.RemoveKeyboardUser(this);
        }
    }
}

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ResourceCollectorXNA.Engine.Actions;
using ResourceCollectorXNA.Engine.Interface;
using ResourceCollectorXNA.Engine.Logic;


//using StillDesign.PhysX;


namespace ResourceCollectorXNA.Engine
{
    public class GameEditor : IKeyboardUserInterface
    {
        public static BasicEffect _visualizationEffect;
        public static SpriteFont Font1;

        public ActionStack actions;
        private ObjectContainer activeObject;
        private GameScene gameScene;
        private List<HotKey> hotkeys1;

        private bool lmbwasreleased = true;
        private bool needUpdate = true;
        public TransformManager transformator;


        public GameEditor(GameScene scene)
        {
            actions = new ActionStack();
            gameScene = scene;

            hotkeys1 = new List<HotKey>();
            KeyboardManager.Manager.AddKeyboardUser(this);
            var ctrlz = new HotKey(new[] { Keys.LeftControl, Keys.Z }, revertAction);
            hotkeys1.Add(ctrlz);
            var biggerarrows = new HotKey(new[] { Keys.OemPlus }, biggerArrows);
            hotkeys1.Add(biggerarrows);
            var smallerarrows = new HotKey(new[] { Keys.OemMinus }, smallerArrows);
            hotkeys1.Add(smallerarrows);
            var deleteobjects = new HotKey(new[] { Keys.Delete }, DeleteSelectedObjects);
            hotkeys1.Add(deleteobjects);
            activeObject = new ObjectContainer();
        }


        public float ArrowSize
        {
            get { return transformator.ArrowSize; }
            set { transformator.ArrowSize = value; }
        }

        #region IKeyboardUserInterface Members
        public bool IsKeyboardCaptured()
        {
            return true;
        }


        public List<HotKey> hotkeys()
        {
            return hotkeys1;
        }
        #endregion

        // метод вызывается при нажатии на delete, происходит удаление объекта со сцены
        public void DeleteSelectedObjects()
        {
            if (activeObject.Length != 0)
            {
                var deletingObjects = new MyContainer<PivotObject>(activeObject.Length, 1);
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
                var action = new AddObjectPivotAction(new ObjectContainer(objects));
                actions.AddNewAction(action);
            }
        }


        public void DeleteObjects(MyContainer<PivotObject> objects, bool back = false)
        {
            gameScene.DeleteObjects(objects);
            if (!back)
            {
                var action = new DeleteObjectPivotAction(new ObjectContainer(objects));
                actions.AddNewAction(action);
            }
        }


        public void Clear()
        {
            actions.clear();
            SetActiveObjects(new ObjectContainer(), true);
        }


        public void SetDestEngine()
        {
            if (_visualizationEffect != null)
            {
                _visualizationEffect.Dispose();
                _visualizationEffect = null;
            }
            _visualizationEffect = new BasicEffect(GameEngine.Device) { VertexColorEnabled = true };
            transformator = new TransformManager(this);
        }


        public void SetFont(SpriteFont font)
        {
            Font1 = font;
        }


        public void SetActiveObjects(ObjectContainer lo, bool back = false)
        {
            if (!back && !lo.Same(activeObject))
            {
                NotificationCenter.postNotification("NOTIFICATION_ACTIVE_OBJECT_CHANGED", activeObject);
                var newaction = new ChangeActiveObject(activeObject);
                actions.AddNewAction(newaction);
            }
            activeObject = lo;
            int numActiveObj = lo.Length;
            uint[] ids = new uint[numActiveObj];
            for (int i = 0; i < numActiveObj; ++i)
            {
                ids[i] = lo[i].editorAspect.id;
            }
            MyGame.levelform.SetActiveObjects(ids, back);
            transformator.SetActiveObject(lo);
        }


        public void SetActiveObjects(uint[] objectIds, bool back = false)
        {
            List<PivotObject> objectslist = new List<PivotObject>();
            for (int i = 0; i < objectIds.Length; i++)
            {
                PivotObject obj = gameScene.GetObjectWithID(objectIds[i]);
                if (obj != null)
                    objectslist.Add(obj);
            }


            SetActiveObjects(new ObjectContainer(objectslist.ToArray()), back);
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
                EditorAction laastaction = actions.RemoveLastAction();
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
                if (!MouseManager.IsMouseCaptured)
                {
                    if (mouseState.LeftButton == ButtonState.Pressed && lmbwasreleased)
                    {
                        PivotObject lo = SearchClickedObject(mouseary);
                        KeyboardState ks = Keyboard.GetState();
                        if (lo != null)
                        {
                            if (ks.IsKeyDown(Keys.LeftControl))
                            {
                                var newactiveobjects = new ObjectContainer(activeObject.objects.ToArray());
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
                                var newactiveobjects = new ObjectContainer();
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
                        else if (!ks.IsKeyDown(Keys.LeftControl))
                            SetActiveObjects(new ObjectContainer(), false);
                    }
                }
            }
            lmbwasreleased = (mouseState.LeftButton != ButtonState.Pressed);
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
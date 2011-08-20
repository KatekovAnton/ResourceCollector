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

namespace ResourceCollectorXNA.Engine.Interface
{
    public class TransformManager:IMouseUserInterface
    {
        private Axis axis;
        private TransformManagerState currentState; 
        private MoveArrows ma;
        private RotateCircles rc;
        private ObjectContainer activeObject;
        private Actions.PivotAction currentaction;
        private bool lockedaxis = false;
        private GameEditor editor;

        public TransformManager(GameEditor edit)
        {
            ma = new MoveArrows();
            rc = new RotateCircles();
            editor = edit;
            currentState = TransformManagerState.select;
            activeObject = new ObjectContainer();
            GameEngine.windowController.setSelect();
        }
        public bool IsFree()
        {
            return currentaction == null;
        }
        public void SetActiveObject(ObjectContainer @object)
        {
            if (activeObject != null)
                foreach (PivotObject po in activeObject.objects)
                    po.SetActive(false);
            if (@object.Length != 0)
            {
                
                foreach (PivotObject po in @object.objects)
                {
                    po.SetActive(true);
                }
               // @object.RecalculateMiddle();
                ma.SetTransformMatrix(@object.middleMarix);
                rc.SetTransformMatrix(@object.middleMarix);
            }
            activeObject = @object;
        }
        public float ArrowSize
        {
            get 
            {
                return ma.visibleArrowsSize;
            }
            set
            {
                ma.visibleArrowsSize = value;
                rc.visibleArrowsSize = value;
            }
        }
        public void Update()
        {
            switch (currentState)
            {
                case TransformManagerState.move:
                    ma.UpdateData();break;
                case TransformManagerState.rotatesame:
                case TransformManagerState.rotatelocal:
                    rc.UpdateData();break;
            }
        }
        public void UpdateView()
        {
            switch (currentState)
            {
                case TransformManagerState.move:
                    {
                        Vector3 transl = ma.transform.Translation;
                        if (activeObject.Length == 1)
                            GameEngine.windowController.setMove(new string[] { transl.X.ToString(), transl.Y.ToString(), transl.Z.ToString() });
                        else
                            GameEngine.windowController.setRelative(threezeros);
                    }break;
                default: break;
            }
        }
        public void SwitchState(TransformManagerState newstate)
        {
            if (newstate != currentState)
            {
                mymouse = false;
                lockedaxis = false;
                axis = Axis.none;
                if (currentaction != null)
                {
                    currentaction.CancelAction(this.editor);
                    ma.SetTransformMatrix(activeObject.middleMarix);
                    lockedaxis = false;
                    currentaction = null;
                    axis = Axis.none;
                }
                switch (newstate)
                {
                    case TransformManagerState.move:
                        {
                            if (activeObject.Length != 0)
                                ma.SetTransformMatrix(activeObject.middleMarix);

                            Vector3 transl = ma.transform.Translation;
                            if (activeObject.Length == 1)
                                GameEngine.windowController.setMove(new string[] { transl.X.ToString(), transl.Y.ToString(), transl.Z.ToString() });
                            else
                                GameEngine.windowController.setRelative(threezeros);
                        }break;
                    case TransformManagerState.rotatelocal:
                    case TransformManagerState.rotatesame:
                        {
                            if (activeObject.Length != 0)
                                rc.SetTransformMatrix(activeObject.middleMarix);

                            
                            
                        }break;
                }
                currentState = newstate;
            }
        }
        private string[] threezeros = new string[] { "0", "0", "0" };
        private bool mymouse;
       // private bool lmbwasreleased = true;
        public override bool IsMouseCaptured()
        {
                return mymouse;
        }
        private void updatemove(Ray mouseary)
        {
            
            MouseState mouseState = Mouse.GetState();
            if (!lockedaxis)
            {
                if (mouseState.RightButton == ButtonState.Pressed)
                    ma.UpdateData();
                else
                {
                    if (activeObject.Length != 0)
                    {
                        axis = ma.UpdateData(mouseary);
                        if (axis != Axis.none && mouseState.LeftButton == ButtonState.Pressed)
                        {
                            lockedaxis = true;
                            mymouse = true;
                        }
                        else
                            mymouse = false;
                    }
                }
            }

            if (currentaction == null)
            {
                Vector3 transl = ma.transform.Translation;
                if (activeObject.Length != 0 && mouseState.LeftButton == ButtonState.Pressed && axis != Axis.none)
                {
                    //begin
                    lockedaxis = true;
                    //Vector3 transl = ma.transform.Translation;
                    currentaction = new Actions.DragPivotObject(activeObject, axis, new Actions.PivotActionUpdateParameters(mouseary, new Vector2(mouseState.X, mouseState.Y), transl));
                    if (activeObject.Length == 1)
                        GameEngine.windowController.setMove(new string[] { transl.X.ToString(), transl.Y.ToString(), transl.Z.ToString() });
                    else
                        GameEngine.windowController.setRelative(threezeros);
                }
                else
                    if (activeObject.Length == 0)
                        GameEngine.windowController.setRelative(null);
                    else if (activeObject.Length == 1)
                        GameEngine.windowController.setMove(new string[] { transl.X.ToString(), transl.Y.ToString(), transl.Z.ToString() });
                    else GameEngine.windowController.setRelative(threezeros);
            }
            else
            {
                if (mouseState.LeftButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released)
                {
                    //continue
                    lockedaxis = true;
                    Vector3 transl = ma.transform.Translation;
                    currentaction.UpdateAction(new Actions.PivotActionUpdateParameters(mouseary, new Vector2(mouseState.X, mouseState.Y), transl));
                    ma.SetTransformMatrix(activeObject.middleMarix);

                    Vector3 difference = (Vector3)currentaction.ActionResult;
                    GameEngine.windowController.setRelative(new string[] { difference.X.ToString(), difference.Y.ToString(), difference.Z.ToString() });

                    return;
                }
                if (mouseState.LeftButton == ButtonState.Released && mouseState.RightButton == ButtonState.Released)
                {
                    //stop
                    lockedaxis = false;
                    if(currentaction.Valid())
                        editor.actions.AddNewAction(currentaction);
                    currentaction = null;
                    axis = Axis.none;

                    
                    if (activeObject.Length == 1)
                    {
                        Vector3 transl = ma.transform.Translation;
                        GameEngine.windowController.setMove(new string[] { transl.X.ToString(), transl.Y.ToString(), transl.Z.ToString() });
                    }
                    else
                        GameEngine.windowController.setRelative(threezeros);

                    return;
                }
                if (mouseState.LeftButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Pressed)
                {
                    //abort
                    currentaction.CancelAction(this.editor);



                    ma.SetTransformMatrix(activeObject.middleMarix);
                    Vector3 transl = ma.transform.Translation;
                    if (activeObject.Length == 1)
                        GameEngine.windowController.setMove(new string[] { transl.X.ToString(), transl.Y.ToString(), transl.Z.ToString() });
                    else
                        GameEngine.windowController.setRelative(threezeros);
                    


                    lockedaxis = false;
                    currentaction = null;
                    axis = Axis.none;
                    return;
                }


            }
        }
        
        private void updaterotate(Ray mouseary)
        {
            MouseState mouseState = Mouse.GetState();
            if (!lockedaxis)
            {
                if (mouseState.RightButton == ButtonState.Pressed)
                    rc.UpdateData();
                else
                {
                    if (activeObject.Length != 0)
                    {
                        axis = rc.UpdateData(mouseary);
                        if (axis != Axis.none && mouseState.LeftButton == ButtonState.Pressed)
                        {
                            lockedaxis = true;
                            mymouse = true;
                        }
                        else
                        {
                            mymouse = false;
                            if (activeObject.Length == 1)
                            {
                                Quaternion q;
                                Vector3 v, vs;

                                activeObject[0].transform.Decompose(out vs, out q, out v);
                                GameEngine.windowController.setRotationLocal(new string[]{q.X.ToString(), q.Y.ToString(), q.Z.ToString(), q.W.ToString()});
                            }
                            else
                                GameEngine.windowController.setRotationLocal(new string[] { "0", "0", "0", "1" });
                        }
                    }
                    else GameEngine.windowController.setRelative(null);
                }
            }

            if (currentaction == null)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && axis != Axis.none)
                {
                    //begin
                    lockedaxis = true;
                    currentaction = new Actions.RotateLocalPivotAction(activeObject, axis, new Actions.PivotActionUpdateParameters(mouseary, new Vector2(mouseState.X, mouseState.Y), rc.transform.Translation));

                    Quaternion res = (Quaternion)currentaction.ActionResult;

                    GameEngine.windowController.setRotationLocal(new string[] { res.X.ToString(), res.Y.ToString(), res.Z.ToString(), res.W.ToString() });
                }
            }
            else
            {
                if (mouseState.LeftButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released)
                {
                    //continue
                    lockedaxis = true;
                    currentaction.UpdateAction(new Actions.PivotActionUpdateParameters(mouseary, new Vector2(mouseState.X, mouseState.Y),rc.transform.Translation));
                   // ma.SetTransformMatrix(activeObject.transform.TranslationMatrix());
                    rc.SetTransformMatrix(activeObject.middleMarix);
                    Quaternion res = (Quaternion)currentaction.ActionResult;
                    GameEngine.windowController.setRotationLocal(new string[] { res.X.ToString(), res.Y.ToString(), res.Z.ToString(), res.W.ToString() });
                    return;
                }
                if (mouseState.LeftButton == ButtonState.Released && mouseState.RightButton == ButtonState.Released)
                {
                    //stop
                    lockedaxis = false;
                    if (currentaction.Valid())
                        editor.actions.AddNewAction(currentaction);

                    if (activeObject.Length == 1)
                    {
                        Quaternion res = (Quaternion)currentaction.ActionResult;
                        GameEngine.windowController.setRotationLocal(new string[] { res.X.ToString(), res.Y.ToString(), res.Z.ToString(), res.W.ToString() });
                    }
                    currentaction = null;
                    axis = Axis.none;
                    return;
                }
                if (mouseState.LeftButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Pressed)
                {
                    //abort
                    currentaction.CancelAction(this.editor);

                    rc.SetTransformMatrix(activeObject.middleMarix);
                    lockedaxis = false;
                    if (activeObject.Length == 1)
                    {
                        Quaternion res = (Quaternion)currentaction.ActionResult;
                        GameEngine.windowController.setRotationLocal(new string[] { res.X.ToString(), res.Y.ToString(), res.Z.ToString(), res.W.ToString() });
                    }
                    currentaction = null;
                    axis = Axis.none;
                    return;
                }
            }
        }

        private void updaterotatesame(Ray mouseary)
        {
            MouseState mouseState = Mouse.GetState();
            if (!lockedaxis)
            {
                if (mouseState.RightButton == ButtonState.Pressed)
                    rc.UpdateData();
                else
                {
                    if (activeObject.Length != 0)
                    {
                        axis = rc.UpdateData(mouseary);
                        if (axis != Axis.none && mouseState.LeftButton == ButtonState.Pressed)
                        {
                            lockedaxis = true;
                            mymouse = true;
                        }
                        else
                        {
                            mymouse = false;
                            if (activeObject.Length == 1)
                            {
                                Quaternion q;
                                Vector3 v, vs;

                                activeObject[0].transform.Decompose(out vs, out q, out v);
                                GameEngine.windowController.setRotationSame(new string[] { q.X.ToString(), q.Y.ToString(), q.Z.ToString(), q.W.ToString() });
                            }
                            else
                                GameEngine.windowController.setRotationSame(new string[] { "0", "0", "0", "1" });
                        }
                    }
                    else GameEngine.windowController.setRelative(null);
                }
            }

            if (currentaction == null)
            {
                if (mouseState.LeftButton == ButtonState.Pressed && axis != Axis.none)
                {
                    //begin
                    lockedaxis = true;
                    currentaction = new Actions.RotateSamePivotAction(activeObject, axis, new Actions.PivotActionUpdateParameters(mouseary, new Vector2(mouseState.X, mouseState.Y), rc.transform.Translation));
                    Quaternion res = (Quaternion)currentaction.ActionResult;

                    GameEngine.windowController.setRotationSame(new string[] { res.X.ToString(), res.Y.ToString(), res.Z.ToString(), res.W.ToString() });
                }
              
            }
            else
            {
                if (mouseState.LeftButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Released)
                {
                    //continue
                    lockedaxis = true;
                    currentaction.UpdateAction(new Actions.PivotActionUpdateParameters(mouseary, new Vector2(mouseState.X, mouseState.Y), rc.transform.Translation));
                    // ma.SetTransformMatrix(activeObject.transform.TranslationMatrix());
                    rc.SetTransformMatrix(activeObject.middleMarix);
                    Quaternion res = (Quaternion)currentaction.ActionResult;
                    GameEngine.windowController.setRotationSame(new string[] { res.X.ToString(), res.Y.ToString(), res.Z.ToString(), res.W.ToString() });
                    return;
                }
                if (mouseState.LeftButton == ButtonState.Released && mouseState.RightButton == ButtonState.Released)
                {
                    //stop
                    lockedaxis = false;
                    if (currentaction.Valid())
                        editor.actions.AddNewAction(currentaction);
                    if (activeObject.Length == 1)
                    {
                        Quaternion res = (Quaternion)currentaction.ActionResult;
                        GameEngine.windowController.setRotationSame(new string[] { res.X.ToString(), res.Y.ToString(), res.Z.ToString(), res.W.ToString() });
                    }
                    
                    currentaction = null;
                    axis = Axis.none;
                    return;
                }
                if (mouseState.LeftButton == ButtonState.Pressed && mouseState.RightButton == ButtonState.Pressed)
                {
                    //abort
                    currentaction.CancelAction(this.editor);

                    rc.SetTransformMatrix(activeObject.middleMarix);
                    lockedaxis = false;
                    if (activeObject.Length == 1)
                    {
                        Quaternion res = (Quaternion)currentaction.ActionResult;
                        GameEngine.windowController.setRotationSame(new string[] { res.X.ToString(), res.Y.ToString(), res.Z.ToString(), res.W.ToString() });
                    }
                    currentaction = null;
                    axis = Axis.none;
                    return;
                }
            }
        }
        
        public void Update(Ray mouseary)
        {
            
            if (activeObject != null)
            {
                switch (currentState)
                {
                    case TransformManagerState.move:
                        {
                            updatemove(mouseary);
                        }break;
                    case TransformManagerState.rotatelocal:
                        {
                            updaterotate(mouseary);
                        }break;
                    case TransformManagerState.rotatesame:
                        {
                            updaterotatesame(mouseary);
                        } break;
                }
            }
        }
        private Vector2 delta = new Vector2(10, 0);
        public void Draw(SpriteBatch sb, Vector2 mousePos)
        {
            if (activeObject.Length != 0)
            {
                switch (currentState)
                {
                    case TransformManagerState.move:
                        {
                            GameEditor._visualizationEffect.World = Matrix.CreateScale(ma.multiplier * 9) * ma.transform;
                            ma.DrawPivot(sb);
                            switch (axis)
                            {
                                case Axis.X:
                                    sb.DrawString(GameEditor.Font1, "x", mousePos - delta, InterfaceColors.ActiveAxisColor);
                                    break;
                                case Axis.Y:
                                    sb.DrawString(GameEditor.Font1, "y", mousePos - delta, InterfaceColors.ActiveAxisColor);
                                    break;
                                case Axis.Z:
                                    sb.DrawString(GameEditor.Font1, "z", mousePos - delta, InterfaceColors.ActiveAxisColor);
                                    break;
                                case Axis.XoZ:
                                    sb.DrawString(GameEditor.Font1, "xoz", mousePos - delta * 2, InterfaceColors.ActiveAxisColor);
                                    break;
                                case Axis.YoZ:
                                    sb.DrawString(GameEditor.Font1, "yoz", mousePos - delta * 2, InterfaceColors.ActiveAxisColor);
                                    break;
                                case Axis.XoY:
                                    sb.DrawString(GameEditor.Font1, "xoy", mousePos - delta * 2, InterfaceColors.ActiveAxisColor);
                                    break;
                            }
                        } break;
                    case TransformManagerState.rotatesame:
                    case TransformManagerState.rotatelocal:
                        {
                            GameEditor._visualizationEffect.World = Matrix.CreateScale(rc.multiplier) * rc.transform;
                            rc.Draw(sb);
                        } break;
                }
            }
        }
        public static float isintersect(Ray ray, VertexPositionColor[] axisvertices, short[] indices, Matrix transform)
        {
            var detransform = Matrix.Invert(transform);

            Vector3 p1 = Vector3.Transform(ray.Position, detransform);
            Vector3 p2 = Vector3.Transform(ray.Direction + ray.Position, detransform);
            p2 -= p1;

            var isIntersected = false;
            var distance = 0.0f;
            for (int i = 0; i < indices.Length; i += 3)
            {
                Vector3 v0 = axisvertices[indices[i]].Position;
                Vector3 v1 = axisvertices[indices[i + 1]].Position - axisvertices[indices[i]].Position;
                Vector3 v2 = axisvertices[indices[i + 2]].Position - axisvertices[indices[i]].Position;

                // solution of linear system
                // finds line and plane intersection point (if exists)
                float determinant =
                    -p2.Z * v1.Y * v2.X + p2.Y * v1.Z * v2.X + p2.Z * v1.X * v2.Y
                    - p2.X * v1.Z * v2.Y - p2.Y * v1.X * v2.Z + p2.X * v1.Y * v2.Z;

                if (determinant * determinant < 0.000000001f)
                    continue;

                float kramer = 1.0f / determinant;

                float t1 =
                     (p1.Z * p2.Y * v2.X - p1.Y * p2.Z * v2.X + p2.Z * v0.Y * v2.X
                    - p2.Y * v0.Z * v2.X - p1.Z * p2.X * v2.Y + p1.X * p2.Z * v2.Y
                    - p2.Z * v0.X * v2.Y + p2.X * v0.Z * v2.Y + p1.Y * p2.X * v2.Z
                    - p1.X * p2.Y * v2.Z + p2.Y * v0.X * v2.Z - p2.X * v0.Y * v2.Z) *
                    kramer;

                if (t1 < 0)
                    continue;

                float t2 =
                    -(p1.Z * p2.Y * v1.X - p1.Y * p2.Z * v1.X + p2.Z * v0.Y * v1.X
                    - p2.Y * v0.Z * v1.X - p1.Z * p2.X * v1.Y + p1.X * p2.Z * v1.Y
                    - p2.Z * v0.X * v1.Y + p2.X * v0.Z * v1.Y + p1.Y * p2.X * v1.Z
                    - p1.X * p2.Y * v1.Z + p2.Y * v0.X * v1.Z - p2.X * v0.Y * v1.Z) *
                    kramer;

                if (t2 < 0)
                    continue;

                float t3 =
                    (-p1.Z * v1.Y * v2.X + v0.Z * v1.Y * v2.X + p1.Y * v1.Z * v2.X
                    - v0.Y * v1.Z * v2.X + p1.Z * v1.X * v2.Y - v0.Z * v1.X * v2.Y
                    - p1.X * v1.Z * v2.Y + v0.X * v1.Z * v2.Y - p1.Y * v1.X * v2.Z
                    + v0.Y * v1.X * v2.Z + p1.X * v1.Y * v2.Z - v0.X * v1.Y * v2.Z) *
                    (-kramer);

                if (t3 < 0)
                    continue;

                // (t1>=0 && t2>=0 && t1+t2<=0.5)  => point is on face
                // (t3>0)  =>  point is on positive ray direction
                if (t1 + t2 > 1.0f)
                    continue;

                if (!isIntersected || distance > t3)
                {
                    isIntersected = true;
                    distance = t3;
                    break;
                }
            }
            if (isIntersected)
                //return new Vector3?( Vector3.Transform((p1 + p2 * distance), transform));
                return distance;
            return -1000;
        }
    }
    public enum TransformManagerState {select, move,rotatesame, rotatelocal,scale};
}

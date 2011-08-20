using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using ResourceCollectorXNA;
using ResourceCollectorXNA.Content;
using ResourceCollectorXNA.Engine;
using ResourceCollectorXNA.Engine.Logic;
using ResourceCollectorXNA.Engine.Interface;

using Microsoft.Xna.Framework;


namespace ResourceCollectorXNA.Engine.Actions
{
    public class DragPivotObject : PivotAction
    {
        Axis axis;
        Vector3 objectMovingUnit1;
        Vector3 startDelta;
        Vector3 startpos;
       // Vector3 startmiddle;
        public DragPivotObject(ObjectContainer dragableObject, Axis _axis, object _data)
            : base(dragableObject)
        {
            axis = _axis;
            if (axis == Axis.none)
                throw new Exception("axis is none in creating DragPivotObject action!");
            PivotActionUpdateParameters data = _data as PivotActionUpdateParameters;
            if (data == null)
                throw new Exception("Invalid parameter in DragPivotObject::UpdateAction");

           // startmiddle = dragableObject.middle;

            startDelta = intersectPoint(data.ray).Value - dragableObject.middle;
            startpos = dragableObject.middle;
            if (axis == Axis.X)
                objectMovingUnit1 = Vector3.UnitX;
            else if (axis == Axis.Y)
                objectMovingUnit1 = Vector3.UnitY;
            else if (axis == Axis.Z)
                objectMovingUnit1 = Vector3.UnitZ;

            else if (axis == Axis.XoY)
                objectMovingUnit1 = Vector3.UnitY + Vector3.UnitX;
            else if (axis == Axis.XoZ)
                objectMovingUnit1 = Vector3.UnitZ + Vector3.UnitX;
            else if (axis == Axis.YoZ)
                objectMovingUnit1 = Vector3.UnitZ + Vector3.UnitY;
            
           
        }
        
        Vector3? intersectPoint(Ray ray)
        {
            Vector3? point = null;
            
            if (axis == Axis.X)
            {
                point = MoveArrows.isintersectVector(ray, MoveArrows.XoZPlaneVertices, MoveArrows.PlaneIndices, operatingObject.middleMarix);
                if (point == null)
                {
                    point = MoveArrows.isintersectVector(ray, MoveArrows.XoYPlaneVertices, MoveArrows.PlaneIndices, operatingObject.middleMarix);
                    if (point == null)
                    {
                        point = MoveArrows.isintersectVector(ray, MoveArrows.YoZPlaneVertices, MoveArrows.PlaneIndices, operatingObject.middleMarix);
                    }
                }
            }
            else if (axis == Axis.Y)
            {
                point = MoveArrows.isintersectVector(ray, MoveArrows.XoYPlaneVertices, MoveArrows.PlaneIndices, operatingObject.middleMarix);
                if (point == null)
                {
                    point = MoveArrows.isintersectVector(ray, MoveArrows.YoZPlaneVertices, MoveArrows.PlaneIndices, operatingObject.middleMarix);
                    if (point == null)
                    {
                        point = MoveArrows.isintersectVector(ray, MoveArrows.XoZPlaneVertices, MoveArrows.PlaneIndices, operatingObject.middleMarix);
                    }
                }
            }
            else if (axis == Axis.Z)
            {
                point = MoveArrows.isintersectVector(ray, MoveArrows.XoZPlaneVertices, MoveArrows.PlaneIndices, operatingObject.middleMarix);
                if (point == null)
                {
                    point = MoveArrows.isintersectVector(ray, MoveArrows.YoZPlaneVertices, MoveArrows.PlaneIndices, operatingObject.middleMarix);
                    if (point == null)
                    {
                        point = MoveArrows.isintersectVector(ray, MoveArrows.XoYPlaneVertices, MoveArrows.PlaneIndices, operatingObject.middleMarix);
                    }
                }
            }
            else if (axis == Axis.XoY)
            {
                point = MoveArrows.isintersectVector(ray, MoveArrows.XoYPlaneVertices, MoveArrows.PlaneIndices, operatingObject.middleMarix);
            }
            else if (axis == Axis.YoZ)
            {
                point = MoveArrows.isintersectVector(ray, MoveArrows.YoZPlaneVertices, MoveArrows.PlaneIndices, operatingObject.middleMarix);
            }
            else if (axis == Axis.XoZ)
            {
                point = MoveArrows.isintersectVector(ray, MoveArrows.XoZPlaneVertices, MoveArrows.PlaneIndices, operatingObject.middleMarix);
            }
            return point;
        }
        public override void UpdateAction(object _data)
        {
            PivotActionUpdateParameters data = _data as PivotActionUpdateParameters;
            if (data == null)
                throw new Exception("Invalid parameter in DragPivotObject::UpdateAction");

            Vector3? point = intersectPoint(data.ray);
            if (point != null)
            { 
                operatingObject.RecalculateMiddle();
                Vector3 delta = point.Value - operatingObject.middle - startDelta;
                delta.X *= objectMovingUnit1.X;
                delta.Y *= objectMovingUnit1.Y;
                delta.Z *= objectMovingUnit1.Z;
               
                for (int i = 0; i < @operatingObject.Length; i++)
                    operatingObject[i].Move(delta);

                if (operatingObject.Length == 1)
                    ActionResult = operatingObject[0].transform.Translation;
                else
                    ActionResult = operatingObject.middle - startpos;
                
            }
        }
        public override void CancelAction(Engine.GameEditor Editor)
        {
            for (int i = 0; i < this.operatingObject.Length; i++)
            {
                operatingObject[i].SetGlobalPose(StartTransform[i]);
                operatingObject[i].neetforceupdate = true;
            }
            operatingObject.RecalculateMiddle();
            Editor.SetActiveObjects(operatingObject,true);
        }
    }
}

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
    class RotateSamePivotAction : PivotAction
    {
        Axis axis;
        Vector3 objectRotatingUnit1;
        Vector2 startMousePos;
        Matrix[] startTransforms;
        Vector3 rotationPoint;
        Matrix transl;
        Matrix mintransl;
        public RotateSamePivotAction(ObjectContainer dragableObject, Axis _axis, object _data)
            : base(dragableObject)
        {
            axis = _axis;
            if (axis == Axis.none)
                throw new Exception("axis is none in creating DragPivotObject action!");
            PivotActionUpdateParameters data = _data as PivotActionUpdateParameters;
            if (data == null)
                throw new Exception("Invalid parameter in DragPivotObject::UpdateAction");

            startMousePos = data.mousepos;
            if (axis == Axis.X)
                objectRotatingUnit1 = Vector3.UnitX;
            else if (axis == Axis.Y)
                objectRotatingUnit1 = Vector3.UnitY;
            else if (axis == Axis.Z)
                objectRotatingUnit1 = Vector3.UnitZ;

            rotationPoint = data.AxisPoint;
            transl = Matrix.CreateTranslation(rotationPoint);
            mintransl = Matrix.CreateTranslation(-rotationPoint);
            startTransforms = new Matrix[operatingObject.Length];
            for (int i = 0; i < operatingObject.Length; i++)
                startTransforms[i] = dragableObject[i].transform;

            ActionResult = Quaternion.CreateFromAxisAngle(objectRotatingUnit1, 0);
        }
        public override void CancelAction(Engine.GameEditor Editor)
        {
            for (int i = 0; i < this.operatingObject.Length; i++)
            {
                operatingObject[i].SetGlobalPose(StartTransform[i]);
                operatingObject[i].neetforceupdate = true;
            }
            Editor.SetActiveObjects(operatingObject, true);
        }
        public override void UpdateAction(object parameters)
        {
            PivotActionUpdateParameters data = parameters as PivotActionUpdateParameters;
            if (data == null)
                throw new Exception("Invalid parameter in RotatePivotAction::UpdateAction");

            Vector2 delta = data.mousepos - startMousePos;
            float length = delta.Y;
            if (length != 0)
            {
                float angle = length / 100;
                if (DragPivotObject.currentstate == state.one)
                {
                    float pi = MathHelper.Pi / 180;
                    angle = ((float)((int)(angle / pi))) * pi;
                }
                else if (DragPivotObject.currentstate == state.five)
                {
                    float pi = MathHelper.Pi / 180 * 5;
                    angle = ((float)((int)(angle / pi))) * pi;
                }
                Quaternion res = Quaternion.CreateFromAxisAngle(objectRotatingUnit1, angle);
                Matrix roa = Matrix.CreateFromQuaternion(res);
                for (int i = 0; i < @operatingObject.Length; i++)
                {
                    Matrix result = startTransforms[i] * mintransl * roa * transl;
                    operatingObject[i].SetGlobalPose(result);
                }

                ActionResult = res;
            }
        }
    }
}

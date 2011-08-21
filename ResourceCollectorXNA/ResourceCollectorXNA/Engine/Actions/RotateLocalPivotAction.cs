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
    class RotateLocalPivotAction : PivotAction
    {
        Axis axis;
        Vector3 objectRotatingUnit1;
        Vector2 startMousePos;
        Quaternion[] startRotation;
        Vector3[] starTranslation;
        Vector3[] startScale;


        public RotateLocalPivotAction(ObjectContainer dragableObject, Axis _axis, object _data)
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

            startScale = new Vector3[operatingObject.Length];
            startRotation = new Quaternion[operatingObject.Length];
            starTranslation = new Vector3[operatingObject.Length];
            for (int i = 0; i < operatingObject.Length; i++)
                StartTransform[i].Decompose(out startScale[i], out startRotation[i], out starTranslation[i]);

            ActionResult = Quaternion.CreateFromAxisAngle(objectRotatingUnit1, 0);

        }


        public override void CancelAction(Engine.GameEditor Editor)
        {
            for (int i = 0; i < this.operatingObject.Length;i++ )
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
            float length = 0;
            switch(axis) {
                case Axis.X:
                    length = delta.Y;
                    break;
                case Axis.Y:
                    length = delta.X;
                    break;
                case Axis.Z:
                    length = delta.Y;
                    break;
            }

            if (length != 0)
            {
                float angle = length/100;
                if (DragPivotObject.currentstate == state.one) {
                    const float pi = MathHelper.Pi / 180;
                    angle = ((float)((int)(angle / pi))) * pi;
                } else if (DragPivotObject.currentstate == state.five) {
                    const float pi = MathHelper.Pi / 180 * 5;
                    angle = ((float)((int)(angle / pi))) * pi;
                }
                Quaternion roa = Quaternion.CreateFromAxisAngle(objectRotatingUnit1, angle);
                Quaternion qr = new Quaternion();
                for (int i = 0; i < @operatingObject.Length; i++)
                {
                    qr = Quaternion.Concatenate(startRotation[i], roa);
                    Matrix result = Matrix.CreateFromQuaternion(qr )* Matrix.CreateTranslation(starTranslation[i]);
                    operatingObject[i].SetGlobalPose(result);
                }
                ActionResult = operatingObject.Length == 1 ? qr : roa;
            }
        }
    }
}

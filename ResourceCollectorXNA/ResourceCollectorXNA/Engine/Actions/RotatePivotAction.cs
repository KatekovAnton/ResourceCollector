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
    class RotatePivotAction : PivotAction
    {
        Axis axis;
        Vector3 objectRotatingUnit1;
        Vector2 startMousePos;
        Quaternion[] startRotation;
        Vector3[] starTranslation;
        Vector3[] startScale;
        public RotatePivotAction(ObjectContainer dragableObject, Axis _axis, object _data)
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
            float length = delta.X;
            if (length != 0)
            {
                float angle = length/100;
                for (int i = 0; i < @operatingObject.Length; i++)
                {
                    Matrix result = Matrix.CreateFromQuaternion(Quaternion.Concatenate(startRotation[i], Quaternion.CreateFromAxisAngle(objectRotatingUnit1, angle))) * Matrix.CreateTranslation(starTranslation[i]);
                    operatingObject[i].SetGlobalPose(result);
                }
               
            }
        }
    }
}

using System;
using Microsoft.Xna.Framework;
using ResourceCollectorXNA.Engine.Interface;
using ResourceCollectorXNA.Engine.Logic;



namespace ResourceCollectorXNA.Engine.Actions{
    internal class RotateSamePivotAction : PivotAction{
        private Axis axis;
        private Matrix mintransl;
        private Vector3 objectRotatingUnit1;
        private Vector3 rotationPoint;
        private Vector2 startMousePos;
        private Matrix[] startTransforms;
        private Matrix transl;


        public RotateSamePivotAction(ObjectContainer dragableObject, Axis _axis, object _data) : base(dragableObject) {
            axis = _axis;
            if(axis == Axis.none)
                throw new Exception("axis is none in creating DragPivotObject action!");
            var data = _data as PivotActionUpdateParameters;
            if(data == null)
                throw new Exception("Invalid parameter in DragPivotObject::UpdateAction");

            startMousePos = data.mousepos;
            if(axis == Axis.X)
                objectRotatingUnit1 = Vector3.UnitX;
            else if(axis == Axis.Y)
                objectRotatingUnit1 = Vector3.UnitY;
            else if(axis == Axis.Z)
                objectRotatingUnit1 = Vector3.UnitZ;

            rotationPoint = data.AxisPoint;
            transl = Matrix.CreateTranslation(rotationPoint);
            mintransl = Matrix.CreateTranslation(-rotationPoint);
            startTransforms = new Matrix[operatingObject.Length];
            for(int i = 0; i < operatingObject.Length; i++)
                startTransforms[i] = dragableObject[i].transform;

            ActionResult = Quaternion.CreateFromAxisAngle(objectRotatingUnit1, 0);
        }


        public override void CancelAction(GameEditor Editor) {
            for(int i = 0; i < operatingObject.Length; i++) {
                operatingObject[i].SetGlobalPose(StartTransform[i]);
                operatingObject[i].neetforceupdate = true;
            }
            Editor.SetActiveObjects(operatingObject, true);
        }


        public override void UpdateAction(object parameters) {
            var data = parameters as PivotActionUpdateParameters;
            if(data == null)
                throw new Exception("Invalid parameter in RotatePivotAction::UpdateAction");

            Vector2 delta = data.mousepos - startMousePos;
            //float length = delta.Y;
            float length = 0;
            switch (axis) {
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

            if(length != 0) {
                float angle = length / 100;
                if(currentstate == state.one) {
                    const float pi = MathHelper.Pi / 180;
                    angle = (((int) (angle / pi))) * pi;
                } else if(currentstate == state.five) {
                    const float pi = MathHelper.Pi / 180 * 5;
                    angle = (((int) (angle / pi))) * pi;
                }
                Quaternion res = Quaternion.CreateFromAxisAngle(objectRotatingUnit1, angle);
                Matrix roa = Matrix.CreateFromQuaternion(res);
                for(int i = 0; i < @operatingObject.Length; i++) {
                    Matrix result = startTransforms[i] * mintransl * roa * transl;
                    operatingObject[i].SetGlobalPose(result);
                }

                ActionResult = res;
            }
        }
    }
}
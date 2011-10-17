using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ResourceCollectorXNA.Engine.Logic.BehaviourModel
{
    class ObjectBoneRelatedBehaviourModel: ObjectStaticBehaviourModel
    {
        public int parentBone;
        public Matrix localMatrix;
        public ObjectBehaviourModel parentObject;
        public ObjectBoneRelatedBehaviourModel(ObjectBehaviourModel parent, int _parentBone)
        {
            parentObject = parent;
            parentBone = _parentBone;
        }

        public override void SetGlobalPose(Matrix GlobalPoseMatrix, object Additionaldata)
        {
            localMatrix = GlobalPoseMatrix;
            Matrix resultMatrix = parentObject.CurrentPosition * localMatrix;
            CurrentPosition = resultMatrix;
        }

        public override void DoFrame(GameTime gametime)
        {
            Matrix newpos = parentObject.CurrentPosition * localMatrix;
            moved = newpos!=CurrentPosition;
            CurrentPosition = newpos;
        }

        public override void Move(Vector3 displacement)
        {
            localMatrix = localMatrix * Matrix.CreateTranslation(displacement);
            Matrix resultMatrix = parentObject.CurrentPosition * localMatrix;
            moved = CurrentPosition != resultMatrix;
            CurrentPosition = resultMatrix;
        }
    }
}

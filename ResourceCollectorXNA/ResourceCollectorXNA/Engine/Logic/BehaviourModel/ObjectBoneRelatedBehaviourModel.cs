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
      //  public ObjectBehaviourModel parentObject;
        public ObjectBoneRelatedBehaviourModel(/*ObjectBehaviourModel parent, int _parentBone*/)
        {
          //  parentObject = null;//parent;
            parentBone = 0;// _parentBone;
        }

        public override void SetGlobalPose(Matrix GlobalPoseMatrix, object Additionaldata)
        {
            localMatrix = GlobalPoseMatrix;
            Matrix resultMatrix = /*parentObject.CurrentPosition **/ localMatrix;
            moved = globalpose != resultMatrix;
            this.globalpose = resultMatrix;
        }

        public override void DoFrame(GameTime gametime)
        {
            CurrentPosition = globalpose;
        }

        public override void Move(Vector3 displacement)
        {
            localMatrix = localMatrix * Matrix.CreateTranslation(displacement);
            Matrix resultMatrix =/* parentObject.CurrentPosition * */localMatrix;
            moved = globalpose != resultMatrix;
            this.globalpose = resultMatrix;
        }
    }
}

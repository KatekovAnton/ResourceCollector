using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ResourceCollectorXNA.Engine.Logic.BehaviourModel
{
    class ObjectStaticBehaviourModel:ObjectBehaviourModel
    {
        public ObjectStaticBehaviourModel()
        {
            CurrentPosition = globalpose =  Matrix.Identity;
        }
        bool mov;
        
        public override void SetGlobalPose(Matrix GlobalPoseMatrix, object Additionaldata)
        {
            moved = true;
            this.globalpose = GlobalPoseMatrix;
        }
       
        public override void DoFrame(GameTime gametime)
        {
            CurrentPosition = globalpose;
        }
        public override void Move(Vector3 displacement)
        {
            SetGlobalPose(Matrix.Multiply(globalpose, Matrix.CreateTranslation(displacement)), null);
        }

    }
}

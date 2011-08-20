using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

//using StillDesign.PhysX;


namespace ResourceCollectorXNA.Engine.Logic.BehaviourModel
{
    /// <summary>
    /// Челобарики
    /// </summary>
    class ObjectPhysicControllerBehaviourModel:ObjectBehaviourModel
    {
      //  private Actor actor;
        private Vector3 lastposition;
        private Vector3 move;

        public ObjectPhysicControllerBehaviourModel(/*Actor _actor*/)
        {
            //a//ctor = _actor;
        }

        public override void SetGlobalPose(Matrix GlobalPoseMatrix, object Additionaldata)
        {
            moved = true;
          this.globalpose = GlobalPoseMatrix;
           /*   actor.GlobalPose = GlobalPoseMatrix;*/
        }

        public override void Move(Vector3 displacement)
        {
            SetGlobalPose(Matrix.Multiply(globalpose, Matrix.CreateTranslation(displacement)), null);
        }

        public override void DoFrame(GameTime gametime)
        {
           /* float elapsed = (float)(gametime.ElapsedGameTime.TotalMilliseconds / 1000.0);
            if (move.LengthSquared() != 0)
            {
                move.Y = 0;
                move.Normalize();
                move.X *= 10000 * elapsed;
                move.Z *= 10000 * elapsed;

                actor.LinearVelocity = new Vector3(move.X, move.Y + actor.LinearVelocity.Y, move.Z);
            }
            else
                actor.LinearVelocity = new Vector3(0, actor.LinearVelocity.Y, 0);

            moved = !actor.LinearVelocity.NewNear(Vector3.Zero, 0.01f);

            lastposition = CurrentPosition.Translation;
            CurrentPosition = actor.GlobalPose;
            move.X = move.Y = move.Z =0;*/
            CurrentPosition = globalpose;
        }
    }
}

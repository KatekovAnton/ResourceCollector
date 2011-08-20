﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ResourceCollectorXNA.Engine.Logic.BehaviourModel
{
    /// <summary>
    /// пули, поварачивающиеся прожекторы и всякая прочая кака которая двигается, но не по законам физики
    /// </summary>
    class ObjectMovingBehaviourModel:ObjectBehaviourModel
    {

        public override void SetGlobalPose(Matrix GlobalPoseMatrix, object Additionaldata)
        {
            this.globalpose = GlobalPoseMatrix;
            moved = true;
        }
        public override void DoFrame(GameTime gametime)
        {
           //двигаем вручную чтоле
        }


        public override void Move(Vector3 displacement)
        {
            moved = true;
        }
        
    }
}

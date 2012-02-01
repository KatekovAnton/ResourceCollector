using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using StillDesign.PhysX;



namespace ResourceCollectorXNA.Engine.Logic
{
    public abstract class PivotObject
    {
        //
        public EditorData editorAspect;
        //mixed
        public RaycastBoundObject raycastaspect;
        public Matrix transform = Matrix.Identity;
        //for one
        public BehaviourModel.ObjectBehaviourModel behaviourmodel;
        public bool neetforceupdate = false;
        protected PivotObject(){ }

        public bool moved;

        public void SetActive(bool active)
        {
            editorAspect.isActive = active;
        }

        public void Move(Microsoft.Xna.Framework.Vector3 d)
        {
            behaviourmodel.Move(d);
            moved = true;
        }

        public void SetGlobalPose(Microsoft.Xna.Framework.Matrix newPose)
        {
            behaviourmodel.SetGlobalPose(newPose, null);
            transform = newPose;
            moved = true;
        }

        public void BeginDoFrame()
        {
            moved = false;
            behaviourmodel.BeginDoFrame();
        }

        public void Update()
        {
            transform = behaviourmodel.globalpose;
            if (behaviourmodel.moved || neetforceupdate)
                raycastaspect.boundingShape.Update(transform);
            neetforceupdate = false;
        }

        public abstract Render.RenderObject HaveRenderAspect();
        public abstract Render.Materials.Material HaveMaterial();
    }
}

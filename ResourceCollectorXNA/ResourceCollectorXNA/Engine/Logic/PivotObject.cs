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

        public bool neetforceupdate = false;
        public PivotObject()
        { }

        public bool moved;
        public abstract void SetActive(bool active);
        public abstract void Move(Vector3 d);
        public abstract void SetGlobalPose(Matrix newPose);
        public abstract Render.RenderObject HaveRenderAspect();
        public abstract Render.Materials.Material HaveMaterial();
        public virtual void Update()
        {
            if (neetforceupdate)
            {
                raycastaspect.boundingShape.Update(transform);
                neetforceupdate = false;
            }
        }
        public virtual void BeginDoFrame()
        {
            moved = false;
        }
    }
}

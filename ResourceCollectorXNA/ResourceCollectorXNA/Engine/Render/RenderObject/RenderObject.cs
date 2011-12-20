using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using ResourceCollectorXNA.Content;
//using StillDesign.PhysX;
namespace ResourceCollectorXNA.Engine.Render
{
    public abstract class RenderObject:IDisposable
    {
        public string PictureTehnique;
        public string ShadowTehnique;
        public bool isanimaated;
        public bool isshadowreceiver;
        public bool isshadowcaster;

        public bool Disposed;

        public bool isTransparent;
        public bool isSelfIllumination;

       // public ResourceCollectorXNA.Engine.Render.Materials.Material mat;

        protected RenderObject() { }

        public virtual void Dispose()
        {
            Disposed = true;
        }

        public abstract void SelfRender(int lod, ResourceCollectorXNA.Engine.Render.Materials.Material mat = null);
    }
}

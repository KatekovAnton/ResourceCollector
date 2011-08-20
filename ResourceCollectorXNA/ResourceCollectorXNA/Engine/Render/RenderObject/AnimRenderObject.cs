using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceCollectorXNA.Engine.Render
{
    class AnimRenderObject : RenderObject
    {
        public AnimRenderObject()
        { }
        public override void SelfRender(int lod, ResourceCollectorXNA.Engine.Render.Materials.Material mate = null)
        {

        }
       
        public override void Dispose()
        {
            if (!Disposed)
            {
                
                Disposed = true;
                GC.SuppressFinalize(this);
            }
        }
    }
}

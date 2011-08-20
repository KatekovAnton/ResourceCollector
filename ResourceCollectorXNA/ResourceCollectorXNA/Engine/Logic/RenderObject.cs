using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using ResourceCollectorXNA.Content;
//using StillDesign.PhysX;
namespace ResourceCollectorXNA.Engine.Logic
{
    public abstract class RenderObject
    {
        
        public string PictureTehnique;
        public string ShadowTehnique;
        public bool isanimaated;
        public bool isshadowreceiver;
        public bool isshadowcaster;

        public bool isactive = false;
        protected RenderObject()
        {
        }
        protected int count;
        public RenderObject Copy()
        {
            count++;
            return this;
        }
        public abstract void SelfRender(int lod, bool applymats = true);


    }
}

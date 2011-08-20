using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace ResourceCollectorXNA.Engine.Render.Materials
{
    public abstract class Material
    {
        public string TehniqueName
        {
            get;
            set;
        }
        private EffectTechnique Technique
        {
            get 
            {
                return ObjectRenderEffect.Techniques[TehniqueName];
            }
            set
            { 
            }
        }

        public static Effect ObjectRenderEffect;



        public abstract void Apply(int lod, int subset);
    }
}

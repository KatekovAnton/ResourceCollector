using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

using ResourceCollectorXNA.Content;
using ResourceCollectorXNA.Engine.Render.Materials;

//using StillDesign.PhysX;

namespace ResourceCollectorXNA.Engine.Logic
{

    public class UnAnimRenderObject : RenderObject
    {
        #region RENDER
        public class SubSet 
        {
            public Mesh mesh;
            public ResourceCollectorXNA.Engine.Render.Materials.Material mat;


            public SubSet(Mesh m, ResourceCollectorXNA.Engine.Render.Materials.Material maa)
            {
                mesh = m;
                mat = maa;
            }
            
            
        }

        public class Model
        {
            public SubSet[] subsets
            {
                get;
                private set;
            }

            public Model(SubSet[] array)
            {
                subsets = new SubSet[array.Length];
                array.CopyTo(subsets, 0);
            }

        }

        public Model[] LODs
        {
            get;
            private set;
        }


        #endregion

        public UnAnimRenderObject(Model[] lods)
        {
            LODs = lods;
        }

       

        public UnAnimRenderObject(
            Model[] lods,
            bool shadowcaster,
            bool shadowreceiver)
        {
            LODs = lods;
            isshadowcaster = shadowcaster;
            isshadowreceiver = shadowreceiver;
        }


        public override void SelfRender(int lod, bool applymats = true)
        {
            if (applymats)
            {
                foreach (SubSet s in LODs[lod].subsets)
                {
                    s.mat.Apply();
                    int tt = 0;
                    foreach (var pass in ResourceCollectorXNA.Engine.Render.Materials.Material.ObjectRenderEffect.CurrentTechnique.Passes)
                    {
                        tt++;
                        pass.Apply();
                        s.mesh.Render();
                    }
                }
            }
            else
            {
                foreach (SubSet s in LODs[lod].subsets)
                {
                    foreach (var pass in ResourceCollectorXNA.Engine.Render.Materials.Material.ObjectRenderEffect.CurrentTechnique.Passes)
                    {
                        pass.Apply();
                        s.mesh.Render();
                    }
                }
            }
        }

    }
}

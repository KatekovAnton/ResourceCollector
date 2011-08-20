using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

using ResourceCollectorXNA.Content;
using ResourceCollectorXNA.Engine.Render.Materials;

//using StillDesign.PhysX;

namespace ResourceCollectorXNA.Engine.Render
{

    public class UnAnimRenderObject : RenderObject
    {
        #region RENDER
        public class SubSet : IDisposable 
        {
            public EngineMesh mesh;
            public bool Disposed = false;
            public SubSet(EngineMesh m, ResourceCollectorXNA.Engine.Render.Materials.Material maa)
            {
                mesh = m;
            }
            public SubSet(EngineMesh m)
            {
                mesh = m;
            }
            public void Dispose()
            {
                if (!Disposed)
                {
                    mesh.Dispose();
                    mesh = null;
                    Disposed = true;
                    GC.SuppressFinalize(this);
                }
            }
            ~SubSet()
            {
                Dispose();
            }
            
        }

        public class Model : IDisposable 
        {
            public SubSet[] subsets;
            public bool Disposed = false;
            public Model(SubSet[] array)
            {
                subsets = new SubSet[array.Length];
                array.CopyTo(subsets, 0);
            }
            public void Dispose()
            {
                if (!Disposed)
                {
                    for(int i =0;i<subsets.Length;i++)
                        subsets[i].Dispose();
                    subsets = null;
                    Disposed = true;
                    GC.SuppressFinalize(this);
                }
            }

            ~Model()
            {

                Dispose();
            }
        }

        public Model[] LODs;

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


        public override void SelfRender(int lod, ResourceCollectorXNA.Engine.Render.Materials.Material mat)
        {
            if (mat!=null)
            {
                for (int i = 0;i<LODs[lod].subsets.Length;i++)
                {
                    mat.Apply(lod, i);
                    int tt = 0;
                    foreach (var pass in ResourceCollectorXNA.Engine.Render.Materials.Material.ObjectRenderEffect.CurrentTechnique.Passes)
                    {
                        tt++;
                        pass.Apply();
                        LODs[lod].subsets[i].mesh.Render();
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

        public override void Dispose()
        {
            if (!Disposed)
            {
                for (int i = 0; i < LODs.Length; i++)
                    LODs[i].Dispose();
                LODs = null;
                Disposed = true;
                GC.SuppressFinalize(this);
            }
        }
    }
}

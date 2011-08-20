using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace ResourceCollectorXNA.Engine.Render.Materials
{
    class TextureMaterial : Material,IDisposable
    {
        public class SubsetMaterial:IDisposable
        {
            public Texture2D diffuseTexture;
            public bool Disposed = false;
            public void Dispose()
            {
                if (!Disposed)
                {
                    Disposed = true;
                    if (diffuseTexture != null && !diffuseTexture.IsDisposed)
                    {
                        diffuseTexture.Dispose();
                        Engine.ContentLoader.ContentLoader.XNAObjectsLoadedCount--;
                        Engine.ContentLoader.ContentLoader.XNAevents.Add("disposed texture");
                    }
                    diffuseTexture = null;
                    GC.SuppressFinalize(this);
                }
            }
            ~SubsetMaterial()
            {
                Dispose();
            }
        }
        public class Lod
        {
            public SubsetMaterial[] mats;
            public Lod(SubsetMaterial[] _mats)
            {
                mats = _mats;
            }
        }
        Lod[] lodmats;
     
        public TextureMaterial(Lod[] _lodmats)
        {
            lodmats = _lodmats;
        }

      
        public override void Apply(int lod, int subset)
        {
            Material.ObjectRenderEffect.Parameters["DiffuseTexture"].SetValue(lodmats[lod].mats[subset].diffuseTexture);
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                for (int i = 0; i < lodmats.Length; i++)
                    for (int j = 0; j < lodmats[i].mats.Length; j++)
                        lodmats[i].mats[j].Dispose();

                // Note disposing has been done.
                Disposed = true;
                GC.SuppressFinalize(this);
            }
        }
        public bool Disposed = false;
        
        ~TextureMaterial()
        {
            Dispose();
        }
    }
}

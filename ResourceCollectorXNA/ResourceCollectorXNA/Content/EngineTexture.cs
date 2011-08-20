using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework.Graphics;


namespace ResourceCollectorXNA.Content
{
    class EngineTexture:IDisposable
    {
        public Texture2D texture;
        public bool Disposed;
        public EngineTexture()
        {
           
        }

        public void Dispose()
        {
            if (!Disposed)
            {
                texture.Dispose();
                Disposed = true;
                GC.SuppressFinalize(this);

                Engine.ContentLoader.ContentLoader.XNAObjectsLoadedCount--;
                Engine.ContentLoader.ContentLoader.XNAevents.Add("disposed texture");
            }
        }
        ~EngineTexture()
        {
            Dispose();
        }

        public void loadbody(byte[] array)
        {
            texture = Texture2D.FromStream(Engine.GameEngine.Device, new System.IO.MemoryStream(array));
            Engine.ContentLoader.ContentLoader.XNAObjectsLoadedCount++;
            Engine.ContentLoader.ContentLoader.XNAevents.Add("created texture");
            
        }
    }
}

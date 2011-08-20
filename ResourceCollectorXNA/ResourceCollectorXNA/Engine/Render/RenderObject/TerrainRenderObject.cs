using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ResourceCollectorXNA.Content;
namespace ResourceCollectorXNA.Engine.Render
{
    public class TerrainRenderObject:RenderObject
    {
        TerrainMesh tm;
        public TerrainRenderObject(TerrainMesh mesh)
        {
            tm = mesh;
        }

        public override void SelfRender(int lod, ResourceCollectorXNA.Engine.Render.Materials.Material mat = null)
        {
            foreach (var pass in ResourceCollectorXNA.Engine.Render.Materials.Material.ObjectRenderEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                tm.Render();
            }
        }
    }
}

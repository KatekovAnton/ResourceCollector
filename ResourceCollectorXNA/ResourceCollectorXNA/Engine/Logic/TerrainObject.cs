using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ResourceCollectorXNA.Engine.Render;

namespace ResourceCollectorXNA.Engine.Logic
{
    public class TerrainObject : PivotObject
    {
        //for group
        public TerrainRenderObject renderaspect;
        
        public TerrainObject(TerrainRenderObject _renderaspect, RaycastBoundObject _raycastaspect, EditorData ed)
        {
            renderaspect = _renderaspect;
            raycastaspect = _raycastaspect;
            editorAspect = ed;
        }
        public override Render.Materials.Material HaveMaterial()
        {
            return null;
        }
        public override RenderObject HaveRenderAspect()
        {
            return renderaspect;
        }
        
        public override void SetActive(bool active)
        {
            editorAspect.isActive = active;
        }
        public override void Move(Microsoft.Xna.Framework.Vector3 d)
        {
            transform = transform * Microsoft.Xna.Framework.Matrix.CreateTranslation(d);
            neetforceupdate = true;
            moved = true;
        }
        public override void SetGlobalPose(Microsoft.Xna.Framework.Matrix newPose)
        {
            transform = newPose;
            neetforceupdate = true;
        }
        public void Render(int lod, ResourceCollectorXNA.Engine.Render.Materials.Material mat)
        {
            ResourceCollectorXNA.Engine.Render.Materials.Material.ObjectRenderEffect.Parameters["World"].SetValue(transform);
            renderaspect.SelfRender(lod, mat);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ResourceCollectorXNA.Engine.Render;

namespace ResourceCollectorXNA.Engine.Logic
{
    public class LevelObject : PivotObject
    {
        public bool deleted = false;
        
        //for group
        public RenderObject renderaspect;
        //for group
        public ResourceCollectorXNA.Engine.Render.Materials.Material material;
        //for one
        public object Character;

        public LevelObject(BehaviourModel.ObjectBehaviourModel _behaviourmodel, RenderObject _renderaspect, ResourceCollectorXNA.Engine.Render.Materials.Material _material, RaycastBoundObject _raycastaspect, EditorData ed)
        {
            behaviourmodel = _behaviourmodel;
            renderaspect = _renderaspect;
            raycastaspect = _raycastaspect;
            editorAspect = ed;
            material = _material;
        }

        public override Render.Materials.Material HaveMaterial()
        {
            return material;
        }

        public override RenderObject HaveRenderAspect()
        {
            return renderaspect;
        }

        ~LevelObject()
        {
            if (!deleted)
            {
                ContentLoader.ContentLoader.UnloadPivotObject(this);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ResourceCollectorXNA.Content;
namespace ResourceCollectorXNA.Engine.Logic
{
    public class RaycastBoundObject
    {
        public bool RCAble
        {
            get;
            set;
        }
        public EngineCollisionMesh RCCM
        {
            get;
            set;
        }
        public SceneGraph.OTBoundingShape boundingShape;
        public float? IntersectionClosestSingle(Ray ray, Matrix transform)
        {
            float? disttobb = boundingShape.aabb.XNAbb.Intersects(ray);
            if(disttobb== null)
                return null;
            disttobb = RCCM.IntersectionClosestSingle(ray, transform);
            return disttobb;
        }
        public Vector3? IntersectionClosest(Ray ray, Matrix transform)
        {
            float? disttobb = boundingShape.aabb.XNAbb.Intersects(ray);
            if (disttobb == null)
                return null;


            Vector3? point = RCCM.IntersectionClosest(ray, transform);
            return point;
        }
        public RaycastBoundObject(SceneGraph.OTBoundingShape bb,EngineCollisionMesh _RCCM)
        {
            boundingShape = bb;
            RCAble = _RCCM != null;
            RCCM = RCAble?_RCCM:bb.cm;
            
        }
   
    }
}

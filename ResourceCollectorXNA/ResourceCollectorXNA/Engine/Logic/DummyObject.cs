using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ResourceCollectorXNA.Engine.Logic
{
    public enum DummyType
    {
        Sphere,
        AABB,
        AlignedPlane
    }
    public class DummyObject : PivotObject
    {
        public DummyObject(DummyType __type, Vector3 __size, Vector3 __aabbSize)
        {
 
        }

        public override Render.Materials.Material HaveMaterial()
        {
            return null;
        }

        public override Render.RenderObject HaveRenderAspect()
        {
            return null;
        }
    }
}

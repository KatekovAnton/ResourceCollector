using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;



namespace ResourceCollectorXNA.Engine.Logic.SceneGraph
{
    public class OTBoundingShape
    {
        public ResourceCollectorXNA.Content.EngineCollisionMesh cm;
        public OTAABoundingBox aabb;
        public BB bb;
        public AABS aabr;

        public OTBoundingShape(Vector3 v)
        {
            aabb = new OTAABoundingBox(v);
            bb = new BB(aabb.XNAbb.Min, aabb.XNAbb.Max);
            aabr = new AABS(aabb);
            Vector3[] points = new Vector3[8];
            points[0] = bb.BasicPoints[0];
            points[1] = bb.BasicPoints[1];
            points[2] = bb.BasicPoints[2];
            points[3] = bb.BasicPoints[3];

            points[4] = bb.BasicPoints[4];
            points[5] = bb.BasicPoints[5];
            points[6] = bb.BasicPoints[6];
            points[7] = bb.BasicPoints[7];
            cm = new Content.EngineCollisionMesh(points, new int[] { 0, 1, 2, 1, 2, 3, 4, 5, 6, 5, 6, 7, 4, 0, 1, 1, 4, 5, 1, 5, 7, 1, 7, 3, 6, 3, 2, 6, 7, 3, 4, 0, 6, 0, 6, 2 });
            
        }

        public OTBoundingShape(ResourceCollectorXNA.Content.EngineCollisionMesh _cm)
        {
            cm = _cm;
            aabb = new OTAABoundingBox();
            bb = new BB(cm);
            aabr = new AABS(aabb);
        }

        public void Update(Matrix newglobalpose)
        {
            bb.Transform(newglobalpose);
            aabr.create(bb.TransformedPoints);
        }
    }
}

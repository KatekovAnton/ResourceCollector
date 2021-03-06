﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace ResourceCollector
{
    class LODMyVertexList : List<LODSkinnedVertex>
    {
        public LODMyVertexList(int count)
            : base(count)
        {

        }

        public int IndexByValue(LODSkinnedVertex value)
        {
            int i = 0;
            foreach (LODSkinnedVertex v in this)
            {
                if (v == value)
                    return i;
                
                i++;
            }
            return -1;
        }

        public int AddUnique(LODSkinnedVertex value)
        {
            int i = 0;
            foreach (LODSkinnedVertex v in this)
            {
                if (v == value)
                    return i;
                
                i++;
            }
            Add(value);
            return Count - 1;
        }

        new public bool Contains(LODSkinnedVertex value)
        {
            return IndexOf(value) != -1;
        }

        public bool Contains(int id)
        {
            foreach (LODSkinnedVertex v in this)
            {
                if (v.id == id)
                    return true;
            }
            return false;
        }
    }

    class LODMyFaceList : List<LODSkinnedFace>
    {
        public int Index(LODSkinnedFace value)
        {
            int i = 0;
            foreach (LODSkinnedFace v in this)
            {
                if (v == value)
                    return i;
                
                i++;
            }
            return -1;
        }

        public int AddUnique(LODSkinnedFace value)
        {
            int i = 0;
            foreach (LODSkinnedFace v in this)
            {
                if (v == value)
                    return i;
                
                i++;
            }
            Add(value);
            return Count - 1;
        }

        new public bool Contains(LODSkinnedFace value)
        {
            return IndexOf(value) != -1;
        }
    }

    struct LODBoneRelation
    {
        public string bonename;
        public float koefficient;

        public override string ToString()
        {
            return bonename.TrimEnd('\0') + " k=\"" + koefficient.ToString() + "\"";
        }
    }

    class LODSkinnedVertex
    {
        public Vector3 pos;					//SV
        public Vector3 normal;				//SV
        public int relationbonescount;		//SV
        public string bone1, bone2, bone3;	//SV
        public float k1, k2, k3;			//SV
        //########################################################################

        //########################################################################
        public Vector2 tc;
        public bool simpletexcoordinates;
        public bool onthecliff;						// cliff
        public int id;									// place of vertex in original list
        public LODMyVertexList neighbor;	// adjacent vertices
        public LODMyFaceList face;			// adjacent triangles


        public bool neighborcontains(int id)
        {
            return neighbor.Contains(id);
        }

        public void RemoveIfNonNeighbor(ref LODSkinnedVertex n)
        {
            if (neighbor.IndexByValue(n) < 0)
                return;
            foreach (LODSkinnedFace f in face)
            {
                if (f.HasVertex(n))
                    return;
            }
            neighbor.Remove(n);
        }

        public int FindNeighbor(int centerid, int lastid)
        {
            int res = -1;
            bool correct = false;
            foreach (LODSkinnedVertex n in neighbor)
            {
                if (n.id == centerid)
                {
                    correct = true;
                    break;
                }
            }
            if (correct)
            {
                foreach (LODSkinnedVertex n in neighbor)
                {
                    if (n.id != centerid && n.id != lastid && n.neighborcontains(centerid))
                    {
                        res = n.id;
                        break;
                    }
                }
            }
            return res;
        }

        public void checktc()
        {
            Vector2 ftc = new Vector2();
            bool set = false;
            bool succesfiirst = true;

            for (int i = 0; i < face.Count; i++)
            {
                if (this == face[i].vertex[0])
                {
                    if (set)
                    {
                        if (!ftc.Near(face[i].t0))
                        {
                            succesfiirst = false;
                            break;
                        }
                    }
                    else
                    {
                        set = true;
                        ftc = new Vector2(face[i].t0.X, face[i].t0.Y);
                    }
                }
                else
                {
                    if (this == face[i].vertex[1])
                    {
                        if (set)
                        {
                            if (!ftc.Near(face[i].t1))
                            {
                                succesfiirst = false;
                                break;
                            }
                        }
                        else
                        {
                            set = true;
                            ftc = new Vector2(face[i].t1.X, face[i].t1.Y);
                        }
                    }
                    else
                    {
                        if (this == face[i].vertex[2])
                        {
                            if (set)
                            {
                                if (!ftc.Near(face[i].t2))
                                {
                                    succesfiirst = false;
                                    break;
                                }
                            }
                            else
                            {
                                set = true;
                                ftc = new Vector2(face[i].t2.X, face[i].t2.Y);
                            }
                        }
                    }
                }
            }
            if (succesfiirst)
                tc = new Vector2(ftc.X, ftc.Y);
            simpletexcoordinates = succesfiirst;
        }

        public LODSkinnedVertex()
        {
            normal = new Vector3(1.0f, 0.0f, 0.0f);
        }

        public LODSkinnedVertex(SkinnedVertex v, int _id)
        {
            pos = v.coordinates;
            id = _id;
            if (v.skin.elementscount != 0)
            {
                bone1 = v.bone1; bone2 = v.bone2; bone3 = v.bone3;
                k1 = v.k1; k2 = v.k2; k3 = v.k3;
            }
            normal = v.normal;

            relationbonescount = v.skin.elementscount;

            //copy data
        }

        public LODSkinnedVertex(ref LODSkinnedVertex v1, ref LODSkinnedVertex v2)
        {
            //  bone1 = bone2 = bone3 = "";
            pos = new Vector3((v1.pos.X + v2.pos.X) / 2, (v1.pos.Y + v2.pos.Y) / 2, (v1.pos.Z + v2.pos.Z) / 2);
            normal = new Vector3((v1.normal.X + v2.normal.X) / 2, (v1.normal.Y + v2.normal.Y) / 2, (v1.normal.Z + v2.normal.Z) / 2);

            List<LODBoneRelation> ourbones = new List<LODBoneRelation>();

            LODBoneRelation tmp;
            tmp.bonename = v1.bone1;
            tmp.koefficient = v1.k1;

            ourbones.Add(tmp);

            if (v1.bone2 != "" && v1.k2 > 0.3f)
            {
                tmp.bonename = v1.bone2;
                tmp.koefficient = v1.k2;
                ourbones.Add(tmp);
            }
            if (v1.bone3 != "" && v1.k3 > 0.3f)
            {
                tmp.bonename = v1.bone3;
                tmp.koefficient = v1.k3;
                ourbones.Add(tmp);
            }

            if (v2.bone1 != "")
            {
                int t = -1;
                for (int i = 0; i < ourbones.Count; i++)
                {
                    if (ourbones[i].bonename == v2.bone1)
                    {
                        t = i;
                        break;
                    }
                }
                if (t != -1)
                {
                    LODBoneRelation b = ourbones[t];
                    b.koefficient += v2.k1;
                    ourbones[t] = b;
                }
                else
                {
                    if (v2.k1 > 0.3)
                    {
                        tmp.bonename = v2.bone1;
                        tmp.koefficient = v2.k1;
                        ourbones.Add(tmp);
                    }
                }
            }
            if (v2.bone2 != "")
            {
                int t = -1;
                for (int i = 0; i < ourbones.Count; i++)
                {
                    if (ourbones[i].bonename == v2.bone2)
                    {
                        t = i;
                        break;
                    }
                }
                if (t != -1)
                {
                    LODBoneRelation b = ourbones[t];
                    b.koefficient += v2.k2;
                    ourbones[t] = b;
                }
                else
                {
                    if (v2.k2 > 0.3)
                    {
                        tmp.bonename = v2.bone2;
                        tmp.koefficient = v2.k2;
                        ourbones.Add(tmp);
                    }
                }
            }
            if (v2.bone3 != "")
            {
                int t = -1;
                for (int i = 0; i < ourbones.Count; i++)
                {
                    if (ourbones[i].bonename == v2.bone3)
                    {
                        t = i;
                        break;
                    }
                }
                if (t != -1)
                {
                    LODBoneRelation b = ourbones[t];
                    b.koefficient += v2.k3;
                    ourbones[t] = b;
                }
                else
                {
                    if (v2.k3 > 0.3)
                    {
                        tmp.bonename = v2.bone3;
                        tmp.koefficient = v2.k3;
                        ourbones.Add(tmp);
                    }
                }
            }

            for (int i = 0; i < ourbones.Count; i++)
            {
                int max = i;
                for (int j = i + 1; j < ourbones.Count; j++)
                {
                    if (ourbones[max].koefficient < ourbones[j].koefficient)
                        max = j;
                }
                LODBoneRelation tmp1 = ourbones[max];
                ourbones[max] = ourbones[i];
                ourbones[i] = tmp1;
            }

            bone1 = ourbones[0].bonename;
            k1 = ourbones[0].koefficient;
            relationbonescount = 1;
            if (ourbones.Count > 1)
            {
                relationbonescount = 2;
                bone2 = ourbones[1].bonename;
                k2 = ourbones[1].koefficient;
                if (ourbones.Count > 2)
                {
                    relationbonescount = 3;
                    bone3 = ourbones[2].bonename;
                    k3 = ourbones[2].koefficient;
                }
            }
            float sum = k1 + (relationbonescount > 1 ? k2 : 0) + (relationbonescount > 2 ? k3 : 0);
            k1 /= sum;
            k2 /= (relationbonescount > 1 ? sum : 1);
            k3 /= (relationbonescount > 2 ? sum : 1);

            foreach (LODSkinnedVertex i in v1.neighbor)
            {
                if (neighbor.IndexOf(i) == -1)
                    neighbor.Add(i);
            }
            foreach (LODSkinnedVertex i in v2.neighbor)
            {
                if (neighbor.IndexOf(i) == -1)
                    neighbor.Add(i);
            }
            foreach (LODSkinnedFace i in v2.face)
            {
                if (face.IndexOf(i) == -1)
                    face.Add(i);
            }
            foreach (LODSkinnedFace i in v1.face)
            {
                if (face.IndexOf(i) == -1)
                    face.Add(i);
            }
        }

        public void CheckCliff(ref LODMyVertexList lodvertices)
        {

            onthecliff = true;
            int CurrentVertex = neighbor[0].id;
            int previousvertex = -1;
            int mind;
            for (int i = 0; i < neighbor.Count; i++)
            {
                mind = CurrentVertex;
                CurrentVertex = lodvertices[CurrentVertex].FindNeighbor(id, previousvertex);
                if (CurrentVertex == -1)
                {
                    onthecliff = true;
                    break;
                }
                else
                {
                    previousvertex = mind;
                    if (CurrentVertex == neighbor[0].id)
                    {
                        onthecliff = false;
                        break;
                    }
                }
            }
        }

        public void CollapseWith(ref LODSkinnedVertex v)
        {
            // bone1 = bone2 = bone3 = -1;

            pos = new Vector3((pos.X + v.pos.X) / 2, (pos.Y + v.pos.Y) / 2, (pos.Z + v.pos.Z) / 2);
            normal = new Vector3((normal.X + v.normal.X) / 2, (normal.Y + v.normal.Y) / 2, (normal.Z + v.normal.Z) / 2);
            LODSkinnedVertex addr = v;
            neighbor.Remove(addr);

            foreach (LODSkinnedVertex i in v.neighbor)
            {
                if (i != this && neighbor.IndexOf(i) == -1)
                    neighbor.Add(i);
            }

            List<LODBoneRelation> ourbones = new List<LODBoneRelation>();
            LODBoneRelation tmp;
            tmp.bonename = bone1;
            tmp.koefficient = k1;

            ourbones.Add(tmp);

            if (bone2 != "" && k2 > 0.3f)
            {
                tmp.bonename = bone2;
                tmp.koefficient = k2;
                ourbones.Add(tmp);
            }
            if (bone3 != "" && k3 > 0.3f)
            {
                tmp.bonename = bone3;
                tmp.koefficient = k3;
                ourbones.Add(tmp);
            }

            if (v.bone1 != "")
            {
                int t = -1;
                for (int i = 0; i < ourbones.Count; i++)
                {
                    if (ourbones[i].bonename == v.bone1)
                    {
                        t = i;
                        break;
                    }
                }
                if (t != -1)
                {
                    LODBoneRelation b = ourbones[t];
                    b.koefficient += v.k1;
                    ourbones[t] = b;
                }
                else
                {
                    if (v.k1 > 0.3)
                    {
                        tmp.bonename = v.bone1;
                        tmp.koefficient = v.k1;
                        ourbones.Add(tmp);
                    }
                }
            }
            if (v.bone2 != "")
            {
                int t = -1;
                for (int i = 0; i < ourbones.Count; i++)
                {
                    if (ourbones[i].bonename == v.bone2)
                    {
                        t = i;
                        break;
                    }
                }
                if (t != -1)
                {
                    LODBoneRelation b = ourbones[t];
                    b.koefficient += v.k2;
                    ourbones[t] = b;
                }
                else
                {
                    if (v.k2 > 0.3)
                    {
                        tmp.bonename = v.bone2;
                        tmp.koefficient = v.k2;
                        ourbones.Add(tmp);
                    }
                }
            }
            if (v.bone3 != "")
            {
                int t = -1;
                for (int i = 0; i < ourbones.Count; i++)
                {
                    if (ourbones[i].bonename == v.bone3)
                    {
                        t = i;
                        break;
                    }
                }
                if (t != -1)
                {
                    LODBoneRelation b = ourbones[t];
                    b.koefficient += v.k3;
                    ourbones[t] = b;
                }
                else
                {
                    if (v.k3 > 0.3)
                    {
                        tmp.bonename = v.bone3;
                        tmp.koefficient = v.k3;
                        ourbones.Add(tmp);
                    }
                }
            }

            for (int i = 0; i < ourbones.Count; i++)
            {
                int max = i;
                for (int j = i + 1; j < ourbones.Count; j++)
                {
                    if (ourbones[max].koefficient < ourbones[j].koefficient)
                        max = j;
                }
                LODBoneRelation tmp1 = ourbones[max];
                ourbones[max] = ourbones[i];
                ourbones[i] = tmp1;
            }

            bone1 = ourbones[0].bonename;
            k1 = ourbones[0].koefficient;
            relationbonescount = 1;
            if (ourbones.Count > 1)
            {
                relationbonescount = 2;
                bone2 = ourbones[1].bonename;
                k2 = ourbones[1].koefficient;
                if (ourbones.Count > 2)
                {
                    relationbonescount = 3;
                    bone3 = ourbones[2].bonename;
                    k3 = ourbones[2].koefficient;
                }
            }
            float sum = k1 + (relationbonescount > 1 ? k2 : 0) + (relationbonescount > 2 ? k3 : 0);
            k1 /= sum;
            k2 /= (relationbonescount > 1 ? sum : 1);
            k3 /= (relationbonescount > 2 ? sum : 1);
        }
    }

    class LODSkinnedFace
    {
        public LODSkinnedVertex[] vertex = new LODSkinnedVertex[3];		//SV
        public Vector2 t0, t1, t2;					//SV
        public int it0, it1, it2;

        //########################################################################

        //########################################################################
        public Vector3 normal;
        public bool HasVertex(LODSkinnedVertex v)
        {
            return (v == vertex[0] || v == vertex[1] || v == vertex[2]);
        }
        public void ComputeNormal()
        {

        }

        public LODSkinnedFace()
        { }
        public LODSkinnedFace(LODSkinnedVertex v0, LODSkinnedVertex v1, LODSkinnedVertex v2, Vector2 _t0, Vector2 _t1, Vector2 _t2)
        {
            vertex[0] = v0;
            vertex[1] = v1;
            vertex[2] = v2;
            ComputeNormal();
            for (int i = 0; i < 3; i++)
            {
                if (vertex[i].face == null)
                    vertex[i].face = new LODMyFaceList();
                vertex[i].face.Add(this);
                if (vertex[i].neighbor == null)
                    vertex[i].neighbor = new LODMyVertexList(0);
                for (int j = 0; j < 3; j++) if (i != j)
                    {
                        vertex[i].neighbor.AddUnique(vertex[j]);
                    }
            }
            t0 = _t0;
            t1 = _t1;
            t2 = _t2;
        }

        //########################################################################

        //########################################################################
        public void ReplaceVertex(ref LODSkinnedVertex vold, ref LODSkinnedVertex vnew)
        {
            if (vold == vertex[0])
            {
                vertex[0] = vnew;
            }
            else
                if (vold == vertex[1])
                {
                    vertex[1] = vnew;
                }
                else
                {
                    vertex[2] = vnew;
                }
            int i;
            vold.face.Remove(this);

            vnew.face.Add(this);
            for (i = 0; i < 3; i++)
            {
                vold.RemoveIfNonNeighbor(ref vertex[i]);
                vertex[i].RemoveIfNonNeighbor(ref vold);
            }

            vertex[0].neighbor.AddUnique(vertex[1]);
            vertex[0].neighbor.AddUnique(vertex[2]);

            vertex[1].neighbor.AddUnique(vertex[0]);
            vertex[1].neighbor.AddUnique(vertex[2]);

            vertex[2].neighbor.AddUnique(vertex[0]);
            vertex[2].neighbor.AddUnique(vertex[1]);

            ComputeNormal();
        }
    }
}

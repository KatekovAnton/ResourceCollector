using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
namespace ResourceCollector
{
    class LODBuilder
    {
        public tridata[] tri;
        public MyVertexList lodvertices;
        public MyFaceList lodtriangles;
        public Mesh BaseMesh;
        public Mesh ProcessedMesh;
        public LODBuilder(Mesh basemesh)
        {
            BaseMesh = new Mesh(basemesh);
            BaseMesh.isstatic = basemesh.isstatic;
            BaseMesh.CheckVir();
            BaseMesh.CollapseSelf();
            GetData();
        }
        public void GetData()
        {
            tri = new tridata[BaseMesh.num_faces];
            for (int i = 0; i < BaseMesh.num_faces; i++)
            {
                tri[i] = new tridata();
                tri[i].v[0] = BaseMesh.faces[i].cv0 - 1;
                tri[i].v[1] = BaseMesh.faces[i].cv1 - 1;
                tri[i].v[2] = BaseMesh.faces[i].cv2 - 1;
            }

            lodvertices = new MyVertexList(BaseMesh.num_verts);

            for (int i = 0; i < BaseMesh.num_verts; i++)
            {
                lodvertices.Add(new LODSkinnedVertex());
            }
            for (int i = 0; i < BaseMesh.num_verts; i++)
            {
                lodvertices[i] = new LODSkinnedVertex(BaseMesh.vertexes[i], i);
            }

            lodtriangles = new MyFaceList();
            for (int i = 0; i < BaseMesh.num_faces; i++)
            {
                if (lodvertices[tri[i].v[0]] != lodvertices[tri[i].v[1]] && lodvertices[tri[i].v[2]] != lodvertices[tri[i].v[1]] && lodvertices[tri[i].v[2]] != lodvertices[tri[i].v[0]])
                {
                    LODSkinnedFace tmp = new LODSkinnedFace(
                                      lodvertices[tri[i].v[0]],
                                      lodvertices[tri[i].v[1]],
                                      lodvertices[tri[i].v[2]],
                                      BaseMesh.tvertexes[BaseMesh.faces[i].tv0 - 1],
                                      BaseMesh.tvertexes[BaseMesh.faces[i].tv1 - 1],
                                      BaseMesh.tvertexes[BaseMesh.faces[i].tv2 - 1]);

                    lodtriangles.Add(tmp);
                }
            }
            for (int i = 0; i < lodvertices.Count; i++)
            {
                lodvertices[i].CheckCliff(ref lodvertices);
                lodvertices[i].checktc();
            }

        }

        public void CollapseMinEdge()
        {
            // search minimum edge length
            int t;
            for (t = 0; t < lodvertices.Count; t++)
                if (lodvertices[t].neighbor.Count > 0)
                    break;

            Vector3 f = new Vector3(
                lodvertices[t].pos.X - lodvertices[t].neighbor[0].pos.X,
                lodvertices[t].pos.Y - lodvertices[t].neighbor[0].pos.Y,
                lodvertices[t].pos.Z - lodvertices[t].neighbor[0].pos.Z);

            float minLength = f.length_squared();
            LODSkinnedVertex v1 = lodvertices[t];
            LODSkinnedVertex v2 = lodvertices[t].neighbor[0];
            for (int i = 0; i < lodvertices.Count; i++)
            {
                for (int j = 0; j < lodvertices[i].neighbor.Count; j++)
                {
                    f = new Vector3(
                        lodvertices[i].pos.X - lodvertices[i].neighbor[j].pos.X,
                        lodvertices[i].pos.Y - lodvertices[i].neighbor[j].pos.Y,
                        lodvertices[i].pos.Z - lodvertices[i].neighbor[j].pos.Z);
                    float curlength = f.length_squared();
                    if (minLength > curlength)
                    {
                        minLength = curlength;
                        v1 = lodvertices[i];
                        v2 = lodvertices[i].neighbor[j];
                    }
                }
            }
            Vector2 ntc;
            if (v1.simpletexcoordinates && v2.simpletexcoordinates)
            {
                ntc = new Vector2((v1.tc.X + v2.tc.X) / 2, (v1.tc.Y + v2.tc.Y) / 2);
                for (int i = 0; i < lodtriangles.Count; i++)
                {
                    LODSkinnedFace face = lodtriangles[i];
                    int angleId = face.vertex[0] == v1 ? 0 : face.vertex[1] == v1 ? 1 : face.vertex[2] == v1 ? 2 : -1;

                    if (angleId == 0)
                    {
                        face.t0 = ntc;
                    }
                    else
                    {
                        if (angleId == 1)
                        {
                            face.t1 = ntc;
                        }
                        else
                        {
                            if (angleId == 2)
                            {
                                face.t2 = ntc;
                            }
                        }
                    }
                }
            }
            else
            {
                if (!v1.simpletexcoordinates)
                {

                }
            }

            // prepare vertices to correct collapsing
            if (v1.onthecliff != v2.onthecliff)
            {
                if (v2.onthecliff)
                {
                    LODSkinnedVertex tmp = v1;
                    v1 = v2;
                    v2 = tmp;
                }
                v2.pos = new Vector3(v1.pos.X,v1.pos.Y,v1.pos.Z);
            }
            // collapsing
            v1.CollapseWith(ref v2);


            // change triangles having erased vertex
            for (int i = 0; i < lodtriangles.Count; i++)
            {
                LODSkinnedFace face = lodtriangles[i];
                int angleId = face.vertex[0] == v2 ? 0 : face.vertex[1] == v2 ? 1 : face.vertex[2] == v2 ? 2 : -1;

                if (angleId != -1)
                {
                    if (face.vertex[0] != v1 && face.vertex[1] != v1 && face.vertex[2] != v1)
                        face.vertex[angleId] = v1;
                    else
                    {
                        lodtriangles.Remove(face);
                        i--;
                    }
                }
            }

            // change v2 neighbors neighbors from v2 to v1
            for (int i = 0; i < v2.neighbor.Count; i++)
            {
                LODSkinnedVertex neighbor = v2.neighbor[i];

                if (!neighbor.neighbor.Contains(v1))
                {
                    for (int j = 0; j < v2.neighbor[i].neighbor.Count; j++)
                    {
                        if (neighbor.neighbor[j] == v2)
                        {
                            neighbor.neighbor[j] = v1;
                            break;
                        }
                    }
                }
                else
                {
                    neighbor.neighbor.Remove(v2);
                }
            }

            for (int i = 0; i < lodvertices.Count; i++)
            {
                lodvertices[i].id = i;
            }

            lodvertices.Remove(v2);
        }

        public void CollapceEdges(int collapce)
        {

            List<int> kkk = new List<int>();
            for (int i = 0; i < collapce; i++)
            {
                int r = lodtriangles.Count;
                CollapseMinEdge();
                r -= lodtriangles.Count;
                kkk.Add(r);
            }
            ProcessedMesh = new Mesh();
            ProcessedMesh.num_verts = lodvertices.Count;
            ProcessedMesh.vertexes = new vertex[ProcessedMesh.num_verts];
            for (int i = 0; i < ProcessedMesh.num_verts; i++)
            {
                ProcessedMesh.vertexes[i] = new vertex();
                ProcessedMesh.vertexes[i].coordinates = new Vector3(lodvertices[i].pos.X, lodvertices[i].pos.Y, lodvertices[i].pos.Z);
                ProcessedMesh.vertexes[i].normal = new Vector3(lodvertices[i].normal.X, lodvertices[i].normal.Y, lodvertices[i].normal.Z);
                ProcessedMesh.vertexes[i].skin = new vertexskin(lodvertices[i].bone1, lodvertices[i].bone2, lodvertices[i].bone3, lodvertices[i].k1, lodvertices[i].k2, lodvertices[i].k3);

                lodvertices[i].id = i;
            }
            ProcessedMesh.vertskins = new vertexskin[ProcessedMesh.num_verts];
            for (int i = 0; i < ProcessedMesh.num_verts; i++)
            {
                ProcessedMesh.vertskins[i] = ProcessedMesh.vertexes[i].skin;
            }
            Vector2[] tmptcoords = new Vector2[lodtriangles.Count * 3];
            int last = 0;
            for (int j = 0; j < lodtriangles.Count; j++)
            {
                int ind = -1;
                for (int i = 0; i < last; i++)
                {
                    if (tmptcoords[i].Near(lodtriangles[j].t0))
                    {
                        ind = i + 1;
                        break;
                    }
                }
                if (ind != -1)
                    lodtriangles[j].it0 = ind;
                else
                {
                    tmptcoords[last] = lodtriangles[j].t0;
                    lodtriangles[j].it0 = last + 1;
                    last++;
                }

                ind = -1;
                for (int i = 0; i < last; i++)
                {
                    if (tmptcoords[i].Near(lodtriangles[j].t1))
                    {
                        ind = i + 1;
                        break;
                    }
                }
                if (ind != -1)
                    lodtriangles[j].it1 = ind;
                else
                {
                    tmptcoords[last] = lodtriangles[j].t1;
                    lodtriangles[j].it1 = last + 1;
                    last++;
                }

                ind = -1;
                for (int i = 0; i < last; i++)
                {
                    if (tmptcoords[i].Near(lodtriangles[j].t2))
                    {
                        ind = i + 1;
                        break;
                    }
                }
                if (ind != -1)
                    lodtriangles[j].it2 = ind;
                else
                {
                    tmptcoords[last] = lodtriangles[j].t2;
                    lodtriangles[j].it2 = last + 1;
                    last++;
                }
            }
            ProcessedMesh.num_tverts = last;
            ProcessedMesh.tvertexes = new Vector2[last];
            for (int i = 0; i < last; i++)
                ProcessedMesh.tvertexes[i] = new Vector2(tmptcoords[i].X,tmptcoords[i].Y);

            ProcessedMesh.num_faces = lodtriangles.Count;
            ProcessedMesh.faces = new face[ProcessedMesh.num_faces];
            for (int i = 0; i < ProcessedMesh.num_faces; i++)
            {
                ProcessedMesh.faces[i] = new face(
                    ProcessedMesh.vertexes[lodtriangles[i].vertex[0].id],
                    ProcessedMesh.vertexes[lodtriangles[i].vertex[1].id],
                    ProcessedMesh.vertexes[lodtriangles[i].vertex[2].id],
                    ProcessedMesh.tvertexes[lodtriangles[i].it0 - 1],
                    ProcessedMesh.tvertexes[lodtriangles[i].it1 - 1],
                    ProcessedMesh.tvertexes[lodtriangles[i].it2 - 1]);
                ProcessedMesh.faces[i].tv0 = lodtriangles[i].it0;
                ProcessedMesh.faces[i].tv1 = lodtriangles[i].it1;
                ProcessedMesh.faces[i].tv2 = lodtriangles[i].it2;
            }
            ProcessedMesh.skinvertexnumber = ProcessedMesh.num_verts;

            ProcessedMesh.name = BaseMesh.name;
            ProcessedMesh.filename = BaseMesh.filename;
            ProcessedMesh.lods = new string[0];
            ProcessedMesh.ismaxdetalized = 0;
            ProcessedMesh.isstatic = BaseMesh.isstatic;
        }

    }
}


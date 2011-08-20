using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
namespace ResourceCollector
{
    public partial class Mesh : PackContent
    {
        public int num_verts;
        public int num_faces;
        public int num_tverts;
        public int bonenumber;
        public int skinvertexnumber;
        public int skinsize;
        public vertex[] vertexes;
        public vertexskin[] vertskins;
        public Vector2[] tvertexes;
        public face[] faces;
        public string filename;
        public int isstatic;
        public int ismaxdetalized = 1;

        public List<tridata> MeshTridata;
        public int havelods = 0;
        public string[] lods;
        public CSkinnedMeshVertex[] BufferVertex;
        public int[] BufferIndex;
        //для ускоренного поиска вырожденных треугольников
        public int Last;
        //##############################################################################
        public Mesh()
        {

        }
        public void mult(float coef)
        {
            if (BufferVertex != null) 
                for (int i = 0; i < BufferVertex.Length; i++)
                {
                    BufferVertex[i].pos *= coef;
                }
        }
        public Mesh(Mesh oldmesh)
        {
            num_verts = oldmesh.num_verts;
            num_faces = oldmesh.num_faces;
            num_tverts = oldmesh.num_tverts;
            bonenumber = oldmesh.bonenumber;
            skinvertexnumber = oldmesh.skinvertexnumber;
            skinsize = oldmesh.skinsize;
            name = oldmesh.name;
            filename = oldmesh.filename;
            offset = oldmesh.offset;
            isstatic = oldmesh.isstatic;
            ismaxdetalized = oldmesh.ismaxdetalized;
            loadedformat = oldmesh.loadedformat;
            forsavingformat = oldmesh.forsavingformat;
            // if (oldmesh.MeshTridata != null)
            // {
            MeshTridata = new List<tridata>(oldmesh.MeshTridata);
            /*}
            else
            {
                MeshTridata = new List<tridata>();
                if (oldmesh.faces != null)
                {
                    for (int i = 0; i < oldmesh.faces.Length; i++)
                    {
                        tridata tmp = new tridata();


                        tmp.v[0] = oldmesh.faces[i].cv0 - 1;
                        tmp.v[1] = oldmesh.faces[i].cv1 - 1;
                        tmp.v[2] = oldmesh.faces[i].cv2 - 1;
                        MeshTridata.Add(tmp);
                    }
                }
                else
                {
                    for (int i = 0; i < oldmesh.BufferIndex.Length; i+=3)
                    {
                        tridata tmp = new tridata();
                        tmp.v[0] = oldmesh.BufferIndex[i];
                        tmp.v[1] = oldmesh.BufferIndex[i + 1];
                        tmp.v[2] = oldmesh.BufferIndex[i + 2];
                        MeshTridata.Add(tmp);
                    }
 
                }
                
            }*/
            havelods = oldmesh.havelods;

            lods = new string[oldmesh.lods.Length];
            oldmesh.lods.CopyTo(lods, 0);
            if (oldmesh.BufferVertex != null)
            {
                BufferVertex = new CSkinnedMeshVertex[oldmesh.BufferVertex.Length];
                for (int i = 0; i < oldmesh.BufferVertex.Length; i++)
                    BufferVertex[i] = new CSkinnedMeshVertex(oldmesh.BufferVertex[i]);
                BufferIndex = new int[oldmesh.BufferIndex.Length];
                oldmesh.BufferIndex.CopyTo(BufferIndex, 0);

            }
            int www = 0;
            for (www = 0; www < oldmesh.vertskins.Length; www++)
            {
                if (oldmesh.vertskins[www].elementscount == 0)
                {
                    oldmesh.isstatic = 1;
                    break;
                }
            }

            if (oldmesh.faces != null)
            {
                vertexes = new vertex[oldmesh.vertexes.Length];
                for (int i = 0; i < oldmesh.vertexes.Length; i++)
                {
                    vertexes[i] = new vertex();
                    vertexes[i].coordinates = new Vector3(oldmesh.vertexes[i].coordinates.X, oldmesh.vertexes[i].coordinates.Y, oldmesh.vertexes[i].coordinates.Z);
                    vertexes[i].normal = new Vector3(oldmesh.vertexes[i].normal.X, oldmesh.vertexes[i].normal.Y, oldmesh.vertexes[i].normal.Z);
                    if (oldmesh.isstatic == 0)
                    {
                        vertexes[i].k1 = oldmesh.vertexes[i].k1;
                        vertexes[i].k2 = oldmesh.vertexes[i].k2;
                        vertexes[i].k3 = oldmesh.vertexes[i].k3;
                        vertexes[i].bone1 = oldmesh.vertexes[i].bone1;
                        vertexes[i].bone2 = oldmesh.vertexes[i].bone2;
                        vertexes[i].bone3 = oldmesh.vertexes[i].bone3;

                        vertexes[i].skin = new vertexskin();
                        vertexes[i].skin.elementscount = oldmesh.vertexes[i].skin.elementscount;
                        vertexes[i].skin.skins = new skinelement[vertexes[i].skin.elementscount];
                        for (int h = 0; h < vertexes[i].skin.elementscount; h++)
                        {
                            vertexes[i].skin.skins[h] = new skinelement();
                            vertexes[i].skin.skins[h].bonename = oldmesh.vertexes[i].skin.skins[h].bonename;
                            vertexes[i].skin.skins[h].coefficient = oldmesh.vertexes[i].skin.skins[h].coefficient;
                        }
                    }
                }

                tvertexes = new Vector2[oldmesh.tvertexes.Length];
                for (int i = 0; i < oldmesh.tvertexes.Length; i++)
                {
                    tvertexes[i] = new Vector2(oldmesh.tvertexes[i].X,oldmesh.tvertexes[i].Y);
                }
                faces = new face[oldmesh.faces.Length];
                for (int i = 0; i < oldmesh.faces.Length; i++)
                {
                    faces[i] = new face(oldmesh.faces[i]);
                }
            }


            Last = oldmesh.Last;
        }
        bool compfloats(float[] frst, float[] scnd)
        {
            if (frst.Length == scnd.Length)
            {
                for (int i = 0; i < frst.Length; i++)
                    if (frst[i] != scnd[i])
                        return false;
            }
            else return false;
            return true;
        }
        bool compstrings(string[] frst, string[] scnd)
        {
            if (frst.Length == scnd.Length)
            {
                for (int i = 0; i < frst.Length; i++)
                    if (frst[i] != scnd[i])
                        return false;
            }
            else return false;
            return true;
        }
        int serchvertex(CSkinnedMeshVertex element, CSkinnedMeshVertex[] buffer)
        {
            for (int i = 0; i < Last; i++)
                if (element.pos.Near(buffer[i].pos) && element.normal.Near(buffer[i].normal) && element.tcoord.Near(buffer[i].tcoord) &&
                    compstrings(element.BoneIDs, buffer[i].BoneIDs) && compfloats(element.BoneWeights, buffer[i].BoneWeights))
                    return i;
            return -1;
        }
        bool compareskins(vertexskin v1, vertexskin v2)
        {
            if (v1.elementscount == v2.elementscount)
            {
                for (int i = 0; i < v1.elementscount; i++)
                {
                    if (v1.skins[i].bonename != v1.skins[i].bonename || Math.Abs(v1.skins[i].coefficient - v2.skins[i].coefficient) < 0.00001f)
                        return false;
                }
            }
            else
                return false;
            return true;
        }
        bool compareverts(vertex v1, vertex v2, bool comskins)
        {
            if (v1.coordinates.Near(v2.coordinates))
            {
                if (v1.normal.Near(v2.normal))
                {
                    if (comskins)
                        return compareskins(v1.skin, v2.skin);
                    else
                        return true;
                }
                else
                    return false;
            }
            else
                return false;

        }
        int insertvertex(ref List<vertex> destverts, vertex source)
        {
            for (int i = 0; i < destverts.Count; i++)
            {
                if (compareverts(destverts[i], source, false))
                    return i;
            }
            destverts.Add(source);
            return destverts.Count - 1;
        }
        public void GenerateOptForStore(ToolStripProgressBar tspb)
        {
            if (BufferIndex == null)
                return;
            float pos = 0.0f; float step1 = 70.0f / Convert.ToSingle(BufferIndex.Length);
            List<vertex> tmpverts = new List<vertex>();
            List<Vector2> tmptexcoords = new List<Vector2>();
            List<vertexskin> tmpvertskins = new List<vertexskin>();
            List<int> inbufferindexes = new List<int>();
            int[] vertindexes = new int[3];
            int[] tcoordindexes = new int[3];
            faces = new face[BufferIndex.Length / 3];
            for (int i = 0; i < BufferIndex.Length; i += 3)
            {
                vertindexes[0] = -1;
                vertindexes[1] = -1;
                vertindexes[2] = -1;

                tcoordindexes[0] = -1;
                tcoordindexes[1] = -1;
                tcoordindexes[2] = -1;

                CSkinnedMeshVertex v0 = BufferVertex[BufferIndex[i]];
                CSkinnedMeshVertex v1 = BufferVertex[BufferIndex[i + 1]];
                CSkinnedMeshVertex v2 = BufferVertex[BufferIndex[i + 2]];

                vertex tmpvert0 = new vertex();
                vertex tmpvert1 = new vertex();
                vertex tmpvert2 = new vertex();

                tmpvert0.coordinates = new Vector3(v0.pos.X, v0.pos.Y, v0.pos.Z);
                tmpvert1.coordinates = new Vector3(v1.pos.X, v1.pos.Y, v1.pos.Z);
                tmpvert2.coordinates = new Vector3(v2.pos.X, v2.pos.Y, v2.pos.Z);
                tmpvert0.normal = new Vector3(v0.normal.X, v0.normal.Y, v0.normal.Z);
                tmpvert1.normal = new Vector3(v1.normal.X, v1.normal.Y, v1.normal.Z);
                tmpvert2.normal = new Vector3(v2.normal.X, v2.normal.Y, v2.normal.Z);
                if (isstatic == 0)
                {
                    tmpvert0.skin = new vertexskin(v0.BoneIDs.Length, v0.BoneIDs, v0.BoneWeights);
                    tmpvert1.skin = new vertexskin(v1.BoneIDs.Length, v1.BoneIDs, v1.BoneWeights);
                    tmpvert2.skin = new vertexskin(v2.BoneIDs.Length, v2.BoneIDs, v2.BoneWeights);
                }
                //##################################################################
                for (int v = 0; v < tmpverts.Count; v++)
                {
                    if (compareverts(tmpverts[v], tmpvert0, false))
                    {
                        vertindexes[0] = v + 1;
                        break;
                    }
                }
                if (vertindexes[0] == -1)
                {
                    tmpverts.Add(tmpvert0);
                    vertindexes[0] = tmpverts.Count;
                }
                //##################################################################

                for (int v = 0; v < tmpverts.Count; v++)
                {
                    if (compareverts(tmpverts[v], tmpvert1, false))
                    {
                        vertindexes[1] = v + 1;
                        break;
                    }
                }
                if (vertindexes[1] == -1)
                {
                    tmpverts.Add(tmpvert1);
                    vertindexes[1] = tmpverts.Count;
                }

                //##################################################################

                for (int v = 0; v < tmpverts.Count; v++)
                {
                    if (compareverts(tmpverts[v], tmpvert2, false))
                    {
                        vertindexes[2] = v + 1;
                        break;
                    }
                }
                if (vertindexes[2] == -1)
                {
                    tmpverts.Add(tmpvert2);
                    vertindexes[2] = tmpverts.Count;
                }

                //##################################################################

                Vector2 tt0 = new Vector2(v0.tcoord.X,v0.tcoord.Y);
                Vector2 tt1 = new Vector2(v1.tcoord.X,v1.tcoord.Y);
                Vector2 tt2 = new Vector2(v2.tcoord.X,v2.tcoord.Y);
                for (int v = 0; v < tmptexcoords.Count; v++)
                {
                    if (tt0.Near(tmptexcoords[v]))
                    {
                        tcoordindexes[0] = v + 1;
                        break;
                    }
                }
                if (tcoordindexes[0] == -1)
                {
                    tmptexcoords.Add(tt0);
                    tcoordindexes[0] = tmptexcoords.Count;
                }

                for (int v = 0; v < tmptexcoords.Count; v++)
                {
                    if (tt1.Near(tmptexcoords[v]))
                    {
                        tcoordindexes[1] = v + 1;
                        break;
                    }
                }
                if (tcoordindexes[1] == -1)
                {
                    tmptexcoords.Add(tt1);
                    tcoordindexes[1] = tmptexcoords.Count;
                }

                for (int v = 0; v < tmptexcoords.Count; v++)
                {
                    if (tt2.Near(tmptexcoords[v]))
                    {
                        tcoordindexes[2] = v + 1;
                        break;
                    }
                }
                if (tcoordindexes[2] == -1)
                {
                    tmptexcoords.Add(tt2);
                    tcoordindexes[2] = tmptexcoords.Count;
                }

                faces[i / 3] = new face();
                faces[i / 3].cv0 = vertindexes[0];
                faces[i / 3].cv1 = vertindexes[1];
                faces[i / 3].cv2 = vertindexes[2];

                faces[i / 3].tv0 = tcoordindexes[0];
                faces[i / 3].tv1 = tcoordindexes[1];
                faces[i / 3].tv2 = tcoordindexes[2];

                faces[i / 3].v0 = tmpverts[vertindexes[0] - 1];
                faces[i / 3].v1 = tmpverts[vertindexes[1] - 1];
                faces[i / 3].v2 = tmpverts[vertindexes[2] - 1];
                pos += step1;
                if (tspb != null)
                    tspb.Value = Convert.ToInt32(pos);

            }

            vertexes = tmpverts.ToArray();
            tvertexes = tmptexcoords.ToArray();
            num_faces = faces.Length;
            num_tverts = tvertexes.Length;
            num_verts = vertexes.Length;
            if (isstatic == 1)
            {

                vertskins = new vertexskin[0];
                bonenumber = 0;
                skinvertexnumber = 0;
                skinsize = 8;
                if (tspb != null)
                    tspb.Value = 100;
            }
            else
            {
                float step2 = 30.0f / Convert.ToSingle(vertexes.Length * 3);
                List<string> tmlist = new List<string>();
                for (int i = 0; i < vertexes.Length; i++)
                {
                    for (int j = 0; j < vertexes[i].skin.elementscount; j++)
                    {
                        if (tmlist.IndexOf(vertexes[i].skin.skins[j].bonename) == -1)
                            tmlist.Add(vertexes[i].skin.skins[j].bonename);
                    }
                    if (tspb != null)
                    {
                        pos += step2;
                        tspb.Value = Convert.ToInt32(pos);
                    }
                }
                bonenumber = tmlist.Count;
                skinvertexnumber = vertexes.Length;
                vertskins = new vertexskin[vertexes.Length];
                for (int i = 0; i < vertskins.Length; i++)
                {
                    vertskins[i] = vertexes[i].skin;
                    if (tspb != null)
                    {
                        pos += step2;
                        tspb.Value = Convert.ToInt32(pos);
                    }
                }
                skinsize = 8;
                for (int i = 0; i < vertskins.Length; i++)
                {
                    skinsize += 4;
                    for (int j = 0; j < vertskins[i].skins.Length; j++)
                    {
                        skinsize += 8;
                        skinsize += vertskins[i].skins[j].bonename.Length;
                    }
                    if (tspb != null)
                    {
                        pos += step2;
                        tspb.Value = Convert.ToInt32(pos);
                    }
                }
            }

            MeshTridata = new List<tridata>(faces.Length);
            for (int i = 0; i < faces.Length; i++)
            {
                tridata tmp = new tridata();
                tmp.v[0] = faces[i].cv0 - 1;
                tmp.v[1] = faces[i].cv1 - 1;
                tmp.v[2] = faces[i].cv2 - 1;
                MeshTridata.Add(tmp);
            }
            if (tspb != null)
                tspb.Value = 100;
            BufferIndex = null;
            BufferVertex = null;
            forsavingformat = ElementType.MeshOptimazedForStore;
        }
        public void GenerateOptForLoading(ToolStripProgressBar tspb)
        {
            if (tvertexes == null)
            {
                return;
            }
            Last = 0;
            int toadd = -1;
            CSkinnedMeshVertex tmpvertex;
            CSkinnedMeshVertex[] tempstruct = new CSkinnedMeshVertex[num_faces * 3];
            List<int> tempIndexes = new List<int>();

            string[] boneindexes;
            float[] tempweigths;

            float pos = 0.0f;
            float step = 100.0f / Convert.ToSingle(num_faces);

            for (int i = 0; i < num_faces; i++)
            {
                boneindexes = new string[faces[i].v0.skin.elementscount];
                tempweigths = new float[faces[i].v0.skin.elementscount];
                for (int j = 0; j < faces[i].v0.skin.elementscount; j++)
                {
                    boneindexes[j] = faces[i].v0.skin.skins[j].bonename;
                    tempweigths[j] = faces[i].v0.skin.skins[j].coefficient;
                }

                tmpvertex = new CSkinnedMeshVertex(
                    faces[i].v0.coordinates,
                    boneindexes,
                    tempweigths,
                    faces[i].v0.normal,
                    new Vector3(), new Vector3(),
                    tvertexes[faces[i].tv0 - 1]);

                toadd = serchvertex(tmpvertex, tempstruct);
                if (toadd == -1)
                {
                    tempIndexes.Add(Last);
                    tempstruct[Last] = new CSkinnedMeshVertex(tmpvertex);
                    Last++;
                }
                else
                    tempIndexes.Add(toadd);



                //############################################################################################
                boneindexes = new string[faces[i].v1.skin.elementscount];
                tempweigths = new float[faces[i].v1.skin.elementscount];
                for (int j = 0; j < faces[i].v1.skin.elementscount; j++)
                {
                    boneindexes[j] = faces[i].v1.skin.skins[j].bonename;
                    tempweigths[j] = faces[i].v1.skin.skins[j].coefficient;
                }
                
                tmpvertex = new CSkinnedMeshVertex(
                    faces[i].v1.coordinates,
                    boneindexes,
                    tempweigths,
                    faces[i].v1.normal,
                    new Vector3(), new Vector3(),
                    tvertexes[faces[i].tv1 - 1]);

                toadd = serchvertex(tmpvertex, tempstruct);
                if (toadd == -1)
                {
                    tempIndexes.Add(Last);
                    tempstruct[Last] = new CSkinnedMeshVertex(tmpvertex);
                    Last++;
                }
                else
                    tempIndexes.Add(toadd);

                //############################################################################################
                boneindexes = new string[faces[i].v2.skin.elementscount];
                tempweigths = new float[faces[i].v2.skin.elementscount];
                for (int j = 0; j < faces[i].v2.skin.elementscount; j++)
                {
                    boneindexes[j] = faces[i].v2.skin.skins[j].bonename;
                    tempweigths[j] = faces[i].v2.skin.skins[j].coefficient;
                }

                tmpvertex = new CSkinnedMeshVertex(
                    faces[i].v2.coordinates,
                    boneindexes,
                    tempweigths,
                    faces[i].v2.normal,
                    new Vector3(), new Vector3(),
                    tvertexes[faces[i].tv2 - 1]);

                toadd = serchvertex(tmpvertex, tempstruct);
                if (toadd == -1)
                {
                    tempIndexes.Add(Last);
                    tempstruct[Last] = new CSkinnedMeshVertex(tmpvertex);
                    Last++;
                }
                else
                    tempIndexes.Add(toadd);
                pos += step;
                if(tspb!=null)
                    tspb.Value = Convert.ToInt32(pos);
            }
            BufferVertex = new CSkinnedMeshVertex[Last];
            for (int i = 0; i < Last; i++)
            {
                BufferVertex[i] = new CSkinnedMeshVertex(tempstruct[i]);
            }
            BufferIndex = tempIndexes.ToArray();
            vertexes = null;
            vertskins = null;
            tvertexes = null;
            forsavingformat = ElementType.MeshOptimazedForLoading;
        }
        void removetridatafronmmeshtridata(tridata td)
        {
            int i;
            for (i = 0; i < MeshTridata.Count; i++)
            {
                if (MeshTridata[i].v[0] == td.v[0] && MeshTridata[i].v[1] == td.v[1] && MeshTridata[i].v[2] == td.v[2])
                    break;
            }
        }
        public void CheckVir()
        {
            if (faces == null)
            {
                ToolStripProgressBar tspb = new ToolStripProgressBar();
                GenerateOptForStore(tspb);
                forsavingformat = ElementType.MeshOptimazedForStore;
            }
            face[] Snewfaces = new face[num_faces];
            int c = 0;

            int index = 0;
            int min = 0;
            for (int t = 0; t < faces.Length; t++)
            {
                if (faces[t].v0.coordinates.Near(faces[t].v1.coordinates) || faces[t].v2.coordinates.Near(faces[t].v1.coordinates) || faces[t].v0.coordinates.Near(faces[t].v2.coordinates))
                {
                    tridata td = MeshTridata[t - min];
                    MeshTridata.Remove(td);
                    min++;
                }
                else
                {
                    Snewfaces[index] = faces[t];
                    c++; index++;
                }
            }

            face[] newfaces = new face[c];
            num_faces = c;

            for (int i = 0; i < c; i++)
                newfaces[i] = Snewfaces[i];

            faces = newfaces;

        }
        struct infopair
        {
            public int index;
            public int number;
        };
        public void Collapse2Vets(int ind1, int ind2)
        {
            bool lastisind = ind2 == num_verts - 1;

            List<infopair> triangs2 = new List<infopair>();
            List<infopair> triangsLAST = new List<infopair>();
            infopair temppair = new infopair();
            int lastvert = num_verts - 1;

            for (int t = 0; t < num_faces; t++)
            {
                if (faces[t].v0.coordinates.Near(vertexes[ind2].coordinates))
                {
                    temppair.index = t; temppair.number = 0; triangs2.Add(temppair);
                }
                else
                {
                    if (faces[t].v1.coordinates.Near(vertexes[ind2].coordinates))
                    {
                        temppair.index = t; temppair.number = 1; triangs2.Add(temppair);
                    }
                    else
                    {
                        if (faces[t].v2.coordinates.Near(vertexes[ind2].coordinates))
                        {
                            temppair.index = t; temppair.number = 2; triangs2.Add(temppair);
                        }
                    }
                }
            }
            if (!lastisind)
            {
                for (int t = 0; t < num_faces; t++)
                {
                    if (faces[t].v0.coordinates.Near(vertexes[lastvert].coordinates))
                    {
                        temppair.index = t; temppair.number = 0; triangsLAST.Add(temppair);
                    }
                    else
                    {
                        if (faces[t].v1.coordinates.Near(vertexes[lastvert].coordinates))
                        {
                            temppair.index = t; temppair.number = 1; triangsLAST.Add(temppair);
                        }
                        else
                        {
                            if (faces[t].v2.coordinates.Near(vertexes[lastvert].coordinates))
                            {
                                temppair.index = t; temppair.number = 2; triangsLAST.Add(temppair);
                            }
                        }
                    }
                }
            }

            vertex tmpvertex = new vertex(vertexes[ind1], vertexes[ind2]);
            vertexes[ind1] = tmpvertex;
            vertexes[ind2] = vertexes[num_verts - 1];
            num_verts--;
            for (int i = 0; i < triangs2.Count; i++)
            {
                switch (triangs2[i].number)
                {
                    case 0:
                        faces[triangs2[i].index].v0 = vertexes[ind1];
                        MeshTridata[triangs2[i].index].v[0] = ind1;
                        break;
                    case 1:
                        faces[triangs2[i].index].v1 = vertexes[ind1];
                        MeshTridata[triangs2[i].index].v[1] = ind1;
                        break;
                    case 2:
                        faces[triangs2[i].index].v2 = vertexes[ind1];
                        MeshTridata[triangs2[i].index].v[2] = ind1;
                        break;
                    default: break;
                }
            }
            if (!lastisind)
            {
                for (int i = 0; i < triangsLAST.Count; i++)
                {
                    switch (triangsLAST[i].number)
                    {
                        case 0:
                            faces[triangsLAST[i].index].v0 = vertexes[ind2];
                            MeshTridata[triangsLAST[i].index].v[0] = ind2;
                            break;
                        case 1:
                            faces[triangsLAST[i].index].v1 = vertexes[ind2];
                            MeshTridata[triangsLAST[i].index].v[1] = ind2;
                            break;
                        case 2:
                            faces[triangsLAST[i].index].v2 = vertexes[ind2];
                            MeshTridata[triangsLAST[i].index].v[2] = ind2;
                            break;
                        default: break;
                    }
                }
            }
        }
        struct Mypair
        {
            public int frst, scnd;
            public override string ToString()
            {
                return "frst = " + frst.ToString() + "; scnd = " + scnd.ToString();
            }
        }
        public void CollapseSelf()
        {

            bool find = true;
            int fff = num_faces;
            Mypair tmppair = new Mypair();
            int upcircle = 0;
            while (find)
            {
                find = false;

                for (int i = upcircle; i < num_verts; i++)
                {
                    for (int j = i + 1; j < num_verts; j++)
                    {
                        if (vertexes[i].coordinates.Near(vertexes[j].coordinates))
                        {
                            tmppair.frst = i;
                            tmppair.scnd = j;

                            find = true;
                            break;
                        }
                    }
                    if (find)
                    {
                        upcircle = i;
                        break;
                    }
                }
                if (find)
                {
                    Collapse2Vets(tmppair.frst, tmppair.scnd);
                }
            }



            vertex[] tmstoraje = new vertex[num_verts];
            for (int i = 0; i < num_verts; i++)
            {
                tmstoraje[i] = vertexes[i];
            }


            vertexes = tmstoraje;
            for (int i = 0; i < num_faces; i++)
            {
                faces[i].v0 = vertexes[MeshTridata[i].v[0]];
                faces[i].v1 = vertexes[MeshTridata[i].v[1]];
                faces[i].v2 = vertexes[MeshTridata[i].v[2]];
                faces[i].cv0 = MeshTridata[i].v[0] + 1;
                faces[i].cv1 = MeshTridata[i].v[1] + 1;
                faces[i].cv2 = MeshTridata[i].v[2] + 1;
            }
        }
        //##############################################################################
        public override int loadbody(System.IO.BinaryReader br, ToolStripProgressBar toolStripProgressBar)
        {
            size = 0;
            switch (loadedformat)
            {
                case ElementType.MeshOptimazedForStore:
                    {
                        long frst = br.BaseStream.Position;

                        num_verts = br.ReadInt32();
                        num_faces = br.ReadInt32();
                        num_tverts = br.ReadInt32();
                        float step = Convert.ToSingle(toolStripProgressBar.Maximum) / Convert.ToSingle(num_verts * 2 + num_tverts + num_faces * 2);
                        float pos = 0;
                        size += 12;
                        vertexes = new vertex[num_verts];

                        for (int i = 0; i < num_verts; i++)
                        {
                            vertexes[i] = new vertex();
                            vertexes[i].coordinates.X = br.ReadSingle(); size += 4;
                            vertexes[i].coordinates.Y = br.ReadSingle(); size += 4;
                            vertexes[i].coordinates.Z = br.ReadSingle(); size += 4;
                            vertexes[i].normal.X = br.ReadSingle(); size += 4;
                            vertexes[i].normal.Y = br.ReadSingle(); size += 4;
                            vertexes[i].normal.Z = br.ReadSingle(); size += 4;
                            pos += step;
                            toolStripProgressBar.Value = Convert.ToInt32(pos);

                        }

                        tvertexes = new Vector2[num_tverts];
                        for (int i = 0; i < num_tverts; i++)
                        {
                            size += 4; size += 4;
                            tvertexes[i] = new Vector2(br.ReadSingle(), br.ReadSingle());
                            pos += step;
                            toolStripProgressBar.Value = Convert.ToInt32(pos);
                        }
                        faces = new face[num_faces];
                        for (int i = 0; i < num_faces; i++)
                        {
                            faces[i] = new face();
                            faces[i].cv0 = br.ReadInt32();
                            faces[i].cv1 = br.ReadInt32();
                            faces[i].cv2 = br.ReadInt32();

                            faces[i].v0 = vertexes[faces[i].cv0 - 1]; size += 4;
                            faces[i].v1 = vertexes[faces[i].cv1 - 1]; size += 4;
                            faces[i].v2 = vertexes[faces[i].cv2 - 1]; size += 4;

                            faces[i].tv0 = br.ReadInt32();
                            faces[i].tv1 = br.ReadInt32();
                            faces[i].tv2 = br.ReadInt32();

                            faces[i].v0.textureccordinates = tvertexes[faces[i].tv0 - 1]; size += 4;
                            faces[i].v1.textureccordinates = tvertexes[faces[i].tv1 - 1]; size += 4;
                            faces[i].v2.textureccordinates = tvertexes[faces[i].tv2 - 1]; size += 4;
                            pos += step;
                            toolStripProgressBar.Value = Convert.ToInt32(pos);
                        }

                        skinvertexnumber = br.ReadInt32(); size += 4;
                        vertskins = new vertexskin[skinvertexnumber];
                        bonenumber = br.ReadInt32(); size += 4;

                        for (int i = 0; i < skinvertexnumber; i++)
                        {
                            vertskins[i] = new vertexskin(br);
                            vertexes[i].skin = vertskins[i];
                            /*pos += step;
                            toolStripProgressBar.Value = Convert.ToInt32(pos);*/
                        }

                        long last = br.BaseStream.Position;
                        long length = last - frst;
                        size = (int)length;
                        MeshTridata = new List<tridata>(faces.Length);
                        for (int i = 0; i < faces.Length; i++)
                        {
                            tridata tmp = new tridata();
                            tmp.v[0] = faces[i].cv0 - 1;
                            tmp.v[1] = faces[i].cv1 - 1;
                            tmp.v[2] = faces[i].cv2 - 1;
                            MeshTridata.Add(tmp);
                            pos += step;
                            toolStripProgressBar.Value = Convert.ToInt32(pos);
                        }

                    } break;
                case ElementType.MeshOptimazedForLoading:
                    {
                        long frst = br.BaseStream.Position;
                        //offset = (int)frst - headersize;
                        int fffff = br.ReadInt32();
                        BufferVertex = new CSkinnedMeshVertex[fffff];
                        BufferIndex = new int[br.ReadInt32()];
                        toolStripProgressBar.Value = 0;
                        float step = Convert.ToSingle(toolStripProgressBar.Maximum) / Convert.ToSingle(BufferVertex.Length + BufferIndex.Length);
                        float pos = 0;
                        size += 8;
                        for (int bv = 0; bv < BufferVertex.Length; bv++)
                        {
                            BufferVertex[bv] = new CSkinnedMeshVertex();
                            BufferVertex[bv].pos = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                            BufferVertex[bv].normal = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
                            BufferVertex[bv].tcoord = new Vector2(br.ReadSingle(), br.ReadSingle());
                            BufferVertex[bv].BoneIDs = new string[br.ReadInt32()];
                            size += 36;

                            for (int i = 0; i < BufferVertex[bv].BoneIDs.Length; i++)
                            {
                                BufferVertex[bv].BoneIDs[i] = br.ReadPackString();
                                size += BufferVertex[bv].BoneIDs[i].Length + 5;
                            }
                            int d = br.ReadInt32();
                            BufferVertex[bv].BoneWeights = new float[d];
                            size += 4;
                            for (int i = 0; i < BufferVertex[bv].BoneWeights.Length; i++)
                            {
                                BufferVertex[bv].BoneWeights[i] = br.ReadSingle();
                                size += 4;
                            }
                            pos += step;
                            toolStripProgressBar.Value = Convert.ToInt32(pos);
                        }

                        int y = 0;

                        for (int bv = 0; bv < BufferIndex.Length; bv++)
                        {
                            BufferIndex[bv] = br.ReadInt32();
                            size += 4;
                            pos += step;
                            toolStripProgressBar.Value = Convert.ToInt32(pos);
                        }

                        long last = br.BaseStream.Position;
                        long length = last - frst;
                        size = (int)length;

                    } break;
                default: break;
            }
            return size;
        }
        public override void saveheader(System.IO.BinaryWriter bw)
        { 
           bw.WritePackString(name);
           // bw.Write3DMaxString(name.Substring(0, name.Length - 1));
            bw.Write(offset);
            bw.Write(forsavingformat);
           // calcheadersize();

            bw.Write(headersize);

            switch (forsavingformat)
            {
                case ElementType.MeshOptimazedForStore:
                    {
                        bw.Write(ismaxdetalized);
                        bw.Write(skinsize);
                        bw.Write(lods.Length);
                        for (int j = 0; j < lods.Length; j++)
                        {
                            bw.WritePackString(lods[j]);
                           // bw.Write3DMaxString(lods[j].TrimEnd('\0'));
                        }
                    } break;
                case ElementType.MeshOptimazedForLoading:
                    {
                        bw.Write(ismaxdetalized);
                        bw.Write(isstatic);
                        bw.Write(lods.Length);
                        for (int j = 0; j < lods.Length; j++)
                        { 
                            bw.WritePackString(lods[j]);
                           // bw.Write3DMaxString(lods[j].TrimEnd('\0'));
                        }
                    } break;
                default:
                    {
                        throw new Exception("WRONG FORMAT!!!!!!");
                    }
            }


        }
        public override void savebody(System.IO.BinaryWriter bw, ToolStripProgressBar toolStripProgressBar)
        {
            toolStripProgressBar.Value = 0;

            switch (forsavingformat)
            {
                case ElementType.MeshOptimazedForStore:
                    {
                        float posit1 = 0.0f;
                        float lalala2 = Convert.ToSingle(toolStripProgressBar.Maximum) / Convert.ToSingle(num_verts + num_faces + num_tverts + vertskins.Length);
                        bw.Write(num_verts);
                        bw.Write(num_faces);
                        bw.Write(num_tverts);
                        for (int i = 0; i < num_verts; i++)
                        {
                            bw.Write(vertexes[i].coordinates.X);
                            bw.Write(vertexes[i].coordinates.Y);
                            bw.Write(vertexes[i].coordinates.Z);

                            bw.Write(vertexes[i].normal.X);
                            bw.Write(vertexes[i].normal.Y);
                            bw.Write(vertexes[i].normal.Z);
                            posit1 += lalala2;
                            toolStripProgressBar.Value = Convert.ToInt32(posit1);
                        }
                        for (int i = 0; i < num_tverts; i++)
                        {
                            bw.Write(tvertexes[i].X);
                            bw.Write(tvertexes[i].Y);
                            posit1 += lalala2;
                            toolStripProgressBar.Value = Convert.ToInt32(posit1);
                        }
                        for (int i = 0; i < num_faces; i++)
                        {
                            bw.Write(faces[i].cv0);
                            bw.Write(faces[i].cv1);
                            bw.Write(faces[i].cv2);

                            bw.Write(faces[i].tv0);
                            bw.Write(faces[i].tv1);
                            bw.Write(faces[i].tv2);
                            posit1 += lalala2;
                            toolStripProgressBar.Value = Convert.ToInt32(posit1);
                        }
                        bw.Write(skinvertexnumber);
                        bw.Write(bonenumber);
                        for (int i = 0; i < vertskins.Length; i++)
                        {
                            bw.Write(vertskins[i].elementscount);
                            for (int j = 0; j < vertskins[i].skins.Length; j++)
                            {
                                bw.WritePackString(vertskins[i].skins[j].bonename);
                               // bw.Write3DMaxString(vertskins[i].skins[j].bonename.Substring(0, vertskins[i].skins[j].bonename.Length - 1));
                                bw.Write(vertskins[i].skins[j].coefficient);
                            }
                            posit1 += lalala2;
                            toolStripProgressBar.Value = Convert.ToInt32(posit1);
                        }
                        toolStripProgressBar.Value = toolStripProgressBar.Maximum;

                    } break;
                case ElementType.MeshOptimazedForLoading:
                    {
                        if (BufferIndex == null)
                            GenerateOptForLoading(null);
                        float posit1 = 0.0f;
                        float lalala2 = Convert.ToSingle(toolStripProgressBar.Maximum) / Convert.ToSingle(BufferVertex.Length + BufferIndex.Length);
                        bw.Write(BufferVertex.Length);
                        bw.Write(BufferIndex.Length);

                        for (int bv = 0; bv < BufferVertex.Length; bv++)
                        {

                            bw.Write(BufferVertex[bv].pos.X);
                            bw.Write(BufferVertex[bv].pos.Y);
                            bw.Write(BufferVertex[bv].pos.Z);
                            bw.Write(BufferVertex[bv].normal.X);
                            bw.Write(BufferVertex[bv].normal.Y);
                            bw.Write(BufferVertex[bv].normal.Z);
                            bw.Write(BufferVertex[bv].tcoord.X);
                            bw.Write(BufferVertex[bv].tcoord.Y);
                            if (isstatic == 0)
                            {
                                bw.Write(BufferVertex[bv].BoneIDs.Length);
                                for (int i = 0; i < BufferVertex[bv].BoneIDs.Length; i++)
                                {
                                    bw.WritePackString(BufferVertex[bv].BoneIDs[i]);
                                   // bw.Write3DMaxString(BufferVertex[bv].BoneIDs[i]);
                                }


                                bw.Write(BufferVertex[bv].BoneWeights.Length);
                                for (int i = 0; i < BufferVertex[bv].BoneWeights.Length; i++)
                                {
                                    bw.Write(BufferVertex[bv].BoneWeights[i]);
                                }
                            }
                            else
                            {
                                bw.Write(0);
                                bw.Write(0);
                            }

                            posit1 += lalala2;
                            toolStripProgressBar.Value = Convert.ToInt32(posit1);
                        }
                        for (int bv = 0; bv < BufferIndex.Length; bv++)
                        {
                            bw.Write(BufferIndex[bv]);
                            posit1 += lalala2;
                            toolStripProgressBar.Value = Convert.ToInt32(posit1);
                        }
                        toolStripProgressBar.Value = toolStripProgressBar.Maximum;
                    } break;
                default:
                    {
                        throw new Exception("WRONG FORMAT!!!!!!");
                    }
            }

        }

        public override System.Windows.Forms.DialogResult createpropertieswindow(Pack p, System.Windows.Forms.TreeView outputtreeview)
        {
            MeshProperties mp = new MeshProperties(this);
            mp.ShowDialog();
            if (mp.lods != null)
            {
                for (int i = 0; i < mp.lods.Length; i++)
                {
                    p.Attach(mp.lods[i], outputtreeview);
                }
            }
            return DialogResult.OK;

        }
        public override void calcheadersize()
        { 
           // headersize = 16 + name.Get3DMaxLength();
            headersize = 16 + name.Length;
            switch (forsavingformat)
            {
                case ElementType.MeshOptimazedForStore:
                    {
                        headersize += 8;
                        for (int j = 0; j < lods.Length; j++)
                        {
                            headersize += lods[j].Length;
                            //headersize += lods[j].Get3DMaxLength();
                        }
                    } break;
                case ElementType.MeshOptimazedForLoading:
                    {
                        headersize += 8;
                        for (int j = 0; j < lods.Length; j++)
                        {
                            headersize += lods[j].Length;
                            //headersize += lods[j].Get3DMaxLength();
                        }
                    } break;
            }
        }
        public override void calcbodysize(ToolStripProgressBar targetbar)
        {
            if(targetbar!=null)
                targetbar.Value = 0;
            if (forsavingformat != loadedformat)
            {

                //вывести нужную структуру данных
                switch (forsavingformat)
                {
                    case ElementType.MeshOptimazedForStore:
                        {
                            GenerateOptForStore(targetbar);
                        } break;
                    case ElementType.MeshOptimazedForLoading:
                        {
                            GenerateOptForLoading(targetbar);
                        } break;
                    default:
                        {
                            throw new Exception("WRONG FORMAT!!!!!");
                        }
                }

                switch (forsavingformat)
                {
                    case ElementType.MeshOptimazedForStore:
                        {
                            size = 0;
                            size += 12;
                            size += 24 * num_verts;
                            size += 8 * num_tverts;
                            size += 24 * num_faces;
                            size += 8;
                            for (int j = 0; j < skinvertexnumber; j++)
                            {
                                size += 4;
                                for (int k = 0; k < vertskins[j].elementscount; k++)
                                {
                                    size += 8;
                                    size += vertskins[j].skins[k].bonename.Length;
                                }
                            }
                        } break;
                    case ElementType.MeshOptimazedForLoading:
                        {
                            size = 0;
                            size += 4;
                            size += 4;

                            for (int bv = 0; bv < BufferVertex.Length; bv++)
                            {
                                size += 36;
                                for (int t = 0; t < BufferVertex[bv].BoneIDs.Length; t++)
                                {
                                    size += 4;
                                    size += BufferVertex[bv].BoneIDs[t].Length;
                                }
                                size += 4;
                                size += 4 * BufferVertex[bv].BoneWeights.Length;
                            }
                            size += BufferIndex.Length * 4;

                        } break;
                    default:
                        {
                            throw new Exception("WRONG FORMAT!!!!!");
                        }
                }
            }

            if(targetbar!=null)
                targetbar.Value = targetbar.Maximum;
        }
        public override void loadobjectheader(HeaderInfo hi, System.IO.BinaryReader br)
        {
            loadedformat = hi.loadedformat;
            name = hi.name;
            forsavingformat = hi.forsavingformat;
            offset = hi.offset;
            size = hi.size;
            headersize = hi.headersize;

            switch (hi.loadedformat)
            {
                case ElementType.MeshOptimazedForStore:
                    {
                        ismaxdetalized = br.ReadInt32();
                        skinsize = br.ReadInt32();
                        lods = new string[br.ReadInt32()];
                        int length;
                        for (int j = 0; j < lods.Length; j++)
                        {
                            length = br.ReadInt32();
                            lods[j] = new string(br.ReadChars(length + 1));
                        }
                    } break;
                case ElementType.MeshOptimazedForLoading:
                    {
                        ismaxdetalized = br.ReadInt32();
                        isstatic = br.ReadInt32();
                        lods = new string[br.ReadInt32()];
                        int length;
                        for (int j = 0; j < lods.Length; j++)
                        {
                            length = br.ReadInt32();
                            lods[j] = new string(br.ReadChars(length + 1));
                        }
                    } break;
                default:
                    {
                        br.BaseStream.Seek(hi.headersize - 16 - hi.name.Length, System.IO.SeekOrigin.Current);
                    } break;
            }
        }
        public override void ViewBasicInfo(
           ComboBox comboBox1, ComboBox comboBox2, Label label1, Label label2, Label label3,
           Label label4, GroupBox groupBox1, TextBox tb, Button button2, Button button1)
        {
            switch (loadedformat)
            {
                case ElementType.MeshOptimazedForStore:
                    {
                        button1.Enabled = true;
                        comboBox1.Items.Clear();
                        comboBox2.Items.Clear();
                        comboBox1.Enabled = false;
                        comboBox2.Enabled = true;
                        tb.Enabled = true;
                        button2.Enabled = true;
                        label1.Text = number.ToString();
                        label2.Text = offset.ToString();
                        label3.Text = headersize.ToString();
                        label4.Text = size.ToString();
                        groupBox1.Text = tb.Text = name;
                        comboBox1.Text = ElementType.ReturnString(ElementType.MeshOptimazedForStore);
                        comboBox2.Items.Add(ElementType.ReturnString(ElementType.MeshOptimazedForStore));
                        comboBox2.Items.Add(ElementType.ReturnString(ElementType.MeshOptimazedForLoading));
                        comboBox2.SelectedIndex = forsavingformat == ElementType.MeshOptimazedForLoading ? 1 : 0;
                    } break;
                case ElementType.MeshOptimazedForLoading:
                    {
                        button1.Enabled = true;
                        comboBox1.Items.Clear();
                        comboBox2.Items.Clear();
                        comboBox1.Enabled = false;
                        comboBox2.Enabled = true;
                        tb.Enabled = true;
                        button2.Enabled = true;
                        label1.Text = number.ToString();
                        label2.Text = offset.ToString();
                        label3.Text = headersize.ToString();
                        label4.Text = size.ToString();
                        groupBox1.Text = tb.Text = name;
                        comboBox1.Text = ElementType.ReturnString(ElementType.MeshOptimazedForLoading);
                        comboBox2.Items.Add(ElementType.ReturnString(ElementType.MeshOptimazedForStore));
                        comboBox2.Items.Add(ElementType.ReturnString(ElementType.MeshOptimazedForLoading));
                        comboBox2.SelectedIndex = forsavingformat == ElementType.MeshOptimazedForLoading ? 1 : 0;
                    } break;
                default:
                    {
                        comboBox1.Items.Clear();
                        comboBox2.Items.Clear();
                        comboBox1.Text = "";
                        comboBox2.Text = "";
                        comboBox1.Enabled = false;
                        comboBox2.Enabled = false;
                        button2.Enabled = false;
                        groupBox1.Text = "";
                        label1.Text = "";
                        label2.Text = "";
                        label3.Text = "";
                        label4.Text = "";
                        tb.Text = "";
                    } break;
            }
        }

        public static Mesh FromBuffers(Mesh[] meshes)
        {
            Mesh newmesh = new Mesh();
            CSkinnedMeshVertex[] vertices;
            int[] indices;
            int indicescount = 0, verticescount = 0;
            for (int i = 0; i < meshes.Length; i++)
            {
                verticescount += meshes[i].BufferVertex.Length;
                indicescount += meshes[i].BufferIndex.Length;
            }
            vertices = new CSkinnedMeshVertex[verticescount];
            indices = new int[indicescount];
            int vertexoffset = 0;
            int indexoffset = 0;
            for (int i = 0; i < meshes.Length; i++)
            {
                Mesh cm = meshes[i];
                int currentvert = cm.BufferVertex.Length;
                int currentindx = cm.BufferIndex.Length;


                for (int ci = 0; ci < currentvert; ci++)
                    vertices[ci + vertexoffset] = new CSkinnedMeshVertex(cm.BufferVertex[ci]);

                for (int ci = 0; ci < currentindx; ci++)
                    indices[ci + indexoffset] = cm.BufferIndex[ci] + vertexoffset;

                vertexoffset += currentvert;
                indexoffset += currentindx;
            }


            newmesh.BufferIndex = indices;
            newmesh.BufferVertex = vertices;


            return newmesh;
        }
    
    }
}

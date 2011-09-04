using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;

namespace ResourceCollector
{
    public class skinnedVertex
    {
        public Vector3 coordinates, normal;
        public Vector2 textureccordinates;
        public vertexskin skin;
        public skinnedVertex()
        {
            coordinates = new Vector3();
            normal = new Vector3();
            textureccordinates = new Vector2();
            skin = new vertexskin();
        }
        public skinnedVertex(skinnedVertex v1, skinnedVertex v2)
        {

            coordinates = new Vector3((v1.coordinates.X + v2.coordinates.X) / 2, (v1.coordinates.Y + v2.coordinates.Y) / 2, (v1.coordinates.Z + v2.coordinates.Z) / 2);
            normal = new Vector3((v1.normal.X + v2.normal.X) / 2, (v1.normal.Y + v2.normal.Y) / 2, (v1.normal.Z + v2.normal.Z) / 2);


            skin = new vertexskin();
            if (v1.skin.elementscount != 0)
            {
                List<BoneRelation> ourbones = new List<BoneRelation>();
                BoneRelation tmp = new BoneRelation();

                tmp.bonename = v1.bone1;
                tmp.koefficient = v1.k1;

                ourbones.Add(tmp);

                if (v1.bone2 != "" && v1.k2 > 0.1f)
                {
                    tmp.bonename = v1.bone2;
                    tmp.koefficient = v1.k2;
                    ourbones.Add(tmp);
                }
                if (v1.bone3 != "" && v1.k3 > 0.1f)
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
                        BoneRelation b = ourbones[t];
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
                        BoneRelation b = ourbones[t];
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
                        BoneRelation b = ourbones[t];
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
                    BoneRelation tmp1 = ourbones[max];
                    ourbones[max] = ourbones[i];
                    ourbones[i] = tmp1;
                }

                bone1 = ourbones[0].bonename;
                k1 = ourbones[0].koefficient;

                skin.elementscount = 1;
                if (ourbones.Count > 1)
                {
                    skin.elementscount = 2;
                    bone2 = ourbones[1].bonename;
                    k2 = ourbones[1].koefficient;
                    if (ourbones.Count > 2)
                    {
                        skin.elementscount = 3;
                        bone3 = ourbones[2].bonename;
                        k3 = ourbones[2].koefficient;
                    }
                }
                float sum = k1 + (skin.elementscount > 1 ? k2 : 0) + (skin.elementscount > 2 ? k3 : 0);
                k1 /= sum;
                k2 /= (skin.elementscount > 1 ? sum : 1);
                k3 /= (skin.elementscount > 2 ? sum : 1);
            }
            else
            {
                skin.elementscount = 0;
                skin.skins = new skinelement[0];
            }
        }
        public override string ToString()
        {
            return string.Format("crd:{0}; norm:{1}; tcrd:{2};", coordinates, normal, textureccordinates);
        }
        public string bone1
        {
            get
            {
                return skin.skins[0].bonename;
            }
            set
            {
                if (skin.elementscount < 1)
                {
                    skin.elementscount = 1;
                    skin.skins = new skinelement[1];
                    skin.skins[0] = new skinelement();
                }
                skin.skins[0].bonename = value;
            }
        }
        public string bone2
        {
            get
            {
                if (skin.skins.Length < 2)
                    return "";
                else
                    return skin.skins[1].bonename;
            }
            set
            {
                if (skin.skins.Length < 2)
                {
                    skinelement[] ttt = skin.skins;
                    skin.elementscount = 2;
                    skin.skins = new skinelement[2];
                    skin.skins[0] = ttt[0];
                    skin.skins[1] = new skinelement();
                }
                skin.skins[1].bonename = value;
            }
        }
        public string bone3
        {
            get
            {
                if (skin.skins.Length < 3)
                    return "";
                else
                    return skin.skins[2].bonename;
            }
            set
            {
                if (skin.skins.Length < 3)
                {
                    skinelement[] ttt = skin.skins;
                    skin.elementscount = 3;
                    skin.skins = new skinelement[3];
                    skin.skins[0] = ttt[0];
                    skin.skins[1] = ttt[1];
                    skin.skins[2] = new skinelement();
                }
                skin.skins[2].bonename = value;
            }
        }
        public float k3
        {
            get
            {
                if (skin.skins.Length < 3)
                    return 0.0f;
                else
                    return skin.skins[2].coefficient;

            }
            set
            {
                if (skin.skins.Length < 3)
                {
                    skinelement[] ttt = skin.skins;
                    skin.elementscount = 3;
                    skin.skins = new skinelement[3];
                    skin.skins[0] = ttt[0];
                    skin.skins[1] = ttt[1];
                    skin.skins[2] = new skinelement();
                }
                skin.skins[2].coefficient = value;
            }
        }
        public float k2
        {
            get
            {
                if (skin.skins.Length < 2)
                    return 0.0f;
                else
                    return skin.skins[1].coefficient;
            }
            set
            {
                if (skin.skins.Length < 2)
                {
                    skinelement[] ttt = skin.skins;
                    skin.elementscount = 2;
                    skin.skins = new skinelement[2];
                    skin.skins[0] = ttt[0];

                    skin.skins[1] = new skinelement();
                }
                skin.skins[1].coefficient = value;
            }
        }
        public float k1
        {
            get
            {
                return skin.skins[0].coefficient;
            }
            set
            {
                if (skin.skins.Length < 1)
                {
                    skin.elementscount = 1;
                    skin.skins = new skinelement[1];
                    skin.skins[0] = new skinelement();
                }
                skin.skins[0].coefficient = value;
            }
        }
    }

    public class skinnedFace
    {
        public skinnedVertex v0, v1, v2;
        public int cv0, cv1, cv2;
        public int tv0, tv1, tv2;
        Vector2 t0, t1, t2;
        public skinnedFace()
        {
        }
        public skinnedFace(skinnedVertex v1, skinnedVertex v2, skinnedVertex v3, Vector2 t0, Vector2 t1, Vector2 t2)
        {
            this.v0 = v1;
            this.v1 = v2;
            this.v2 = v3;
            this.t0 = t0;
            this.t1 = t1;
            this.t2 = t2;
        }
        public skinnedFace(skinnedFace f)
        {
            this.v0 = f.v0;
            this.v1 = f.v1;
            this.v2 = f.v2;
            this.v0 = f.v0;
            this.cv1 = f.cv1;
            this.cv2 = f.cv2;
            this.cv0 = f.cv0;
            this.tv1 = f.tv1;
            this.tv2 = f.tv2;
            this.tv0 = f.tv0;
            this.t0 = f.t0;
            this.t1 = f.t1;
            this.t2 = f.t2;
        }
        public override string ToString()
        {
            return string.Format("V0:({0});\t\tV1:({1});\t\tV2:({2});", v0, v1, v2);
        }
    }

    public class vertexskin
    {
        public int elementscount;
        public skinelement[] skins;
        public vertexskin()
        {
            elementscount = 0;
            skins = new skinelement[0];
        }
        public vertexskin(string bn1, string bn2, string bn3, float k1, float k2, float k3)
        {
            elementscount = bn3 == null || bn3 == "" ? bn2 == null || bn2 == "" ? 1 : 2 : 3;
            skins = new skinelement[elementscount];
            skins[0] = new skinelement();
            skins[0].bonename = bn1; skins[0].coefficient = k1;
            if (elementscount > 1)
            {
                skins[1] = new skinelement();
                skins[1].bonename = bn2; skins[1].coefficient = k2;
                if (elementscount > 2)
                {
                    skins[2] = new skinelement();
                    skins[2].bonename = bn2; skins[2].coefficient = k2;
                }
            }

        }
        public vertexskin(System.IO.BinaryReader br)
        {

            elementscount = br.ReadInt32();
            skins = new skinelement[elementscount];
            for (int i = 0; i < elementscount; i++)
                skins[i] = new skinelement(br);



        }
        public vertexskin(int elcount, string[] bonenames, float[] koeffs)
        {
            elementscount = elcount;
            skins = new skinelement[elcount];
            for (int i = 0; i < elcount; i++)
            {
                skins[i] = new skinelement();
                skins[i].bonename = bonenames[i];
                skins[i].coefficient = koeffs[i];
            }
        }
    }

    public class skinelement
    {
        public string bonename;
        public float coefficient;
        public int boneindex;
        public skinelement(System.IO.BinaryReader br)
        {
           // int count = br.ReadInt32();
            bonename = br.ReadPackString(); ;
            coefficient = br.ReadSingle();
        }
        public skinelement()
        {
        }
    }

    public class CSkinnedMeshVertex
    {
        public Vector3 pos;
        public Vector3 normal;
        public Vector2 tcoord;
        public string[] BoneIDs;
        public float[] BoneWeights;

        public CSkinnedMeshVertex()
        { }
        public CSkinnedMeshVertex(Vector3 _pos, string[] _bonenames, float[] _BoneWeights, Vector3 _normal, Vector3 _tangent, Vector3 _binormal, Vector2 _tcoord)
        {
            pos = new Vector3(_pos.X, _pos.Y, _pos.Z);

            BoneIDs = new string[_bonenames.Length];
            _bonenames.CopyTo(BoneIDs, 0);
            BoneWeights = new float[_BoneWeights.Length];
            _BoneWeights.CopyTo(BoneWeights, 0);
            normal = new Vector3(_normal.X, _normal.Y, _normal.Z);

            tcoord = new Vector2(_tcoord.X,_tcoord.Y);

            int i;
            for (i = 0; i < BoneIDs.Length; i++)
                if (BoneIDs[i] == null)
                    break;
            string[] newBoneIDs = new string[i];
            for (i = 0; i < newBoneIDs.Length; i++)
                newBoneIDs[i] = BoneIDs[i];
            BoneIDs = newBoneIDs;
            float[] newBoneWeights = new float[newBoneIDs.Length];
            for (i = 0; i < newBoneIDs.Length; i++)
                newBoneWeights[i] = BoneWeights[i];
            BoneWeights = newBoneWeights;
        }
        public CSkinnedMeshVertex(CSkinnedMeshVertex _vert)
        {
            pos = new Vector3(_vert.pos.X, _vert.pos.Y, _vert.pos.Z);
            BoneIDs = new string[_vert.BoneIDs.Length];
            _vert.BoneIDs.CopyTo(BoneIDs, 0);
            BoneWeights = new float[_vert.BoneWeights.Length];
            _vert.BoneWeights.CopyTo(BoneWeights, 0);
            normal = new Vector3(_vert.normal.X, _vert.normal.Y, _vert.normal.Z);
            tcoord = new Vector2(_vert.tcoord.X, _vert.tcoord.Y);

            int i;
            for (i = 0; i < BoneIDs.Length; i++)
                if (BoneIDs[i] == null)
                    break;
            string[] newBoneIDs = new string[i];
            for (i = 0; i < newBoneIDs.Length; i++)
                newBoneIDs[i] = BoneIDs[i];
            BoneIDs = newBoneIDs;
            float[] newBoneWeights = new float[newBoneIDs.Length];
            for (i = 0; i < newBoneIDs.Length; i++)
                newBoneWeights[i] = BoneWeights[i];
            BoneWeights = newBoneWeights;
        }
        public bool comareto(CSkinnedMeshVertex comp)
        {
            return (comp.pos == pos) && (normal == comp.normal) && (tcoord == comp.tcoord) && (BoneIDs == comp.BoneIDs);
        }
    }

    public class CMeshVertex
    {
        public Vector3 pos;
        public Vector3 normal;
        public Vector2 tcoord;
        public string[] BoneIDs;
        public float[] BoneWeights;

        public CMeshVertex()
        { }

        public CMeshVertex(Vector3 _pos, Vector3 _normal, Vector3 _tangent, Vector3 _binormal, Vector2 _tcoord)
        {
            pos = new Vector3(_pos.X, _pos.Y, _pos.Z);
            normal = new Vector3(_normal.X, _normal.Y, _normal.Z);
            tcoord = new Vector2(_tcoord.X, _tcoord.Y);
        }

        public CMeshVertex(CMeshVertex _vert)
        {
            pos = new Vector3(_vert.pos.X, _vert.pos.Y, _vert.pos.Z);
           
            normal = new Vector3(_vert.normal.X, _vert.normal.Y, _vert.normal.Z);
            tcoord = new Vector2(_vert.tcoord.X, _vert.tcoord.Y);
        }

        public bool comareto(CMeshVertex comp)
        {
            return (comp.pos == pos) && (normal == comp.normal) && (tcoord == comp.tcoord);
        }
    }

    public class tridata
    {
        public int[] v;

        public tridata()
        {
            v = new int[3];
        }

        public override string ToString()
        {
            return v[0].ToString() + " " + v[1].ToString() + " " + v[2].ToString();
        }
    }
}
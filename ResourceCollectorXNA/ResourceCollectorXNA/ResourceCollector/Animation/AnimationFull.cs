using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace ResourceCollector
{
    public class FullAnim : Animation // 
    {
        public DecomposedMatrix[][] matrices;
        public float length;

        public int BonesCount;

        public FullAnim()
        {
            type = 0;
        }

        //public Skeleton Skeleton
        //{
        //    get;
        //    protected set;
        //}

        public override Matrix[] GetMatrices(float frame)
        {
            return DecomposedMatrix.ConvertToMartixArray(matrices[Convert.ToInt32(frame)]);
        }

        public static FullAnim FromStream(System.IO.Stream stream, Skeleton skeleton)
        {
            Matrix[][] Frames;

            var clip = new FullAnim();
            var reader = new System.IO.BinaryReader(stream);

            var start = reader.ReadInt32();
            var end = reader.ReadInt32();
            var length = end - start + 1;

            clip.BonesCount = reader.ReadInt32();
            var counter = reader.ReadInt32();

            Frames = new Matrix[length][];
            for (int i = 0; i < length; i++)
                Frames[i] = new Matrix[clip.BonesCount];

            for (int i = 0; i < clip.BonesCount; i++)
                for (int j = 0; j < length; j++)
                    Frames[j][i] = reader.ReadMatrix();

            for (int i = 0; i < clip.BonesCount; i++)
                for (int @in = 0; @in < length; @in++)
                    Frames[@in][i] = skeleton.bones[i].BaseMatrix * Frames[@in][i];




            Matrix[][] sss = Animation.otnosmatrix(Frames, skeleton);
            DecomposedMatrix[][] qtr = GetQuater(skeleton, sss);

            clip.matrices = qtr;

            return clip;
        }

        public static FullAnim FromStream(System.IO.Stream stream, SkeletonWithAddInfo skeleton)
        {
            Matrix[][] Frames;

            var clip = new FullAnim();
            var reader = new System.IO.BinaryReader(stream);

            var start = reader.ReadInt32();
            var end = reader.ReadInt32();
            var length = end - start + 1;

            clip.BonesCount = reader.ReadInt32();
            var counter = reader.ReadInt32();

            Frames = new Matrix[length][];
            for (int i = 0; i < length; i++)
                 Frames[i] = new Matrix[clip.BonesCount];


            for (int i = 0; i < clip.BonesCount; i++)
                for (int j = 0; j < length; j++)
                    Frames[j][i] = reader.ReadMatrix();


            for (int i = 0; i < clip.BonesCount; i++)
                for (int @in = 0; @in < length; @in++)
                    Frames[@in][i] = skeleton.baseskelet.bones[i].BaseMatrix * Frames[@in][i];
                
            

            //clip.Skeleton = skeleton.baseskelet;

            Matrix[][] sss = Animation.otnosmatrix(Frames, skeleton.baseskelet);
            DecomposedMatrix[][] qtr = GetQuater(skeleton.baseskelet, sss);

            clip.matrices = qtr;

            //Matrix[][] ssss = SetQuater(qtr);

            //Matrix[][] ppp = priammatrix(0, clip, ssss);

            //clip.Frames = ppp;

            return clip;
        }

        public static bool MtrxToStream(System.IO.BinaryWriter stream, DecomposedMatrix[][] mtrx)
        {
            try
            {
                stream.Write(mtrx.Length);
                for (int i = 0; i < mtrx.Length; i++)
                {
                    stream.Write(mtrx[i].Length);
                    for (int j = 0; j < mtrx[i].Length; j++)
                    {
                        stream.Write((double)mtrx[i][j].rotation.W);
                        stream.Write((double)mtrx[i][j].rotation.X);
                        stream.Write((double)mtrx[i][j].rotation.Y);
                        stream.Write((double)mtrx[i][j].rotation.Z);
                        stream.WriteVector3(mtrx[i][j].translation);
                        stream.WriteVector3(mtrx[i][j].scale);

                    }

                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static DecomposedMatrix[][] MtrxFromStream(System.IO.BinaryReader stream)
        {
            DecomposedMatrix[][] res;
            int length = stream.ReadInt32();
            res = new DecomposedMatrix[length][];
            //stream.Write(mtrx.Length);
            for (int i = 0; i < length; i++)
            {
                int length2 = stream.ReadInt32();
                res[i] = new DecomposedMatrix[length2];
                //stream.Write(mtrx[i].Length);
                for (int j = 0; j < length2; j++)
                {
                    res[i][j] = new DecomposedMatrix();
                    float W = (float)stream.ReadDouble();
                    float X = (float)stream.ReadDouble();
                    float Y = (float)stream.ReadDouble();
                    float Z = (float)stream.ReadDouble();
                    //stream.Write(mtrx[i][j].rotation.W);
                    //stream.Write(mtrx[i][j].rotation.X);
                    //stream.Write(mtrx[i][j].rotation.Y);
                    //stream.Write(mtrx[i][j].rotation.Z);                
                    res[i][j].rotation = new Quaternion(W, Y, Z, W);
                    res[i][j].translation = stream.ReadVector3();
                    //stream.WriteVector3(mtrx[i][j].translation);
                    res[i][j].scale = stream.ReadVector3();
                    //stream.WriteVector3(mtrx[i][j].scale);

                }
            }
            return res;
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}

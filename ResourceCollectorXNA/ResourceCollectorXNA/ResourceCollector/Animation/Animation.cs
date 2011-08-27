using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
namespace ResourceCollector
{

    public abstract class Animation // абстракный общий класс анимации
    {
        public abstract Matrix[] GetMatrices(float frame);
        public bool isTransition;
        public int type;

        //public Skeleton skeleton;
        public String name;


        public static Matrix[][] otnosmatrix(Matrix[][] Frames, Skeleton _skeleton)
        {
            int ss = _skeleton.RootIndex;
            Matrix[][] otnframes = new Matrix[Frames.Length][];
            for (int i = 0; i < Frames.Length; i++)
            {
                otnframes[i] = new Matrix[Frames[i].Length];
                Frames[i].CopyTo(otnframes[i], 0);
            }
            foreach (Bone bns in _skeleton.bones[ss].Childrens)
            {
                obhod(bns, Frames, otnframes, _skeleton);
            }

            return otnframes;
        }

        public static void obhod(Bone curbone, Matrix[][] Frames, Matrix[][] otnframes, Skeleton _skeleton)
        {

            int ss = curbone.index;
            int parind = curbone.Parent.index;

            for (int i = 0; i < Frames.Length; i++)
            {

                otnframes[i][ss] = Frames[i][ss] * Matrix.Invert(Frames[i][parind]);

            }

            foreach (Bone bns in _skeleton.bones[ss].Childrens)
            {
                obhod(bns, Frames, otnframes, _skeleton);
            }


        }
        public static DecomposedMatrix[][] GetQuater(Skeleton _skeleton, Matrix[][] otnframes)
        {

            DecomposedMatrix[][] Qframes = new DecomposedMatrix[otnframes.Length][];
            for (int i = 0; i < otnframes.Length; i++)
            {
                Qframes[i] = new DecomposedMatrix[_skeleton.bones.Length];
            }
            int j = 0;
            foreach (Matrix[] mtrx in otnframes)
            {
                int k = 0;
                foreach (Matrix xxx in mtrx)
                {
                    Qframes[j][k] = new DecomposedMatrix(xxx);
                    k++;
                }
                j++;
            }
            return Qframes;
        }



        public static Matrix[] priammatrix(Skeleton _Skeleton, Matrix[] otnosm)
        {
            int ss = _Skeleton.RootIndex;

            Matrix[] frames = new Matrix[_Skeleton.bones.Length];
            //otnosm[15] = otnosm[i][15] * (Matrix.CreateRotationX((float)(Math.PI / 8)));
            frames[ss] = otnosm[ss];

            foreach (Bone bns in _Skeleton.bones[ss].Childrens)
            {
                antiobhod(bns, otnosm, frames);
            }

            return frames;
        }
        public static void antiobhod(Bone curbone, Matrix[] otnosm, Matrix[] frames)
        {
            int ss = curbone.index;
            int parind = curbone.Parent.index;
            frames[ss] = otnosm[ss] * frames[parind];
            foreach (Bone bns in curbone.Childrens)
            {
                antiobhod(bns, otnosm, frames);
            }
        }

    }
}

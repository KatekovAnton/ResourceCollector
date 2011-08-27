using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ResourceCollector
{
    public class DecomposedMatrix
    {
        public Quaternion rotation;
        public Vector3 translation;
        public Vector3 scale;

        public DecomposedMatrix(Matrix _matrix)
        {
            _matrix.Decompose(out scale, out rotation, out translation);
        }

        public DecomposedMatrix()
        {
        }

        public Matrix GetMartix()
        {
            Matrix mtrx = Matrix.CreateScale(scale) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(translation);
            return mtrx;
        }

        public static Matrix[] ConvertToMartixArray(DecomposedMatrix[] entermatrix)
        {
            Matrix[] result = new Matrix[entermatrix.Length];
            for (int i = 0; i < entermatrix.Length; i++)
            {
                result[i] = Matrix.CreateScale(entermatrix[i].scale) * Matrix.CreateFromQuaternion(entermatrix[i].rotation) * Matrix.CreateTranslation(entermatrix[i].translation);// entermatrix[i].GetMartix();
            }
            return result;
        }
    }
}

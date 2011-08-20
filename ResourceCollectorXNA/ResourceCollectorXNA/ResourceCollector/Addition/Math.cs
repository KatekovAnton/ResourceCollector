using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace ResourceCollector
{
    public static class MyMath
    {
        public static bool Near(this Vector3 v, Vector3 v1)
        {
            return Math.Abs(v.X - v1.X) < 0.0001f && Math.Abs(v.Y - v1.Y) < 0.0001f && Math.Abs(v.Z - v1.Z) < 0.0001f;
            // return Math.Abs(x - v.x) + Math.Abs(y - v.y) + Math.Abs(z - v.z) < 0.00001f;
        }
        public static bool Near(this Vector2 v, Vector2 v1)
        {
            return Math.Abs(v.X - v1.X) < 0.0001f && Math.Abs(v.Y - v1.Y) < 0.0001f;
            // return Math.Abs(x - v.x) + Math.Abs(y - v.y) + Math.Abs(z - v.z) < 0.00001f;
        }
        public static float length_squared(this Vector3 v)
        {
            return v.X * v.X + v.Y * v.Y + v.Z * v.Z;
        }
    }
   /* public class vector3
    {
        public float x, y, z;

        public vector3()
        {

        }
        public float length_squared()
        {
            return x * x + y * y + z * z;
        }
        public vector3(float _x, float _y, float _z)
        {
            x = _x; y = _y; z = _z;
        }

        public vector3(vector3 _z)
        {
            x = _z.x; y = _z.y; z = _z.z;
        }
        public bool Near(vector3 v)
        {
            return Math.Abs(v.x - x) < 0.0001f && Math.Abs(v.y - y) < 0.0001f && Math.Abs(v.z - z) < 0.0001f;
            // return Math.Abs(x - v.x) + Math.Abs(y - v.y) + Math.Abs(z - v.z) < 0.00001f;
        }
        public override string ToString()
        {
            return string.Format("({0}; {1}; {2})", x.ToString(), y.ToString(), z.ToString());
        }

        public float X
        {
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public float Y
        {
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        public float Z
        {
            get
            {
                return z;
            }
            set
            {
                z = value;
            }
        }

    }
    public class vector2
    {
        public float x, y;
        public vector2(float _x, float _y)
        {
            x = _x; y = _y;
        }
        public vector2()
        {

        }
        public vector2(vector2 _z)
        {
            x = _z.x; y = _z.y;
        }
        public bool Near(vector2 v)
        {
            return Math.Abs(x - v.x) + Math.Abs(y - v.y) < 0.00001f;
        }
        public override string ToString()
        {
            return string.Format("({0}; {1})", x.ToString(), y.ToString());
        }
    }
    public class Matrix
    {
        public float[] elements
        {
            get;
            private set;
        }
        public Matrix()
        {
            elements = new float[16];
        }
        public static readonly Matrix Identity = new Matrix(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
        public Matrix(float a11, float a12, float a13, float a14,
            float a21, float a22, float a23, float a24,
            float a31, float a32, float a33, float a34,
                float a41, float a42, float a43, float a44)
        {
            elements = new float[16];
            elements[0] = a11;
            elements[1] = a12;
            elements[2] = a13;
            elements[3] = a14;
            elements[4] = a21;
            elements[5] = a22;
            elements[6] = a23;
            elements[7] = a24;
            elements[8] = a31;
            elements[9] = a32;
            elements[10] = a33;
            elements[11] = a34;
            elements[12] = a41;
            elements[13] = a42;
            elements[14] = a43;
            elements[15] = a44;
        }
        public Matrix(Matrix m)
        {
            elements = new float[16];
            m.elements.CopyTo(elements, 0);
        }


        public float M11
        {
            get
            {
                return elements[0];
            }
            set
            {
                elements[0] = value;
            }
        }
        public float M12
        {
            get
            {
                return elements[1];
            }
            set
            {
                elements[1] = value;
            }
        }
        public float M13
        {
            get
            {
                return elements[2];
            }
            set
            {
                elements[2] = value;
            }
        }
        public float M14
        {
            get
            {
                return elements[3];
            }
            set
            {
                elements[3] = value;
            }
        }

        public float M21
        {
            get
            {
                return elements[4];
            }
            set
            {
                elements[4] = value;
            }
        }
        public float M22
        {
            get
            {
                return elements[5];
            }
            set
            {
                elements[5] = value;
            }
        }
        public float M23
        {
            get
            {
                return elements[6];
            }
            set
            {
                elements[6] = value;
            }
        }
        public float M24
        {
            get
            {
                return elements[7];
            }
            set
            {
                elements[7] = value;
            }
        }

        public float M31
        {
            get
            {
                return elements[8];
            }
            set
            {
                elements[8] = value;
            }
        }
        public float M32
        {
            get
            {
                return elements[9];
            }
            set
            {
                elements[9] = value;
            }
        }
        public float M33
        {
            get
            {
                return elements[10];
            }
            set
            {
                elements[10] = value;
            }
        }
        public float M34
        {
            get
            {
                return elements[11];
            }
            set
            {
                elements[11] = value;
            }
        }

        public float M41
        {
            get
            {
                return elements[12];
            }
            set
            {
                elements[12] = value;
            }
        }
        public float M42
        {
            get
            {
                return elements[13];
            }
            set
            {
                elements[13] = value;
            }
        }
        public float M43
        {
            get
            {
                return elements[14];
            }
            set
            {
                elements[14] = value;
            }
        }
        public float M44
        {
            get
            {
                return elements[15];
            }
            set
            {
                elements[15] = value;
            }
        }

      
    }*/
}

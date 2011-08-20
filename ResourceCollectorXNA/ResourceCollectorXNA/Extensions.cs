﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using ResourceCollectorXNA.Content;
using System.IO;


namespace ResourceCollectorXNA
{
    public class StaticInterfaceParameters
    {
        public static float ViewportBorder = 200.0f;
    }
    public static class Extensions
    {
        public static string ReadPackString(this System.IO.BinaryReader self)//стринг с нулём
        {
            var length = self.ReadInt32();
            return new string(self.ReadChars(length + 1));
        }
        public static bool Contains(this System.Array self, object obj)//стринг с нулём
        {
            foreach(object t in self)
                if(t == obj)
                    return true;
            return false;
        }
        public static Matrix ReadMatrix(this BinaryReader self)
        {
            return new Matrix(
                self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), 0.0f,
                self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), 0.0f,
                self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), 0.0f,
                self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), 1.0f
            );
        }
        public static Vector3 ReadVector3(this BinaryReader self)
        {
            return new Vector3(self.ReadSingle(), self.ReadSingle(), self.ReadSingle());
        }

        public static void WriteVector3(this BinaryWriter self, Vector3 v)
        {
            self.Write(v.X);
            self.Write(v.Y);
            self.Write(v.Z);
        }
        public static void Times(this int self, Action<int> action)
        {
            for (var i = 0; i < self; i++)
            {
                action(i);
            }
        }

        public static Microsoft.Xna.Framework.Ray FromScreenPoint(Viewport viewport, Vector2 mousePos, Matrix view, Matrix project)
        {
            Vector3 nearPoint = viewport.Unproject(new Vector3(mousePos, 0), project, view, Matrix.Identity);
            Vector3 farPoint = viewport.Unproject(new Vector3(mousePos, 1), project, view, Matrix.Identity);

            return new Microsoft.Xna.Framework.Ray(nearPoint, farPoint - nearPoint);
        }

        public static void GetCenter(this BoundingBox bbox, out Vector3 center)
        {
            center = new Vector3(
             (bbox.Min.X + bbox.Max.X) / 2,
             (bbox.Min.Y + bbox.Max.Y) / 2,
             (bbox.Min.Z + bbox.Max.Z) / 2);
        }

        public static void GetSize(this BoundingBox bbox, out Vector3 size)
        {
            size = new Vector3(
             bbox.Max.X - bbox.Min.X,
             bbox.Max.Y - bbox.Min.Y,
             bbox.Max.Z - bbox.Min.Z);
        }

        public static double Round(double value, int digits)
        {
            double scale = Math.Pow(10.0, digits);
            double round = Math.Floor(Math.Abs(value) * scale + 0.5);
            return (Math.Sign(value) * round / scale);
        }
        public static Vector2 GetVector2(this Vector3 value)
        {
            return new Vector2(value.X, value.Y);
        }
        public static bool ContainsPoint(this Viewport port, Vector2 position)
        {
            return (position.X >= -StaticInterfaceParameters.ViewportBorder) &&
                (position.Y >= -StaticInterfaceParameters.ViewportBorder) &&
                (position.X <= port.Width +StaticInterfaceParameters.ViewportBorder) &&
                (position.Y <= port.Height + StaticInterfaceParameters.ViewportBorder);
        }
    }
}
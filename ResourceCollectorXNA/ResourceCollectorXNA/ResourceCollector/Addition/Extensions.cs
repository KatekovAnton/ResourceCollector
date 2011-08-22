using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
namespace ResourceCollector
{
    public static class Extensions
    {
        
       /* public static GameTime FromFloat(long elapsed)
        {
            return new GameTime(new TimeSpan(Content.TerrainEditorXNA.starttime.Ticks), new TimeSpan(elapsed));
        }*/
        public static void WriteMatrix(this BinaryWriter self, Matrix m)
        {
            self.Write(m.M11); self.Write(m.M12); self.Write(m.M13);
            self.Write(m.M21); self.Write(m.M22); self.Write(m.M23);
            self.Write(m.M31); self.Write(m.M32); self.Write(m.M33);
            self.Write(m.M41); self.Write(m.M42); self.Write(m.M43); 
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

        public static void WriteMatrixFull(this BinaryWriter self, Matrix m)
        {
            self.Write(m.M11); self.Write(m.M12); self.Write(m.M13);
            self.Write(m.M21); self.Write(m.M22); self.Write(m.M23);
            self.Write(m.M31); self.Write(m.M32); self.Write(m.M33);
            self.Write(m.M41); self.Write(m.M42); self.Write(m.M43); self.Write(m.M44);
        }

        public static Matrix ReadMatrixFull(this BinaryReader self)
        {
            return new Matrix(
                self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), 0.0f,
                self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), 0.0f,
                self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), 0.0f,
                self.ReadSingle(), self.ReadSingle(), self.ReadSingle(), self.ReadSingle()
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

      
    }
}

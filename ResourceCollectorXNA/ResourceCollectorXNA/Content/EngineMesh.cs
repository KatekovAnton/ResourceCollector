using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ResourceCollectorXNA.Engine;

namespace ResourceCollectorXNA.Content
{
    public class EngineMesh : IDisposable
    {
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;

        public EngineMesh(EngineVertex[] vertices, ushort[] indices)
        {
            Engine.ContentLoader.ContentLoader.XNAObjectsLoadedCount++;
            Engine.ContentLoader.ContentLoader.XNAevents.Add("created vertex buffer");
      
            Engine.ContentLoader.ContentLoader.XNAObjectsLoadedCount++;
            Engine.ContentLoader.ContentLoader.XNAevents.Add("created index buffer");
            



            vertexBuffer = new VertexBuffer(GameEngine.Device, typeof(EngineVertex), vertices.Length, BufferUsage.None);
            vertexBuffer.SetData<EngineVertex>(vertices);

            indexBuffer = new IndexBuffer(GameEngine.Device, IndexElementSize.SixteenBits, indices.Length, BufferUsage.None);
            indexBuffer.SetData<ushort>(indices);
        }

        public EngineMesh()
        {
        }

        public void Render()
        {
            GameEngine.Device.SetVertexBuffer(vertexBuffer);
            GameEngine.Device.Indices = indexBuffer;
            GameEngine.Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);
        }

        bool Disposed = false;

        public void Dispose()
        {
            if (!Disposed)
            {
                indexBuffer.Dispose();
                vertexBuffer.Dispose();

                Disposed = true;
                GC.SuppressFinalize(this);

                Engine.ContentLoader.ContentLoader.XNAObjectsLoadedCount--;
                Engine.ContentLoader.ContentLoader.XNAevents.Add("disposed vertex buffer");

                Engine.ContentLoader.ContentLoader.XNAObjectsLoadedCount--;
                Engine.ContentLoader.ContentLoader.XNAevents.Add("disposed index buffer");
            }
        }

        ~EngineMesh()
        {
            Dispose();
        }

        public static void LoadBuffers(Stream stream, out EngineVertex[] vertices, out int[] indices)
        {
            var reader = new BinaryReader(stream);

            var vertexBufferLength = reader.ReadInt32();
            var indexBufferLength = reader.ReadInt32();

            vertices = new EngineVertex[vertexBufferLength];
            indices = new int[indexBufferLength];

            for (int i = 0; i < vertexBufferLength; i++)
            {
                vertices[i].position.X = reader.ReadSingle();
                vertices[i].position.Y = reader.ReadSingle();
                vertices[i].position.Z = reader.ReadSingle();

                vertices[i].normal.X = reader.ReadSingle();
                vertices[i].normal.Y = reader.ReadSingle();
                vertices[i].normal.Z = reader.ReadSingle();

                vertices[i].textureCoordinate.X = reader.ReadSingle();
                vertices[i].textureCoordinate.Y = 1.0f - reader.ReadSingle();


            }

            for (int i = 0; i < indexBufferLength; i += 3)
            {
                indices[i + 2] = reader.ReadInt32();
                indices[i + 1] = reader.ReadInt32();
                indices[i] = reader.ReadInt32();
            }
        }

        public void loadbody(byte[] buffer)
        {
          //  vertexdeclaration = new VertexPositionNormalTexture();
            EngineVertex[] vertices;
            ushort[] indices;

            BinaryReader br = new BinaryReader(new MemoryStream(buffer));
            vertices = new EngineVertex[br.ReadInt32()];
            indices = new ushort[br.ReadInt32()];

            for (int bv = 0; bv < vertices.Length; bv++)
            {
                vertices[bv] = new EngineVertex(
                    new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                    new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle()),
                    new Vector2(br.ReadSingle(), 1.0f -br.ReadSingle()));

                string sss;
                int t = br.ReadInt32();
                for (int i = 0; i < t; i++)
                    sss = br.ReadPackString();

                int d = br.ReadInt32();

                br.BaseStream.Seek(d * 4, SeekOrigin.Current);
            }

            for (int bv = 0; bv < indices.Length; bv++)
                indices[bv] = Convert.ToUInt16(br.ReadInt32());

            br.Close();
            Engine.ContentLoader.ContentLoader.XNAObjectsLoadedCount++;
            Engine.ContentLoader.ContentLoader.XNAevents.Add("created vertex buffer");
            vertexBuffer = new VertexBuffer(GameEngine.Device, typeof(VertexPositionNormalTexture), vertices.Length, BufferUsage.None);
            vertexBuffer.SetData<EngineVertex>(vertices);
            Engine.ContentLoader.ContentLoader.XNAObjectsLoadedCount++;
            Engine.ContentLoader.ContentLoader.XNAevents.Add("created index buffer");
            indexBuffer = new IndexBuffer(GameEngine.Device, IndexElementSize.SixteenBits, indices.Length, BufferUsage.None);
            indexBuffer.SetData<ushort>(indices);
        }
   
        public static EngineMesh FromContentMeshes(ResourceCollector.Mesh[] meshes)
        {
            EngineVertex[] vertices;

            int indicescount = 0, verticescount = 0;
            for (int i = 0; i < meshes.Length; i++)
            {
                verticescount += meshes[i].BufferVertex.Length;
                indicescount += meshes[i].BufferIndex.Length;
            }
            vertices = new EngineVertex[verticescount];
            int vertexoffset = 0;
            int indexoffset = 0;
            EngineMesh newmesh = null;
            
            if (indicescount > ushort.MaxValue)
                throw new Exception("Mesh have too many vertices!!!");
            ushort[] indices = new ushort[indicescount];
            for (int i = 0; i < meshes.Length; i++)
            {
                ResourceCollector.Mesh cm = meshes[i];
                int currentvert = cm.BufferVertex.Length;
                int currentindx = cm.BufferIndex.Length;


                for (int ci = 0; ci < currentvert; ci++)
                {
                    vertices[ci + vertexoffset] = new EngineVertex(cm.BufferVertex[ci].pos, cm.BufferVertex[ci].normal, cm.BufferVertex[ci].tcoord);
                    vertices[ci + vertexoffset].textureCoordinate.Y = 1.0f - vertices[ci + vertexoffset].textureCoordinate.Y;
                }

                for (int ci = 0; ci < currentindx; ci++)
                    indices[ci + indexoffset] = Convert.ToUInt16(cm.BufferIndex[ci] + vertexoffset);

                vertexoffset += currentvert;
                indexoffset += currentindx;
            }
            newmesh = new EngineMesh(vertices, indices);
            return newmesh;
        }
    }
}

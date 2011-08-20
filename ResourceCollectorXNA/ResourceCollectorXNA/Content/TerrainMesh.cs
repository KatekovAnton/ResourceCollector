using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ResourceCollectorXNA.Engine;

namespace ResourceCollectorXNA.Content
{
    public class TerrainMesh: IDisposable
    {
        private VertexBuffer vertexBuffer;
        private IndexBuffer indexBuffer;
     
        private bool Disposed = false;
        public Vector3 Size;
        public static RasterizerState terrainRasterizerState;

        public TerrainMesh(TerrainVertex[] vertices, ushort[] indices)
        {
            if (terrainRasterizerState == null)
            {
                terrainRasterizerState = new RasterizerState();
                terrainRasterizerState.CullMode = CullMode.None;
                terrainRasterizerState.FillMode = FillMode.WireFrame;
            }

            vertexBuffer = new VertexBuffer(GameEngine.Device, typeof(TerrainVertex), vertices.Length, BufferUsage.None);
            vertexBuffer.SetData<TerrainVertex>(vertices);

            indexBuffer = new IndexBuffer(GameEngine.Device, IndexElementSize.SixteenBits, indices.Length, BufferUsage.None);
            indexBuffer.SetData<ushort>(indices);
        }

        public TerrainMesh()
        {
        }

        public VertexBuffer VertexBuffer
        {
            get { return vertexBuffer; }
        }

        public IndexBuffer IndexBuffer
        {
            get { return indexBuffer; }
        }

        public void Render()
        {
            GameEngine.Device.SetVertexBuffer(vertexBuffer);
            GameEngine.Device.Indices = indexBuffer;
            /*RasterizerState back = GameEngine.Device.RasterizerState;
            GameEngine.Device.RasterizerState = terrainRasterizerState;
            */

            GameEngine.Device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vertexBuffer.VertexCount, 0, indexBuffer.IndexCount / 3);

          //  GameEngine.Device.RasterizerState = back;
        }
        
        public void Dispose()
        {
            indexBuffer.Dispose();
            vertexBuffer.Dispose();
            Disposed = true;
        }
        ~TerrainMesh()
        {
            if (!Disposed)
                Dispose();
        }
        public static TerrainMesh GenerateMesh(int WidthCount, int LenghtCount, float step)
        {
           // TerrainMesh tm = new TerrainMesh();
            int wverticesCount = WidthCount + 1;
            int lverticesCount = LenghtCount + 1;
            TerrainVertex[] vertices = new TerrainVertex[wverticesCount * lverticesCount];
            float sizew = step * WidthCount;
            float sizel = step * LenghtCount;
            float scaletcoord = 0.5f;
            Vector3 startvector = new Vector3(-sizew / 2, 0, -sizel / 2);
            
            for (int i = 0; i < wverticesCount;i++ )
                for (int j = 0; j < lverticesCount; j++)
                {
                    int index= i * lverticesCount + j;
                    vertices[index].position = startvector + new Vector3(i * step, 0, j * step);
                    vertices[index].textureCoordinate = new Vector2(i * scaletcoord, j * scaletcoord);
                    vertices[index].normal = Vector3.UnitY;
                }
            ushort[] indices = new ushort[WidthCount * LenghtCount * 6];
            int indexadd = 0;
            for (int i = 0; i < WidthCount; i++)
            {
                for (int j = 0; j < LenghtCount; j++)
                {
                    int qwadindex = i * LenghtCount + j;
                    int firsttrindex = qwadindex;
                    ushort index1 = Convert.ToUInt16(LenghtCount * i + j + indexadd);
                    ushort index2 = Convert.ToUInt16(LenghtCount * i + j + 1 + indexadd);
                    ushort index3 = Convert.ToUInt16(LenghtCount * (i + 1) + j + 1 + indexadd);


                    indices[firsttrindex * 3] = index1;
                    indices[firsttrindex * 3 + 1] = index2;
                    indices[firsttrindex * 3 + 2] = index3;



                    ushort index6 = Convert.ToUInt16(LenghtCount * (i + 1) + j + 2 + indexadd);


                    indices[firsttrindex * 3 + indices.Length / 2] = index3;
                    indices[firsttrindex * 3 + 1 + indices.Length / 2] = index2;
                    indices[firsttrindex * 3 + 2 + indices.Length / 2] = index6;
                }
                indexadd++;
            }
            TerrainMesh tm = new TerrainMesh(vertices, indices);
            tm.Size = new Vector3(WidthCount*step,0,LenghtCount*step);
            
            return tm;
        }
        public static void LoadBuffers(Stream stream, out EngineVertex[] vertices, out ushort[] indices)
        {
            var reader = new BinaryReader(stream);

            var vertexBufferLength = reader.ReadInt32();
            var indexBufferLength = reader.ReadInt32();

            vertices = new EngineVertex[vertexBufferLength];
            indices = new ushort[indexBufferLength];

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
                indices[i + 2] = Convert.ToUInt16(reader.ReadInt32());
                indices[i + 1] = Convert.ToUInt16(reader.ReadInt32());
                indices[i] = Convert.ToUInt16(reader.ReadInt32());
            }
        }

        public void loadbody(byte[] buffer)
        {
            TerrainVertex[] vertices;
            ushort[] indices;

            BinaryReader br = new BinaryReader(new MemoryStream(buffer));
            vertices = new TerrainVertex[br.ReadInt32()];
            indices = new ushort[br.ReadInt32()];

            for (int bv = 0; bv < vertices.Length; bv++)
            {
                vertices[bv] = new TerrainVertex(
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

            vertexBuffer = new VertexBuffer(GameEngine.Device, typeof(TerrainVertex), vertices.Length, BufferUsage.None);
            vertexBuffer.SetData<TerrainVertex>(vertices);

            indexBuffer = new IndexBuffer(GameEngine.Device, IndexElementSize.SixteenBits, indices.Length, BufferUsage.None);
            indexBuffer.SetData<ushort>(indices);
        }
    }
}

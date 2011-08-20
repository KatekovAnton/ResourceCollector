﻿using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace ResourceCollectorXNA.Content
{


    public struct EngineVertex : IVertexType
    {
        public Vector3 position;
        public Vector3 normal;
        public Vector2 textureCoordinate;


        public EngineVertex(Vector3 position = new Vector3(), Vector3 normal = new Vector3(), Vector2 textureCoordinate = new Vector2())
            
        {
            this.position = position;
            this.normal = normal;
            this.textureCoordinate = textureCoordinate;

        }

        public readonly static VertexDeclaration declaration = new VertexDeclaration
        (
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(24, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0)

        );

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return declaration; }
        }

    }
}


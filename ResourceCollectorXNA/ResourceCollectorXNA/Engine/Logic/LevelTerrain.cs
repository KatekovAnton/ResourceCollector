using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ResourceCollectorXNA.Engine.Logic
{
    public class LevelTerrain
    {
        public TerrainObject[,] terrain;
        public TerrainObject backterrain;
        public float step;
        public LevelTerrain(Vector2 size)
        {
            int count1 = (int)(size.X / (LevelTerrainProperties.VerticesInTerrainQuad * LevelTerrainProperties.CertexStep));
            int count2 = (int)(size.Y / (LevelTerrainProperties.VerticesInTerrainQuad * LevelTerrainProperties.CertexStep));

            Vector3 ElementSize = new Vector3(LevelTerrainProperties.VerticesInTerrainQuad * LevelTerrainProperties.CertexStep,0,LevelTerrainProperties.VerticesInTerrainQuad * LevelTerrainProperties.CertexStep);


            terrain = new TerrainObject[count1, count2];
            for (int i = 0; i < count1; i++)
                for (int j = 0; j < count2; j++)
                {
                    TerrainObject currentTerrain = ContentLoader.ContentLoader.GenerateTerrain((int)LevelTerrainProperties.VerticesInTerrainQuad, (int)LevelTerrainProperties.VerticesInTerrainQuad, LevelTerrainProperties.CertexStep, false, true);
                    terrain[i, j] = currentTerrain;
                    Matrix currentTerrainPosition = Matrix.CreateTranslation(-ElementSize / 2 + new Vector3(ElementSize.X * (float)i - size.X / 2, 0, ElementSize.Z * (float)j - size.Y / 2));
                    currentTerrain.SetGlobalPose(currentTerrainPosition);
                }
            backterrain = ContentLoader.ContentLoader.GenerateTerrain(1, 1, size.X,true, false);
            backterrain.SetGlobalPose(Matrix.CreateTranslation(new Vector3(-(LevelTerrainProperties.VerticesInTerrainQuad * LevelTerrainProperties.CertexStep), -5, -(LevelTerrainProperties.VerticesInTerrainQuad * LevelTerrainProperties.CertexStep))));
        }
    }
    public static class LevelTerrainProperties
    {
        public const float CertexStep = 2.0f;
        public const float VerticesInTerrainQuad = 50;
        
    }
}

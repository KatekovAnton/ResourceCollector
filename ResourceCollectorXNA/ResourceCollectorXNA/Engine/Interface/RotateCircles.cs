using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ResourceCollectorXNA.Engine.Interface
{
    public class RotateCircles
    {
        public static VertexPositionColor[] XoYvertices;
        public static VertexPositionColor[] YoZvertices;
        public static VertexPositionColor[] XoZvertices;
        public static short[] indices = {
0, 1, 2, 2, 3, 4, 4, 5, 6, 2, 4, 6, 6, 7, 8, 8, 9, 10, 6, 8, 10, 10, 11, 12, 12, 13, 14, 10, 12, 14, 6, 10, 14, 2, 6, 14, 0, 2, 14, 15, 0, 14};


        public static VertexPositionColor[] DrawingPointsXY;
        public static VertexPositionColor[] DrawingPointsXZ;
        public static VertexPositionColor[] DrawingPointsYZ;
        public static short[] DrawingIndices;
        /// <summary>
        /// DO NOT SET DIRECTLY!!!!! USE SetTransformMatrix()
        /// </summary>
        public Matrix transform;
        private Matrix arrowscale;
        private float k = 9.0f;
        public float multiplier = 1.0f;
        private float lastpmultiplier = 1.0f;
        public float visibleArrowsSize = 2.0f;

        public RotateCircles()
        {
            transform = Matrix.Identity;
            arrowscale = Matrix.Identity;
            #region createpasepoints
            if (XoYvertices == null)
            {
                XoYvertices = new VertexPositionColor[]{
                    new VertexPositionColor(new Vector3(-7.07107f,-7.071065f,-1.067702E-06f), Color.White),
                    new VertexPositionColor(new Vector3(-3.826836f,-9.238794f,-5.778362E-07f), Color.White),
                    new VertexPositionColor(new Vector3(-1.192488E-07f,-10f,-1.800607E-14f), Color.White),
                    new VertexPositionColor(new Vector3(3.826836f,-9.238795f,5.778361E-07f), Color.White),
                    new VertexPositionColor(new Vector3(7.07107f,-7.071066f,1.067702E-06f), Color.White),
                    new VertexPositionColor(new Vector3(9.238796f,-3.826832f,1.395019E-06f), Color.White),
                    new VertexPositionColor(new Vector3(10f,1.509958E-06f,1.509958E-06f), Color.White),
                    new VertexPositionColor(new Vector3(9.238795f,3.826835f,1.395019E-06f), Color.White),
                    new VertexPositionColor(new Vector3(7.071068f,7.071068f,1.067702E-06f), Color.White),
                    new VertexPositionColor(new Vector3(3.826835f,9.238795f,5.77836E-07f), Color.White),
                    new VertexPositionColor(new Vector3(4.371139E-07f,10f,6.600237E-14f), Color.White),
                    new VertexPositionColor(new Vector3(-3.826834f,9.238795f,-5.778359E-07f), Color.White),
                    new VertexPositionColor(new Vector3(-7.071068f,7.071068f,-1.067702E-06f), Color.White),
                    new VertexPositionColor(new Vector3(-9.238795f,3.826835f,-1.395019E-06f), Color.White),
                    new VertexPositionColor(new Vector3(-10f,0f,-1.509958E-06f), Color.White),
                    new VertexPositionColor(new Vector3(-9.238797f,-3.82683f,-1.39502E-06f), Color.White)
                    };
                XoZvertices = new VertexPositionColor[]{
                    new VertexPositionColor(new Vector3(7.071067f,3.090861E-07f,-7.071069f), Color.White),
                    new VertexPositionColor(new Vector3(9.238795f,4.038405E-07f,-3.826834f), Color.White),
                    new VertexPositionColor(new Vector3(10f,4.371139E-07f,1.827823E-06f), Color.White),
                    new VertexPositionColor(new Vector3(9.238794f,4.038406E-07f,3.826838f), Color.White),
                    new VertexPositionColor(new Vector3(7.071064f,3.090861E-07f,7.071072f), Color.White),
                    new VertexPositionColor(new Vector3(3.82683f,1.672762E-07f,9.238797f), Color.White),
                    new VertexPositionColor(new Vector3(-3.45703E-06f,-6.600236E-14f,10f), Color.White),
                    new VertexPositionColor(new Vector3(-3.826837f,-1.672763E-07f,9.238794f), Color.White),
                    new VertexPositionColor(new Vector3(-7.071069f,-3.090862E-07f,7.071066f), Color.White),
                    new VertexPositionColor(new Vector3(-9.238796f,-4.038406E-07f,3.826833f), Color.White),
                    new VertexPositionColor(new Vector3(-10f,-4.371139E-07f,-1.509958E-06f), Color.White),
                    new VertexPositionColor(new Vector3(-9.238794f,-4.038406E-07f,-3.826836f), Color.White),
                    new VertexPositionColor(new Vector3(-7.071066f,-3.090862E-07f,-7.071069f), Color.White),
                    new VertexPositionColor(new Vector3(-3.826833f,-1.672763E-07f,-9.238796f), Color.White),
                    new VertexPositionColor(new Vector3(1.947072E-06f,0f,-10f), Color.White),
                    new VertexPositionColor(new Vector3(3.826832f,1.67276E-07f,-9.238796f), Color.White)
                    };
                YoZvertices = new VertexPositionColor[]{
                    new VertexPositionColor(new Vector3(1.376788E-06f,-7.071065f,-7.07107f), Color.White),
                    new VertexPositionColor(new Vector3(7.451125E-07f,-9.238794f,-3.826836f), Color.White),
                    new VertexPositionColor(new Vector3(2.32186E-14f,-10f,-1.192488E-07f), Color.White),
                    new VertexPositionColor(new Vector3(-7.451124E-07f,-9.238795f,3.826836f), Color.White),
                    new VertexPositionColor(new Vector3(-1.376788E-06f,-7.071066f,7.07107f), Color.White),
                    new VertexPositionColor(new Vector3(-1.79886E-06f,-3.826832f,9.238796f), Color.White),
                    new VertexPositionColor(new Vector3(-1.947072E-06f,1.509958E-06f,10f), Color.White),
                    new VertexPositionColor(new Vector3(-1.79886E-06f,3.826835f,9.238795f), Color.White),
                    new VertexPositionColor(new Vector3(-1.376788E-06f,7.071068f,7.071068f), Color.White),
                    new VertexPositionColor(new Vector3(-7.451123E-07f,9.238795f,3.826835f), Color.White),
                    new VertexPositionColor(new Vector3(-8.510921E-14f,10f,4.371139E-07f), Color.White),
                    new VertexPositionColor(new Vector3(7.451121E-07f,9.238795f,-3.826834f), Color.White),
                    new VertexPositionColor(new Vector3(1.376788E-06f,7.071068f,-7.071068f), Color.White),
                    new VertexPositionColor(new Vector3(1.79886E-06f,3.826835f,-9.238795f), Color.White),
                    new VertexPositionColor(new Vector3(1.947072E-06f,0f,-10f), Color.White),
                    new VertexPositionColor(new Vector3(1.79886E-06f,-3.82683f,-9.238797f), Color.White),
                    };
                float step = MathHelper.Pi / 10;
                int stapcount = (int)Math.Floor((double)MathHelper.Pi * 2.0f / step) ;
                DrawingPointsXY = new VertexPositionColor[stapcount];
                DrawingPointsXZ = new VertexPositionColor[stapcount];
                DrawingPointsYZ = new VertexPositionColor[stapcount];
                float x, y;
                float s = 0;
                for (int i = 0; i <stapcount; i++)
                {
                    x = k * (float)Math.Sin((double)s);
                    y = k * (float)Math.Cos((double)s);
                    DrawingPointsXY[i].Position.X = x;
                    DrawingPointsXY[i].Position.Y = y;
                    DrawingPointsXY[i].Color = InterfaceColors.ZAxisColor;

                    DrawingPointsXZ[i].Position.X = x;
                    DrawingPointsXZ[i].Position.Z = y;
                    DrawingPointsXZ[i].Color = InterfaceColors.YAxisColor;

                    DrawingPointsYZ[i].Position.Y = x;
                    DrawingPointsYZ[i].Position.Z = y;
                    DrawingPointsYZ[i].Color = InterfaceColors.XAxisColor;

                    s += step;
                }
                DrawingIndices = new short[stapcount * 2];
                for (int i = 0; i < DrawingIndices.Length; i += 2)
                {
                    DrawingIndices[i] = Convert.ToInt16( i / 2);
                    DrawingIndices[i + 1] = Convert.ToInt16((i+2) / 2) ;
                }
                DrawingIndices[DrawingIndices.Length - 1] = 0;
            }
            #endregion
        }
        public void CreateAxises()
        {
            float range = (transform.Translation -  GameEngine.Instance.Camera.View.Translation).Length() / 300.0f;
            multiplier = (float)Math.Atan(range);
            multiplier = multiplier * visibleArrowsSize;
        }
        public void UpdateData()
        {
            CreateAxises();
        }
        public void SetTransformMatrix(Matrix m)
        {
            transform = m;
            UpdateData();
        }
        public Axis SelectingAxis(Ray ray)
        {
            Axis resultaxis = Axis.none;
            if (lastpmultiplier != multiplier)
            {
                arrowscale = Matrix.CreateScale(multiplier);
                lastpmultiplier = multiplier;
            }
            Matrix trans = arrowscale * transform;


            float xrange = TransformManager.isintersect(ray, XoYvertices, indices, trans);
            float minrange = -1000;
            if (xrange != -1000)
            {
                minrange = xrange;
                resultaxis = Axis.Z;
            }
            float yrange = TransformManager.isintersect(ray, YoZvertices, indices, trans);
            if (yrange != -1000)
            {
                if (minrange == -1000)
                {
                    minrange = yrange;
                    resultaxis = Axis.X;
                }
                else if (yrange < minrange)
                {
                    minrange = yrange;
                    resultaxis = Axis.X;
                }
            }
            float zrange = TransformManager.isintersect(ray, XoZvertices, indices, trans);
            if (zrange != -1000)
            {
                if (minrange == -1000)
                {
                    minrange = zrange;
                    resultaxis = Axis.Y;
                }
                else if (zrange < minrange)
                {
                    minrange = zrange;
                    resultaxis = Axis.Y;
                }
            }
           

            return resultaxis;
        }
        private void SetColors()
        {
            for (int i = 0; i < DrawingPointsXY.Length; i++)
            {
                DrawingPointsXY[i].Color = InterfaceColors.ZAxisColor;

                DrawingPointsXZ[i].Color = InterfaceColors.YAxisColor;

                DrawingPointsYZ[i].Color = InterfaceColors.XAxisColor;
            }
        }
        public void SetColors(Axis activeaxis)
        {
            SetColors();
            switch (activeaxis)
            {
                case Axis.X:
                    for (int i = 0; i < DrawingPointsXY.Length; i++)
                    {
                        DrawingPointsYZ[i].Color = InterfaceColors.ActiveAxisColor;
                    }
                    break;
                case Axis.Y:
                    for (int i = 0; i < DrawingPointsXY.Length; i++)
                    {
                        DrawingPointsXZ[i].Color = InterfaceColors.ActiveAxisColor;
                    }
                    break;
                case Axis.Z:
                    for (int i = 0; i < DrawingPointsXY.Length; i++)
                    {
                        DrawingPointsXY[i].Color = InterfaceColors.ActiveAxisColor;
                    }
                    break;
            }
        }
        public Axis UpdateData(Ray ray)
        {
            CreateAxises();
            Axis selectedaxis = SelectingAxis(ray);
            SetColors(selectedaxis);
            return selectedaxis;
        }
        public void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch sprite)
        {
            RasterizerState rs = new RasterizerState();
            rs.CullMode = CullMode.None;
            rs.FillMode = FillMode.WireFrame;
            RasterizerState back = GameEngine.Device.RasterizerState;
            DepthStencilState dss = GameEngine.Device.DepthStencilState;
            DepthStencilState newdss = new DepthStencilState();
            newdss.DepthBufferEnable = false;
            GameEngine.Device.DepthStencilState = newdss;
            GameEngine.Device.RasterizerState = rs;
            GameEngine.Device.DrawUserIndexedPrimitives<VertexPositionColor>(
                     PrimitiveType.LineList, DrawingPointsXY, 0, DrawingPointsXY.Length, DrawingIndices, 0, DrawingIndices.Length / 2);
            GameEngine.Device.DrawUserIndexedPrimitives<VertexPositionColor>(
                     PrimitiveType.LineList, DrawingPointsXZ, 0, DrawingPointsXZ.Length, DrawingIndices, 0, DrawingIndices.Length / 2);
            GameEngine.Device.DrawUserIndexedPrimitives<VertexPositionColor>(
                     PrimitiveType.LineList, DrawingPointsYZ, 0, DrawingPointsYZ.Length, DrawingIndices, 0, DrawingIndices.Length / 2);
          /*  sprite.DrawString(Engine.Font1, " X", XEndPos, InterfaceColors.XAxisColor);
            sprite.DrawString(Engine.Font1, " Y", YEndPos, InterfaceColors.YAxisColor);
            sprite.DrawString(Engine.Font1, " Z", ZEndPos, InterfaceColors.ZAxisColor);*/
           /* if (needdrawarrowmeshes)
            {
                GameEngine.Device.DrawUserIndexedPrimitives<VertexPositionColor>(
                     PrimitiveType.TriangleList, Xvertices, 0, Xvertices.Length, AxisIndices, 0, AxisIndices.Length / 3);
                GameEngine.Device.DrawUserIndexedPrimitives<VertexPositionColor>(
                     PrimitiveType.TriangleList, Yvertices, 0, Yvertices.Length, AxisIndices, 0, AxisIndices.Length / 3);
                GameEngine.Device.DrawUserIndexedPrimitives<VertexPositionColor>(
                     PrimitiveType.TriangleList, Zvertices, 0, Zvertices.Length, AxisIndices, 0, AxisIndices.Length / 3);

            }*/
            GameEngine.Device.RasterizerState = back;
            GameEngine.Device.DepthStencilState = dss;
        }
    }
}

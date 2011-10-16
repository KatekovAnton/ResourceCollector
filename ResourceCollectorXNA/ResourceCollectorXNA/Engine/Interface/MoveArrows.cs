using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;




namespace ResourceCollectorXNA.Engine.Interface{
    public enum Axis{
        X,
        Y,
        Z,
        XoY,
        YoZ,
        XoZ,
        none
    };




    public sealed class InterfaceColors{
        public static Color XAxisColor = Color.Red;
        public static Color YAxisColor = Color.DarkGreen;
        public static Color ZAxisColor = Color.Blue;

        public static Color XAxisActiveColor = Color.OrangeRed;
        public static Color YAxisActiveColor = Color.LimeGreen;
        public static Color ZAxisActiveColor = Color.LightBlue;

        public static Color ActiveAxisColor = Color.White;
        public static Color TextColor = Color.Black;
        public InterfaceColors() { }
    }




    public class MoveArrows{
        public static bool needdrawarrowmeshes;

        public static VertexPositionColor[] Xvertices;
        public static VertexPositionColor[] Yvertices;
        public static VertexPositionColor[] Zvertices;

        public static VertexPositionColor[] XoYvertices;
        public static VertexPositionColor[] YoZvertices;
        public static VertexPositionColor[] XoZvertices;


        public static VertexPositionColor[] XoYPlaneVertices;
        public static VertexPositionColor[] YoZPlaneVertices;
        public static VertexPositionColor[] XoZPlaneVertices;

        public static short[] AxisIndices = {0, 1, 2, 2, 3, 0, 4, 5, 6, 6, 7, 4, 8, 9, 10, 10, 11, 8, 12, 13, 14, 14, 15, 12};
        public static short[] PlaneIndices = {0, 1, 2, 2, 3, 0};

        public static VertexPositionColor[] MoveArrowPoints;
        public static short[] MoveArrowsIndices;

        private Vector2 XEndPos;
        private Vector2 YEndPos;
        private Vector2 ZEndPos;
        private Matrix arrowscale;


        private const float k = 10.0f;
        private const float km = 3.5f;
        private float lastpmultiplier = 1.0f;
        public float multiplier = 1.0f;
        private bool prime = true;

        /// <summary>
        /// DO NOT SET DIRECTLY!!!!! USE SetTransformMatrix()
        /// </summary>
        public Matrix transform;

        public float visibleArrowsSize = 1.0f;

        //  public float _MOVEMULTIPLYER;


        public MoveArrows (bool primary = true) {
            prime = primary;
            transform = Matrix.Identity;
            arrowscale = Matrix.Identity;

            #region createpasepoints

            if (Xvertices == null) {
                Xvertices = new[]{new VertexPositionColor (new Vector3 (-0.5000001f, -0.4999999f, -0.5f), Color.White), new VertexPositionColor (new Vector3 (0.4862234f, 0.5059329f, -0.5f), Color.White), new VertexPositionColor (new Vector3 (10f, 0.5f, -0.5f), Color.White), new VertexPositionColor (new Vector3 (10f, -0.5000001f, -0.5f), Color.White), new VertexPositionColor (new Vector3 (10f, 0.5f, -0.5f), Color.White), new VertexPositionColor (new Vector3 (0.4862234f, 0.5059329f, -0.5f), Color.White), new VertexPositionColor (new Vector3 (0.5000001f, 0.5f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (10f, 0.5f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (10f, 0.5f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (0.5000001f, 0.5f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (0.5f, -0.4999999f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (10f, -0.5000001f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (10f, -0.5000001f, -0.5f), Color.White), new VertexPositionColor (new Vector3 (10f, -0.5000001f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (0.5f, -0.4999999f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (-0.5000001f, -0.4999999f, -0.5f), Color.White)};
                Yvertices = new[]{new VertexPositionColor (new Vector3 (-0.5f, -0.5f, -0.5f), Color.White), new VertexPositionColor (new Vector3 (-0.5f, 0.5f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (-0.5f, 10f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (-0.5f, 10f, -0.5f), Color.White), new VertexPositionColor (new Vector3 (-0.5f, 10f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (-0.5f, 0.5f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (0.5f, 0.5f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (0.5f, 10f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (0.5f, 10f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (0.5f, 0.5f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (0.5f, 0.5f, -0.5f), Color.White), new VertexPositionColor (new Vector3 (0.5f, 10f, -0.5f), Color.White), new VertexPositionColor (new Vector3 (-0.5f, 10f, -0.5f), Color.White), new VertexPositionColor (new Vector3 (0.5f, 10f, -0.5f), Color.White), new VertexPositionColor (new Vector3 (0.5f, 0.5f, -0.5f), Color.White), new VertexPositionColor (new Vector3 (-0.5f, -0.5f, -0.5f), Color.White)};
                Zvertices = new[]{new VertexPositionColor (new Vector3 (-0.5f, -0.5f, -0.5f), Color.White), new VertexPositionColor (new Vector3 (0.5f, -0.5f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (0.5f, -0.5f, 10f), Color.White), new VertexPositionColor (new Vector3 (-0.5f, -0.5f, 10f), Color.White), new VertexPositionColor (new Vector3 (0.5f, -0.5f, 10f), Color.White), new VertexPositionColor (new Vector3 (0.5f, -0.5f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (0.5f, 0.5f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (0.5f, 0.5f, 10f), Color.White), new VertexPositionColor (new Vector3 (0.5f, 0.5f, 10f), Color.White), new VertexPositionColor (new Vector3 (0.5f, 0.5f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (-0.5f, 0.5f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (-0.5f, 0.5f, 10f), Color.White), new VertexPositionColor (new Vector3 (-0.5f, -0.5f, 10f), Color.White), new VertexPositionColor (new Vector3 (-0.5f, 0.5f, 10f), Color.White), new VertexPositionColor (new Vector3 (-0.5f, 0.5f, 0.5f), Color.White), new VertexPositionColor (new Vector3 (-0.5f, -0.5f, -0.5f), Color.White)};

                XoYvertices = new[]{new VertexPositionColor (new Vector3 (0f, 0f, 0f), Color.White), new VertexPositionColor (new Vector3 (0f, km, 0f), Color.White), new VertexPositionColor (new Vector3 (km, km, 0f), Color.White), new VertexPositionColor (new Vector3 (km, 0f, 0f), Color.White)};
                XoZvertices = new[]{new VertexPositionColor (new Vector3 (0f, 0f, 0f), Color.White), new VertexPositionColor (new Vector3 (km, 0f, 0f), Color.White), new VertexPositionColor (new Vector3 (km, 0f, km), Color.White), new VertexPositionColor (new Vector3 (0f, 0f, km), Color.White)};
                YoZvertices = new[]{new VertexPositionColor (new Vector3 (0f, km, 0f), Color.White), new VertexPositionColor (new Vector3 (0f, 0f, 0f), Color.White), new VertexPositionColor (new Vector3 (0f, 0f, km), Color.White), new VertexPositionColor (new Vector3 (0f, km, km), Color.White)};

                XoYPlaneVertices = new[]{new VertexPositionColor (new Vector3 (-k * 10, -k * 10, 0f), Color.White), new VertexPositionColor (new Vector3 (-k * 10, k * 10, 0f), Color.White), new VertexPositionColor (new Vector3 (k * 10, k * 10, 0f), Color.White), new VertexPositionColor (new Vector3 (k * 10, -k * 10, 0f), Color.White)};
                XoZPlaneVertices = new[]{new VertexPositionColor (new Vector3 (-k * 10, 0f, -k * 10), Color.White), new VertexPositionColor (new Vector3 (k * 10, 0f, -k * 10), Color.White), new VertexPositionColor (new Vector3 (k * 10, 0f, k * 10), Color.White), new VertexPositionColor (new Vector3 (-k * 10, 0f, k * 10), Color.White)};
                YoZPlaneVertices = new[]{new VertexPositionColor (new Vector3 (0f, k * 10, -k * 10), Color.White), new VertexPositionColor (new Vector3 (0f, -k * 10, -k * 10), Color.White), new VertexPositionColor (new Vector3 (0f, -k * 10, k * 10), Color.White), new VertexPositionColor (new Vector3 (0f, k * 10, k * 10), Color.White)};

                MoveArrowPoints = new[]{new VertexPositionColor (new Vector3 (), InterfaceColors.XAxisColor), //0x //0
                                        new VertexPositionColor (new Vector3 (), InterfaceColors.YAxisColor), //0y //1
                                        new VertexPositionColor (new Vector3 (), InterfaceColors.ZAxisColor), //0z //2

                                        new VertexPositionColor (Vector3.UnitX, InterfaceColors.XAxisColor), //1x //3
                                        new VertexPositionColor (Vector3.UnitY, InterfaceColors.YAxisColor), //1y //4
                                        new VertexPositionColor (Vector3.UnitZ, InterfaceColors.ZAxisColor), //1z //5

                                        new VertexPositionColor (Vector3.UnitX * km / k, InterfaceColors.XAxisColor), //mxy1                  //6
                                        new VertexPositionColor ((Vector3.UnitX + Vector3.UnitY) * km / k, InterfaceColors.XAxisColor), //mxy2  //7
                                        new VertexPositionColor (Vector3.UnitY * km / k, InterfaceColors.YAxisColor), //myx1                  //8
                                        new VertexPositionColor ((Vector3.UnitX + Vector3.UnitY) * km / k, InterfaceColors.YAxisColor), //myx2  //9

                                        new VertexPositionColor (Vector3.UnitX * km / k, InterfaceColors.XAxisColor), //mxy1                  //10
                                        new VertexPositionColor ((Vector3.UnitX + Vector3.UnitZ) * km / k, InterfaceColors.XAxisColor), //mxy2  //11
                                        new VertexPositionColor (Vector3.UnitZ * km / k, InterfaceColors.ZAxisColor), //myx1                  //12
                                        new VertexPositionColor ((Vector3.UnitX + Vector3.UnitZ) * km / k, InterfaceColors.ZAxisColor), //myx2  //13

                                        new VertexPositionColor (Vector3.UnitY * km / k, InterfaceColors.YAxisColor), //mxy1                  //14
                                        new VertexPositionColor ((Vector3.UnitY + Vector3.UnitZ) * km / k, InterfaceColors.YAxisColor), //mxy2  //15
                                        new VertexPositionColor (Vector3.UnitZ * km / k, InterfaceColors.ZAxisColor), //myx1                  //16
                                        new VertexPositionColor ((Vector3.UnitY + Vector3.UnitZ) * km / k, InterfaceColors.ZAxisColor) //myx2  //17
                                       };

                MoveArrowsIndices = new short[]{0, 3, 1, 4, 2, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17};
            }

            #endregion

            SetColors ();
            //Xline.CreateLine(new Vector2(100, 100), new Vector2(200, 200));
        }


        public void SetColors () {
            MoveArrowPoints[0].Color = InterfaceColors.XAxisColor; //0x //0
            MoveArrowPoints[1].Color = InterfaceColors.YAxisColor; //0y //1
            MoveArrowPoints[2].Color = InterfaceColors.ZAxisColor; //0z //2

            MoveArrowPoints[3].Color = InterfaceColors.XAxisColor; //1x //3
            MoveArrowPoints[4].Color = InterfaceColors.YAxisColor; //1y //4
            MoveArrowPoints[5].Color = InterfaceColors.ZAxisColor; //1z  //5

            MoveArrowPoints[6].Color = InterfaceColors.XAxisColor; //mxy1                  //6
            MoveArrowPoints[7].Color = InterfaceColors.XAxisColor; //mxy2  //7
            MoveArrowPoints[8].Color = InterfaceColors.YAxisColor; //myx1                  //8
            MoveArrowPoints[9].Color = InterfaceColors.YAxisColor; //myx2  //9

            MoveArrowPoints[10].Color = InterfaceColors.XAxisColor; //mxy1                  //10
            MoveArrowPoints[11].Color = InterfaceColors.XAxisColor; //mxy2  //11
            MoveArrowPoints[12].Color = InterfaceColors.ZAxisColor; //myx1                  //12
            MoveArrowPoints[13].Color = InterfaceColors.ZAxisColor; //myx2  //13

            MoveArrowPoints[14].Color = InterfaceColors.YAxisColor; //mxy1                  //14
            MoveArrowPoints[15].Color = InterfaceColors.YAxisColor; //mxy2  //15
            MoveArrowPoints[16].Color = InterfaceColors.ZAxisColor; //myx1                  //16
            MoveArrowPoints[17].Color = InterfaceColors.ZAxisColor; //myx2  //17
        }


        public void SetColors (Axis activeaxis) {
            SetColors ();
            switch (activeaxis) {
                case Axis.X:
                    MoveArrowPoints[0].Color = //0x //0
                        MoveArrowPoints[3].Color = InterfaceColors.ActiveAxisColor; //1x //3
                    break;
                case Axis.Y:
                    MoveArrowPoints[1].Color = //0x //0
                        MoveArrowPoints[4].Color = InterfaceColors.ActiveAxisColor; //1x //3
                    break;
                case Axis.Z:
                    MoveArrowPoints[2].Color = //0x //0
                        MoveArrowPoints[5].Color = InterfaceColors.ActiveAxisColor; //1x //3
                    break;
                case Axis.XoZ:
                    MoveArrowPoints[0].Color = //0x //0
                        MoveArrowPoints[3].Color = //1x //3
                        MoveArrowPoints[2].Color = //0x //0
                        MoveArrowPoints[5].Color = //0x //0
                        MoveArrowPoints[10].Color = //1x //3
                        MoveArrowPoints[11].Color = //0x //0
                        MoveArrowPoints[12].Color = //1x //3
                        MoveArrowPoints[13].Color = InterfaceColors.ActiveAxisColor; //1x //3
                    break;
                case Axis.YoZ:
                    MoveArrowPoints[2].Color = //0x //0
                        MoveArrowPoints[5].Color = //1x //3
                        MoveArrowPoints[1].Color = //0x //0
                        MoveArrowPoints[14].Color = //0x //0
                        MoveArrowPoints[15].Color = //1x //3
                        MoveArrowPoints[16].Color = //0x //0
                        MoveArrowPoints[17].Color = //0x //0
                        MoveArrowPoints[4].Color = InterfaceColors.ActiveAxisColor; //1x //3
                    break;
                case Axis.XoY:
                    MoveArrowPoints[0].Color = //0x //0
                        MoveArrowPoints[3].Color = //1x //3
                        MoveArrowPoints[1].Color = //0x //0
                        MoveArrowPoints[4].Color = //0x //0
                        MoveArrowPoints[6].Color = //1x //3
                        MoveArrowPoints[7].Color = //0x //0
                        MoveArrowPoints[8].Color = //1x //3
                        MoveArrowPoints[9].Color = InterfaceColors.ActiveAxisColor; //1x //3
                    break;
            }
        }


        public static Vector3? isintersectVector (Ray ray, VertexPositionColor[] axisvertices, short[] indices, Matrix transform) {
            Matrix detransform = Matrix.Invert (transform);

            Vector3 p1 = Vector3.Transform (ray.Position, detransform);
            Vector3 p2 = Vector3.Transform (ray.Direction + ray.Position, detransform);
            p2 -= p1;

            bool isIntersected = false;
            float distance = 0.0f;
            for (int i = 0; i < indices.Length; i += 3) {
                Vector3 v0 = axisvertices[indices[i]].Position;
                Vector3 v1 = axisvertices[indices[i + 1]].Position - axisvertices[indices[i]].Position;
                Vector3 v2 = axisvertices[indices[i + 2]].Position - axisvertices[indices[i]].Position;

                // solution of linear system
                // finds line and plane intersection point (if exists)
                float determinant = -p2.Z * v1.Y * v2.X + p2.Y * v1.Z * v2.X + p2.Z * v1.X * v2.Y - p2.X * v1.Z * v2.Y - p2.Y * v1.X * v2.Z + p2.X * v1.Y * v2.Z;

                if (determinant * determinant < 0.000000001f)
                    continue;

                float kramer = 1.0f / determinant;

                float t1 = (p1.Z * p2.Y * v2.X - p1.Y * p2.Z * v2.X + p2.Z * v0.Y * v2.X - p2.Y * v0.Z * v2.X - p1.Z * p2.X * v2.Y + p1.X * p2.Z * v2.Y - p2.Z * v0.X * v2.Y + p2.X * v0.Z * v2.Y + p1.Y * p2.X * v2.Z - p1.X * p2.Y * v2.Z + p2.Y * v0.X * v2.Z - p2.X * v0.Y * v2.Z) * kramer;

                if (t1 < 0)
                    continue;

                float t2 = -(p1.Z * p2.Y * v1.X - p1.Y * p2.Z * v1.X + p2.Z * v0.Y * v1.X - p2.Y * v0.Z * v1.X - p1.Z * p2.X * v1.Y + p1.X * p2.Z * v1.Y - p2.Z * v0.X * v1.Y + p2.X * v0.Z * v1.Y + p1.Y * p2.X * v1.Z - p1.X * p2.Y * v1.Z + p2.Y * v0.X * v1.Z - p2.X * v0.Y * v1.Z) * kramer;

                if (t2 < 0)
                    continue;

                float t3 = (-p1.Z * v1.Y * v2.X + v0.Z * v1.Y * v2.X + p1.Y * v1.Z * v2.X - v0.Y * v1.Z * v2.X + p1.Z * v1.X * v2.Y - v0.Z * v1.X * v2.Y - p1.X * v1.Z * v2.Y + v0.X * v1.Z * v2.Y - p1.Y * v1.X * v2.Z + v0.Y * v1.X * v2.Z + p1.X * v1.Y * v2.Z - v0.X * v1.Y * v2.Z) * (-kramer);

                if (t3 < 0)
                    continue;

                // (t1>=0 && t2>=0 && t1+t2<=0.5)  => point is on face
                // (t3>0)  =>  point is on positive ray direction
                if (t1 + t2 > 1.0f)
                    continue;

                if (!isIntersected || distance > t3) {
                    isIntersected = true;
                    distance = t3;
                    break;
                }
            }
            if (isIntersected)
                return Vector3.Transform ((p1 + p2 * distance), transform);
            return null;
        }


        public void CreateAxises () {
            float range = (transform.Translation - GameEngine.Instance.Camera.View.Translation).Length () / 100.0f;
            //ConsoleWindow.TraceMessage(range.ToString());
            multiplier = (float) Math.Atan (range);
            multiplier = multiplier * visibleArrowsSize;


            XEndPos = GameEngine.Device.Viewport.Project (Vector3.UnitX * (k * multiplier), GameEngine.Instance.Camera.Projection, GameEngine.Instance.Camera.View, transform).GetVector2 ();
            YEndPos = GameEngine.Device.Viewport.Project (Vector3.UnitY * (k * multiplier), GameEngine.Instance.Camera.Projection, GameEngine.Instance.Camera.View, transform).GetVector2 ();
            ZEndPos = GameEngine.Device.Viewport.Project (Vector3.UnitZ * (k * multiplier), GameEngine.Instance.Camera.Projection, GameEngine.Instance.Camera.View, transform).GetVector2 ();
        }


        public Axis SelectingAxis (Ray ray) {
            Axis resultaxis = Axis.none;
            if (lastpmultiplier != multiplier) {
                arrowscale = Matrix.CreateScale (multiplier);
                lastpmultiplier = multiplier;
            }
            Matrix trans = arrowscale * transform;


            float xrange = TransformManager.isintersect (ray, Xvertices, AxisIndices, trans);
            float minrange = -1000;
            if (xrange != -1000) {
                minrange = xrange;
                resultaxis = Axis.X;
            }
            float yrange = TransformManager.isintersect (ray, Yvertices, AxisIndices, trans);
            if (yrange != -1000) {
                if (minrange == -1000) {
                    minrange = yrange;
                    resultaxis = Axis.Y;
                } else if (yrange < minrange) {
                    minrange = yrange;
                    resultaxis = Axis.Y;
                }
            }
            float zrange = TransformManager.isintersect (ray, Zvertices, AxisIndices, trans);
            if (zrange != -1000) {
                if (minrange == -1000) {
                    minrange = zrange;
                    resultaxis = Axis.Z;
                } else if (zrange < minrange) {
                    minrange = zrange;
                    resultaxis = Axis.Z;
                }
            }
            float xozrange = TransformManager.isintersect (ray, XoZvertices, PlaneIndices, trans);
            if (xozrange != -1000) {
                if (minrange == -1000) {
                    minrange = xozrange;
                    resultaxis = Axis.XoZ;
                } else if (xozrange < minrange) {
                    minrange = xozrange;
                    resultaxis = Axis.XoZ;
                }
            }
            float xoyrange = TransformManager.isintersect (ray, XoYvertices, PlaneIndices, trans);
            if (xoyrange != -1000) {
                if (minrange == -1000) {
                    minrange = xoyrange;
                    resultaxis = Axis.XoY;
                } else if (xozrange < minrange) {
                    minrange = xoyrange;
                    resultaxis = Axis.XoY;
                }
            }
            float yozrange = TransformManager.isintersect (ray, YoZvertices, PlaneIndices, trans);
            if (yozrange != -1000) {
                if (minrange == -1000) {
                    minrange = yozrange;
                    resultaxis = Axis.YoZ;
                } else if (yozrange < minrange) {
                    minrange = yozrange;
                    resultaxis = Axis.YoZ;
                }
            }

            return resultaxis;
        }


        public void SetTransformMatrix (Matrix m) {
            transform = m;
            UpdateData ();
        }


        public Axis UpdateData (Ray ray) {
            CreateAxises ();
            Axis selectedaxis = SelectingAxis (ray);
            SetColors (selectedaxis);
            return selectedaxis;
        }


        public void UpdateData () {
            CreateAxises ();
        }


        public void DrawPivot (SpriteBatch sprite) {
            var rs = new RasterizerState ();
            rs.CullMode = CullMode.None;
            rs.FillMode = FillMode.WireFrame;
            RasterizerState back = GameEngine.Device.RasterizerState;
            DepthStencilState dss = GameEngine.Device.DepthStencilState;
            var newdss = new DepthStencilState ();
            newdss.DepthBufferEnable = false;
            GameEngine.Device.DepthStencilState = newdss;
            GameEngine.Device.RasterizerState = rs;
            GameEngine.Device.DrawUserIndexedPrimitives (PrimitiveType.LineList, MoveArrowPoints, 0, MoveArrowPoints.Length, MoveArrowsIndices, 0, MoveArrowsIndices.Length / 2);
            sprite.DrawString (GameEngine.Instance.Font1, " X", XEndPos, InterfaceColors.XAxisColor);
            sprite.DrawString (GameEngine.Instance.Font1, " Y", YEndPos, InterfaceColors.YAxisColor);
            sprite.DrawString (GameEngine.Instance.Font1, " Z", ZEndPos, InterfaceColors.ZAxisColor);
            if (needdrawarrowmeshes) {
                GameEngine.Device.DrawUserIndexedPrimitives (PrimitiveType.TriangleList, Xvertices, 0, Xvertices.Length, AxisIndices, 0, AxisIndices.Length / 3);
                GameEngine.Device.DrawUserIndexedPrimitives (PrimitiveType.TriangleList, Yvertices, 0, Yvertices.Length, AxisIndices, 0, AxisIndices.Length / 3);
                GameEngine.Device.DrawUserIndexedPrimitives (PrimitiveType.TriangleList, Zvertices, 0, Zvertices.Length, AxisIndices, 0, AxisIndices.Length / 3);
            }
            GameEngine.Device.RasterizerState = back;
            GameEngine.Device.DepthStencilState = dss;
        }
    }
}
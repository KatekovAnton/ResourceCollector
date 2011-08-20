using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace ResourceCollectorXNA.Engine.Render.Render2D
{
    //////////////////////////////////////////////////////////////////////////
    // Class    :   BasicPrimitives
    //
    // Purpose  :   Render simple 2D shapes.
    //////////////////////////////////////////////////////////////////////////
    public class BasicPrimitives
    {
        /*********************************************************************/
        // Members.
        /*********************************************************************/

        #region Enumerations

        /// <summary>Spline Interpolations.</summary>
        private enum Spline
        {
            Linear,
            Cosine,
            Cubic,
            Hermite
        }

        #endregion // Enumerations

        #region Fields

        /// <summary>The color of the primitive object.</summary>
        private Color m_Color = Color.White;

        /// <summary>The position of the primitive object.</summary>
        private Vector2 m_vPosition = Vector2.Zero;

        /// <summary>The render depth of the primitive line object (0 = front, 1 = back).</summary>
        private float m_fDepth = 0f;

        /// <summary>The thickness of the shape's edge.</summary>
        private float m_fThickness = 1f;

        /// <summary>1x1 pixel that creates the shape.</summary>
        private Texture2D m_Pixel = null;

        /// <summary>List of vectors.</summary>
        private List<Vector2> m_VectorList = new List<Vector2>();

        #endregion // Fields

        #region Properties

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get/Set the colour of the primitive object.
        /// </summary>
        //////////////////////////////////////////////////////////////////////////
        public Color Colour
        {
            get { return m_Color; }
            set { m_Color = value; }
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get/Set the position of the primitive object.
        /// </summary>
        //////////////////////////////////////////////////////////////////////////
        public Vector2 Position
        {
            get { return m_vPosition; }
            set { m_vPosition = value; }
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get/Set the render depth of the primitive line object (0 = front, 1 = back).
        /// </summary>
        //////////////////////////////////////////////////////////////////////////
        public float Depth
        {
            get { return m_fDepth; }
            set { m_fDepth = value; }
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get/Set the thickness of the shape's edge.
        /// </summary>
        //////////////////////////////////////////////////////////////////////////
        public float Thickness
        {
            get { return m_fThickness; }
            set { m_fThickness = value; }
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the number of vectors which make up the primitive object.
        /// </summary>
        //////////////////////////////////////////////////////////////////////////
        public int CountVectors
        {
            get { return m_VectorList.Count; }
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Gets the vector position from the list.
        /// </summary>
        /// <param name="_nIndex">The index to get from.</param>
        //////////////////////////////////////////////////////////////////////////
        public Vector2 GetVector(int _nIndex)
        {
            return m_VectorList[_nIndex];
        }

        #endregion // Properties

        /*********************************************************************/
        // Functions.
        /*********************************************************************/

        #region Initialization | Dispose

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates a new primitive object.
        /// </summary>
        /// <param name="_graphicsDevice">The graphics device object to use.</param>
        //////////////////////////////////////////////////////////////////////////
        public BasicPrimitives(GraphicsDevice _graphicsDevice)
        {
            //////////////////////////////////////////////////////////////////////////
            // Create the pixel texture.
            m_Pixel = new Texture2D(_graphicsDevice, 1, 1, false,SurfaceFormat.Color);
            m_Pixel.SetData<Color>(new Color[] { Color.White });
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Called when the primitive object is destroyed.
        /// </summary>
        //////////////////////////////////////////////////////////////////////////
        ~BasicPrimitives()
        {
            m_Pixel.Dispose();
            m_VectorList.Clear();
        }

        #endregion // Initialization | Dispose

        #region List Manipulation Methods

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Adds a vector to the primitive object.
        /// </summary>
        /// <param name="_vPosition">The vector to add.</param>
        //////////////////////////////////////////////////////////////////////////
        public void AddVector(Vector2 _vPosition)
        {
            m_VectorList.Add(_vPosition);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Inserts a vector into the primitive object.
        /// </summary>
        /// <param name="_nIndex">The index to insert it at.</param>
        /// <param name="_vPosition">The vector to insert.</param>
        //////////////////////////////////////////////////////////////////////////
        public void InsertVector(int _nIndex, Vector2 _vPosition)
        {
            m_VectorList.Insert(_nIndex, _vPosition);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Removes a vector from the primitive object.
        /// </summary>
        /// <param name="_vPosition">The vector to remove.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RemoveVector(Vector2 _vPosition)
        {
            m_VectorList.Remove(_vPosition);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Removes a vector from the primitive object.
        /// </summary>
        /// <param name="_nIndex">The index of the vector to remove.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RemoveVector(int _nIndex)
        {
            m_VectorList.RemoveAt(_nIndex);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Clears all vectors from the list.
        /// </summary>
        //////////////////////////////////////////////////////////////////////////
        public void ClearVectors()
        {
            m_VectorList.Clear();
        }

        #endregion // List Manipulation Methods

        #region Creation Methods

        //////////////////////////////////////////////////////////////////////////
        /// <summary> 
        /// Create a line primitive.
        /// </summary>
        /// <param name="_vStart">Start of the line, in pixels.</param>
        /// <param name="_vEnd">End of the line, in pixels.</param>
        //////////////////////////////////////////////////////////////////////////
        public void CreateLine(Vector2 _vStart, Vector2 _vEnd)
        {
            m_VectorList.Clear();
            m_VectorList.Add(_vStart);
            m_VectorList.Add(_vEnd);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create a triangle primitive.
        /// </summary>
        /// <param name="_vPoint1">Fist point, in pixels.</param>
        /// <param name="_vPoint2">Second point, in pixels.</param>
        /// <param name="_vPoint3">Third point, in pixels.</param>
        //////////////////////////////////////////////////////////////////////////
        public void CreateTriangle(Vector2 _vPoint1, Vector2 _vPoint2, Vector2 _vPoint3)
        {
            m_VectorList.Clear();
            m_VectorList.Add(_vPoint1);
            m_VectorList.Add(_vPoint2);
            m_VectorList.Add(_vPoint3);
            m_VectorList.Add(_vPoint1);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create a square primitive.
        /// </summary>
        /// <param name="_vTopLeft">Top left hand corner of the square.</param>
        /// <param name="_vBottomRight">Bottom right hand corner of the square.</param>
        //////////////////////////////////////////////////////////////////////////
        public void CreateSquare(Vector2 _vTopLeft, Vector2 _vBottomRight)
        {
            m_VectorList.Clear();
            m_VectorList.Add(_vTopLeft);
            m_VectorList.Add(new Vector2(_vTopLeft.X, _vBottomRight.Y));
            m_VectorList.Add(_vBottomRight);
            m_VectorList.Add(new Vector2(_vBottomRight.X, _vTopLeft.Y));
            m_VectorList.Add(_vTopLeft);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates a circle starting from (0, 0).
        /// </summary>
        /// <param name="_fRadius">The radius (half the width) of the circle.</param>
        /// <param name="_nSides">The number of sides on the circle. (64 is average).</param>
        //////////////////////////////////////////////////////////////////////////
        public void CreateCircle(float _fRadius, int _nSides)
        {
            m_VectorList.Clear();

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            float fMax = (float)MathHelper.TwoPi;
            float fStep = fMax / (float)_nSides;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Create the full circle.
            for (float fTheta = fMax; fTheta >= -1; fTheta -= fStep)
            {
                m_VectorList.Add(new Vector2(_fRadius * (float)Math.Cos((double)fTheta),
                                             _fRadius * (float)Math.Sin((double)fTheta)));
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Creates an ellipse starting from (0, 0) with the given width and height.
        /// Vectors are generated using the parametric equation of an ellipse.
        /// </summary>
        /// <param name="_fSemiMajorAxis">The width of the ellipse at its center.</param>
        /// <param name="_fSemiMinorAxis">The height of the ellipse at its center.</param>
        /// <param name="_nSides">The number of sides on the ellipse. (64 is average).</param>
        //////////////////////////////////////////////////////////////////////////
        public void CreateEllipse(float _fSemiMajorAxis, float _fSemiMinorAxis, int _nSides)
        {
            m_VectorList.Clear();

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            float fMax = (float)MathHelper.TwoPi;
            float fStep = fMax / (float)_nSides;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Create full ellipse.
            for (float fTheta = fMax; fTheta >= -1; fTheta -= fStep)
            {
                m_VectorList.Add(new Vector2((float)(_fSemiMajorAxis * Math.Cos(fTheta)),
                                             (float)(_fSemiMinorAxis * Math.Sin(fTheta))));
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create a linear spline based on 4 point positions.
        /// </summary>
        /// <param name="_vPoint1">First position.</param>
        /// <param name="_vPoint2">Second position.</param>
        /// <param name="_vPoint3">Third position.</param>
        /// <param name="_vPoint4">Fourth position.</param>
        //////////////////////////////////////////////////////////////////////////
        public void CreateLinearSpline(Vector2 _vPoint1, Vector2 _vPoint2, Vector2 _vPoint3, Vector2 _vPoint4)
        {
            CalculateSpline(Spline.Linear, new Vector2[] { _vPoint1, _vPoint2, _vPoint3, _vPoint4 }, 0f, 0f);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create a cosine spline based on 4 point positions.
        /// </summary>
        /// <param name="_vPoint1">First position.</param>
        /// <param name="_vPoint2">Second position.</param>
        /// <param name="_vPoint3">Third position.</param>
        /// <param name="_vPoint4">Fourth position.</param>
        //////////////////////////////////////////////////////////////////////////
        public void CreateCosineSpline(Vector2 _vPoint1, Vector2 _vPoint2, Vector2 _vPoint3, Vector2 _vPoint4)
        {
            CalculateSpline(Spline.Cosine, new Vector2[] { _vPoint1, _vPoint2, _vPoint3, _vPoint4 }, 0f, 0f);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create a cubic spline based on 4 point positions.
        /// </summary>
        /// <param name="_vPoint1">First position.</param>
        /// <param name="_vPoint2">Second position.</param>
        /// <param name="_vPoint3">Third position.</param>
        /// <param name="_vPoint4">Fourth position.</param>
        //////////////////////////////////////////////////////////////////////////
        public void CreateCubicSpline(Vector2 _vPoint1, Vector2 _vPoint2, Vector2 _vPoint3, Vector2 _vPoint4)
        {
            CalculateSpline(Spline.Cubic, new Vector2[] { _vPoint1, _vPoint2, _vPoint3, _vPoint4 }, 0f, 0f);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create a hermite spline based on 4 point positions and modifications.
        /// </summary>
        /// <param name="_vPoint1">First position.</param>
        /// <param name="_vPoint2">Second position.</param>
        /// <param name="_vPoint3">Third position.</param>
        /// <param name="_vPoint4">Fourth position.</param>
        /// <param name="_fTension">Tension allows to tighten up the curvature. 1 (or greater) = high tension, 0 = normal tension, -1 (or lower) = low tension.</param>
        /// <param name="_fBias">Bias allows to twist the curve. 1 (or greater) = Towards first segment, 0 = even, -1 (or lower) = Towards end segment.</param>
        //////////////////////////////////////////////////////////////////////////
        public void CreateHermiteSpline(Vector2 _vPoint1, Vector2 _vPoint2, Vector2 _vPoint3, Vector2 _vPoint4,
                                        float _fTension, float _fBias)
        {
            CalculateSpline(Spline.Hermite, new Vector2[] { _vPoint1, _vPoint2, _vPoint3, _vPoint4 }, _fTension, _fBias);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Create a bézier spline based on 4 point positions.
        /// </summary>
        /// <param name="_vPoint1">First position.</param>
        /// <param name="_vPoint2">Second position.</param>
        /// <param name="_vPoint3">Third position.</param>
        /// <param name="_vPoint4">Fourth position.</param>
        //////////////////////////////////////////////////////////////////////////
        public void CreateBezierSpline(Vector2 _vPoint1, Vector2 _vPoint2, Vector2 _vPoint3, Vector2 _vPoint4)
        {
            BezierSpline(_vPoint1, _vPoint2, _vPoint3, _vPoint4);
        }

        #endregion // Creation Methods

        #region Render Methods

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render points of the primitive.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderPointPrimitive(SpriteBatch _spriteBatch)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count <= 0)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero;
            float fAngle = 0f;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));

                // Stretch the pixel between the two vectors.
                _spriteBatch.Draw(m_Pixel,
                                  m_vPosition + m_VectorList[i],
                                  null,
                                  m_Color,
                                  fAngle,
                                  new Vector2(0.5f, 0.5f),
                                  m_fThickness,
                                  SpriteEffects.None,
                                  m_fDepth);
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render points of the primitive.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        /// <param name="_fAngle">The counterclockwise rotation in radians. (0.0f is default).</param>
        /// <param name="_vPivot">Position in which to rotate around.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderPointPrimitive(SpriteBatch _spriteBatch, float _fAngle, Vector2 _vPivot)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count <= 0)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Rotate object based on pivot.
            Rotate(_fAngle, _vPivot);
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero;
            float fAngle = 0f;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));

                // Stretch the pixel between the two vectors.
                _spriteBatch.Draw(m_Pixel,
                                  m_vPosition + m_VectorList[i],
                                  null,
                                  m_Color,
                                  fAngle,
                                  new Vector2(0.5f, 0.5f),
                                  m_fThickness,
                                  SpriteEffects.None,
                                  m_fDepth);
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render the lines of the primitive.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        //////////////////////////////////////////////////////////////////////////
        private static Vector2 vPosition1, vPosition2;
        public void RenderLinePrimitive(SpriteBatch _spriteBatch)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count < 2)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            vPosition1 = Vector2.Zero;
            vPosition2 = Vector2.Zero;
            float fDistance = 0f, fAngle = 0f;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];

                // Calculate the distance between the two vectors.
                fDistance = Vector2.Distance(vPosition1, vPosition2);

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));

                // Stretch the pixel between the two vectors.
                _spriteBatch.Draw(m_Pixel,
                                  m_vPosition + vPosition1,
                                  null,
                                  m_Color,
                                  fAngle,
                                  new Vector2(0, 0.5f),
                                  new Vector2(fDistance, m_fThickness),
                                  SpriteEffects.None,
                                  m_fDepth);
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render the lines of the primitive.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        /// <param name="_fAngle">The counterclockwise rotation in radians. (0.0f is default).</param>
        /// <param name="_vPivot">Position in which to rotate around.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderLinePrimitive(SpriteBatch _spriteBatch, float _fAngle, Vector2 _vPivot)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count < 2)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Rotate object based on pivot.
            Rotate(_fAngle, _vPivot);
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero;
            float fDistance = 0f, fAngle = 0f;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];

                // Calculate the distance between the two vectors.
                fDistance = Vector2.Distance(vPosition1, vPosition2);

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));

                // Stretch the pixel between the two vectors.
                _spriteBatch.Draw(m_Pixel,
                                  m_vPosition + vPosition1,
                                  null,
                                  m_Color,
                                  fAngle,
                                  new Vector2(0, 0.5f),
                                  new Vector2(fDistance, m_fThickness),
                                  SpriteEffects.None,
                                  m_fDepth);
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render primitive by using a square algorithm.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderSquarePrimitive(SpriteBatch _spriteBatch)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count < 2)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero, vLength = Vector2.Zero;
            float fDistance = 0f, fAngle = 0f;
            int nCount = 0;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                //////////////////////////////////////////////////////////////////////////
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Calculate the distance between the two vectors.
                fDistance = Vector2.Distance(vPosition1, vPosition2);

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));

                // Calculate length.
                vLength = vPosition2 - vPosition1;
                vLength.Normalize();

                // Calculate count for roundness.
                nCount = (int)Math.Round(fDistance);
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Run through and render the primitive.
                while (nCount-- > 0)
                {
                    // Increment position.
                    vPosition1 += vLength;

                    // Stretch the pixel between the two vectors.
                    _spriteBatch.Draw(m_Pixel,
                                      m_vPosition + vPosition1,
                                      null,
                                      m_Color,
                                      0,
                                      Vector2.Zero,
                                      m_fThickness,
                                      SpriteEffects.None,
                                      m_fDepth);
                }
                //
                //////////////////////////////////////////////////////////////////////////
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render primitive by using a square algorithm.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        /// <param name="_fAngle">The counterclockwise rotation in radians. (0.0f is default).</param>
        /// <param name="_vPivot">Position in which to rotate around.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderSquarePrimitive(SpriteBatch _spriteBatch, float _fAngle, Vector2 _vPivot)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count < 2)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Rotate object based on pivot.
            Rotate(_fAngle, _vPivot);
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero, vLength = Vector2.Zero;
            float fDistance = 0f, fAngle = 0f;
            int nCount = 0;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                //////////////////////////////////////////////////////////////////////////
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Calculate the distance between the two vectors.
                fDistance = Vector2.Distance(vPosition1, vPosition2);

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));

                // Calculate length.
                vLength = vPosition2 - vPosition1;
                vLength.Normalize();

                // Calculate count for roundness.
                nCount = (int)Math.Round(fDistance);
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Run through and render the primitive.
                while (nCount-- > 0)
                {
                    // Increment position.
                    vPosition1 += vLength;

                    // Stretch the pixel between the two vectors.
                    _spriteBatch.Draw(m_Pixel,
                                      m_vPosition + vPosition1,
                                      null,
                                      m_Color,
                                      0,
                                      Vector2.Zero,
                                      m_fThickness,
                                      SpriteEffects.None,
                                      m_fDepth);
                }
                //
                //////////////////////////////////////////////////////////////////////////
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render primitive by using a round algorithm.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderRoundPrimitive(SpriteBatch _spriteBatch)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count < 2)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero, vLength = Vector2.Zero;
            float fDistance = 0f, fAngle = 0f;
            int nCount = 0;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                //////////////////////////////////////////////////////////////////////////
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Calculate the distance between the two vectors.
                fDistance = Vector2.Distance(vPosition1, vPosition2);

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));

                // Calculate length.
                vLength = vPosition2 - vPosition1;
                vLength.Normalize();

                // Calculate count for roundness.
                nCount = (int)Math.Round(fDistance);
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Run through and render the primitive.
                while (nCount-- > 0)
                {
                    // Increment position.
                    vPosition1 += vLength;

                    // Stretch the pixel between the two vectors.
                    _spriteBatch.Draw(m_Pixel,
                                      m_vPosition + vPosition1 + 0.5f * (vPosition2 - vPosition1),
                                      null,
                                      m_Color,
                                      fAngle,
                                      new Vector2(0.5f, 0.5f),
                                      new Vector2(fDistance, m_fThickness),
                                      SpriteEffects.None,
                                      m_fDepth);
                }
                //
                //////////////////////////////////////////////////////////////////////////
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render primitive by using a round algorithm.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        /// <param name="_fAngle">The counterclockwise rotation in radians. (0.0f is default).</param>
        /// <param name="_vPivot">Position in which to rotate around.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderRoundPrimitive(SpriteBatch _spriteBatch, float _fAngle, Vector2 _vPivot)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count < 2)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Rotate object based on pivot.
            Rotate(_fAngle, _vPivot);
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero, vLength = Vector2.Zero;
            float fDistance = 0f, fAngle = 0f;
            int nCount = 0;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                //////////////////////////////////////////////////////////////////////////
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Calculate the distance between the two vectors.
                fDistance = Vector2.Distance(vPosition1, vPosition2);

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));

                // Calculate length.
                vLength = vPosition2 - vPosition1;
                vLength.Normalize();

                // Calculate count for roundness.
                nCount = (int)Math.Round(fDistance);
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Run through and render the primitive.
                while (nCount-- > 0)
                {
                    // Increment position.
                    vPosition1 += vLength;

                    // Stretch the pixel between the two vectors.
                    _spriteBatch.Draw(m_Pixel,
                                      m_vPosition + vPosition1 + 0.5f * (vPosition2 - vPosition1),
                                      null,
                                      m_Color,
                                      fAngle,
                                      new Vector2(0.5f, 0.5f),
                                      new Vector2(fDistance, m_fThickness),
                                      SpriteEffects.None,
                                      m_fDepth);
                }
                //
                //////////////////////////////////////////////////////////////////////////
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render primitive by using a point and line algorithm.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderPolygonPrimitive(SpriteBatch _spriteBatch)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count < 2)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero;
            float fDistance = 0f, fAngle = 0f;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                //////////////////////////////////////////////////////////////////////////
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Calculate the distance between the two vectors.
                fDistance = Vector2.Distance(vPosition1, vPosition2);

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Stretch the pixel between the two vectors.
                _spriteBatch.Draw(m_Pixel,
                                  Position + vPosition1 + 0.5f * (vPosition2 - vPosition1),
                                  null,
                                  m_Color,
                                  fAngle,
                                  new Vector2(0.5f, 0.5f),
                                  new Vector2(fDistance, Thickness),
                                  SpriteEffects.None,
                                  m_fDepth);

                // Render the points of the polygon.
                _spriteBatch.Draw(m_Pixel,
                                  m_vPosition + vPosition1,
                                  null,
                                  m_Color,
                                  fAngle,
                                  new Vector2(0.5f, 0.5f),
                                  m_fThickness,
                                  SpriteEffects.None,
                                  m_fDepth);
                //
                //////////////////////////////////////////////////////////////////////////
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Render primitive by using a point and line algorithm.
        /// </summary>
        /// <param name="_spriteBatch">The sprite batch to use to render the primitive object.</param>
        /// <param name="_fAngle">The counterclockwise rotation in radians. (0.0f is default).</param>
        /// <param name="_vPivot">Position in which to rotate around.</param>
        //////////////////////////////////////////////////////////////////////////
        public void RenderPolygonPrimitive(SpriteBatch _spriteBatch, float _fAngle, Vector2 _vPivot)
        {
            //////////////////////////////////////////////////////////////////////////
            // Validate.
            if (m_VectorList.Count < 2)
                return;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Rotate object based on pivot.
            Rotate(_fAngle, _vPivot);
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            Vector2 vPosition1 = Vector2.Zero, vPosition2 = Vector2.Zero;
            float fDistance = 0f, fAngle = 0f;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the list of vectors.
            for (int i = m_VectorList.Count - 1; i >= 1; --i)
            {
                //////////////////////////////////////////////////////////////////////////
                // Store positions.
                vPosition1 = m_VectorList[i - 1];
                vPosition2 = m_VectorList[i];
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Calculate the distance between the two vectors.
                fDistance = Vector2.Distance(vPosition1, vPosition2);

                // Calculate the angle between the two vectors.
                fAngle = (float)Math.Atan2((double)(vPosition2.Y - vPosition1.Y),
                                           (double)(vPosition2.X - vPosition1.X));
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Stretch the pixel between the two vectors.
                _spriteBatch.Draw(m_Pixel,
                                  Position + vPosition1 + 0.5f * (vPosition2 - vPosition1),
                                  null,
                                  m_Color,
                                  fAngle,
                                  new Vector2(0.5f, 0.5f),
                                  new Vector2(fDistance, Thickness),
                                  SpriteEffects.None,
                                  m_fDepth);

                // Render the points of the polygon.
                _spriteBatch.Draw(m_Pixel,
                                  m_vPosition + vPosition1,
                                  null,
                                  m_Color,
                                  fAngle,
                                  new Vector2(0.5f, 0.5f),
                                  m_fThickness,
                                  SpriteEffects.None,
                                  m_fDepth);
                //
                //////////////////////////////////////////////////////////////////////////
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        #endregion // Render Methods

        #region Public Methods

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Rotate primitive object based on pivot.
        /// </summary>
        /// <param name="_fAngle">The counterclockwise rotation in radians. (0.0f is default).</param>
        /// <param name="_vPivot">Position in which to rotate around.</param>
        //////////////////////////////////////////////////////////////////////////
        public void Rotate(float _fAngle, Vector2 _vPivot)
        {
            //////////////////////////////////////////////////////////////////////////
            // Subtract pivot from all points.
            for (int i = m_VectorList.Count - 1; i >= 0; --i)
                m_VectorList[i] -= _vPivot;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Rotate about the origin.
            Matrix mat = Matrix.CreateRotationZ(_fAngle);
            for (int i = m_VectorList.Count - 1; i >= 0; --i)
                m_VectorList[i] = Vector2.Transform(m_VectorList[i], mat);
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Add pivot to all points.
            for (int i = m_VectorList.Count - 1; i >= 0; --i)
                m_VectorList[i] += _vPivot;
            //
            //////////////////////////////////////////////////////////////////////////
        }

        #endregion // Public Methods

        #region Private Methods

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calculate a spline based on 4 point positions and type.
        /// </summary>
        /// <param name="_type">Spline type to create.</param>
        /// <param name="_vPointList">List of points.</param>
        /// <param name="_fTension">Tension allows to tighten up the curvature. 1 (or greater) = high tension, 0 = normal tension, -1 (or lower) = low tension.</param>
        /// <param name="_fBias">Bias allows to twist the curve. 1 (or greater) = Towards first segment, 0 = even, -1 (or lower) = Towards end segment.</param>
        //////////////////////////////////////////////////////////////////////////
        private void CalculateSpline(Spline _type, Vector2[] _vPointList,
                                     float _fTension, float _fBias)
        {
            //////////////////////////////////////////////////////////////////////////
            // Clear out the list.
            m_VectorList.Clear();
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            float fTime = 0f, fTimeCube = 0f, fTimeSq = 0f;
            int nIndex = 0, nCurrentSegment = 0;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Calculate the number of segments, within the spline, using the end points.
            int nSegments = (int)(Math.Sqrt(Math.Pow((_vPointList[nIndex + 1].X - _vPointList[nIndex].X), 2) + Math.Pow((_vPointList[nIndex + 1].Y - _vPointList[nIndex].Y), 2)));
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the number of segments in reverse order.
            for (int i = nSegments; nCurrentSegment != _vPointList.Length - 1; --i)
            {
                //////////////////////////////////////////////////////////////////////////
                // Determine to add the next segment.
                if (i < 0)
                {
                    //////////////////////////////////////////////////////////////////////////
                    // Determine if we reached the end of the point list.
                    if (++nIndex >= _vPointList.Length - 1)
                        break;

                    // Otherwise continue on with the next segment on the spline.
                    ++nCurrentSegment;
                    i = nSegments = (int)(Math.Sqrt(Math.Pow((_vPointList[nIndex + 1].X - _vPointList[nIndex].X), 2) + Math.Pow((_vPointList[nIndex + 1].Y - _vPointList[nIndex].Y), 2)));
                    //
                    //////////////////////////////////////////////////////////////////////////
                }
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Determine the elapsed time value based on the segment between 0 and 1.
                fTime = (float)i / (float)nSegments;
                fTimeSq = fTime * fTime;
                fTimeCube = fTime * fTimeSq;
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Add the new position to the list.
                switch (nCurrentSegment)
                {
                    case 0: // End segment.
                        {
                            switch (_type)
                            {
                                case Spline.Cubic:
                                    {
                                        m_VectorList.Add(new Vector2(CubicInterpolate(_vPointList[1].X, _vPointList[2].X, _vPointList[3].X, GetPointOnSpline(_vPointList[0], _vPointList[1], _vPointList[2], _vPointList[3], fTime).X, fTime),
                                                                     CubicInterpolate(_vPointList[1].Y, _vPointList[2].Y, _vPointList[3].Y, GetPointOnSpline(_vPointList[0], _vPointList[1], _vPointList[2], _vPointList[3], fTime).Y, fTime)));
                                    }
                                    break;
                                case Spline.Linear:
                                    {
                                        m_VectorList.Add(new Vector2(LinearInterpolate(_vPointList[2].X, _vPointList[3].X, fTime),
                                                                     LinearInterpolate(_vPointList[2].Y, _vPointList[3].Y, fTime)));
                                    }
                                    break;
                                case Spline.Cosine:
                                    {
                                        m_VectorList.Add(Vector2.CatmullRom(_vPointList[1], _vPointList[2], _vPointList[3], GetPointOnSpline(_vPointList[0], _vPointList[1], _vPointList[2], _vPointList[3], fTime), fTime));
                                    }
                                    break;
                                case Spline.Hermite:
                                    {
                                        m_VectorList.Add(new Vector2(HermiteInterpolate(_vPointList[1].X, _vPointList[2].X, _vPointList[3].X, GetPointOnSpline(_vPointList[0], _vPointList[1], _vPointList[2], _vPointList[3], fTime).X, fTime, _fTension, _fBias),
                                                                     HermiteInterpolate(_vPointList[1].Y, _vPointList[2].Y, _vPointList[3].Y, GetPointOnSpline(_vPointList[0], _vPointList[1], _vPointList[2], _vPointList[3], fTime).Y, fTime, _fTension, _fBias)));
                                    }
                                    break;

                                default: break;
                            }
                        }
                        break;
                    case 1: // Middle segment.
                        {
                            switch (_type)
                            {
                                case Spline.Cubic:
                                    {
                                        m_VectorList.Add(new Vector2(CubicInterpolate(_vPointList[0].X, _vPointList[1].X, _vPointList[2].X, _vPointList[3].X, fTime),
                                                                     CubicInterpolate(_vPointList[0].Y, _vPointList[1].Y, _vPointList[2].Y, _vPointList[3].Y, fTime)));
                                    }
                                    break;
                                case Spline.Linear:
                                    {
                                        m_VectorList.Add(new Vector2(LinearInterpolate(_vPointList[1].X, _vPointList[2].X, fTime),
                                                                     LinearInterpolate(_vPointList[1].Y, _vPointList[2].Y, fTime)));
                                    }
                                    break;
                                case Spline.Cosine:
                                    {
                                        m_VectorList.Add(Vector2.CatmullRom(_vPointList[0], _vPointList[1], _vPointList[2], _vPointList[3], fTime));
                                    }
                                    break;
                                case Spline.Hermite:
                                    {
                                        m_VectorList.Add(new Vector2(HermiteInterpolate(_vPointList[0].X, _vPointList[1].X, _vPointList[2].X, _vPointList[3].X, fTime, _fTension, _fBias),
                                                                     HermiteInterpolate(_vPointList[0].Y, _vPointList[1].Y, _vPointList[2].Y, _vPointList[3].Y, fTime, _fTension, _fBias)));
                                    }
                                    break;

                                default: break;
                            }
                        }
                        break;
                    case 2: // First segment.
                        {
                            switch (_type)
                            {
                                case Spline.Cubic:
                                    {
                                        m_VectorList.Add(new Vector2(CubicInterpolate(GetPointOnSpline(_vPointList[0], _vPointList[1], _vPointList[2], _vPointList[3], fTime).X, _vPointList[0].X, _vPointList[1].X, _vPointList[2].X, fTime),
                                                                     CubicInterpolate(GetPointOnSpline(_vPointList[0], _vPointList[1], _vPointList[2], _vPointList[3], fTime).Y, _vPointList[0].Y, _vPointList[1].Y, _vPointList[2].Y, fTime)));
                                    }
                                    break;
                                case Spline.Linear:
                                    {
                                        m_VectorList.Add(new Vector2(LinearInterpolate(_vPointList[0].X, _vPointList[1].X, fTime),
                                                                     LinearInterpolate(_vPointList[0].Y, _vPointList[1].Y, fTime)));
                                    }
                                    break;
                                case Spline.Cosine:
                                    {
                                        m_VectorList.Add(Vector2.CatmullRom(GetPointOnSpline(_vPointList[0], _vPointList[1], _vPointList[2], _vPointList[3], fTime), _vPointList[0], _vPointList[1], _vPointList[2], fTime));
                                    }
                                    break;
                                case Spline.Hermite:
                                    {
                                        m_VectorList.Add(new Vector2(HermiteInterpolate(GetPointOnSpline(_vPointList[0], _vPointList[1], _vPointList[2], _vPointList[3], fTime).X, _vPointList[0].X, _vPointList[1].X, _vPointList[2].X, fTime, _fTension, _fBias),
                                                                     HermiteInterpolate(GetPointOnSpline(_vPointList[0], _vPointList[1], _vPointList[2], _vPointList[3], fTime).Y, _vPointList[0].Y, _vPointList[1].Y, _vPointList[2].Y, fTime, _fTension, _fBias)));
                                    }
                                    break;

                                default: break;
                            }
                        }
                        break;

                    default: break;
                }
                //
                //////////////////////////////////////////////////////////////////////////
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Calculate a bézier spline based on 4 point positions.
        /// </summary>
        /// <param name="_vPoint1">First position.</param>
        /// <param name="_vPoint2">Second position.</param>
        /// <param name="_vPoint3">Third position.</param>
        /// <param name="_vPoint4">Fourth position.</param>
        //////////////////////////////////////////////////////////////////////////
        private void BezierSpline(Vector2 _vPoint1, Vector2 _vPoint2, Vector2 _vPoint3, Vector2 _vPoint4)
        {
            //////////////////////////////////////////////////////////////////////////
            // Calculate the X values.
            float fX1 = _vPoint4.X - 3 * _vPoint3.X + 3 * _vPoint2.X - _vPoint1.X;
            float fX2 = 3 * _vPoint3.X - 6 * _vPoint2.X + 3 * _vPoint1.X;
            float fX3 = 3 * _vPoint2.X - 3 * _vPoint1.X;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Calculate the Y values.
            float fY1 = _vPoint4.Y - 3 * _vPoint3.Y + 3 * _vPoint2.Y - _vPoint1.Y;
            float fY2 = 3 * _vPoint3.Y - 6 * _vPoint2.Y + 3 * _vPoint1.Y;
            float fY3 = 3 * _vPoint2.Y - 3 * _vPoint1.Y;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Calculate the number of segments, within the spline, using the mid-points.
            int nSegments = (int)(Math.Sqrt(Math.Pow((_vPoint3.X - _vPoint2.X), 2) + Math.Pow((_vPoint3.Y - _vPoint2.Y), 2)));

            // Create a new list.
            m_VectorList = new List<Vector2>(nSegments);
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Local variables.
            float fTime = 0f, fTimeCube = 0f, fTimeSq = 0f;
            //
            //////////////////////////////////////////////////////////////////////////

            //////////////////////////////////////////////////////////////////////////
            // Run through the number of segments in reverse order.
            for (int i = nSegments; i >= 0; --i)
            {
                //////////////////////////////////////////////////////////////////////////
                // Determine the elapsed time value based on the segment between 0 and 1.
                fTime = (float)i / (float)nSegments;
                fTimeSq = fTime * fTime;
                fTimeCube = fTime * fTimeSq;
                //
                //////////////////////////////////////////////////////////////////////////

                //////////////////////////////////////////////////////////////////////////
                // Add the new position to the list.
                m_VectorList.Add(new Vector2(fX1 * fTimeCube + fX2 * fTimeSq + fX3 * fTime + _vPoint1.X,
                                             fY1 * fTimeCube + fY2 * fTimeSq + fY3 * fTime + _vPoint1.Y));
                //
                //////////////////////////////////////////////////////////////////////////
            }
            //
            //////////////////////////////////////////////////////////////////////////
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Get a point on the spline.
        /// </summary>
        /// <param name="_vP1">First position.</param>
        /// <param name="_vP2">Second position.</param>
        /// <param name="_vP3">Third position.</param>
        /// <param name="_vP4">Fourth position.</param>
        /// <param name="_fTime">Time elapsed between 0 and 1.</param>
        //////////////////////////////////////////////////////////////////////////
        public Vector2 GetPointOnSpline(Vector2 _vP1, Vector2 _vP2, Vector2 _vP3, Vector2 _vP4, float _fTime)
        {
            return (((_vP4 * _fTime) + _vP3) * _fTime + _vP2) * _fTime + _vP1;
        }

        #endregion // Private Methods

        #region Spline Interpolations

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Spline interpolation resulting in straight lines.
        /// </summary>
        /// <param name="_fP1">First point.</param>
        /// <param name="_fP2">Second point.</param>
        /// <param name="_fTime">Time elapsed between 0 and 1.</param>
        //////////////////////////////////////////////////////////////////////////
        float LinearInterpolate(float _fP1, float _fP2, float _fTime)
        {
            return (_fP1 * (1 - _fTime) + _fP2 * _fTime);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Spline interpolation resulting in smooth curvy lines.
        /// </summary>
        /// <param name="_fP1">First point.</param>
        /// <param name="_fP2">Second point.</param>
        /// <param name="_fP3">Third point.</param>
        /// <param name="_fP4">Fourth point.</param>
        /// <param name="_fTime">Time elapsed between 0 and 1.</param>
        //////////////////////////////////////////////////////////////////////////
        float CubicInterpolate(float _fP1, float _fP2, float _fP3, float _fP4, float _fTime)
        {
            float fTimeSq = _fTime * _fTime;
            float fA1 = _fP4 - _fP3 - _fP1 + _fP2;
            float fA2 = _fP1 - _fP2 - fA1;
            float fA3 = _fP3 - _fP1;
            float fA4 = _fP2;

            return (fA1 * _fTime * fTimeSq + fA2 * fTimeSq + fA3 * _fTime + fA4);
        }

        //////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Spline interpolation resulting in, customizable, smooth curvy lines.
        /// </summary>
        /// <param name="_fP1">First point.</param>
        /// <param name="_fP2">Second point.</param>
        /// <param name="_fP3">Third point.</param>
        /// <param name="_fP4">Fourth point.</param>
        /// <param name="_fTime">Time elapsed between 0 and 1.</param>
        /// <param name="_fTension">Tension allows to tighten up the curvature. 1 (or greater) = high tension, 0 = normal tension, -1 (or lower) = low tension.</param>
        /// <param name="_fBias">Bias allows to twist the curve. 1 (or greater) = Towards first segment, 0 = even, -1 (or lower) = Towards end segment.</param>
        //////////////////////////////////////////////////////////////////////////
        float HermiteInterpolate(float _fP1, float _fP2, float _fP3, float _fP4,
                                 float _fTime, float _fTension, float _fBias)
        {
            float fTimeSq = _fTime * _fTime;
            float fTimeCube = fTimeSq * _fTime;
            float fTensionBias1 = (_fP2 - _fP1) * (1 + _fBias) * (1 - _fTension) / 2f;
            fTensionBias1 += (_fP3 - _fP2) * (1 - _fBias) * (1 - _fTension) / 2f;
            float fTensionBias2 = (_fP3 - _fP2) * (1 + _fBias) * (1 - _fTension) / 2f;
            fTensionBias2 += (_fP4 - _fP3) * (1 - _fBias) * (1 - _fTension) / 2f;
            float fA1 = 2 * fTimeCube - 3 * fTimeSq + 1;
            float fA2 = fTimeCube - 2 * fTimeSq + _fTime;
            float fA3 = fTimeCube - fTimeSq;
            float fA4 = -2 * fTimeCube + 3 * fTimeSq;

            return (fA1 * _fP2 + fA2 * fTensionBias1 + fA3 * fTensionBias2 + fA4 * _fP3);
        }

        #endregion // Spline Interpolations
    }
}

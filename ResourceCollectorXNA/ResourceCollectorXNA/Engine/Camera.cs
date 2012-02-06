using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace ResourceCollectorXNA.Engine{
    public class Camera{
        #region Variables
        public float _cameraPitch,
                      _cameraYaw;
        public Matrix Projection;
        public Matrix View;
        private GameEngine _engine;
        public BoundingFrustum cameraFrustum = new BoundingFrustum(Matrix.Identity);
        #endregion

        private Vector2 lastmousepos;


        public Camera(GameEngine engine, Vector3? cameraposition = null, Vector3? cameratarget = null) {
            if(cameratarget == null)
                cameratarget = new Vector3(0, 0, 0);
            if(cameraposition == null)
                cameraposition = new Vector3(20, 20, 10);
            _engine = engine;
            float katet = -(cameratarget.Value.Y - cameraposition.Value.Y);
            float gipotenuza = (cameraposition.Value - cameratarget.Value).Length();
            _cameraPitch = -MathHelper.PiOver2 + Convert.ToSingle(Math.Acos(Convert.ToDouble(katet / gipotenuza)));
            float determ = (cameratarget.Value.Z - cameraposition.Value.Z);
            if(determ == 0)
                determ = 0.001f;
            _cameraYaw = Convert.ToSingle(Math.Atan(Convert.ToSingle((cameratarget.Value.X - cameraposition.Value.X) / determ)));
            View = Matrix.CreateLookAt(cameraposition.Value, cameratarget.Value, new Vector3(0, 1, 0));
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, GameEngine.Device.Viewport.AspectRatio, 1, 250.0f);
            cameraFrustum.Matrix = View * Projection;
        }


        public void ResetProjection(float aspectratio) {
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspectratio, 1, 300.0f);
        }


        public void Update(GameTime elapsedTime) {
            var elapsed = (float) (elapsedTime.ElapsedGameTime.TotalMilliseconds / 1000.0);
            const float speed = 40.0f;
            float distance = speed * elapsed;


            var cursorPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            Vector3 vv = Vector3.TransformNormal(Vector3.Forward, View);

            Vector2 delta = cursorPosition - lastmousepos;
            Vector3 forward = Matrix.Invert(View).Forward;
            Vector3 position = Matrix.Invert(View).Translation;
            Matrix cameraRotation = Matrix.CreateFromYawPitchRoll(_cameraYaw, _cameraPitch, 0.0f);
            Vector3 newForward = Vector3.TransformNormal(Vector3.Forward, cameraRotation);
            MouseState ss = Mouse.GetState();
            KeyboardState states = Keyboard.GetState();
            //if (ss.RightButton == ButtonState.Pressed)
            if(states.IsKeyDown(Keys.LeftShift)) {
                Vector2 deltaDampened = delta * 0.0015f;

                _cameraYaw -= deltaDampened.X;
                _cameraPitch -= deltaDampened.Y;

                cameraRotation = Matrix.CreateFromYawPitchRoll(_cameraYaw, _cameraPitch, 0.0f);
                newForward = Vector3.TransformNormal(Vector3.Forward, cameraRotation);
            }


            Vector3 translateDirection = Vector3.Zero;
            if (!GameEngine.actionToInterface)
            {
                if (states.IsKeyDown(Keys.S)) // Forwards
                    translateDirection += Vector3.TransformNormal(Vector3.Forward, cameraRotation);

                if (states.IsKeyDown(Keys.X)) // Backwards
                    translateDirection += Vector3.TransformNormal(Vector3.Backward, cameraRotation);


                if (MouseManager.Manager.mmbState == ButtonState.Pressed)
                {
                    translateDirection += Vector3.TransformNormal(Vector3.Left * MouseManager.Manager.d_mouse.X, cameraRotation);
                    translateDirection += Vector3.TransformNormal(Vector3.Up * MouseManager.Manager.d_mouse.Y, cameraRotation);
                }
                //ебучее колесо не пашет
                translateDirection += Vector3.TransformNormal(Vector3.Forward * MouseManager.Manager.scrollWheelDelta, cameraRotation);
            }
            Vector3 newPosition = position;
            if(translateDirection.LengthSquared() > 0)
                newPosition += translateDirection * distance;

            View = Matrix.CreateLookAt(newPosition, newPosition + newForward, Vector3.Up);

            lastmousepos = cursorPosition;
            cameraFrustum.Matrix = View * Projection;
        }
    }
}
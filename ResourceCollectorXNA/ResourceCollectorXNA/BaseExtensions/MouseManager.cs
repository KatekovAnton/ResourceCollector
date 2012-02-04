using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ResourceCollectorXNA
{
    public class MouseManager
    {
        
        private sealed class MouseManagerCreator
        {
            private static readonly MouseManager instance = new MouseManager();

            public static MouseManager _MouseManager
            {
                get { return instance; }
            }
        }
        public static MouseManager Manager
        {
            get { return MouseManagerCreator._MouseManager; }
        }

        private List<IMouseUserInterface> mouseusers;
        public static  bool IsMouseCaptured
        {
            get
            {
                foreach (IMouseUserInterface user in Manager.mouseusers)
                {
                    if (user.IsMouseCaptured())
                        return true;
                }
                return false;
            }
        }







        public int scrollWheelSTARTValue;
        public int scrollWheelValue;
        public int scrollWheelDelta;

        public Vector2 mousePos;

        public ButtonState lmbState;
        public ButtonState lmblastState;

        public ButtonState rmbState;
        public ButtonState rmblastState;

        public ButtonState mmbState;
        public ButtonState mmblastState;

        public bool isJustPressed;
        public bool isJustReleased;

        public MouseState state;
        public MouseState lastState;
        public Vector2 d_mouse;

        public bool moved;
        bool ft = true;





        protected MouseManager()
        {
            mouseusers = new List<IMouseUserInterface>();
            state = Mouse.GetState();
            lastState = state;

            scrollWheelValue = state.ScrollWheelValue;
            scrollWheelSTARTValue = state.ScrollWheelValue;
        }
        public void AddMouseUser(IMouseUserInterface newUser)
        {
            mouseusers.Add(newUser);
        }
        public void RemoveMouseUser(IMouseUserInterface user)
        {
            mouseusers.Remove(user);
        }

        public void Update()
        {
            if (ft)
            {
                ft = false;
                return;
            }
            state = Mouse.GetState();
            if (mousePos.X != state.X || mousePos.Y != state.Y)
                moved = true;
            else
                moved = false;
            mousePos.X = state.X;
            mousePos.Y = state.Y;

            if (moved)
                d_mouse = new Vector2(mousePos.X - lastState.X, mousePos.Y - lastState.Y);
            else
                d_mouse.Y = d_mouse.X = 0;
            scrollWheelDelta = scrollWheelValue - state.ScrollWheelValue;
            scrollWheelValue = state.ScrollWheelValue;


            lmblastState = lmbState;
            rmblastState = rmbState;
            mmblastState = mmbState;

            lmbState = state.LeftButton;
            rmbState = state.RightButton;
            mmbState = state.MiddleButton;
            isJustPressed = isJustReleased = false;
            if (lmblastState == ButtonState.Pressed && lmbState == ButtonState.Released)
                isJustReleased = true;
            else if (lmblastState == ButtonState.Released && lmbState == ButtonState.Pressed)
                isJustPressed = true;


            lastState = state;
        }
    }
   
}

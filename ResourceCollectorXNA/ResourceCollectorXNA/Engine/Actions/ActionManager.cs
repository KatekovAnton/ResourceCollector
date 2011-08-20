using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


using ResourceCollectorXNA.Content;
using ResourceCollectorXNA.Engine;
using ResourceCollectorXNA.Engine.Render;
using ResourceCollectorXNA.Engine.ContentLoader;
using ResourceCollectorXNA.Engine.Logic;
using ResourceCollectorXNA.Engine.Interface;
using ResourceCollectorXNA.Engine.Actions;


namespace ResourceCollectorXNA.Engine
{
    public class ActionManager
    {
        EditorAction currentaction;
        public ActionManager()
        {
           
        }
        public void Update(MouseState mouseState)
        {
            //тут уже аксис не НОНЕ
            if (mouseState.RightButton == ButtonState.Pressed)
                if (currentaction != null)
                {
                    //stop drag, cancel
                }
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (currentaction != null)
                {
                    //continue drag
                }
                else
                {
                    //begin drag
                }
            }
            else
                if (currentaction != null)
                {
                    //stop drag, apply
                }
        }
        public bool IsActive
        {
            get
            {
                return currentaction == null;
            }
        }
    }
}

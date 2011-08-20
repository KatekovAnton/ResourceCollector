using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Reflection;

namespace ResourceCollectorXNA.Engine.Interface
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
        protected MouseManager()
        {
            mouseusers = new List<IMouseUserInterface>();
        }
        public void AddMouseUser(IMouseUserInterface newUser)
        {
            mouseusers.Add(newUser);
        }
        public void RemoveMouseUser(IMouseUserInterface user)
        {
            mouseusers.Remove(user);
        }
    }
   
}

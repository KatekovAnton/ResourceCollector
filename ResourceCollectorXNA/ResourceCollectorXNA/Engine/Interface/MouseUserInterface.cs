using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceCollectorXNA.Engine.Interface
{
    public abstract class IMouseUserInterface
    {
        public abstract bool IsMouseCaptured();
        public IMouseUserInterface()
        {
            MouseManager.Manager.AddMouseUser(this);
        }
        ~IMouseUserInterface()
        {
            MouseManager.Manager.RemoveMouseUser(this);
        }
    }
}

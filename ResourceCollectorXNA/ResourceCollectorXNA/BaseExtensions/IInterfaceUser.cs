using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceCollectorXNA
{
    public abstract class IInterfaceUser
    {
        public virtual bool IsMouseCaptured() { return false; }
        public virtual bool IsKeyboardCaptured(Microsoft.Xna.Framework.Input.Keys key) { return false; }
        public virtual bool IsKeyboardCaptured() { return false; }
    }
}

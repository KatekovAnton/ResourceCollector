﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceCollectorXNA.Engine.Interface
{
    public abstract class IMouseUserInterface:IInterfaceUser
    {
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ResourceCollectorXNA.Engine.Actions
{
    public abstract class EditorAction
    {
        public abstract void UpdateAction(object parameters);
        public abstract void CancelAction(Engine.GameEditor Editor);
        public virtual void onActionDeleted()
        { }
    }

}

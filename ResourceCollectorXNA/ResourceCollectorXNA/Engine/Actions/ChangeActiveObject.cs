using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ResourceCollectorXNA;
using ResourceCollectorXNA.Content;
using ResourceCollectorXNA.Engine;
using ResourceCollectorXNA.Engine.Logic;

using Microsoft.Xna.Framework;

namespace ResourceCollectorXNA.Engine.Actions
{
    class ChangeActiveObject : PivotAction
    {
        public ChangeActiveObject(ObjectContainer obj)
            : base(obj)
        { 

        }

        public override void CancelAction(Engine.GameEditor Editor)
        {
            Editor.SetActiveObjects(operatingObject,true);
        }

        public override void UpdateAction(object parameters)
        {
            
        }
    }
}

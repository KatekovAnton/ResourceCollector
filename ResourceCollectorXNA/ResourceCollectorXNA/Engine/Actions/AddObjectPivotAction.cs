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
    public class AddObjectPivotAction : PivotAction
    {
        public AddObjectPivotAction(ObjectContainer objects)
            : base(objects)
        {

        }

        public override void onActionDeleted()
        {
            
        }

        public override void CancelAction(GameEditor Editor)
        {
            MyContainer<PivotObject> objects = new MyContainer<PivotObject>(operatingObject.Length, 1);
            objects.AddRange(operatingObject.objects.ToArray());
            Editor.DeleteObjects(objects, true);
            for (int i = 0; i < operatingObject.Length;i++ )
                ContentLoader.ContentLoader.UnloadPivotObject(operatingObject[i]);
        }

        public override void UpdateAction(object parameters)
        {

        }

    }
}

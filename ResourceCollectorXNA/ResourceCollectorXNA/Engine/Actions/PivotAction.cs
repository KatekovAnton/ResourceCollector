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
    public enum state {cont,one,five};
    public abstract class PivotAction : EditorAction
    {
        public static state currentstate = state.cont;
        protected ObjectContainer operatingObject;
        protected Matrix[] StartTransform;
        public object ActionResult;

        protected PivotAction(ObjectContainer @object)
        {
            if (@object != null)
            {
                operatingObject = @object;
                StartTransform = new Matrix[@object.Length];

                for (int i = 0; i < @object.Length; i++)
                    StartTransform[i] = @object[i].transform;
            }
        }

        public bool Valid()
        {
            return StartTransform.Length > 0 ? StartTransform[0] != operatingObject[0].transform : false;
        }

        ~PivotAction()
        {
            operatingObject = null;
        }
    }

    public class PivotActionUpdateParameters
    {
        public Ray ray;
        public Vector2 mousepos;
        public Vector3 AxisPoint;
        public PivotActionUpdateParameters(Ray r, Vector2 mousepos, Vector3 ap)
        {
            AxisPoint = ap;
            ray = r;
            this.mousepos = mousepos;
        }
    }
}

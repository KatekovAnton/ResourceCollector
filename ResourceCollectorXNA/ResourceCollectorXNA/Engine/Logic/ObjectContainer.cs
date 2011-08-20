using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ResourceCollectorXNA;
using ResourceCollectorXNA.Content;
using ResourceCollectorXNA.Engine;
using ResourceCollectorXNA.Engine.Logic;

using Microsoft.Xna.Framework;

namespace ResourceCollectorXNA.Engine.Logic
{
    public class ObjectContainer
    {
        public Vector3 middle;
        public Matrix middleMarix;
        public List<PivotObject> objects;
        
        public ObjectContainer()
        {
            objects = new List<PivotObject>();
        }
        public ObjectContainer(PivotObject[] _objects)
        {
            objects = new List<PivotObject>(_objects);
            RecalculateMiddle();
        }
        public ObjectContainer(MyContainer< PivotObject> _objects)
        {
            objects = new List<PivotObject>();
            for (int i = 0; i < _objects.Count; i++)
                objects.Add( _objects[i]);
            RecalculateMiddle();
        }
        public void AddObject(PivotObject obj)
        {
            objects.Add(obj);
            RecalculateMiddle();
        }
        public void RemoveObject(PivotObject obj)
        {
            objects.Remove(obj);
            RecalculateMiddle();
        }
        public void SetObjects(PivotObject[] _objects)
        {
            objects.Clear();
            objects = new List<PivotObject>(_objects);
            RecalculateMiddle();
        }
        public void RecalculateMiddle()
        {
            middle = Vector3.Zero;
            for (int i = 0; i < objects.Count; i++)
            {
                middle += objects[i].transform.Translation;
            }
            middle /= (float)objects.Count;
            middleMarix = Matrix.CreateTranslation(middle);
        }
        public int Length
        {
            get
            {
                return objects.Count;
            }
        }
        public bool Contains(PivotObject obj)//стринг с нулём
        {
            foreach (PivotObject t in objects)
                if (t == obj)
                    return true;
            return false;
        }
        public PivotObject this[int index]
        {
            get
            {
                return objects[index];
            }
        }
        public bool Same(ObjectContainer anotherConainer)
        {
            if (Length == anotherConainer.Length)
            {
                for (int i = 0; i < Length; i++)
                    if (anotherConainer[i] != this[i])
                        return false;
                return true;
            }
            return false;
        }
    }
}

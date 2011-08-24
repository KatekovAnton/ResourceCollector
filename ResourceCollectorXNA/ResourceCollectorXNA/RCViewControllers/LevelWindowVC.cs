using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ResourceCollectorXNA.Engine.Logic;

namespace ResourceCollectorXNA.RCViewControllers
{
    public class LevelWindowVC
    {
        LevelWindow levelform;
        public LevelWindowVC(ResourceCollectorXNA.LevelWindow lv)
        {
            this.levelform = lv;
        }
        public void ObjectAdded(PivotObject newObject)
        {
            Console.WriteLine("ObjectAdded");
            levelform.AddObject(newObject.editorAspect.id.ToString(), newObject.editorAspect.DescriptionName, newObject.editorAspect.objtype.ToString(), newObject.editorAspect.isActive);
        }


        // добавление элементов в таблицу. Параеметр: список объектов
        public void ObjectsAdded(MyContainer<PivotObject> newObjects)
        {
            Console.WriteLine("ObjectsAdded");
            int masLen = newObjects.Count;
            for (int i = 0; i < masLen; ++i)
            {
                levelform.AddObject(newObjects[i].editorAspect.id.ToString(), newObjects[i].editorAspect.DescriptionName, newObjects[i].editorAspect.objtype.ToString(), newObjects[i].editorAspect.isActive);
            }
        }


        // удаление элементов из таблицы. Параметр: список удаляемых объектов
        public void ObjectsDeleted(MyContainer<PivotObject> deletingobjects)
        {
            if (deletingobjects.Count == 0)
            {
                return;
            }

            Console.WriteLine("ObjectsDeleted");
            foreach (PivotObject deletingobject in deletingobjects)
            {
                levelform.RemoveObject(deletingobject.editorAspect.id);
            }
        }


        // удаление единичного объекта
        public void ObjectDeleted(PivotObject curObject) { }


        // Полная очистка объетов в таблице
        public void Cleared()
        {
            Console.WriteLine("Cleared");
            levelform.ClearAllObjectsInGrid();
        }
    }
}

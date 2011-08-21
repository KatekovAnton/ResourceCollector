using System;



namespace ResourceCollectorXNA.Engine.Logic
{
    public static class LevelEditor
    {
        public static void ObjectAdded(PivotObject newObject) {
            Console.WriteLine("ObjectAdded");
            MyGame.levelform.AddObject(newObject.editorAspect.id.ToString(),
                                       newObject.editorAspect.DescriptionName,
                                       newObject.editorAspect.objtype.ToString(),
                                       newObject.editorAspect.isActive);
        }

        public static void ObjectsAdded(MyContainer<PivotObject> newObjects) {
            Console.WriteLine("ObjectsAdded");
            int masLen = newObjects.Count;
            for(int i = 0; i < masLen; ++i) {
                MyGame.levelform.AddObject(newObjects[i].editorAspect.id.ToString(),
                                           newObjects[i].editorAspect.DescriptionName,
                                           newObjects[i].editorAspect.objtype.ToString(),
                                           newObjects[i].editorAspect.isActive);
            }
        }

        public static void ObjectsDeleted(MyContainer<PivotObject> deletingobjects) {
            Console.WriteLine("ObjectsDeleted");
        }

        public static void Cleared() {
            Console.WriteLine("Cleared");
            MyGame.levelform.ClearAllObjectsInGrid();
        }
    }
}
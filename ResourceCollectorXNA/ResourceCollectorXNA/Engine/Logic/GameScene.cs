﻿namespace ResourceCollectorXNA.Engine.Logic {
    public class GameScene {
        public MyContainer<PivotObject> ShadowObjects;
        public MyContainer<PivotObject> VisibleObjects;
        public MyContainer<PivotObject> objects;
        public SceneGraph.SceneGraph sceneGraph;


        public GameScene() {
            objects = new MyContainer<PivotObject>(100, 10);
            VisibleObjects = new MyContainer<PivotObject>(100, 2);
            ShadowObjects = new MyContainer<PivotObject>(100, 2);
            sceneGraph = new SceneGraph.SceneGraph(this);
        }


        public void Clear() {
            VisibleObjects.Clear();
            ShadowObjects.Clear();
            sceneGraph.Clear();
            objects.Clear();
            IdGenerator.ClearIdsCounter();
            LevelEditor.Cleared();
        }

        public PivotObject GetObjectWithID(int id)
        {
            for (int i = 0; i < objects.Count; i++)
                if (objects[i].editorAspect.id == id)
                    return objects[i];
            ConsoleWindow.TraceMessage("Unable to find object with id = " + id.ToString());
            return null;
        }

        public void AddObject(PivotObject newObject) {
            objects.Add(newObject);
            sceneGraph.AddObject(newObject);
            LevelEditor.ObjectAdded(newObject);
        }


        public void DeleteObjects(MyContainer<PivotObject> deletingobjects) {
            foreach(PivotObject t in deletingobjects) {
                objects.Remove(t);
                sceneGraph.DeleteObject(t);
            }
            // счетчик идов будет начинать все делать с 0
            if(objects.Count == 0) {
                IdGenerator.ClearIdsCounter();
            }

            LevelEditor.ObjectsDeleted(deletingobjects);
        }


        public void AddObjects(MyContainer<PivotObject> newobjects) {
            foreach(PivotObject t in newobjects) {
                objects.Add(t);
                sceneGraph.AddObject(t);
            }
            LevelEditor.ObjectsAdded(newobjects);
        }


        public void UpdateScene() {
            foreach(PivotObject po in objects) {
                po.Update();
            }
            sceneGraph.NewFrame();
            sceneGraph.calculateVisibleObjects(GameEngine.Instance.Camera.cameraFrustum, VisibleObjects);
            sceneGraph.calculateShadowVisibleObjects(GameEngine.Instance.GraphicPipeleine.frustumForShadow, ShadowObjects);
        }
    }
}
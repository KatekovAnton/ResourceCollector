namespace ResourceCollectorXNA.Engine.Logic {
    public class GameScene {
        public MyContainer<PivotObject> ShadowObjects;
        public MyContainer<PivotObject> VisibleObjects;
        public MyContainer<PivotObject> objects;
        //мы же хотим переключать сцены?? поэтому каждой - свой сценграф
        public SceneGraph.SceneGraph sceneGraph;
        public IdGenerator idgenertor;

        public GameScene() {
            objects = new MyContainer<PivotObject>(100, 10);
            VisibleObjects = new MyContainer<PivotObject>(100, 2);
            ShadowObjects = new MyContainer<PivotObject>(100, 2);
            sceneGraph = new SceneGraph.SceneGraph(this);
            idgenertor = new IdGenerator(0);
        }

        public GameScene(GameScene s, uint generatorId = 0)
        {
            ShadowObjects = s.ShadowObjects;
            VisibleObjects = s.VisibleObjects;
            objects = s.objects;
            sceneGraph = s.sceneGraph;
            idgenertor = new IdGenerator(generatorId);
        }

        public void Clear() {
            VisibleObjects.Clear();
            ShadowObjects.Clear();
            sceneGraph.Clear();
            objects.Clear();

            idgenertor.ClearIdsCounter();
            
        }


        public PivotObject GetObjectWithID(uint id)
        {
            foreach(PivotObject t in objects)
                if (t.editorAspect.id == id)
                    return t;
            ConsoleWindow.TraceMessage("Unable to find object with id = " + id.ToString());
            return null;
        }


        public void AddObject(PivotObject newObject)
        {
           
            newObject.editorAspect.id = idgenertor.NewId();
            objects.Add(newObject);
            sceneGraph.AddObject(newObject);
        }


        public void AddObjectWithoutId(PivotObject newObject)
        {
            
            objects.Add(newObject);
            sceneGraph.AddObject(newObject);
        }


        public void DeleteObjects(MyContainer<PivotObject> deletingobjects)
        {
            foreach(PivotObject t in deletingobjects) {
                objects.Remove(t);
                sceneGraph.DeleteObject(t);
            }
            // счетчик идов будет начинать все делать с 0
            if(objects.Count == 0) {
                idgenertor.ClearIdsCounter(); ;
            }

           
        }


        public void AddObjects(MyContainer<PivotObject> newobjects)
        {
            foreach(PivotObject t in newobjects) {
                
                t.editorAspect.id = idgenertor.NewId();
                objects.Add(t);
                sceneGraph.AddObject(t);
            }
            
        }


        public void UpdateScene()
        {
            foreach(PivotObject po in objects) {
                po.Update();
            }
            sceneGraph.NewFrame();
            sceneGraph.calculateVisibleObjects(GameEngine.Instance.Camera.cameraFrustum, VisibleObjects);
            sceneGraph.calculateShadowVisibleObjects(GameEngine.Instance.GraphicPipeleine.frustumForShadow, ShadowObjects);
        }
    }
}
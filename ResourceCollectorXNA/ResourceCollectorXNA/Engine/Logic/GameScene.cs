﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ResourceCollectorXNA.Engine.Logic
{
    public class GameScene
    {
        public MyContainer<PivotObject> objects;
        public MyContainer<PivotObject> VisibleObjects;
        public MyContainer<PivotObject> ShadowObjects;
        public SceneGraph.SceneGraph sceneGraph;
        private Level levelWindow;


        public GameScene()
        {
            objects = new MyContainer<PivotObject>(100, 10);
            VisibleObjects = new MyContainer<PivotObject>(100, 2);
            ShadowObjects = new MyContainer<PivotObject>(100, 2);
            sceneGraph = new Logic.SceneGraph.SceneGraph(this);
            levelWindow = new Level();
        }
        virtual public void Clear()
        {
            VisibleObjects.Clear();
            ShadowObjects.Clear();
            sceneGraph.Clear();
            objects.Clear();
            levelWindow.Cleared();
        }
        virtual public void AddObject(PivotObject newObject)
        {
            objects.Add(newObject);
            sceneGraph.AddObject(newObject);
            levelWindow.ObjectAdded(newObject);
        }
        virtual public void DeleteObjects(MyContainer<PivotObject> deletingobjects)
        {
            for (int i = 0; i < deletingobjects.Count; i++)
            {
                objects.Remove(deletingobjects[i]);
                sceneGraph.DeleteObject(deletingobjects[i]);
            }
        }
        virtual public void AddObjects(MyContainer<PivotObject> newobjects)
        {
            for (int i = 0; i < newobjects.Count; i++)
            {
                objects.Add(newobjects[i]);
                sceneGraph.AddObject(newobjects[i]);
            }
        }        
        public void UpdateScene()
        {
            foreach (PivotObject po in objects)
            {
                po.Update();
            }
            sceneGraph.NewFrame();
            sceneGraph.calculateVisibleObjects(GameEngine.Instance.Camera.cameraFrustum, VisibleObjects);
            sceneGraph.calculateShadowVisibleObjects(GameEngine.Instance.GraphicPipeleine.frustumForShadow, ShadowObjects);
        }
    }
}

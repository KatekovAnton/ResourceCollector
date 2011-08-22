using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ResourceCollector;
using ResourceCollectorXNA.Engine.Logic;

namespace ResourceCollectorXNA.Engine.Level
{
    public class EngineLevel:GameScene
    {
        LevelContent levelContent;
        public EngineLevel()
        {
            levelContent = new LevelContent();
            levelContent.Enginereadedobject.Add(this);
        }
        public EngineLevel(GameScene scene):base(0)
        {
            levelContent = new LevelContent();
            levelContent.Enginereadedobject.Add(this);
            objects = scene.objects;
            sceneGraph = scene.sceneGraph;
            ShadowObjects = scene.ShadowObjects;
            VisibleObjects = scene.VisibleObjects;
        }
        public void FillContent()
        {
            levelContent.objectInformation.Clear();
            for (int i = 0; i < objects.Count; i++)
            {
                PivotObject currentObject = objects[i];
                levelContent.objectInformation.Add(new LevelContent.ObjectElement(currentObject.editorAspect.id, currentObject.editorAspect.group_id, currentObject.editorAspect.DescriptionName, currentObject.transform));
            }
        }
    }
}

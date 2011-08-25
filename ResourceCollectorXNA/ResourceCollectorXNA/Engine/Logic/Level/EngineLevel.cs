using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ResourceCollector;
using ResourceCollectorXNA.Engine.Logic;

namespace ResourceCollectorXNA.Engine.Level
{
    /// <summary>
    /// Вот этот класс и будет разростаться для перемещения данных из 
    /// сцены в пак контент, при этом он же и должен содержать всякую 
    /// прочую инфу типа скриптов
    /// </summary>
    public class EngineLevel:GameScene
    {
        public LevelContent levelContent;

        public EngineLevel()
        {
            levelContent = new LevelContent();
        }

        public EngineLevel(LevelContent _levelContent)
        {
            levelContent = _levelContent;
        }

        public void load()
        {
            idgenertor = new IdGenerator(levelContent.generator);
            for(int i = 0;i<levelContent.objectInformation.Count;i++)
            {
                ResourceCollector.Content.LevelObjectDescription lod= levelContent.pack.getobject(levelContent.objectInformation[i].descriptionName) as ResourceCollector.Content.LevelObjectDescription;
                LevelObject lo = ContentLoader.ContentLoader.LevelObjectFromDescription(lod, levelContent.pack);
                lo.SetGlobalPose(levelContent.objectInformation[i].objectMatrix);
                lo.editorAspect.group_id = levelContent.objectInformation[i].group_id;
                lo.editorAspect.id = levelContent.objectInformation[i].id;
                GameEngine.Instance.GraphicPipeleine.ProceedObject(lo.renderaspect);
                AddObjectWithoutId(lo);
            }
        }

        public void unload()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                ContentLoader.ContentLoader.UnloadPivotObject(objects[i]);
            }
            Clear();
        }

        public EngineLevel(GameScene scene)
            : base(scene)
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
                switch (currentObject.editorAspect.objtype)
                {
                    case ObjectEditorType.SolidObject:
                        {
                            levelContent.objectInformation.Add(new LevelContent.ObjectElement(currentObject.editorAspect.id, currentObject.editorAspect.group_id, currentObject.editorAspect.objtype, currentObject.editorAspect.DescriptionName, currentObject.transform));
                        }break;
                    default: break;
                }
            }
        }
    }
}

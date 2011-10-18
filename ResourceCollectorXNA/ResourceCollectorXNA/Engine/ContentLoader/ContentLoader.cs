using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ResourceCollectorXNA.Content;
using ResourceCollectorXNA.Engine;
using ResourceCollectorXNA.Engine.Logic;
using ResourceCollectorXNA.Engine.Render;
using ResourceCollectorXNA.Engine.Render.Materials;

using Microsoft.Xna.Framework.Graphics;


namespace ResourceCollectorXNA.Engine.ContentLoader
{
    public abstract class ContentLoader
    {
        public static int XNAObjectsLoadedCount = 0;
        public static MyContainer<string> XNAevents = new MyContainer<string>(100, 2);


        private static RaycastBoundObject loadrcbo( 
            ResourceCollector.Content.LevelObjectDescription description,
            ResourceCollector.Pack packs)
        {
            //клижмеш мб один на несколько объектов//его прекрасно удалит сам гк если что
            EngineCollisionMesh ObjectRCCM = null;
            //бш уникален для каждого //его тож гк удалит
            Logic.SceneGraph.OTBoundingShape bs = null;
            if (description.IsRCCMEnabled)
            {
                ResourceCollector.Content.CollisionMesh cm = packs.getobject(description.RCCMName) as ResourceCollector.Content.CollisionMesh;

                if (cm.Enginereadedobject.Count == 0)
                    ObjectRCCM = EngineCollisionMesh.FromcontentCollisionMesh(cm);
                else
                    ObjectRCCM = cm.Enginereadedobject[0] as EngineCollisionMesh;
                    
                cm.Enginereadedobject.Add(ObjectRCCM);
                bs = new Logic.SceneGraph.OTBoundingShape(ObjectRCCM);
            }
            else
                bs = new Logic.SceneGraph.OTBoundingShape(description.RCShapeSize);
            //его тоже удалят
            RaycastBoundObject raycastaspect = new RaycastBoundObject(bs, ObjectRCCM);
            return raycastaspect;
        }

        private static Material loadMaterial(string name, ResourceCollector.Pack packs)
        {
            ResourceCollector.Content.Material mat = packs.getobject(name) as ResourceCollector.Content.Material;
            if (mat.Enginereadedobject.Count == 0)
            {
                XNAevents.Add("creating material " + name);
                TextureMaterial.Lod[] lods = new TextureMaterial.Lod[mat.lodMats.Count];
                for (int i = 0; i < mat.lodMats.Count; i++)
                {
                    TextureMaterial.SubsetMaterial[] mats = new TextureMaterial.SubsetMaterial[mat.lodMats[i].mats.Count];
                    for (int j = 0; j < mat.lodMats[i].mats.Count; j++)
                    {
                        mats[j] = new TextureMaterial.SubsetMaterial();
                        Content.EngineTexture texture;
                        ResourceCollector.ImageContent inage = packs.getobject(mat.lodMats[i].mats[j].DiffuseTextureName) as ResourceCollector.ImageContent;
                        if (inage.Enginereadedobject.Count == 0)
                        {
                            texture = new Content.EngineTexture();
                            XNAevents.Add("creating texture " + mat.lodMats[i].mats[j].DiffuseTextureName);
                            texture.loadbody(inage.data);
                            inage.usercount++;
                        }
                        else
                        {
                            texture = inage.Enginereadedobject[0] as Content.EngineTexture;
                        }
                        inage.Enginereadedobject.Add(texture);
                        mats[j].diffuseTexture = texture.texture;
                    }
                    lods[i] = new TextureMaterial.Lod(mats);

                }
                TextureMaterial tm = new TextureMaterial(lods);
                mat.Enginereadedobject.Add(tm);
                return tm;
            }
            else
            {
                TextureMaterial result = mat.Enginereadedobject[0] as TextureMaterial;
                mat.Enginereadedobject.Add(result);

                for (int i = 0; i < mat.lodMats.Count; i++)
                {
                    for (int j = 0; j < mat.lodMats[i].mats.Count; j++)
                    {
                        ResourceCollector.ImageContent inage = packs.getobject(mat.lodMats[i].mats[j].DiffuseTextureName) as ResourceCollector.ImageContent;
                        inage.Enginereadedobject.Add(inage.Enginereadedobject[0]);
                    }
                }
                return result;
            }
        }

        private static RenderObject loadro(
            ResourceCollector.Content.LevelObjectDescription description,
            ResourceCollector.Pack packs)
        {
            ResourceCollector.Content.RenderObjectDescription rod = packs.getobject(description.RODName) as ResourceCollector.Content.RenderObjectDescription;
            if (rod.Enginereadedobject.Count == 0)
            {
                XNAevents.Add("creating render object " + description.RODName);
                UnAnimRenderObject.Model[] models = new UnAnimRenderObject.Model[rod.LODs.Count];
                for (int i = 0; i < models.Length; i++)
                {
                    UnAnimRenderObject.SubSet[] modelsubsets = new UnAnimRenderObject.SubSet[rod.LODs[i].subsets.Count];
                    for (int j = 0; j < modelsubsets.Length; j++)
                    {
                        ResourceCollector.PackContent[] objects = packs.GetObjects(rod.LODs[i].subsets[j].MeshNames);
                        ResourceCollector.MeshSkinned[] subsetmeshes = new ResourceCollector.MeshSkinned[objects.Length];
                        for (int t = 0; t < subsetmeshes.Length; t++)
                            subsetmeshes[t] = objects[t] as ResourceCollector.MeshSkinned;
                        //меши могут быть по-разному сгруппированы поэтому будем их каждый раз по новой загружать
                        EngineMesh subsetmesh = EngineMesh.FromContentMeshes(subsetmeshes);
                        modelsubsets[j] = new UnAnimRenderObject.SubSet(subsetmesh);
                    }
                    models[i] = new UnAnimRenderObject.Model(modelsubsets);
                }
                //его тоже удалят
                RenderObject result = new UnAnimRenderObject(models, rod.IsShadowCaster, rod.IsShadowReceiver);
                rod.Enginereadedobject.Add(result);
                return result;
            }
            else
            {
                RenderObject result = rod.Enginereadedobject[0] as RenderObject;
                rod.Enginereadedobject.Add(result);
                return result;
            }
        }

        public static void UnloadPivotObject(
            PivotObject theobject)
        {
            LevelObject gobject = theobject as LevelObject;
            if (gobject == null)
                return;
            ConsoleWindow.TraceMessage("Content loader: Unloading object: " + theobject.editorAspect.DescriptionName);
            System.Drawing.Point p = new System.Drawing.Point();
            ResourceCollector.PackContent pc  = ResourceCollector.PackList.Instance.findobject(theobject.editorAspect.DescriptionName, ref p);
          
            ResourceCollector.Content.LevelObjectDescription description = pc as ResourceCollector.Content.LevelObjectDescription;
            
            if (description != null)
            {
                description.Enginereadedobject.RemoveAt(description.Enginereadedobject.Count - 1);
                
                //unload ro
                ResourceCollector.Content.RenderObjectDescription rod = ResourceCollector.PackList.Instance.findobject(description.RODName, ref p) as ResourceCollector.Content.RenderObjectDescription;
                RenderObject obj = rod.Enginereadedobject[rod.Enginereadedobject.Count - 1] as RenderObject;
                rod.Enginereadedobject.RemoveAt(rod.Enginereadedobject.Count - 1);
                if (rod.Enginereadedobject.Count == 0)
                {
                    XNAevents.Add("disposing render object " + description.RODName);
                    IDisposable i = obj;
                    i.Dispose();
                }

                
                //unload material
                ResourceCollector.Content.Material matd = ResourceCollector.PackList.Instance.findobject(description.matname, ref p) as ResourceCollector.Content.Material;
                
                matd.Enginereadedobject.RemoveAt(matd.Enginereadedobject.Count - 1);
                
                for (int i = 0; i < matd.lodMats.Count; i++)
                {
                    for (int j = 0; j < matd.lodMats[i].mats.Count; j++)
                    {
                        ResourceCollector.ImageContent inage = ResourceCollector.PackList.Instance.findobject(matd.lodMats[i].mats[j].DiffuseTextureName, ref p) as ResourceCollector.ImageContent;
                        Content.EngineTexture tex = inage.Enginereadedobject[inage.Enginereadedobject.Count - 1] as Content.EngineTexture;
                        inage.Enginereadedobject.RemoveAt(inage.Enginereadedobject.Count - 1);
                        if (inage.Enginereadedobject.Count == 0)
                        {
                            XNAevents.Add("loading texture " + matd.lodMats[i].mats[j].DiffuseTextureName);
                            tex.Dispose();
                        }
                    }
                }

                if (description.IsRCCMEnabled)
                {
                    //unload raycast
                    ResourceCollector.Content.CollisionMesh cm = ResourceCollector.PackList.Instance.findobject(description.RCCMName, ref p) as ResourceCollector.Content.CollisionMesh;
                    cm.Enginereadedobject.RemoveAt(cm.Enginereadedobject.Count - 1);
                }
            }
            gobject.deleted = true;
        }

        public static LevelObject LevelObjectFromDescription(
            ResourceCollector.Content.LevelObjectDescription description,
            ResourceCollector.Pack packs)
        {
            if (description.Enginereadedobject.Count==0)
            {
                //рендераспект - мб один на несколько объектов
                RenderObject renderaspect = loadro(description, packs);
                Material m = loadMaterial(description.matname, packs);
                //его тоже удалят
                RaycastBoundObject raycastaspect = loadrcbo(description, packs);
                
                #region lalala
                /*switch (description.BehaviourType)
            {
                case ResourceCollector.Content.WorldObjectDescription.objectmovingbehaviourmodel:
                    {
                        throw new Exception("Unsupported behaviour model!");
                    }break;
                case ResourceCollector.Content.WorldObjectDescription.objectphysiccharcontrollerbehaviourmodel:
                    {
                        StillDesign.PhysX.ActorDescription ObjectActorDescription = new StillDesign.PhysX.ActorDescription();
                        
                        if (description.ShapeType == 0)
                        {
                            if (description.PhysXShapeType == 0)
                            {
                                StillDesign.PhysX.BoxShapeDescription boxshape = new StillDesign.PhysX.BoxShapeDescription(description.ShapeSize);
                                boxshape.LocalRotation = Microsoft.Xna.Framework.Matrix.CreateRotationX(Microsoft.Xna.Framework.MathHelper.PiOver2);
                                ObjectActorDescription.Shapes.Add(boxshape);
                            }
                            else if (description.PhysXShapeType == 1)
                            {
                                StillDesign.PhysX.CapsuleShapeDescription capsshape = new StillDesign.PhysX.CapsuleShapeDescription(description.ShapeSize.X, description.ShapeSize.Z);
                                capsshape.LocalRotation = Microsoft.Xna.Framework.Matrix.CreateRotationX(Microsoft.Xna.Framework.MathHelper.PiOver2);
                                ObjectActorDescription.Shapes.Add(capsshape);
                            }
                        }
                        else if (description.ShapeType == 1)
                        {
                            CollisionMesh physicCM = new CollisionMesh();
                            physicCM = packs.GetObject(description.RCCMName, physicCM) as CollisionMesh;

                            ObjectActorDescription.Shapes.Add(physicCM.CreatreConvexShape(scene.Core));
                        }


                        ObjectActorDescription.BodyDescription = new StillDesign.PhysX.BodyDescription(description.Mass);
                        Microsoft.Xna.Framework.Matrix MassCenterMatrix;
                        Microsoft.Xna.Framework.Matrix.CreateTranslation(ref description.CenterOfMass, out MassCenterMatrix);
                        ObjectActorDescription.BodyDescription.MassLocalPose = MassCenterMatrix;

                        ObjectActor = scene.CreateActor(ObjectActorDescription);
                        ObjectActor.RaiseBodyFlag(StillDesign.PhysX.BodyFlag.FrozenRotation);
                        foreach (var c in ObjectActor.Shapes)
                        {
                            c.Group = 31;
                        }

                    } break;
                case ResourceCollector.Content.WorldObjectDescription.objectstaticbehaviourmodel:
                    {
                        
                    }break;
                case ResourceCollector.Content.WorldObjectDescription.objectphysicbehaviourmodel:
                    {
                        StillDesign.PhysX.ActorDescription ObjectActorDescription = new StillDesign.PhysX.ActorDescription();
                        if (description.ShapeType == 0)
                        {
                            if (description.PhysXShapeType == 0)
                            {
                                StillDesign.PhysX.BoxShapeDescription boxshape = new StillDesign.PhysX.BoxShapeDescription(description.ShapeSize);
                                Microsoft.Xna.Framework.Matrix m;
                                Microsoft.Xna.Framework.Vector3 v = description.ShapeRotationAxis;
                                Microsoft.Xna.Framework.Matrix.CreateFromAxisAngle(ref v, description.ShapeRotationAngle, out m);
                                boxshape.LocalRotation = m;
                                
                                ObjectActorDescription.Shapes.Add(boxshape);
                            }
                            else if (description.PhysXShapeType == 1)
                            {
                                StillDesign.PhysX.CapsuleShapeDescription capsshape = new StillDesign.PhysX.CapsuleShapeDescription(description.ShapeSize.X,description.ShapeSize.Z);
                                Microsoft.Xna.Framework.Matrix m;
                                Microsoft.Xna.Framework.Vector3 v = description.ShapeRotationAxis;
                                Microsoft.Xna.Framework.Matrix.CreateFromAxisAngle(ref v, description.ShapeRotationAngle, out m);
                                capsshape.LocalRotation = m;

                                ObjectActorDescription.Shapes.Add(capsshape);
                            }
                        }
                        else if (description.ShapeType == 1)
                        {
                            CollisionMesh physicCM = new CollisionMesh();
                            physicCM = packs.GetObject(description.RCCMName, physicCM) as CollisionMesh;


                            if (description.IsStatic)
                                ObjectActorDescription.Shapes.Add(physicCM.CreateTriangleMeshShape(scene.Core));
                            else
                                ObjectActorDescription.Shapes.Add(physicCM.CreatreConvexShape(scene.Core));
                        }

                        if (description.IsStatic)
                        {
                            ObjectActorDescription.BodyDescription = null;
                        }
                        else
                        {
                            ObjectActorDescription.BodyDescription = new StillDesign.PhysX.BodyDescription(description.Mass);
                            Microsoft.Xna.Framework.Matrix MassCenterMatrix;
                            Microsoft.Xna.Framework.Matrix.CreateTranslation(ref description.CenterOfMass, out MassCenterMatrix);
                            ObjectActorDescription.BodyDescription.MassLocalPose = MassCenterMatrix;
                        }
                        ObjectActor = scene.CreateActor(ObjectActorDescription);
                        if (description.IsStatic)
                        {
                            foreach (var c in ObjectActor.Shapes)
                            {
                                c.Group = 1;
                            }
                        }
                        else
                        {
                            foreach (var c in ObjectActor.Shapes)
                            {
                                c.Group = 31;
                            }
                        }
                        
                        //CONTACT REPORT DISABLED TEMPORARY
                        //ObjectActor.ContactReportFlags = StillDesign.PhysX.ContactPairFlag.All;
                    }break;
                default:
                    {
                        throw new Exception("Unsupported behaviour model!");
                    } break;
            }*/
                #endregion
                
                Logic.BehaviourModel.ObjectBehaviourModel behaviourmodel = null;
                switch (description.BehaviourType)
                {
                    case ResourceCollector.Content.LevelObjectDescription.objectmovingbehaviourmodel:
                        {
                            throw new Exception("Unsupported behaviour model!");
                        } break;
                    case ResourceCollector.Content.LevelObjectDescription.objectphysiccharcontrollerbehaviourmodel:
                        {
                            behaviourmodel = new Logic.BehaviourModel.ObjectPhysicControllerBehaviourModel(/*_actor*/);
                        } break;
                    case ResourceCollector.Content.LevelObjectDescription.objectstaticbehaviourmodel:
                        {
                            behaviourmodel = new Logic.BehaviourModel.ObjectStaticBehaviourModel();
                        } break;
                    case ResourceCollector.Content.LevelObjectDescription.objectphysicbehaviourmodel:
                        {
                            behaviourmodel = new Logic.BehaviourModel.ObjectPhysicBehaviourModel(/*_actor*/);
                        } break;
                    case ResourceCollector.Content.LevelObjectDescription.objectBonerelatedbehaviourmodel:
                        {
                            behaviourmodel = new Logic.BehaviourModel.ObjectBoneRelatedBehaviourModel(/*_actor*/);
                        } break;
                   
                    default:
                        {
                            throw new Exception("Unsupported behaviour model!");
                        } break;
                }
                //её гк удалит
                EditorData ed = new EditorData(description.name, ObjectEditorType.SolidObject);
                LevelObject createdobject = new LevelObject(behaviourmodel, renderaspect, m, raycastaspect, ed);
                description.Enginereadedobject.Add(createdobject);
                return createdobject;
            }
            else
            {
                LevelObject createdobject = description.Enginereadedobject[0] as LevelObject;
                RenderObject ro = loadro(description, packs);
                Material m = loadMaterial(description.matname, packs);


                Logic.BehaviourModel.ObjectBehaviourModel behaviourmodel = null;
                switch (description.BehaviourType)
                {
                    case ResourceCollector.Content.LevelObjectDescription.objectmovingbehaviourmodel:
                        {
                            throw new Exception("Unsupported behaviour model!");
                        } break;
                    case ResourceCollector.Content.LevelObjectDescription.objectphysiccharcontrollerbehaviourmodel:
                        {
                            behaviourmodel = new Logic.BehaviourModel.ObjectPhysicControllerBehaviourModel(/*_actor*/);
                        } break;
                    case ResourceCollector.Content.LevelObjectDescription.objectstaticbehaviourmodel:
                        {
                            behaviourmodel = new Logic.BehaviourModel.ObjectStaticBehaviourModel();
                        } break;
                    case ResourceCollector.Content.LevelObjectDescription.objectphysicbehaviourmodel:
                        {
                            behaviourmodel = new Logic.BehaviourModel.ObjectPhysicBehaviourModel(/*_actor*/);
                        } break;
                    default:
                        {
                            throw new Exception("Unsupported behaviour model!");
                        } break;
                }

                RaycastBoundObject raycastaspect = loadrcbo(description, packs);
                EditorData ed = new EditorData(createdobject.editorAspect.DescriptionName, createdobject.editorAspect.objtype);
                LevelObject createdobject1 = new LevelObject(behaviourmodel, ro, m, raycastaspect, ed);
                description.Enginereadedobject.Add(createdobject1);
                return createdobject1;
            }
        }

        public static TerrainObject SampleTerrain()
        {
            TerrainMesh to = TerrainMesh.GenerateMesh(30, 30, 2.0f);
            TerrainRenderObject tro = new TerrainRenderObject(to);
            tro.isshadowcaster = tro.isshadowreceiver = true;
            RaycastBoundObject rcbo = new RaycastBoundObject(new Logic.SceneGraph.OTBoundingShape(to.Size), null);
            return new TerrainObject(tro, rcbo, new EditorData("sample terrain1", ObjectEditorType.TerrainObject));
        }
        /// <summary>
        /// генерирует террайн расположенный в центре своих лок координат
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static TerrainObject GenerateTerrain(int WidthCount, int LenghtCount, float step, bool shadowcaster, bool shadowreceiver)
        {
            TerrainMesh to = TerrainMesh.GenerateMesh(WidthCount, LenghtCount, step);
            TerrainRenderObject tro = new TerrainRenderObject(to);
            tro.isshadowcaster = shadowcaster;
            tro.isshadowreceiver = shadowreceiver;
            RaycastBoundObject rcbo = new RaycastBoundObject(new Logic.SceneGraph.OTBoundingShape(to.Size), null);
            return new TerrainObject(tro, rcbo, new EditorData("sample terrain", ObjectEditorType.TerrainObject));
        }
    }
}

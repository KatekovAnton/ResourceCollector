using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Microsoft.Xna.Framework;

namespace ResourceCollector.Content
{
    /// <summary>
    /// world presentation of any visible object
    /// how it moves, how it looks
    /// </summary>
    public class LevelObjectDescription:PackContent
    {
        public string matname;

        public string RODName;

        public string RCCMName;

        public bool IsRCCMEnabled;

        public bool IsRCCMAnimated;

        public bool IsAnimated;

        public string CharacterName;

        public int BehaviourType;

        public string PhysicCollisionName;

        public float Mass;

        public int ShapeType;
            //0-physix shape
            //1-collision mesh

        public int PhysXShapeType;
            //0-box
            //1-capsule
            //2-sphere

        public bool IsStatic;

        public Vector3 CenterOfMass;

        public Vector3 ShapeSize;

        public Vector3 ShapeRotationAxis;

        public float ShapeRotationAngle;

        public int RCShapeType;
        public Vector3 RCShapeSize;

        public System.Windows.Forms.TreeNode TreeNode;

        public LevelObjectDescription()
        {
            name = "NewWorldObject" + DateTime.Now.Millisecond.ToString() ;
          //  LODs = new List<Model>();
            this.ShapeType = 1;
            this.loadedformat = this.forsavingformat = ElementType.WorldObjectDescription;
        }

        public void setdata(LevelObjectDescription b)
        {
            name = b.name;

            RCCMName = b.RCCMName;
        }

        public override int loadbody(BinaryReader br, System.Windows.Forms.ToolStripProgressBar toolStripProgressBar)
        {
            long startpos = br.BaseStream.Position;
            matname = br.ReadPackString();
            RODName = br.ReadPackString();
            IsRCCMEnabled = br.ReadBoolean();
            if (IsRCCMEnabled)
                RCCMName = br.ReadPackString();
            else
            {
                RCShapeType = br.ReadInt32();
                RCShapeSize.X = br.ReadSingle();
                RCShapeSize.Y = br.ReadSingle();
                RCShapeSize.Z = br.ReadSingle();
            }
            IsAnimated = br.ReadBoolean();
            if (IsAnimated)
            {
                CharacterName = br.ReadPackString();
                IsRCCMAnimated = br.ReadBoolean();
            }
            BehaviourType = br.ReadInt32();
            switch (BehaviourType)
            {
                case objectmovingbehaviourmodel:
                    { } break;
                case objectphysicbehaviourmodel:
                    {
                        ShapeType = br.ReadInt32();
                        if (ShapeType == 0)
                        {
                            PhysXShapeType = br.ReadInt32(); ;
                            ShapeSize = new Vector3(br.ReadSingle(),
                             br.ReadSingle(),
                            br.ReadSingle());


                            //read shape type, size of shape, rotation axis and angle
                        }
                        if (ShapeType == 1)
                        {
                            PhysicCollisionName = br.ReadPackString();
                        }
                        IsStatic = br.ReadBoolean();
                        if (!IsStatic)
                        {
                            Mass = br.ReadSingle();
                            CenterOfMass.X = br.ReadSingle();
                            CenterOfMass.Y = br.ReadSingle();
                            CenterOfMass.Z = br.ReadSingle();
                        }
                    } break;
                case objectphysiccharcontrollerbehaviourmodel:
                    {
                        ShapeType = br.ReadInt32();
                        if (ShapeType == 0)
                        {
                            PhysXShapeType = br.ReadInt32(); ;
                            ShapeSize = new Vector3(br.ReadSingle(),
                             br.ReadSingle(),
                            br.ReadSingle());


                            //read shape type, size of shape, rotation axis and angle
                        }
                        if (ShapeType == 1)
                        {
                            PhysicCollisionName = br.ReadPackString();
                        }
                        Mass = br.ReadSingle();
                        CenterOfMass.X = br.ReadSingle();
                        CenterOfMass.Y = br.ReadSingle();
                        CenterOfMass.Z = br.ReadSingle();

                    } break;
                case objectstaticbehaviourmodel:
                    { } break;
                case objectBonerelatedbehaviourmodel:
                    { } break;
                case objectrelatedbehaviourmodel:
                    { } break;
                default: break;
            }


            startpos = br.BaseStream.Position - startpos;
            return Convert.ToInt32(startpos);
        }

        public override void calcbodysize(System.Windows.Forms.ToolStripProgressBar targetbar)
        {
            BinaryWriter br = new BinaryWriter(new MemoryStream());
            savebody(br, null);

            size = Convert.ToInt32(br.BaseStream.Length);

        }

        public override System.Windows.Forms.DialogResult createpropertieswindow(Pack p, System.Windows.Forms.TreeView outputtreeview)
        {
            if (Enginereadedobject.Count == 0)
            {
                FormLevelObjectProperties form = new FormLevelObjectProperties(this, outputtreeview);
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (TreeNode != null)
                        TreeNode.Text = name;
                }
                return form.DialogResult;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("object are loaded to scene. cannot to edit");
                return System.Windows.Forms.DialogResult.Cancel;
            }
        }

        public override void calcheadersize()
        {
            headersize = 20 + name.Length;
        }

        public override void loadobjectheader(HeaderInfo hi, BinaryReader br)
        {
            loadedformat = hi.loadedformat;
            name = hi.name;
            forsavingformat = hi.forsavingformat;
            offset = hi.offset;
            size = hi.size;
            headersize = hi.headersize;
            size = br.ReadInt32();
        }

        public override void savebody(BinaryWriter br, System.Windows.Forms.ToolStripProgressBar toolStripProgressBar)
        {
            br.WritePackString(matname);
            br.WritePackString(RODName);
            br.Write(IsRCCMEnabled);
            if (IsRCCMEnabled)
                br.WritePackString(RCCMName);
            else
            {
                br.Write(RCShapeType);
                br.Write(RCShapeSize.X);
                br.Write(RCShapeSize.Y);
                br.Write(RCShapeSize.Z);
            }
            br.Write(IsAnimated);
            if (IsAnimated)
            {
                br.WritePackString(CharacterName);
                br.Write(IsRCCMAnimated);
            }

            br.Write(BehaviourType);
            switch (BehaviourType)
            {
                case objectmovingbehaviourmodel:
                    { } break;
                case objectphysicbehaviourmodel:
                    {
                        br.Write(ShapeType);
                        if (ShapeType == 0)
                        {
                            br.Write(PhysXShapeType);
                            br.Write(ShapeSize.X);
                            br.Write(ShapeSize.Y);
                            br.Write(ShapeSize.Z);


                            //read shape type, size of shape, rotation axis and angle
                        }
                        if (ShapeType == 1)
                        {
                            br.WritePackString(PhysicCollisionName);
                        }
                        br.Write(IsStatic);
                        if (!IsStatic)
                        {
                            br.Write(Mass);
                            br.Write(CenterOfMass.X);
                            br.Write(CenterOfMass.Y);
                            br.Write(CenterOfMass.Z);
                        }
                    } break;
                case objectphysiccharcontrollerbehaviourmodel:
                    {
                        br.Write(ShapeType);
                        if (ShapeType == 0)
                        {
                            br.Write(PhysXShapeType);
                            br.Write(ShapeSize.X);
                            br.Write(ShapeSize.Y);
                            br.Write(ShapeSize.Z);


                            //read shape type, size of shape, rotation axis and angle
                        }
                        if (ShapeType == 1)
                        {
                            br.WritePackString(PhysicCollisionName);
                        }
                        br.Write(Mass);
                        br.Write(CenterOfMass.X);
                        br.Write(CenterOfMass.Y);
                        br.Write(CenterOfMass.Z);
                    } break;
                case objectstaticbehaviourmodel:
                    { } break;
                case objectBonerelatedbehaviourmodel:
                    { } break;
                case objectrelatedbehaviourmodel:
                    { } break;
                default: break;
            }

        }

        public override void saveheader(BinaryWriter bw)
        {
            bw.WritePackString(name);
            bw.Write(offset);
            bw.Write(forsavingformat);
            bw.Write(headersize);
            calcbodysize(null);
            bw.Write(size);
        }

        public override void ViewBasicInfo(System.Windows.Forms.ComboBox comboBox1, System.Windows.Forms.ComboBox comboBox2, System.Windows.Forms.Label label1, System.Windows.Forms.Label label2, System.Windows.Forms.Label label3, System.Windows.Forms.Label label4, System.Windows.Forms.GroupBox groupBox1, System.Windows.Forms.TextBox tb, System.Windows.Forms.Button button2, System.Windows.Forms.Button button1)
        {
            //System.Windows.Forms.MessageBox.Show("Руддыщ, ЦORLD!");
           // createpropertieswindow();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox1.Text = ElementType.ReturnString(this.loadedformat);
            comboBox2.Text = ElementType.ReturnString(this.forsavingformat); 
            groupBox1.Text = tb.Text = name;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            tb.Enabled = true;
            button2.Enabled = true;
            button1.Enabled = true;
            label1.Text = this.number.ToString() ;
            label2.Text = this.offset.ToString();
            label3.Text = this.headersize.ToString();
            label4.Text = this.size.ToString();
            
        }

        public Pack Pack
        {
            get;
            set;
        }
        //движущийся объект(не по законам физики, а собств вычислениями - пули например)
        public const int objectmovingbehaviourmodel = 0;
        const string ObjectMovingBehaviourModel = "Moving behaviour model";
        //недвижимый объект, НЕ ИМЕЮЩИЙ ФИЗ МОДЕЛИ
        public const int objectstaticbehaviourmodel = 1;
        const string ObjectStaticBehaviourModel = "Static behaviour model";
        //любой объект, который обрабатывается физиксом, кроме чарактерконтроллеров
        public const int objectphysicbehaviourmodel = 2;
        const string ObjectPhysicBehaviourModel = "Physic behaviour model";
        //физические чарактерконтроллеры
        public const int objectphysiccharcontrollerbehaviourmodel = 3;
        const string ObjectPhysicCharacterControllerBehaviourModel = "Physic character controller behaviour model";
        //зависимые от парента объекты
        public const int objectrelatedbehaviourmodel = 4;
        const string ObjectRelatedBehaviourModel = "Related behaviour model";
        //зависимые от кости парента объекты
        public const int objectBonerelatedbehaviourmodel = 5;
        const string ObjectBoneRelatedBehaviourModel = "Bone related behaviour model";

        public static string GetName(int hash)
        {
            switch (hash)
            {
                case objectmovingbehaviourmodel:
                    return ObjectMovingBehaviourModel;
                case objectstaticbehaviourmodel:
                    return ObjectStaticBehaviourModel;
                case objectphysicbehaviourmodel:
                    return ObjectPhysicBehaviourModel;
                case objectphysiccharcontrollerbehaviourmodel:
                    return ObjectPhysicCharacterControllerBehaviourModel;
                case objectrelatedbehaviourmodel:
                    return ObjectRelatedBehaviourModel;
                case objectBonerelatedbehaviourmodel:
                    return ObjectBoneRelatedBehaviourModel;
                default :
                    return "What are hell....";
            }

        }
        public static int GetHash(string name)
        {
            switch (name)
            {
                case ObjectMovingBehaviourModel:
                    return objectmovingbehaviourmodel;
                case ObjectStaticBehaviourModel:
                    return objectstaticbehaviourmodel;
                case ObjectPhysicBehaviourModel:
                    return objectphysicbehaviourmodel;
                case ObjectPhysicCharacterControllerBehaviourModel:
                    return objectphysiccharcontrollerbehaviourmodel;
                case ObjectRelatedBehaviourModel:
                    return objectrelatedbehaviourmodel;
                case ObjectBoneRelatedBehaviourModel:
                    return objectBonerelatedbehaviourmodel;
                default:
                    return -1;
            }
        }
        
        
        
    }
}

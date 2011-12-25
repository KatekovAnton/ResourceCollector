using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ResourceCollector.Content;
namespace ResourceCollector
{
    public class Pack
    {
        public List<PackContent> Objects;
        public int headersize;
        static int formayid = 43647457;
        public bool fullsucces;
        public Pack()
        { }
        public void AddObjectsToPack(string filename, System.IO.BinaryReader br)
        {
            var loadedObjectCount = Objects.Count;
            System.IO.FileStream str1 = new System.IO.FileStream(filename, System.IO.FileMode.Open);
            br = new System.IO.BinaryReader(str1);
            
            int foratID = br.ReadInt32();
            headersize += 4;
            if (foratID != formayid)
            {
                MessageBox.Show("Not a pack!!");
                return;
            }
            int objectcount = br.ReadInt32();
            headersize += 4;

        

            for (int i = 0; i < objectcount; i++)
            {
                HeaderInfo hi = new HeaderInfo();
                bool succes = false;
                try
                {
                    PackContent.loadbaseheader(br, hi);
                    succes = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                if (succes)
                {
                    switch (hi.loadedformat)
                    {
                        case ElementType.MeshSkinnedOptimazedForStore:
                        case ElementType.MeshSkinnedOptimazedForLoading:
                            {
                                MeshSkinned msh = new MeshSkinned();
                                Objects.Add(msh);
                                msh.loadobjectheader(hi, br);

                            } break;
                        case ElementType.PNGTexture:
                            {
                                ImageContent image = new ImageContent();
                                Objects.Add(image);
                                image.loadobjectheader(hi, br);
                            } break;
                        case ElementType.Skeleton:
                            {
                                var content = new Skeleton();
                                Objects.Add(content);
                                content.loadobjectheader(hi, br);
                            } break;
                       /* case ElementType.BaseAnimation:
                            {
                                var content = new AnimationBase();
                                Objects.Add(content);
                                content.loadobjectheader(hi, br);
                            } break;*/
                        case ElementType.SkeletonWithAddInfo:
                            {
                                var content = new CharacterStaticInfo();
                                Objects.Add(content);
                                content.loadobjectheader(hi, br);
                            }break;
                        case ElementType.TextureList:
                            {
                                var content = new TextureListContent();
                                content.Pack = this;
                                Objects.Add(content);
                                content.loadobjectheader(hi, br);
                            } break;
                        case ElementType.MeshList:
                            {
                                var content = new MeshListContent();
                                Objects.Add(content);
                                content.Pack = this;
                                content.loadobjectheader(hi, br);
                            } break;
                        case ElementType.CollisionMesh:
                            {
                                var content = new CollisionMesh();
                                Objects.Add(content);
                                content.loadobjectheader(hi, br);
                            } break;
                        case ElementType.LevelObjectDescription:
                            {
                                var content = new LevelObjectDescription();
                                content.Pack = this;
                                Objects.Add(content);
                                content.loadobjectheader(hi, br);
                            } break;
                        case ElementType.RenderObjectDescription:
                            {
                                var content = new RenderObjectDescription();
                                content.pack = this;
                                Objects.Add(content);
                                content.loadobjectheader(hi, br);
                            } break;
                        case ElementType.Material:
                            {
                                var content = new Material();
                                content.pack = this;
                                Objects.Add(content);
                                content.loadobjectheader(hi, br);
                            } break;
                        case ElementType.LevelContent  :
                            {
                                var content = new LevelContent();
                                content.pack = this;
                                Objects.Add(content);
                                content.loadobjectheader(hi, br);
                            } break;
                        default:
                            {
                                MissingObject msh = new MissingObject(i);
                                msh.pack = this;
                                msh.loadobjectheader(hi, br);
                                Objects.Add(msh);
                                //br.BaseStream.Seek(hi.headersize - 16 - hi.name.Length, System.IO.SeekOrigin.Current);
                            } break;
                    }

                    var lastObject = Objects[Objects.Count - 1];

                    headersize += hi.headersize;
                    lastObject.number = Objects.Count - 1;
                   
                }
                else
                {
                    Objects = null;
                   // MessageBox.Show("cannot read file: wrong basic header data at element " + i.ToString());
                    br.Close();
                    fullsucces = false;
                   
                    return;
                }
            }
            //treeView1.Nodes.Add(root);
            headersize = (int)br.BaseStream.Position;
            for (int i = loadedObjectCount; i < loadedObjectCount+objectcount; i++)
            {
                    br.BaseStream.Seek(headersize + Objects[i].offset, System.IO.SeekOrigin.Begin);
                    Objects[i].loadbody(br);
                    Objects[i].SuccessedReadBody = true;
              
            }

            br.Close();

            fullsucces = true;
        }
        public string filename;
        public void Init(string filename, System.IO.BinaryReader br)
        {
            this.filename = filename;
            headersize = 0;
            Objects = new List<PackContent>();
           
            AddObjectsToPack(filename, br);
        }
        public void Init()
        {

            fullsucces = true;
            headersize = 0;
            Objects = new List<PackContent>();
            fullsucces = true;
        }
        public void recalculateparameters()
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                Objects[i].calcheadersize();
                Objects[i].calcbodysize();
            }
            if (Objects.Count > 0)
            {
                Objects[0].offset = 0;
                if (Objects.Count > 1)
                    for (int i = 1; i < Objects.Count; i++)
                    {
                        Objects[i].offset = Objects[i - 1].offset + Objects[i - 1].size;
                    }
            }
        }
        public void recalculateparameters(int firstoffset, ToolStripProgressBar toolStripProgressBar1)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                Objects[i].calcheadersize();
                Objects[i].calcbodysize();
            }
            Objects[0].offset = firstoffset;
            if (Objects.Count > 1)
                for (int i = 1; i < Objects.Count; i++)
                {
                    Objects[i].offset = Objects[i - 1].offset + Objects[i - 1].size;
                }
        }
        public void Save(string fln)
        {
            recalculateparameters();
            if (System.IO.File.Exists(fln))
                System.IO.File.Delete(fln);
            System.IO.FileStream str1;
            str1 = new System.IO.FileStream(fln, (System.IO.File.Exists(fln) ? System.IO.FileMode.Create : 0) | System.IO.FileMode.CreateNew);
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(str1);

            bw.Write(Pack.formayid);
            bw.Write(Objects.Count);
            for (int i = 0; i < Objects.Count; i++)
            {
                Objects[i].saveheader(bw);
            }
            for (int i = 0; i < Objects.Count; i++)
            {
                Objects[i].savebody(bw);
            }
            bw.Close();
        }
        public static int imgindex(int type)
        {
            switch (type)
            {
                case ElementType.BaseAnimation:
                    return 0;
                case ElementType.Character:
                    return 1;
                case ElementType.PNGTexture:
                    return 3;
                case ElementType.MeshSkinnedOptimazedForLoading:
                case ElementType.MeshSkinnedOptimazedForStore:
                    return 4;
                case ElementType.CollisionMesh:
                    return 14;
                case ElementType.Skeleton:
                case ElementType.SkeletonWithAddInfo:
                    return 5;
                case ElementType.MissingObject:
                    return 7;
                case ElementType.TextureList:
                    return 8;
                case ElementType.MeshList:
                    return 9;
                case ElementType.RenderObjectDescription:
                    return 12;
                case ElementType.StringList:
                    return 10;
                case ElementType.Material:
                    return 11;
                case ElementType.LevelObjectDescription:
                    return 13;
                default: return 6;
            }
        }
        public void DropElement(int number)
        {
            Objects.RemoveAt(number);
        }

        public bool rename(int Elementnumver, string newname)
        {
            for (int i = 0; i < Objects.Count; i++)
            {
                if (newname == Objects[i].name && i != Elementnumver)
                    return false;
            }
            Objects[Elementnumver].name = newname;
            return true;
        }

        public void Attach(PackContent obj)
        {
            Objects.Add(obj);

        /*    var node = new TreeNode(obj.name);
            node.Name = node.Text = obj.name;
            node.ImageIndex = node.SelectedImageIndex = Pack.imgindex(obj.loadedformat);

            treeView.Nodes[0].Nodes.Add(node);*/
            obj.number = Objects.Count - 1;

            obj.offset = 0;
           // FormMainPackExplorer.Instance.UpdateData();
        }

        public PackContent getobject(string name)
        {
            for(int i=0;i<Objects.Count;i++)
                if (Objects[i].name == name)
                {
                    return Objects[i];
                }
            return null;
        }

        public PackContent[] GetObjects(string[] names)
        {
            PackContent[] result = new PackContent[names.Length];
            for (int d = 0; d < names.Length; d++)
                for (int i = 0; i < Objects.Count; i++)
                    if (Objects[i].name == names[d])
                    {
                        result[d] = Objects[i];
                    }
            return result;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;

namespace ResourceCollector.Content
{
    public class RenderObjectDescription: PackContent
    {
        System.Windows.Forms.TreeNode TreeNode;
        public Pack pack;
        //отбрасывает ли тени
        public bool IsShadowCaster = false;

        //принимает ли тени(затеняется или нет)
        public bool IsShadowReceiver = false;

        public bool NeedRotate;
        public bool isTransparent;
        public bool isSelfIllumination;
      //  public string matname;

        public void addlod()
        {
            LODs.Add(new Model());
        }

        public RenderObjectDescription(System.Windows.Forms.TreeNode _TreeNode)
        {
            TreeNode = _TreeNode;
            LODs = new List<Model>();
        }

        public RenderObjectDescription()
        {
           // this.ShapeType = 1;
            LODs = new List<Model>();
            this.loadedformat = this.forsavingformat = ElementType.RenderObjectDescription;
        }

        public void setdata(RenderObjectDescription b)
        {
            name = b.name;

            this.LODs = new List<Model>();
            for (int i = 0; i < LODs.Count; i++)
            {
                this.LODs.Add(new Model(b.LODs[i].subsets.ToArray()));
            }
           // RCCMName = b.RCCMName;
            this.IsShadowCaster = b.IsShadowCaster;
            this.IsShadowReceiver = b.IsShadowReceiver;





        }

        //к меш листу надо ассоциировать текстуру. это субсет. при загрузке мешлист группируется в один меш.
        //набор субсетов - это модель одного лода. рендеробжект - набор нескольких моделей(разной детализации)
        public class SubSet : ICloneable
        {
            public string[] MeshNames;
  


            public SubSet(string[] meshnames)
            {
                MeshNames = new string[meshnames.Length];
                meshnames.CopyTo(MeshNames, 0);
  
            }
            public object Clone()
            {
                return new SubSet(this.MeshNames);
            }
            public override string ToString()
            {
                return ("Count of meshes: " + MeshNames.Length.ToString());
            }
        }

        public class Model
        {
            public List<SubSet> subsets
            {
                get;
                private set;
            }
            public Model()
            {
                subsets = new List<SubSet>();
            }
            public Model(SubSet[] array)
            {
                subsets = new List<SubSet>(array);
            }

        }

        public List<Model> LODs
        {
            get;
            private set;
        }

        public override void calcbodysize()
        {
            BinaryWriter br = new BinaryWriter(new MemoryStream());

          //  br.WritePackString(matname);
            br.Write(LODs.Count);

            for (int i = 0; i < LODs.Count; i++)
            {
                br.Write(LODs[i].subsets.Count);

                for (int j = 0; j < LODs[i].subsets.Count; j++)
                {
                    br.Write(LODs[i].subsets[j].MeshNames.Length);
                    for (int n = 0; n < LODs[i].subsets[j].MeshNames.Length; n++)
                    {
                        br.WritePackString(LODs[i].subsets[j].MeshNames[n]);
                    }
                //    br.WritePackString(LODs[i].subsets[j].TextureName);

                }

            }


            br.Write(IsShadowCaster);
            br.Write(IsShadowReceiver);
            br.Write(NeedRotate);
            br.Write(isTransparent);
            br.Write(isSelfIllumination);
            size = Convert.ToInt32(br.BaseStream.Length);
        }

        public override void calcheadersize()
        {
            headersize = 20 + name.Length;
        }

        public override System.Windows.Forms.DialogResult createpropertieswindow(Pack p, System.Windows.Forms.TreeView outputtreeview)
        {
            if (Enginereadedobject.Count == 0)
            {
                FormRenderObjectProperties form = new FormRenderObjectProperties(this, outputtreeview);

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

        public override int loadbody(System.IO.BinaryReader br)
        {
            long startpos = br.BaseStream.Position;
         //   matname = br.ReadPackString();
            int lodcount = br.ReadInt32();
            LODs = new List<Model>();
            for (int i = 0; i < lodcount; i++)
            {
                SubSet[] LodSubset = new SubSet[br.ReadInt32()];
                for (int j = 0; j < LodSubset.Length; j++)
                {
                    string[] names = new string[br.ReadInt32()];
                    for (int n = 0; n < names.Length; n++)
                        names[n] = br.ReadPackString();
                    LodSubset[j] = new SubSet(names);
                }
                LODs.Add(new Model(LodSubset));
            }

            IsShadowCaster = br.ReadBoolean();
            IsShadowReceiver = br.ReadBoolean();
            NeedRotate = br.ReadBoolean();
            isTransparent = br.ReadBoolean();
            isSelfIllumination = br.ReadBoolean();
            startpos = br.BaseStream.Position - startpos;
            return Convert.ToInt32(startpos);
        }

        public override void loadobjectheader(HeaderInfo hi, System.IO.BinaryReader br)
        {
            loadedformat = hi.loadedformat;
            name = hi.name;
            forsavingformat = hi.forsavingformat;
            offset = hi.offset;
            size = hi.size;
            headersize = hi.headersize;
            size = br.ReadInt32();
        }
    
        public override void savebody(System.IO.BinaryWriter br)
        {
           // br.WritePackString(matname);
            br.Write(LODs.Count);

            for (int i = 0; i < LODs.Count; i++)
            {
                br.Write(LODs[i].subsets.Count);
                for (int j = 0; j < LODs[i].subsets.Count; j++)
                {
                    br.Write(LODs[i].subsets[j].MeshNames.Length);
                    for (int n = 0; n < LODs[i].subsets[j].MeshNames.Length; n++)
                    {
                        br.WritePackString(LODs[i].subsets[j].MeshNames[n]);
                    }
                   // br.WritePackString(LODs[i].subsets[j].TextureName);
                }
            }
            
                br.Write(IsShadowCaster);
                br.Write(IsShadowReceiver);
                br.Write(NeedRotate);
                br.Write(isTransparent);
                br.Write(isSelfIllumination);
        }

        public override void saveheader(System.IO.BinaryWriter bw)
        {
            bw.WritePackString(name);
            bw.Write(offset);
            bw.Write(forsavingformat);
            bw.Write(headersize);
            calcbodysize();
            bw.Write(size);
        }

        public override void ViewBasicInfo(System.Windows.Forms.ComboBox comboBox1, System.Windows.Forms.ComboBox comboBox2, System.Windows.Forms.Label label1, System.Windows.Forms.Label label2, System.Windows.Forms.Label label3, System.Windows.Forms.Label label4, System.Windows.Forms.GroupBox groupBox1, System.Windows.Forms.TextBox tb, System.Windows.Forms.Button button2, System.Windows.Forms.Button button1)
        {
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
            label1.Text = this.number.ToString();
            label2.Text = this.offset.ToString();
            label3.Text = this.headersize.ToString();
            label4.Text = this.size.ToString();
        }
    }
}

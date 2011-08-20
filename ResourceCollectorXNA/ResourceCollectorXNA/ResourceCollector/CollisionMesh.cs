using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;
namespace ResourceCollector.Content
{
    public class CollisionMesh:PackContent
    {
        public Vector3[] Vertices
        {
            get;
            protected set;
        }

        public int[] Indices
        {
            get;
            protected set;
        }
        public void mult(float coef)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] *= coef;
            }
        }

        public void Compress()
        {
            List<Vector3> vertices = new List<Vector3>();
            for (int i = 0; i < Vertices.Length; i++)
            {
                bool needadd = true;
                for (int j = 0; j < vertices.Count; j++)
                {
                    if (vertices[j].Near(Vertices[i]))
                    {
                        needadd = false;
                        break;
                    }
                }
                if (needadd)
                    vertices.Add(Vertices[i]);
            }

            if (Vertices.Length != vertices.Count)
            {
                int[] newindexes = new int[Indices.Length];

                for (int i = 0; i < Indices.Length; i++)
                {
                    bool found = false;
                    for (int j = 0; j < i; j++)
                    {
                        if (Indices[i] == Indices[j])
                        {
                            newindexes[i] = newindexes[j];
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        for (int j = 0; j < vertices.Count; j++)
                        {
                            if (vertices[j].Near(Vertices[Indices[i]]))
                            {
                                newindexes[i] = j;
                                break;
                            }
                        }
                    }
                }
                Indices = newindexes;
                Vertices = vertices.ToArray();
            }

            
            
        }
        public CollisionMesh()
        {
            loadedformat = forsavingformat = ElementType.CollisionMesh;
        }
        public CollisionMesh(Mesh m)
        {
            if (m.BufferVertex == null || m.BufferIndex == null)
                m.GenerateOptForLoading(null);
            Vertices = new Vector3[m.BufferVertex.Length];
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertices[i] = m.BufferVertex[i].pos;
            }
            Indices = new int[m.BufferIndex.Length];
            m.BufferIndex.CopyTo(Indices, 0);
            loadedformat = forsavingformat = ElementType.CollisionMesh;
        }
     

   

        public override void calcbodysize(System.Windows.Forms.ToolStripProgressBar targetbar)
        {
            size = Vertices.Length * 12 + Indices.Length * 4 + 8;
        }
        public override System.Windows.Forms.DialogResult createpropertieswindow(Pack p, System.Windows.Forms.TreeView outputtreeview)
        {
            FormCollisionMeshProperties fcmp = new FormCollisionMeshProperties(this);
            fcmp.ShowDialog();
            return System.Windows.Forms.DialogResult.OK;
        }
        public override void calcheadersize()
        {
            headersize = 16 + OtherFunctions.GetPackStringLengthForWrite(name);
        }
        public override int loadbody(BinaryReader br, System.Windows.Forms.ToolStripProgressBar toolStripProgressBar)
        {
            long t = br.BaseStream.Position;
            Vertices = new Vector3[br.ReadInt32()];
            for (int i = 0; i < Vertices.Length; i++)
                Vertices[i] = new Vector3(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
            Indices = new int[br.ReadInt32()];
            for (int i = 0; i < Indices.Length; i++)
                Indices[i] = br.ReadInt32();
            return Convert.ToInt32(br.BaseStream.Position - t);

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
            br.Write(Vertices.Length);
            for (int i = 0; i < Vertices.Length; i++)
            {
                br.Write(Vertices[i].X);
                br.Write(Vertices[i].Y);
                br.Write(Vertices[i].Z);
                
            }
            br.Write(Indices.Length);
            for (int i = 0; i < Indices.Length; i++)
                br.Write(Indices[i]);
        }
        public override void saveheader(BinaryWriter bw)
        {
            bw.WritePackString(name);
            bw.Write(offset);
            bw.Write(ElementType.CollisionMesh);

            calcheadersize();
            bw.Write(headersize);
            calcbodysize(null);
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

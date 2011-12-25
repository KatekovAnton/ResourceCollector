using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace ResourceCollector
{
    public class TextureListContent:PackContent
    {
        public System.Windows.Forms.TreeNode TreeNode
        {
            get;
            set;
        }

        public List<string> Names
        {
            protected set;
            get;
        }

        public TextureListContent()
        {
            Names = new List<string>();
            this.loadedformat = this.forsavingformat = ElementType.TextureList;
        }

        public override void calcbodysize()
        {
            size = name.Length+8;
            for(int i =0;i< Names.Count;i++)
            {
                size += Names[i].Length+4;
            }
        }

        public override System.Windows.Forms.DialogResult createpropertieswindow(Pack p, System.Windows.Forms.TreeView outputtreeview)
        {
            FormTextureList form = new FormTextureList(this,outputtreeview);

            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {


                name = form.TextBoxName.Text + "\0";
                if (TreeNode != null)
                    TreeNode.Text = name;
                if (form.generatedimages != null)
                {
                    for (int i = 0; i < form.generatedimages.Length; i++)
                    {
                        Pack.Attach(form.generatedimages[i]);
                    }
                    FormMainPackExplorer.Instance.UpdateData();
                }
            }
            return  form.DialogResult;
        }

        public override void calcheadersize()
        {
            headersize = 20 + name.Length;
        }

        public override int loadbody(BinaryReader br)
        {
            var name = br.ReadPackString();
            int meshCount = br.ReadInt32();
            int size = name.Length + 9;
            Names = new List<string>(meshCount);
            for (int i = 0; i < meshCount; i++)
            {
                var meshName = br.ReadPackString();
                Names.Add(meshName);
                size += meshName.Length + 4;
            }
            return size;
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

        public override void savebody(BinaryWriter bw)
        {
            bw.WritePackString(name);
            bw.Write(Names.Count);
            foreach (var meshName in Names)
            {
                bw.WritePackString(meshName);

            }
        }

        public override void saveheader(BinaryWriter bw)
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
    }
   
}

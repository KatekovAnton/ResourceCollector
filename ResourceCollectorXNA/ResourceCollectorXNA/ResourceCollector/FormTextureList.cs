using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ResourceCollector
{
    public partial class FormTextureList : Form
    {
        public ImageContent[] generatedimages
        {
            get;
            private set;
        }
        public TextureListContent List
        {
            get;
            set;
        }
        public TextBox TextBoxName
        {
            get { return textBox1; }
        }

        TreeView ttt;
        public FormTextureList(TextureListContent tl, TreeView tv)
        {
            InitializeComponent();
            List = tl;
            ttt = tv;
            listBox1.Items.Clear();
            listBox1.Items.AddRange(List.Pack.Objects.FindAll(o =>
                o.loadedformat == ElementType.PNGTexture ).ConvertAll(o => o.name).ToArray());

            listBox2.Items.Clear();
            listBox2.Items.AddRange(List.Names.ToArray());
            listBox1.Enabled = button1.Enabled = button2.Enabled = List.Names.Count == 0;
            if (List.name != null)
                textBox1.Text = List.name;
            else
                textBox1.Text = "New_texture_list";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var i = listBox1.SelectedIndex;
            if (i != -1)
            {
                var n = listBox1.SelectedItem as string;
                listBox2.Items.Add(n);
                List.Names.Add(n);
                listBox1.Items.Remove(n);

                if (listBox1.Items.Count > i)
                {
                    listBox1.SelectedIndex = i;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && listBox2.SelectedItem != null)
            {
                var n = listBox2.SelectedItem as string;
                listBox1.Items.Add(n);
                List.Names.Remove(n);
                listBox2.Items.Remove(n);
            }
        }
       
        private void button3_Click(object sender, EventArgs e)
        {
            if(listBox2.Items.Count!=0)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Enabled = false;
            int[] denumenators = { 2, 4};
            string name = listBox1.SelectedItem.ToString();
            ImageContent baseimage = null;
            for (int i = 0; i < List.Pack.Objects.Count; i++)
            {
                if (List.Pack.Objects[i].name == name)
                {
                    baseimage = List.Pack.Objects[i] as ImageContent;
                    break;
                }
            }
            if (baseimage != null)
            {
                Image bbb = Image.FromStream(new System.IO.MemoryStream(baseimage.data));
                generatedimages = new ImageContent[denumenators.Length];
                List.Names.Clear();
                List.Names.Add(name);
                listBox2.Items.Clear();
                listBox2.Items.Add(name);
                for (int i = 0; i < denumenators.Length; i++)
                {
                    generatedimages[i] = new ImageContent(name.TrimEnd('\0') + "_lod" + i.ToString() + "\0", new Bitmap(bbb, new Size(bbb.Width / denumenators[i], bbb.Height / denumenators[i])));
                    List.Names.Add(generatedimages[i].name);
                    listBox2.Items.Add(generatedimages[i].name);
                    listBox1.Items.Add(generatedimages[i].name);
                }
            }
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            button4.Enabled = listBox1.SelectedIndex != -1;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.Enabled)
            {
                string name = listBox1.SelectedItem.ToString();
                ImageContent baseimage = null;
                for (int i = 0; i < List.Pack.Objects.Count; i++)
                {
                    if (List.Pack.Objects[i].name == name)
                    {
                        baseimage = List.Pack.Objects[i] as ImageContent;
                        break;
                    }
                }
                if (baseimage != null)
                    baseimage.createpropertieswindow(List.Pack, ttt);
            }
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox2.Enabled)
            {
                string name = listBox2.SelectedItem.ToString();
                ImageContent baseimage = null;
                for (int i = 0; i < List.Pack.Objects.Count; i++)
                {
                    if (List.Pack.Objects[i].name == name)
                    {
                        baseimage = List.Pack.Objects[i] as ImageContent;
                        break;
                    }
                }
                if (baseimage != null)
                    baseimage.createpropertieswindow(List.Pack, ttt);
            }
        }


         


       
    }
}

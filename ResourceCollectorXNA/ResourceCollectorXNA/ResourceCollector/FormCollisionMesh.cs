using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ResourceCollector.Content
{
    public partial class FormCollisionMesh : Form
    {
        public CollisionMesh cm;
        Pack p;
        public FormCollisionMesh(TreeView tv, Pack _p)
        {
            p = _p;
            InitializeComponent();

        //    for (int i = 0; i < p.packs.Count;i++ )
                listBox1.Items.AddRange(p.Objects.FindAll(o =>
                   o.loadedformat == ElementType.MeshOptimazedForStore || o.loadedformat == ElementType.MeshOptimazedForLoading).ConvertAll(o => o.name).ToArray());
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
          
            PackContent pa = p.getobject(listBox1.SelectedItem.ToString());
            Mesh m = null;
            try
            {
                if (pa != null)
                    m = pa as Mesh;
            }
            catch (Exception ex)
            { }

            if (m != null)
            {
                cm = new CollisionMesh(m);
                cm.name = textBox1.Text + "\0";
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }
    }
}

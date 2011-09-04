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
    public partial class FormMeshCombiner : Form
    {
        MeshSkinned createdMesh;
        public bool removemeshes;
        public FormMeshCombiner()
        {
            InitializeComponent();
            createdMesh = new MeshSkinned();
            textBox1.Text = "Newmesh" + DateTime.Now.Millisecond.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            if (textBox1.Text != "")
                createdMesh.name = textBox1.Text + "\0";
            else
                createdMesh.name = "Newmesh" + DateTime.Now.Millisecond.ToString();
            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            removemeshes = checkBox1.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

    }
}

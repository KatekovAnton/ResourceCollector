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
    public partial class FormCollisionMeshProperties : Form
    {
        CollisionMesh cm;
        public FormCollisionMeshProperties(CollisionMesh _cm)
        {
            InitializeComponent();
            cm = _cm;
            label4.Text = cm.Vertices.Length.ToString();
            label3.Text = cm.Indices.Length.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            cm.Compress();
            //cm.mult(1.05f);
            label4.Text = cm.Vertices.Length.ToString();
            label3.Text = cm.Indices.Length.ToString();
            button1.Enabled = true;
        }
    }
}

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
    public partial class MeshImportList : Form
    {
        public Pack p;
        string s;
        public MeshImportList(string str)
        {
            InitializeComponent();
            s = str;
           
        }

        private void MeshImportList_Shown(object sender, EventArgs e)
        {
            p = new Pack();

            p.Init(s, null, toolStripProgressBar1, toolStripProgressBar2, treeView1);
            TreeNodeCollection c = treeView1.Nodes[0].Nodes;
           TreeNode[] nodes = new TreeNode[c.Count];

           c.CopyTo(nodes,0);
            treeView1.Nodes.Clear();
            treeView1.Nodes.AddRange(nodes);
            toolStripProgressBar1.Value = toolStripProgressBar2.Value = 100;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < treeView1.Nodes.Count; i++)
                treeView1.Nodes[i].Checked = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < treeView1.Nodes.Count; i++)
                treeView1.Nodes[i].Checked = false;
        }
    }
}

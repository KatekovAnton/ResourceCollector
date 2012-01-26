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
            ResourceCollectorXNA.ConsoleWindow.SetOUT(richTextBox1);
        }

        private void MeshImportList_Shown(object sender, EventArgs e)
        {
            p = new Pack();
            
            p.Init(s, null);

            TreeNode root = new TreeNode(s);
            root.Text = s;
            treeView1.Nodes.Add(root);
            for (int o = 0; o < p.Objects.Count; o++)
            {
                TreeNode tn = new TreeNode(p.Objects[o].name);
                tn.Text = p.Objects[o].name;
                root.Nodes.Add(tn);
            }

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

        private void MeshImportList_Load(object sender, EventArgs e)
        {

        }

        private void MeshImportList_FormClosing(object sender, FormClosingEventArgs e)
        {
            ResourceCollectorXNA.ConsoleWindow.SetOUT(null);
        }

        private void treeView1_NodeMouseHover(object sender, TreeNodeMouseHoverEventArgs e)
        {
            if (md) e.Node.Checked = true;
            if (mr) e.Node.Checked = false;
        }
        bool md;
        bool mr;

        private void treeView1_MouseMove(object sender, MouseEventArgs e)
        {
            md = e.Button == System.Windows.Forms.MouseButtons.Left;
            mr = e.Button == System.Windows.Forms.MouseButtons.Right;
            richTextBox1.Text = md.ToString();
        }
    }
}

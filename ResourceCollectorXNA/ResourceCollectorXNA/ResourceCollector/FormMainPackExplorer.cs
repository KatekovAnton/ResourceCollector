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
    public partial class FormMainPackExplorer : Form
    {
        PackList packs = new PackList();
        PackContent CurrentContent;
        Point contentcoords = new Point(-1, -1);
        System.IO.BinaryReader br;
        TreeNode lastselectednode;
        public static FormMainPackExplorer Instance;
        public FormMainPackExplorer()
        {
            Instance = this;
            InitializeComponent();

            groupBox1.Height = Height - groupBox1.Location.Y - 90;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
          //  try
          //  {
                ofd.Filter = "Паки *.pack|*.pack";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ResourceCollectorXNA.Engine.GameEngine.Instance.CreateNewLevel();
                    ResourceCollectorXNA.Engine.GameEngine.Instance.UpdateLevelPart();
                    treeView1.Nodes.Clear();
                    ClearInterface();
                    packs.packs = new List<Pack>();
                    packs.AddPack(ofd.FileName, br, toolStripProgressBar1, toolStripProgressBar2, treeView1);
                }
                if (!packs.SuccessLast)
                    ClearInterface();
                ofd.Dispose();
            /*}
            catch (Exception ex)
            {
                if (br != null)
                {
                    br.Close();
                }
                MessageBox.Show(ex.ToString());
                packs.packs = new List<Pack>();
                treeView1.Nodes.Clear();
                ClearInterface();
            }*/
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                int a = this.Width - groupBox1.Width - 30;
                a = a > splitContainer1.Panel1MinSize ? a : splitContainer1.Panel1MinSize;
                splitContainer1.SplitterDistance = a;
                groupBox1.Height = Height - groupBox1.Location.Y - 90;

            }
            catch (Exception ex)
            { }
        }

        void treeView1NodeMouseClick(TreeNode tn)
        {
            CurrentContent = packs.findobject(tn.Text, ref contentcoords);
            if (CurrentContent != null)
                CurrentContent.ViewBasicInfo(comboBox1, comboBox2, label1, label2, label3, label4, groupBox1, textBox1, button2, button1);
            else
                ClearInterface();
        }

        private void ClearInterface()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox1.Text = "";
            comboBox2.Text = "";
            groupBox1.Text = "";
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            textBox1.Enabled = false;
            button2.Enabled = false;
            //button1.Enabled = true;
            label1.Text = "";
            label2.Text = "";
            label3.Text = "";
            label4.Text = "";
            textBox1.Text = "";
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1NodeMouseClick(e.Node);
            if (e.Button == MouseButtons.Right)
                if (CurrentContent != null && CurrentContent.contextmenu != null)
                    CurrentContent.contextmenu.Show(treeView1, e.Location);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (packs.packs.Count != 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                    packs.Save(0, toolStripProgressBar2, sfd.FileName);
            }
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            switch (CurrentContent.loadedformat)
            {
                case ElementType.MeshOptimazedForLoading:
                    {
                        CurrentContent.forsavingformat = ElementType.ReturnFormat(comboBox2.Text);
                    } break;
                case ElementType.MeshOptimazedForStore:
                    {
                        CurrentContent.forsavingformat = ElementType.ReturnFormat(comboBox2.Text);
                    } break;
                default: break;
            }
        }

        private void treeView1_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Up || e.KeyCode == Keys.Down) && treeView1.SelectedNode != null)
                treeView1NodeMouseClick(treeView1.SelectedNode);
        }

        private void aboutProgrammToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 ab1 = new AboutBox1();
            ab1.ShowDialog();
        }

        private void openAndAddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    ClearInterface();
                    packs.AddPack(ofd.FileName, br, toolStripProgressBar1, toolStripProgressBar2, treeView1);
                }
                ofd.Dispose();
            }
            catch (Exception ex)
            {
                br.Close();
                MessageBox.Show(ex.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CurrentContent = packs.findobject(textBox1.Text+"\0", ref contentcoords);
            if (CurrentContent!= null && CurrentContent.Enginereadedobject.Count == 0)
            {
                if (MessageBox.Show("Are you shure want to drop this object (operation cannot to be canceled!)?", "Droping object", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (contentcoords.X != -1 && contentcoords.Y != -1)
                    {
                        packs.Drop(contentcoords.X, contentcoords.Y);
                        ClearInterface();
                        treeView1.Nodes.Remove(treeView1.SelectedNode);
                        treeView1NodeMouseClick(treeView1.SelectedNode);
                    }
                }
            }
            else
            {
                MessageBox.Show("cannot to remove this object becouse its loaded to the engine");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (packs.packs[contentcoords.X].Objects[contentcoords.Y].Enginereadedobject.Count == 0)
            {
                if (!packs.packs[contentcoords.X].rename(contentcoords.Y, textBox1.Text + "\0"))
                    MessageBox.Show("Wrong name");
                else
                {
                    CurrentContent.ViewBasicInfo(comboBox1, comboBox2, label1, label2, label3, label4, groupBox1, textBox1, button2, button1);
                    treeView1.SelectedNode.Text = CurrentContent.name;
                }
            }
            else
            {
                textBox1.Text = packs.packs[contentcoords.X].Objects[contentcoords.Y].name;
                MessageBox.Show("Unable to rename object becouse its loaded to the engine");
            }
        }

        private void treeView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                lastselectednode = treeView1.SelectedNode;
                string ttt = treeView1.SelectedNode.Text;
                if (!ttt.EndsWith("\0") && treeView1.SelectedNode.ImageIndex != 2)
                    MessageBox.Show(ttt + " NOT ENDS WITH \\0!!");
                CurrentContent = packs.findobject(treeView1.SelectedNode.Text, ref contentcoords);
                if (CurrentContent != null)
                {
                    if (CurrentContent.createpropertieswindow(packs.packs[0], treeView1) == System.Windows.Forms.DialogResult.OK)
                    {
                        //Set any changed data
                        treeView1.SelectedNode.Text = CurrentContent.name;
                    }
                    CurrentContent.ViewBasicInfo(comboBox1, comboBox2, label1, label2, label3, label4, groupBox1, textBox1, button2, button1);
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormNewElement f = new FormNewElement(packs, treeView1);
            f.ShowDialog();
        }

        private void newPackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearInterface();
            packs.AddEmptyPack(toolStripProgressBar1, toolStripProgressBar2, treeView1);
        }

        private void addPackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    packs.packs[0].AddObjectsToPack(ofd.FileName, br, toolStripProgressBar1, toolStripProgressBar2, treeView1);
                }
                ofd.Dispose();
            }
            catch (Exception ex)
            {
                if (br != null)
                {
                    br.Close();
                }
                MessageBox.Show(ex.ToString());
            }
        }

        private void animationToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (packs.packs.Count != 0)
            {
                FormAnimationTools fff = new FormAnimationTools();
                fff.ShowDialog();
                if (fff.skelet != null && fff.checkBox1.Checked)
                {
                    packs.packs[0].Attach(fff.skelet, treeView1);
                }
            }
            else
            {
                MessageBox.Show("No packs loaded");
            }
        }
        
        private void treeView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (treeView1.SelectedNode!= null && treeView1.SelectedNode != lastselectednode)
            {
                lastselectednode = treeView1.SelectedNode;
                treeView1NodeMouseClick(treeView1.SelectedNode);
            }
        }
      
        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {
            if (treeView1.SelectedNode != null && treeView1.SelectedNode != lastselectednode)
            {
                lastselectednode = treeView1.SelectedNode;
                treeView1NodeMouseClick(treeView1.SelectedNode);
            }

            if (e.KeyCode == Keys.Delete)
            {
                CurrentContent = packs.findobject(textBox1.Text + "\0", ref contentcoords);
                if (MessageBox.Show("Are you shure want to drop this object (operation cannot to be canceled!)?", "Droping object", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (contentcoords.X != -1 && contentcoords.Y != -1)
                    {
                        packs.Drop(contentcoords.X, contentcoords.Y);
                        ClearInterface();
                        treeView1.Nodes.Remove(treeView1.SelectedNode);
                        treeView1NodeMouseClick(treeView1.SelectedNode);
                    }
                }
            }
        }

        private void importPackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ResourceCollector.Content.MeshImportList m = new Content.MeshImportList(ofd.FileName);
                if (m.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    for (int i = 0; i < m.treeView1.Nodes.Count; i++)
                    {
                        if (m.treeView1.Nodes[i].Checked)
                        {
                            packs.packs[0].Attach(m.p.Objects[i], treeView1);
                            if (m.p.Objects[i].forsavingformat == ElementType.MeshOptimazedForStore)
                                m.p.Objects[i].forsavingformat = ElementType.MeshOptimazedForLoading;
                        }
                    }
                }
            }
        }

        private void combineMeshesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMeshCombiner fmc = new FormMeshCombiner();
            if (fmc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
 
            }
        }

        private void xNAMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormXNAMonitor fxm = new FormXNAMonitor();
            fxm.ShowDialog();
        }
    }
}

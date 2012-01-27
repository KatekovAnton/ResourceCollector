using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

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
        List<int> visibleformats;

        public FormMainPackExplorer()
        {
            Instance = this;
            InitializeComponent();

            groupBox2.Height = Height - groupBox2.Location.Y - 90;
            visibleformats = new List<int>();
            CheckVisibleFormats();
            AppConfiguration.ReadProperties();
        }

        public void CheckVisibleFormats()
        {
            visibleformats.Clear();
            visibleformats.Add(ElementType.MissingObject);
            if (checkBox1.Checked)
            {
                visibleformats.Add(ElementType.MeshSkinnedOptimazedForStore);
                visibleformats.Add(ElementType.MeshSkinnedOptimazedForLoading);
            }
            if (checkBox2.Checked)
            {
                visibleformats.Add(ElementType.PNGTexture);
                visibleformats.Add(ElementType.TextureList);
            }
            if (checkBox3.Checked)
                visibleformats.Add(ElementType.LevelObjectDescription);
            if (checkBox4.Checked)
                visibleformats.Add(ElementType.RenderObjectDescription);
            if (checkBox5.Checked)
                visibleformats.Add(ElementType.CollisionMesh);
            if (checkBox6.Checked)
            {
                visibleformats.Add(ElementType.Skeleton);
                visibleformats.Add(ElementType.SkeletonWithAddInfo);
            }
            if (checkBox7.Checked)
                visibleformats.Add(ElementType.Material);
            if (checkBox8.Checked)
                visibleformats.Add(ElementType.LevelContent);
            if (checkBox9.Checked)
                visibleformats.Add(ElementType.ParticelRenderObjectDescription);
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender != null)
            {
                OpenFileDialog ofd = new OpenFileDialog();

                ofd.Filter = "Паки *.pack|*.pack";
                string path = AppConfiguration.PackPlaceFolder;
                if (path.Length > 2)
                    ofd.InitialDirectory = path;
                if (ofd.ShowDialog() == DialogResult.OK)
                  add_pack(ofd.FileName);
                ofd.Dispose();
                UpdateData();
            }
            else
            {
               
            }

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
                string str = AppConfiguration.PackPlaceFolder;
                if (str.Length > 2)
                    sfd.InitialDirectory = str;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    packs.Save(0, sfd.FileName);
                    AppConfiguration.PackPlaceFolder = System.IO.Path.GetDirectoryName(sfd.FileName);
                }
            }
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            switch (CurrentContent.loadedformat)
            {
                case ElementType.MeshSkinnedOptimazedForLoading:
                    {
                        CurrentContent.forsavingformat = ElementType.ReturnFormat(comboBox2.Text);
                    } break;
                case ElementType.MeshSkinnedOptimazedForStore:
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
                    packs.AddPack(ofd.FileName, br);
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
            UpdateData();
        }

        private void addPackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    packs.packs[0].AddObjectsToPack(ofd.FileName, br);
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
                    packs.packs[0].Attach(fff.skelet);
                    FormMainPackExplorer.Instance.UpdateData();
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
                    if (packs.packs.Count == 0)
                    {
                        MessageBox.Show("Open Pack First");
                        openToolStripMenuItem_Click(sender, e);
                    }


                    for (int i = 0; i < m.treeView1.Nodes.Count; i++)
                    {
                        if (m.treeView1.Nodes[i].Checked)
                        {
                            packs.packs[0].Attach(m.p.Objects[i]);
                            if (m.p.Objects[i].forsavingformat == ElementType.MeshSkinnedOptimazedForStore)
                                m.p.Objects[i].forsavingformat = ElementType.MeshSkinnedOptimazedForLoading;
                        }
                    }
                    FormMainPackExplorer.Instance.UpdateData();
                }
            }
        }

        public void UpdateData()
        {
            treeView1.Nodes.Clear();
            autoCompleteComboBox1.Items.Clear();

            bool enabled_ = (packs!=null) && (packs.packs!=null) && (packs.packs.Count > 0);
                toolStripButton1.Enabled = enabled_;
                scripts_toolmenu.Enabled = enabled_;

            if (enabled_)
            for (int i = 0; i < packs.packs.Count; i++)
            {
                
                TreeNode root = new TreeNode(packs.packs[i].filename);
                root.ImageIndex = root.SelectedImageIndex = 2;
                root.Text = packs.packs[i].filename;
                
                for (int j = 0; j < packs.packs[i].Objects.Count; j++)
                {
                    if(!(visibleformats.Contains(packs.packs[i].Objects[j].loadedformat) ||(visibleformats.Contains(packs.packs[i].Objects[j].forsavingformat))))
                        continue;
                    string name = packs.packs[i].Objects[j].name;
                    if (Regex.Match(name, regex).Success)
                    {
                        PackContent pc = packs.packs[i].Objects[j];
                        var node = new TreeNode(pc.name);
                        node.Name = node.Text = pc.name;
                        node.ImageIndex = node.SelectedImageIndex = Pack.imgindex(pc.loadedformat);
                        root.Nodes.Add(node);
                        autoCompleteComboBox1.Items.Add(pc.name);
                    }
                }
                treeView1.Nodes.Add(root);
                root.Expand();
            }
        }

        private void combineMeshesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormMeshCombiner fmc = new FormMeshCombiner();
            Point contc = new Point();
            if (fmc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if(fmc._removemeshes)
                {
                    for (int i = 0; i < fmc._meshesForCombine.Count;i++ )
                        if (fmc._meshesForCombine[i].Enginereadedobject.Count == 0)
                        {
                            object o= packs.findobject(fmc._meshesForCombine[i].name, ref  contc);
                            if (contc.X != -1 && contc.Y != -1)
                                packs.Drop(contc.X, contc.Y);
                        }
                    ClearInterface();
                    UpdateData();
                }
            }
        }

        private void xNAMonitorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormXNAMonitor fxm = new FormXNAMonitor();
            fxm.ShowDialog();
        }

        private void FormatUpdate()
        {
            CheckVisibleFormats();
            UpdateData();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            FormatUpdate();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            FormatUpdate();
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            FormatUpdate();
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            FormatUpdate();
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            FormatUpdate();
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            FormatUpdate();
        }

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            FormatUpdate();
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            FormatUpdate();
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            FormatUpdate();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {

        }

        private void FormMainPackExplorer_Load(object sender, EventArgs e)
        {
           ResourceCollectorXNA.SE.Instance.ExScript("_onload");

           List<string> resc = new List<string>();

           //добавь свои паки и радуйся жизни!!! ))) или грузи из конфига...
           resc.Add( @"D:\projects\ULJANIK493DEMO\PhysX test2\PhysX test2\Data\Ship.pack");

           foreach (string s in resc)
                rescentToolStripMenuItem.DropDownItems.Add(s, null , new EventHandler(rescent_click));
        }

        private void rescent_click(object sender, EventArgs e)
        {
            add_pack(((ToolStripItem)sender).Text);
        }

        void add_pack(string FileName)
        {
                ResourceCollectorXNA.Engine.GameEngine.Instance.CreateNewLevel();
                ResourceCollectorXNA.Engine.GameEngine.Instance.UpdateLevelPart();
                treeView1.Nodes.Clear();
                ClearInterface();
                packs.packs = new List<Pack>();
                packs.AddPack(FileName, br);
                AppConfiguration.PackPlaceFolder = System.IO.Path.GetDirectoryName(FileName);
                ResourceCollectorXNA.SE.Instance.scriptscope.SetVariable("pack", packs.packs[0]);
                ResourceCollectorXNA.SE.Instance.scriptscope.SetVariable("tv", treeView1);
                ResourceCollectorXNA.SE.Instance.scriptscope.SetVariable("objects", packs.packs[0].Objects);
                if (!packs.SuccessLast) ClearInterface();
                UpdateData();
        }
        
        private void formToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenScriptsForm("^_[a-z]", true);// добавить все, которые не начинаются с одного подчеркивания
        }

        private void toolStripTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                OpenScriptsForm(toolStripTextBox1.Text, false);
            }
        }

        public void OpenScriptsForm(string str, bool invert)
        {
            FormScripts fs = new FormScripts(Eggs.Filter(ResourceCollectorXNA.SE.Instance, str, invert));
            fs.ShowDialog();
        }

        string regex = "";
        private void textBox2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                regex = textBox2.Text;
                FormatUpdate();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void autoCompleteComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                treeView1.SelectedNode = treeView1.Nodes[0].Nodes[autoCompleteComboBox1.Text];
            }
            catch { }
        }
        Regex selreg;
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            selreg = new Regex(textBox4.Text);
        }

        private void textBox4_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    if (selreg != null)
                    {
                        bool succ = false;

                        for (int i = treeView1.SelectedNode == null ? 0 : treeView1.SelectedNode.Index + 1; i < treeView1.Nodes[0].Nodes.Count; i++)
                            if (selreg.Match(treeView1.Nodes[0].Nodes[i].Name).Success) { treeView1.SelectedNode = treeView1.Nodes[0].Nodes[i]; succ = true; break; }
                        if (!succ) treeView1.SelectedNode = treeView1.Nodes[0].Nodes[0];
                    }
                }
            }
            catch { }
        }

        

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ResourceCollectorXNA.ConsoleWindow.TraceMessage(ResourceCollectorXNA.Program.help);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void textureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string name = ((ToolStripMenuItem)sender).Text;

            FormNewElement f = new FormNewElement(packs, treeView1);
            switch (name)
            {
                case "Texture": f.button2_Click(null, null); break;
                case "CollisionMesh": f.button4_Click(null, null); break;
                case "LevelObject": f.button7_Click(null, null); break;
                case "RenderObject": f.button6_Click(null, null); break;
                case "Particle": f.button11_Click(null, null); break;
                case "Material": f.button10_Click(null, null); break;
                    
                default: break;
            }
        }

        
    }
}

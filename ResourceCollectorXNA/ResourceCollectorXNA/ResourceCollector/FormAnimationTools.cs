using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace ResourceCollector
{
    public partial class FormAnimationTools : Form
    {
        public CharacterStaticInfo skelet;

        private bool finalstepredy;
        private SkeletonViewer skeletonviever;
        private SkeletonInformation skinfo;

        public FormAnimationTools()
        {
            InitializeComponent();
            groupBox2.Enabled = false;
            this.WindowState = FormWindowState.Maximized;
        }

        public FormAnimationTools(Skeleton s)
        {
            InitializeComponent();


            treeView1.Nodes.Clear();
            skelet = null;
            textBox1.Text = label3.Text = string.Empty;
            skelet = new CharacterStaticInfo(s);
            if (s.name != null)
                textBox1.Text = s.name;
            label3.Text = "Count of bones: " + skelet.baseskelet.bones.Length.ToString();

            TreeNode root = new TreeNode(skelet.baseskelet.Root.Name + string.Format(" [{0}]", skelet.baseskelet.Root.index.ToString()));
            addchildrens(skelet.baseskelet.Root, root, 0);
            treeView1.Nodes.Add(root);
            treeView1.ExpandAll();
            OutInfo();
            button6.Enabled = true;

            button5.Enabled = true;
            skeletonviever = new SkeletonViewer(skelet.baseskelet);
            skinfo = new SkeletonInformation(skelet.baseskelet);
            groupBox2.Enabled = true;
            checkBox1.Enabled = false;
            checkBox1.Checked = false;
            button1.Enabled = button4.Enabled = button9.Enabled = false;
           // this.WindowState = FormWindowState.Maximized;
        }

        public FormAnimationTools(CharacterStaticInfo s, Pack p, System.Windows.Forms.TreeView outputtreeview)
        {
            InitializeComponent();

            treeView1.Nodes.Clear();
            skelet = null;

            skelet = s;

            if (s.name != null)
                textBox1.Text = s.name;
            label3.Text = "Count of bones: " + skelet.baseskelet.bones.Length.ToString();


            TreeNode root = new TreeNode(skelet.baseskelet.Root.Name + string.Format(" [{0}]", skelet.baseskelet.Root.index.ToString()));
            addchildrens(skelet.baseskelet.Root, root, 0);
            treeView1.Nodes.Add(root);
            treeView1.ExpandAll();
            finalstepredy = ((s.TopIndexes.Length + s.BottomIndexes.Length) == s.baseskelet.bones.Length);
            button6.Enabled = !finalstepredy;
            button8.Enabled = !finalstepredy;
            OutInfo();
            button5.Enabled = true;
            skeletonviever = new SkeletonViewer(skelet.baseskelet);
            skinfo = new SkeletonInformation(skelet.baseskelet);
            groupBox2.Enabled = true;
            checkBox1.Enabled = false;
            checkBox1.Checked = false;
            button1.Enabled = button4.Enabled = button9.Enabled = false;


            if (finalstepredy)
            {
                listBox1.Items.Clear();
                if (skelet.BottomGraph == null && skelet.BottomIndexes.Length > 0)
                    skelet.BottomGraph = new AnimationGraph("Bottom", skelet.BottomIndexes);
                if (skelet.BottomGraph != null)
                    listBox1.Items.Add(skelet.BottomGraph);

                if (skelet.TopGraph == null && skelet.TopIndexes.Length > 0)
                    skelet.TopGraph = new AnimationGraph("Top", skelet.TopIndexes);
                if (skelet.TopGraph != null)
                    listBox1.Items.Add(skelet.TopGraph);
            }
            //this.WindowState = FormWindowState.Maximized;
        }

        private void addchildrens(Bone b,TreeNode _tn,int level)
        {
            b.level = level;
            for(int t = 0;t<b.Childrens.Count;t++)
            {
                TreeNode tn = new TreeNode(b.Childrens[t].Name + string.Format(" [{0}]",b.Childrens[t].index.ToString()));
                addchildrens(b.Childrens[t], tn,level+1);
                _tn.Nodes.Add(tn);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormSelectSkeletonType fst = new FormSelectSkeletonType();
            if (fst.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (fst.type == ElementType.Skeleton)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        treeView1.Nodes.Clear();
                        skelet = null;
                        textBox1.Text = label3.Text = string.Empty;
                        try
                        {
                            skelet = new CharacterStaticInfo(Skeleton.FromStream(new System.IO.BinaryReader(new System.IO.FileStream(ofd.FileName, System.IO.FileMode.Open))));
                            textBox1.Text = ofd.FileName;
                            label3.Text = "Count of bones: " + skelet.baseskelet.bones.Length.ToString();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), "Error in loading skeleton");
                            return;
                        }
                    }
                    else
                        return;
                    TreeNode root = new TreeNode(skelet.baseskelet.Root.Name + string.Format(" [{0}]", skelet.baseskelet.Root.index.ToString()));
                    addchildrens(skelet.baseskelet.Root, root, 0);
                    treeView1.Nodes.Add(root);
                    treeView1.ExpandAll();
                    OutInfo();
                    button6.Enabled = true;
                }
                if (fst.type == ElementType.SkeletonWithAddInfo)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        treeView1.Nodes.Clear();
                        skelet = null;

                        skelet = new CharacterStaticInfo();
                        try
                        {
                            skelet.loadbody(new System.IO.BinaryReader(new System.IO.FileStream(ofd.FileName, System.IO.FileMode.Open)), null);
                            textBox1.Text = ofd.FileName;
                            label3.Text = "Count of bones: " + skelet.baseskelet.bones.Length.ToString();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), "Error in loading skeleton");
                            return;
                        }
                    }
                    else
                        return;
                    TreeNode root = new TreeNode(skelet.baseskelet.Root.Name + string.Format(" [{0}]", skelet.baseskelet.Root.index.ToString()));
                    addchildrens(skelet.baseskelet.Root, root, 0);
                    treeView1.Nodes.Add(root);
                    treeView1.ExpandAll();
                    button6.Enabled = false;
                    OutInfo();
                    finalstepredy = true;
                    listBox1.Items.Clear();
                    if (skelet.BottomGraph == null && skelet.BottomIndexes.Length > 0)
                        skelet.BottomGraph = new AnimationGraph("Bottom", skelet.BottomIndexes);
                    if (skelet.BottomGraph != null)
                        listBox1.Items.Add(skelet.BottomGraph);

                    if (skelet.TopGraph == null && skelet.TopIndexes.Length > 0)
                        skelet.TopGraph = new AnimationGraph("Top", skelet.TopIndexes);
                    if (skelet.TopGraph != null)
                        listBox1.Items.Add(skelet.TopGraph);
                }
            }
            button5.Enabled = true;
            skeletonviever = new SkeletonViewer(skelet.baseskelet);
            skinfo = new SkeletonInformation(skelet.baseskelet);
            groupBox2.Enabled = true;
            checkBox1.Enabled = true;
            checkBox1.Checked = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            skeletonviever.ShowDialog();
        }

        private void OutInfo()
        {
            textBox2.Text = "Top indexes:\r\n" + arraytostring(skelet.TopIndexes) + "\r\n--------------\r\nBottom indexes:\r\n" + arraytostring(skelet.BottomIndexes) +
                        string.Format("\r\n--------------\r\nWeapon index = {0}\r\nHead index = {1}\r\nTop root index = {2}\r\nBottom root index = {3}\r\nRoot index = {4}",
                skelet.WeaponIndex, skelet.HeadIndex, skelet.TopRootIndex, skelet.BottomRootIndex, skelet.RootIndex);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ListView lv = new ListView(skelet.baseskelet);
            if(lv.ShowDialog() == System.Windows.Forms.DialogResult.OK && lv.indexes != null)
            {
                if (skelet.BottomIndexes == null)
                {
                    skelet.TopIndexes = lv.indexes;
                }
                else
                {
                    List<int> finalindexes = new List<int>(lv.indexes);
                    for (int i = 0; i < skelet.BottomIndexes.Length; i++)
                    {
                        for (int u = 0; u < finalindexes.Count; u++)
                        {
                            if (finalindexes[u] == skelet.BottomIndexes[i])
                            {
                                finalindexes.RemoveAt(u);
                                break;
                            }
                        }
                    }
                    skelet.TopIndexes = finalindexes.ToArray();
                }
                textBox2.Text = arraytostring(skelet.TopIndexes);
                button7.Enabled = true;
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            ListView lv = new ListView(skelet.baseskelet);
            if(lv.ShowDialog() == System.Windows.Forms.DialogResult.OK && lv.indexes != null)
            {
                if (skelet.TopIndexes == null)
                {
                    skelet.BottomIndexes = lv.indexes;
                }
                else
                {
                    List<int> finalindexes = new List<int>(lv.indexes);
                    for (int i = 0; i < skelet.TopIndexes.Length; i++)
                    {
                        for (int u = 0; u < finalindexes.Count; u++)
                        {
                            if (finalindexes[u] == skelet.TopIndexes[i])
                            {
                                finalindexes.RemoveAt(u);
                                break;
                            }
                        }
                    }
                    skelet.BottomIndexes = finalindexes.ToArray();
                }
                textBox2.Text = arraytostring(skelet.BottomIndexes);
               // button7.Enabled = false;
                button8.Enabled = true;
            }
        }

        private string arraytostring<T>(T array) where T : IEnumerable
        {
            string s = "";
            if (array != null)
                foreach (var y in array)
                {
                    s += skelet.baseskelet.bones[(int)y].ToString().TrimEnd('\0') + ";  ";
                }
            return s;
        }

        private class ListView:Form
        {
            Skeleton skelet;
            public ListView(Skeleton s)
            {
                InitializeComponent();
                skelet = s;
                for (int i = 0; i < skelet.bones.Length; i++)
                {
                    listBox1.Items.Add(skelet.bones[i]);
                }
            }
            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);
            }

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent()
            {
                this.listBox1 = new System.Windows.Forms.ListBox();
                this.button1 = new System.Windows.Forms.Button();
                this.SuspendLayout();
                // 
                // listBox1
                // 
                this.listBox1.FormattingEnabled = true;
                this.listBox1.Location = new System.Drawing.Point(12, 12);
                this.listBox1.Name = "listBox1";
                this.listBox1.Size = new System.Drawing.Size(217, 368);
                this.listBox1.TabIndex = 0;
                listBox1.SelectedIndexChanged += new EventHandler(listBox1_SelectedIndexChanged);
                // 
                // button1
                // 
                this.button1.Location = new System.Drawing.Point(12, 386);
                this.button1.Name = "button1";
                this.button1.Size = new System.Drawing.Size(217, 23);
                this.button1.TabIndex = 1;
                this.button1.Text = "OK";
                this.button1.UseVisualStyleBackColor = true;
                button1.Click += new EventHandler(button1_Click);
                // 
                // ListView
                // 
                this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
                this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
                this.ClientSize = new System.Drawing.Size(243, 421);
                this.Controls.Add(this.button1);
                this.Controls.Add(this.listBox1);
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.Name = "ListView";
                this.RightToLeftLayout = true;
                this.ShowIcon = false;
                this.ShowInTaskbar = false;
                this.Text = "ListView";
                this.ResumeLayout(false);

            }
           
            public int[] indexes;
            void button1_Click(object sender, EventArgs e)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            void addchildrens(Bone b, List<int> boneindexes)
            {
                boneindexes.Add(b.index);
                for (int i = 0; i < b.Childrens.Count; i++)
                    addchildrens(b.Childrens[i], boneindexes);

            }
            void listBox1_SelectedIndexChanged(object sender, EventArgs e)
            {
                List<int> boneindexes = new List<int>();
                string s = listBox1.SelectedItem.ToString();
                addchildrens(skelet.bones[skelet.map[s.Split(new char[]{' '})[1]]],boneindexes);
                indexes = boneindexes.ToArray();
            }

            #endregion

            private System.Windows.Forms.ListBox listBox1;
            private System.Windows.Forms.Button button1;

        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
         //   treeView1.ExpandAll();
        }

        private void button8_Click(object sender, EventArgs e)
        {

            int b = skelet.TopIndexes[0];
            for (int i = 0; i < skelet.TopIndexes.Length; i++)
            {
                if (skelet.TopIndexes[i] < b)
                    b = skelet.TopIndexes[i];
            }
            skelet.TopRootIndex = b;

            b = skelet.BottomIndexes[0];
            for (int i = 0; i < skelet.BottomIndexes.Length; i++)
            {
                if (skelet.BottomIndexes[i] < b)
                    b = skelet.BottomIndexes[i];
            }
            skelet.BottomRootIndex = b;

            skelet.RootIndex = skelet.baseskelet.RootIndex;

            foreach (var bb in skelet.baseskelet.bones)
            {
                if (bb.Name.Contains("WEAPON"))
                {
                    skelet.WeaponIndex = bb.index;
                    break;
                }
            }

            foreach (var bb in skelet.baseskelet.bones)
            {
                if (bb.Name.Contains("HEAD"))
                {
                    skelet.HeadIndex = bb.index;
                    break;
                }
            }

            textBox2.Text = string.Format("Weapon index = {0}\r\nHead index = {1}\r\nTop root index = {2}\r\nBottom root index = {3}\r\nRoot index = {4}",
                skelet.WeaponIndex, skelet.HeadIndex, skelet.TopRootIndex, skelet.BottomRootIndex, skelet.RootIndex);
            button8.Enabled = false;
            finalstepredy = true;

            listBox1.Items.Clear();
            if (skelet.BottomGraph == null && skelet.BottomIndexes.Length > 0)
                skelet.BottomGraph = new AnimationGraph("Bottom", skelet.BottomIndexes);
            if (skelet.BottomGraph != null)
                listBox1.Items.Add(skelet.BottomGraph);

            if (skelet.TopGraph == null && skelet.TopIndexes.Length > 0)
                skelet.TopGraph = new AnimationGraph("Top", skelet.TopIndexes);
            if (skelet.TopGraph != null)
                listBox1.Items.Add(skelet.TopGraph);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 20)
            {
                MessageBox.Show("Too long text");
                return;
            }
            if (MessageBox.Show("Set name \"" + textBox2.Text + "\"?", "set name", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                skelet.name = textBox2.Text + "\0";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(new System.IO.FileStream(sfd.FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None));
                skelet.savebody(bw, null);
            }
        }
        /// <summary>
        /// LOADING ANIMATION
        /// </summary>
        /// <param name="sender">Thats it</param>
        /// <param name="e">Thats it</param>
        private void button2_Click(object sender, EventArgs e)
        {
            List<string> actionKeys = new List<string>();
            
            if (listBox1.SelectedItem !=null && skelet != null && skelet.BottomIndexes != null && skelet.TopIndexes != null && finalstepredy)
            {

                AnimationGraph editedGraph = listBox1.SelectedItem as AnimationGraph;
                
                List<string> addKeys = new List<string>();
                foreach (object g in listBox1.Items)
                {
                    AnimationGraph graph = g as AnimationGraph;
                    if (g != listBox1.SelectedItem)
                    {
                        string[] graphkeys = graph.getAllEvents();
                        foreach (string key in graphkeys)
                            if (!addKeys.Contains(key))
                                addKeys.Add(key);
                    }
                }
                AnimGrafEditor dlg = new AnimGrafEditor(editedGraph, skelet, addKeys.ToArray());
                if (dlg.ShowDialog() == DialogResult.OK)
                {                   
                    listBox1.Items[listBox1.SelectedIndex] = dlg.AnimGraf;
                    if (dlg.AnimGraf.description.CompareTo(skelet.TopGraph.description) == 0)
                    {
                        skelet.TopGraph = dlg.AnimGraf;
                    }
                    else if(dlg.AnimGraf.description.CompareTo(skelet.BottomGraph.description) == 0)
                    {
                        skelet.BottomGraph = dlg.AnimGraf;
                    }
                   
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                button2.Enabled = true;
            }
        }

        private void FormAnimationTools_Load(object sender, EventArgs e)
        {

        }
    }
}

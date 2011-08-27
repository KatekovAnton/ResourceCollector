using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ResourceCollectorXNA.Content;

namespace ResourceCollector
{
    public partial class AnimGrafEditor : Form
    {
        public AnimationGraph AnimGraf;
        public List<PictureBox> nodesAnim;
        public ResourceCollector.Skeleton skeleton;
        public Point cur, curnew;
        bool selected;
        CharEvents chev;
        public AnimGrafEditor(ResourceCollector.Skeleton _skeleton)
        {
            InitializeComponent();
            this.skeleton = _skeleton;
            nodesAnim = new List<PictureBox>();
            selected = false;
            chev = new CharEvents();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            FullAnim curanim;// = new Content.FullAnim();
            AnimationNode curnode;// = new Content.FullAnim();
           AddAnim dlg = new AddAnim();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                curanim = FullAnim.FromStream(dlg.dlg.OpenFile(), skeleton);
                curanim.name = dlg.textBox2.Text;
                curnode = new AnimationNode(curanim);
                curnode.Tag = listBox1.Items.Count;
                listBox1.Items.Add(curnode);
                addNodesAnim(curanim);             
            }
        }
        private void addNodesAnim(Animation curanim)
        {
            PictureBox pic = new PictureBox();
            pic.Parent = pictureBox1;
            pic.MouseDown += new MouseEventHandler(pictureBox2_MouseDown);
            pic.MouseMove += new MouseEventHandler(pictureBox2_MouseMove);
            pic.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);
            pic.MouseDoubleClick += new MouseEventHandler(pictureBox1_MouseDoubleClick);
            pic.Click += new EventHandler(pictureBox1_Click);
            pic.BorderStyle = BorderStyle.FixedSingle;
            Bitmap btm = (Bitmap)imageList1.Images[0].Clone();
            Graphics gr = Graphics.FromImage(btm);
            pic.SizeMode = PictureBoxSizeMode.AutoSize;
            gr.DrawString(curanim.name, new System.Drawing.Font("Times New Roman", 8), Brushes.Black, 5, btm.Height / 2 - 4);
            gr.Dispose();
            pic.Image = btm;
            pic.Tag = nodesAnim.Count;

            pic.Location = new Point(nodesAnim.Count * (imageList1.Images[0].Width + 5), nodesAnim.Count * (imageList1.Images[0].Height + 3));
            nodesAnim.Add(pic);
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                nodesAnim[listBox1.SelectedIndex].Dispose();
                nodesAnim.RemoveAt(listBox1.SelectedIndex);
                AnimationNode an = (AnimationNode)listBox1.Items[listBox1.SelectedIndex];
                RemoveNodeEventsByAnimationNode(an);
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                TagRefresh();
                DrawNodeEvent();
            }
        }
        private void RemoveNodeEventsByAnimationNode(AnimationNode an )
        {
            if (an.NodeEvents != null)
            {
                foreach (NodeEvent ne in an.NodeEvents)
                {
                    listBox2.Items.Remove(ne);
                }
            }
            List<NodeEvent> toremove = new List<NodeEvent>();
            foreach(Object ss in listBox2.Items)
            {
                NodeEvent ne = (NodeEvent)ss;
                if ((int)ne.associatedNode.Tag == (int)an.Tag)
                {
                    toremove.Add(ne);
                    //listBox2.Items.Remove(ne);
                }
            }
            foreach (NodeEvent ss in toremove)
            {
                listBox2.Items.Remove(ss);
            }
          
        }

        private void TagRefresh()
        {
            for (int i = 0; i < nodesAnim.Count; i++)
            {
                nodesAnim[i].Tag = i;
                AnimationNode an = (AnimationNode)listBox1.Items[i];
                an.Tag = i;
                nodesAnim[i].Tag = i;
            }
        }

        private void drawnode(int select)
        {
            for (int i = 0; i < nodesAnim.Count; i++)
            {
                Bitmap btm;
                PictureBox pic = nodesAnim[i];
                AnimationNode curanim = (AnimationNode)listBox1.Items[i];
                if (select == i)
                {
                    btm = (Bitmap)imageList1.Images[1].Clone();                    
                }
                else
                {
                     btm = (Bitmap)imageList1.Images[0].Clone();
                }
                Graphics gr = Graphics.FromImage(btm);
                gr.DrawString(curanim.animation.name, new System.Drawing.Font("Times New Roman", 8), Brushes.Black, 5, btm.Height / 2 - 4);
                gr.Dispose();
                pic.Image = btm;
            }

        }

        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            selected = true;
            cur = Cursor.Position;
            PictureBox ss = (PictureBox)sender;
            curnew = ss.Location;
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {           
            if (selected)
            { 
                PictureBox ss = (PictureBox)sender;
                ss.Location = new Point(Cursor.Position.X - cur.X + curnew.X, Cursor.Position.Y - cur.Y + curnew.Y);
                pictureBox1.Invalidate();
                DrawNodeEvent();
            }
        }



        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            selected = false;
            PictureBox ss = (PictureBox)sender;  
            curnew = ss.Location;
            DrawNodeEvent();

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox ss = (PictureBox)sender;
            ss.Select();
            listBox1.SelectedIndex = (int)ss.Tag;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            drawnode(listBox1.SelectedIndex);            
        }

        private void button1_Click(object sender, EventArgs e)
        { 
            Object[] obj=new Object[listBox1.Items.Count];
            listBox1.Items.CopyTo(obj,0);
            NodeEventEditor needlg = new NodeEventEditor(chev, obj);
            if (needlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                
                NodeEvent nodeev = new NodeEvent(needlg.textBox1.Text, (AnimationNode)listBox1.Items[needlg.comboBox1.SelectedIndex], 
                    (AnimationNode)listBox1.Items[needlg.comboBox2.SelectedIndex],(CharacterEvent)chev.listBox1.Items[needlg.CharEventIndex]);

                listBox2.Items.Add(nodeev);
            }
            DrawNodeEvent();


        }

        public void DrawNodeEvent()
        {
            Bitmap btm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics gr = Graphics.FromImage(btm);
            for (int i = 0; i < listBox2.Items.Count; i++)
            {
                NodeEvent ne=(NodeEvent)listBox2.Items[i];
                PictureBox n1=nodesAnim[(int) ne.parent.Tag];
                PictureBox n2=nodesAnim[(int) ne.associatedNode.Tag];

                int x1 = n1.Location.X + (n1.Width / 2);
                int y1 = n1.Location.Y + (n1.Height / 2);
                int x2 = n2.Location.X + (n2.Width / 2);
                int y2 = n2.Location.Y + (n2.Height / 2);

                int x3 = x2-((x2-x1)/5);
                int y3 = y2 - ((y2 - y1) / 5);

                if (i == listBox2.SelectedIndex)
                {
                    gr.DrawLine(new Pen(Brushes.Red, 3), new Point(x1, y1), new Point(x2, y2));
                    
                }
                else
                {
                    gr.DrawLine(new Pen(Brushes.Green,3),new Point( x1,y1),new Point(x2,y2));

                }
                gr.DrawLine(new Pen(Brushes.Black, 6), new Point(x3, y3), new Point(x2, y2));
            }
            gr.Dispose();
            pictureBox1.Image = btm;
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawNodeEvent();
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            Object[] obj = new Object[listBox1.Items.Count];
            listBox1.Items.CopyTo(obj, 0);
            NodeEventEditor needlg = new NodeEventEditor(chev, obj);
            needlg.comboBox1.SelectedIndex = (int)pic.Tag;
            if (needlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                NodeEvent nodeev = new NodeEvent(needlg.textBox1.Text, (AnimationNode)listBox1.Items[needlg.comboBox1.SelectedIndex],
                    (AnimationNode)listBox1.Items[needlg.comboBox2.SelectedIndex], (CharacterEvent)chev.listBox1.Items[needlg.CharEventIndex]);

                listBox2.Items.Add(nodeev);
            }
            DrawNodeEvent();
        }

        public void BildAGraf()
        {
            AnimationNode[] res = new AnimationNode[listBox1.Items.Count];
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                int count = 0;
                AnimationNode an = (AnimationNode)listBox1.Items[i];
                an.NodeEvents = null;
                for (int j = 0; j < listBox2.Items.Count; j++)
                {
                    NodeEvent ne = (NodeEvent)listBox2.Items[j];
                    
                    if (((int)ne.parent.Tag) == ((int)an.Tag) /*|| ((int)ne.associatedNode.Tag) == ((int)an.Tag)*/)
                    {
                        NodeEvent[] ss = new NodeEvent[count + 1];
                        if (an.NodeEvents != null)
                        {
                            an.NodeEvents.CopyTo(ss, 0);
                        }
                        ss[count] = ne;
                        count++;
                        an.NodeEvents = ss;
                    }
                }
                res[i] = an;
            }
            AnimGraf = new AnimationGraph(res);

        }
        private void button5_Click(object sender, EventArgs e)
        {
            BildAGraf(); 
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "AnimGraf |*.agr";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(sfd.OpenFile());
                AnimationGraph.AnimationGraffToStream(AnimGraf,bw);
                bw.Close();

            }

        }
      

        private void button6_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter(sfd.OpenFile());                
                bw.Write(nodesAnim.Count);
                for (int i = 0; i < nodesAnim.Count;i++ )
                {
                    bw.Write(nodesAnim[i].Location.X);
                    bw.Write(nodesAnim[i].Location.Y);
                }
                bw.Close();

            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                System.IO.BinaryReader bw = new System.IO.BinaryReader(ofd.OpenFile());
                int count =bw.ReadInt32();
                if(count<=nodesAnim.Count);
                for (int i = 0; i < count; i++)
                {
                  int x=  bw.ReadInt32();
                  int y= bw.ReadInt32();
                    nodesAnim[i].Location=new Point(x,y);
                }
                bw.Close();
                DrawNodeEvent();
                drawnode(listBox1.SelectedIndex);

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            int lenth = 0;
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "AnimGraf |*.agr|All files|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {                
                System.IO.BinaryReader br = new System.IO.BinaryReader(sfd.OpenFile());             
                AnimGraf = AnimationGraph.AnimationGraffFromStream(br);
                loadFormByAnimationGraph(AnimGraf);
            }

        }
  
       

        public void loadFormByAnimationGraph(AnimationGraph AGrf)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox1.Items.AddRange(AGrf.nodes);
            chev.listBox1.Items.Clear();
            for (int i = 0; i < nodesAnim.Count; i++)
            {
                nodesAnim[i].Dispose();
            }
            nodesAnim.Clear();
            for (int i = 0; i < AGrf.nodes.Length; i++)
            {
                addNodesAnim(AGrf.nodes[i].animation);
                if (AGrf.nodes[i].NodeEvents != null)
                {
                    listBox2.Items.AddRange(AGrf.nodes[i].NodeEvents);
                }
                if (AGrf.nodes[i].NodeEvents != null)
                {
                    for (int j = 0; j < AGrf.nodes[i].NodeEvents.Length; j++)
                    {
                        if (!FindCharacterEvent(AGrf.nodes[i].NodeEvents[j].neededevent))
                        {
                            chev.listBox1.Items.Add(AGrf.nodes[i].NodeEvents[j].neededevent);
                        }
                    }
                }

            }
            TagRefresh();
            listBox1.SelectedIndex = 0;
            drawnode(0);
            DrawNodeEvent();
        }
        private bool FindCharacterEvent(CharacterEvent charev)
        {
            bool res = false;
            if (chev.listBox1.Items.Count > 0)
            {
                for (int i = 0; i < chev.listBox1.Items.Count; i++)
                {
                    CharacterEvent ce = (CharacterEvent)chev.listBox1.Items[i];
                    if (ce.eventName.Equals(charev.eventName))
                    {
                        res = true;
                    }
                }
            }
            return res;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1)
            {
                listBox2.Items.RemoveAt(listBox2.SelectedIndex);
                DrawNodeEvent();
            }
        }
    }
}

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

        public AnimGraphViewIfo viewInfo;
        public List<PictureBox> nodesAnim;
        public ResourceCollector.SkeletonWithAddInfo skeleton;
        public Point cur, curnew;
        bool selected;
        public BaseView selectedView;
        CharEvents chev;
        public int[] boneIndexes;

        public AnimGrafEditor(ResourceCollector.SkeletonWithAddInfo _skeleton, int skeletonPart)
        {
            
            AnimGraf = new AnimationGraph();

            InitializeComponent();
            skeleton = _skeleton;
            chev = new CharEvents();
            viewInfo = new AnimGraphViewIfo(AnimGraf, this.pictureBox1);
            switch (skeletonPart)
            {
                case 0:
                    boneIndexes = _skeleton.BottomIndexes;
                    break;
                case 1:
                    boneIndexes = _skeleton.TopIndexes;
                    break;
                default: break;
            }
        }

        public AnimGrafEditor(ResourceCollector.AnimationGraph _animGraph, ResourceCollector.SkeletonWithAddInfo _skeleton, int skeletonPart)
        {
            InitializeComponent();
            AnimGraf = _animGraph;
            skeleton = _skeleton;
            viewInfo = new AnimGraphViewIfo(AnimGraf, this.pictureBox1);
            switch (skeletonPart)
            {
                case 0:
                    boneIndexes = _skeleton.BottomIndexes;
                    break;
                case 1:
                    boneIndexes = _skeleton.TopIndexes;
                    break;
                default: break;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FullAnimation curanim;
            AnimationNode curnode;
            AddAnim dlg = new AddAnim();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                curanim = FullAnimation.From3DMAXStream(dlg.dlg.OpenFile(), skeleton);
                curnode = new AnimationNode(dlg.textBox2.Text);
                curnode.index = listBox1.Items.Count;
                listBox1.Items.Add(curnode);
                addNodesAnim(curnode);
            }
        }

        private void addNodesAnim(AnimationNode curnode)
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
            gr.DrawString(curnode.name, new System.Drawing.Font("Times New Roman", 8), Brushes.Black, 5, btm.Height / 2 - 4);
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
                if ((int)ne.associatedNode.index == (int)an.index)
                {
                    toremove.Add(ne);
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
                an.index = i;
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
                gr.DrawString(curanim.name, new System.Drawing.Font("Times New Roman", 8), Brushes.Black, 5, btm.Height / 2 - 4);
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
                    (AnimationNode)listBox1.Items[needlg.comboBox2.SelectedIndex],chev.listBox1.Items[needlg.CharEventIndex].ToString());

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
                PictureBox n1=nodesAnim[ne.parentNode.index];
                PictureBox n2=nodesAnim[ne.associatedNode.index];

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
            pictureBox1.Invalidate();
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
                    (AnimationNode)listBox1.Items[needlg.comboBox2.SelectedIndex], chev.listBox1.Items[needlg.CharEventIndex].ToString());

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
                    
                    if (((int)ne.parentNode.index) == ((int)an.index) )
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
                AnimationGraph.AnimationGraphToStream(AnimGraf,bw);
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
                int count = bw.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    int x = bw.ReadInt32();
                    int y = bw.ReadInt32();
                    nodesAnim[i].Location = new Point(x, y);
                }
                bw.Close();
                DrawNodeEvent();
                drawnode(listBox1.SelectedIndex);

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "AnimGraf |*.agr|All files|*.*";
            if (sfd.ShowDialog() == DialogResult.OK)
            {                
                System.IO.BinaryReader br = new System.IO.BinaryReader(sfd.OpenFile());             
                AnimGraf = AnimationGraph.AnimationGraphFromStream(br);
                loadFormByAnimationGraph(AnimGraf);
            }
        }

        public void loadFormByAnimationGraph(AnimationGraph AGrf)
        {
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox1.Items.AddRange(AGrf.nodes.ToArray());
            chev.listBox1.Items.Clear();
            for (int i = 0; i < nodesAnim.Count; i++)
            {
                nodesAnim[i].Dispose();
            }
            nodesAnim.Clear();
            for (int i = 0; i < AGrf.nodes.Count; i++)
            {
                addNodesAnim(AGrf.nodes[i]);
                if (AGrf.nodes[i].NodeEvents != null)
                {
                    listBox2.Items.AddRange(AGrf.nodes[i].NodeEvents);
                }
                if (AGrf.nodes[i].NodeEvents != null)
                {
                    for (int j = 0; j < AGrf.nodes[i].NodeEvents.Length; j++)
                    {
                        if (!FindCharacterEvent(AGrf.nodes[i].NodeEvents[j].neededEvent))
                        {
                            chev.listBox1.Items.Add(AGrf.nodes[i].NodeEvents[j].neededEvent);
                        }
                    }
                }

            }
            TagRefresh();
            listBox1.SelectedIndex = 0;
            drawnode(0);
            DrawNodeEvent();
        }
        private bool FindCharacterEvent(string charev)
        {
            bool res = false;
            if (chev.listBox1.Items.Count > 0)
            {
                for (int i = 0; i < chev.listBox1.Items.Count; i++)
                {
                    CharacterEvent ce = (CharacterEvent)chev.listBox1.Items[i];
                    if (ce.eventName.CompareTo(charev) == 0)
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

    public class AnimGraphViewIfo
    {
        public List<NodeView> nodes;
        public List<EdgeView> edges;

        public Control baseControl;

        public AnimGraphViewIfo(AnimationGraph _graph, Control baseControl)
        {
            nodes = new List<NodeView>();
            //create nodes
            edges = new List<EdgeView>();
            //create edges
        }

        public void Save(System.IO.BinaryWriter bw)
        { }

        public void Load(System.IO.BinaryReader br)
        { }
    }

    public abstract class BaseView
    {
        public bool selected;
    }

    public class NodeView : BaseView
    {
        public PictureBox pictureBox;
        public AnimationNode baseNode;
        public NodeView(AnimationNode _node, PictureBox parentBox)
        {
            baseNode = _node;
            pictureBox = new PictureBox();
        }
        public void Draw()
        { }
    }

    public class EdgeView : BaseView
    {
        public NodeView startView;
        public NodeView endView;
        public EdgeView(NodeView _start, NodeView _end)
        {
            startView = _start;
            endView = _end;
        }
        public void Draw(Graphics g)
        { }
    }
}

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
        public ResourceCollector.SkeletonWithAddInfo skeleton;
        CharEvents chev;
        public int[] boneIndexes;

        public AnimGrafEditor(ResourceCollector.SkeletonWithAddInfo _skeleton, int skeletonPart)
        {
            
            AnimGraf = new AnimationGraph();

            InitializeComponent();
            skeleton = _skeleton;
            chev = new CharEvents();

            viewInfo = new AnimGraphViewIfo(AnimGraf, this.pictureBox1, imageList1, new MouseEventHandler(pictureBox1_MouseDoubleClick), new EventHandler(pictureBox1_Click));
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
            chev = new CharEvents();            
            viewInfo = new AnimGraphViewIfo(AnimGraf, this.pictureBox1,imageList1, new MouseEventHandler(pictureBox1_MouseDoubleClick), new EventHandler(pictureBox1_Click));
            loadFormByAnimationGraph(_animGraph);
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
            //if (AnimGraf.nodes.Count > 0)
            //{
            //    listBox1.Items.Clear();
            //    listBox2.Items.Clear();
            //    AnimationNode[] tmp=new AnimationNode[AnimGraf.nodes.Count];
            //    AnimGraf.nodes.ToArray().CopyTo(tmp, 0);
            //    listBox1.Items.AddRange(tmp);
            //    foreach (AnimationNode an in AnimGraf.nodes.ToArray())
            //    {
            //        if (an.nodeEvents.Count > 0)
            //        {
            //            NodeEvent[] tmp2 = new NodeEvent[an.nodeEvents.Count];
            //            an.nodeEvents.ToArray().CopyTo(tmp2, 0);
            //            listBox2.Items.AddRange(tmp2);
            //        }
            //    }
            //}

            

        }

        private void button4_Click(object sender, EventArgs e)
        {
            FullAnimation curanim;
            AnimationNode curnode;
            AddAnim dlg = new AddAnim();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                curanim = FullAnimation.From3DMAXStream(dlg.dlg.OpenFile(), skeleton,boneIndexes);
                curnode = new AnimationNode(dlg.textBox2.Text,curanim);
                curnode.index = listBox1.Items.Count;
                listBox1.Items.Add(curnode);
                viewInfo.addNodeView(curnode,new MouseEventHandler(pictureBox1_MouseDoubleClick),new EventHandler(pictureBox1_Click));
            }
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                viewInfo.RemoveNodesViewAt(listBox1.SelectedIndex);
                AnimationNode an = (AnimationNode)listBox1.Items[listBox1.SelectedIndex]; 
                RemoveNodeByAnimationNode(an);                     
                //listBox1.Items.RemoveAt(listBox1.SelectedIndex);             
                      
                viewInfo.selectedNode = listBox1.SelectedIndex;
                viewInfo.selectedEdge = listBox2.SelectedIndex;
                viewInfo.Refresh();    
            }
        }

        private void RemoveNodeByAnimationNode(AnimationNode an )
        {

            foreach (NodeEvent ne in an.nodeEvents)
                {
                    listBox2.Items.Remove(ne);
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
            listBox1.Items.Remove(an);  
            TagRefresh();      
            if (listBox2.Items.Count > 0)
            {
                Object[] obj = new Object[listBox2.Items.Count];
                listBox2.Items.CopyTo(obj, 0);
                NodeEvent[] tmp = new NodeEvent[obj.Length];
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    tmp[i] = (NodeEvent)obj[i];
                }
                viewInfo.loadEdgeView(tmp);
            }
        }

        private void TagRefresh()
        {
            for (int i = 0; i <viewInfo.nodes.Count; i++)
            {
                AnimationNode an = (AnimationNode)listBox1.Items[i];
                an.index = i;
                viewInfo.nodes[i].pictureBox.Tag = i;
            }
        }    

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox ss = (PictureBox)sender;
            ss.Select();
            listBox1.SelectedIndex = (int)ss.Tag;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            viewInfo.selectedNode = listBox1.SelectedIndex;
            viewInfo.selectedEdge = listBox2.SelectedIndex;
            viewInfo.Refresh();        
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
                viewInfo.addEdgeView(nodeev);
            }
            viewInfo.selectedNode = listBox1.SelectedIndex;
            viewInfo.selectedEdge = listBox2.SelectedIndex;
            viewInfo.Refresh();         
        }
           

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            viewInfo.selectedNode = listBox1.SelectedIndex;
            viewInfo.selectedEdge = listBox2.SelectedIndex;
            viewInfo.Refresh();
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
                viewInfo.addEdgeView(nodeev);
            }
            viewInfo.selectedNode = listBox1.SelectedIndex;
            viewInfo.selectedEdge = listBox2.SelectedIndex;
            viewInfo.Refresh();
        }

        public void BildAGraf()
        {
            AnimationNode[] res = new AnimationNode[listBox1.Items.Count];
            for (int i = 0; i < listBox1.Items.Count; i++)
            {
                int count = 0;
                AnimationNode an = (AnimationNode)listBox1.Items[i];
               // an.nodeEvents = null;
                an.nodeEvents.Clear();
                for (int j = 0; j < listBox2.Items.Count; j++)
                {
                    NodeEvent ne = (NodeEvent)listBox2.Items[j];
                    
                    if (((int)ne.parentNode.index) == ((int)an.index) )
                    {
                        //NodeEvent[] ss = new NodeEvent[count + 1];
                        //an.nodeEvents.CopyTo(ss, 0);                        
                        //ss[count] = ne;
                        //count++;
                        //an.nodeEvents = new List<NodeEvent>( ss);
                        an.nodeEvents.Add(ne);
                    }
                }
                res[i] = an;
            }
            AnimGraf.nodes = new List<AnimationNode>(res);
           // AnimGraf = new AnimationGraph();

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
                bw.Write(viewInfo.nodes.Count);
                for (int i = 0; i < viewInfo.nodes.Count; i++)
                {
                    bw.Write(viewInfo.nodes[i].pictureBox.Location.X);
                    bw.Write(viewInfo.nodes[i].pictureBox.Location.Y);
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
                    viewInfo.nodes[i].pictureBox.Location = new Point(x, y);
                }
                bw.Close();
                viewInfo.selectedNode = listBox1.SelectedIndex;
                viewInfo.selectedEdge = listBox2.SelectedIndex;
                viewInfo.Refresh();

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
                viewInfo.Clear();
                viewInfo = new AnimGraphViewIfo(AnimGraf, pictureBox1, imageList1, new MouseEventHandler(pictureBox1_MouseDoubleClick), new EventHandler(pictureBox1_Click));
            }
        }

        public void loadFormByAnimationGraph(AnimationGraph AGrf)
        {
           
            //viewInfo = new AnimGraphViewIfo(AGrf, pictureBox1, imageList1, new MouseEventHandler(pictureBox1_MouseDoubleClick), new EventHandler(pictureBox1_Click));
            //viewInfo.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            listBox1.Items.AddRange(AGrf.nodes.ToArray());
            chev.listBox1.Items.Clear();
            for (int i = 0; i < AGrf.nodes.Count; i++)
            {

                listBox2.Items.AddRange(AGrf.nodes[i].nodeEvents.ToArray());


                for (int j = 0; j < AGrf.nodes[i].nodeEvents.Count; j++)
                {
                    if (!FindCharacterEvent(AGrf.nodes[i].nodeEvents[j].neededEvent))
                    {
                        chev.listBox1.Items.Add(AGrf.nodes[i].nodeEvents[j].neededEvent);
                    }
                }

            }
            TagRefresh();
            listBox1.SelectedIndex = -1;
            //viewInfo.selectedNode = listBox1.SelectedIndex;
            //viewInfo.selectedEdge = listBox2.SelectedIndex;
            //viewInfo.Refresh();
        }
        private bool FindCharacterEvent(string charev)
        {
            bool res = false;
            if (chev.listBox1.Items.Count > 0)
            {
                for (int i = 0; i < chev.listBox1.Items.Count; i++)
                {
                    String ce = (String)chev.listBox1.Items[i];
                    if (ce.CompareTo(charev) == 0)
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
                viewInfo.edges.RemoveAt(listBox2.SelectedIndex);
                listBox2.Items.RemoveAt(listBox2.SelectedIndex);               
                viewInfo.Refresh();
            }

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }
    }

    public class AnimGraphViewIfo
    {
        public List<NodeView> nodes;
        public List<EdgeView> edges;
        ImageList imageList1;
        public bool selected;
        public Control baseControl;
        public int selectedNode;
        public int selectedEdge;
        Point cur, curnew;
        public AnimGraphViewIfo(AnimationGraph _graph, Control _baseControl, ImageList _imageList1, MouseEventHandler doubleClick, EventHandler click)
        {
            nodes = new List<NodeView>();
            edges = new List<EdgeView>();
            baseControl = _baseControl;
            imageList1 = _imageList1;
            for (int i = 0; i < _graph.nodes.Count; i++)
            {
                addNodeView(_graph.nodes[i], doubleClick, click); //create nodes  
            }
            for (int i = 0; i < _graph.nodes.Count; i++)
            {
                for (int j = 0; j < _graph.nodes[i].nodeEvents.Count; j++) //create edges
                {
                    addEdgeView(_graph.nodes[i].nodeEvents[j]);
                }
            }
            drawnodes();
            DrawNodeEvent();
        }

        public void Save(System.IO.BinaryWriter bw)
        { }

        public void Load(System.IO.BinaryReader br)
        { }

        public void RemoveNodesViewAt(int index)
        {
            nodes[index].pictureBox.Dispose();
            nodes.RemoveAt(index);
        }
        public void Refresh()
        {
            drawnodes();
            DrawNodeEvent();
        }
        public void Clear()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].pictureBox.Dispose();
            }
            nodes.Clear();
            edges.Clear();
        }
        public void addNodeView(AnimationNode curnode, MouseEventHandler doubleClick, EventHandler click)
        {
            PictureBox pic = new PictureBox();
            pic.Parent = baseControl;
            pic.MouseDown += new MouseEventHandler(pictureBox2_MouseDown);
            pic.MouseMove += new MouseEventHandler(pictureBox2_MouseMove);
            pic.MouseUp += new MouseEventHandler(pictureBox1_MouseUp);
            pic.MouseDoubleClick += doubleClick;
            pic.Click += click;
            pic.BorderStyle = BorderStyle.FixedSingle;
            Bitmap btm = (Bitmap)imageList1.Images[0].Clone();
            Graphics gr = Graphics.FromImage(btm);
            pic.SizeMode = PictureBoxSizeMode.AutoSize;
            gr.DrawString(curnode.name, new System.Drawing.Font("Times New Roman", 8), Brushes.Black, 5, btm.Height / 2 - 4);
            gr.Dispose();
            pic.Image = btm;
            pic.Tag = nodes.Count;
            pic.Location = new Point(nodes.Count * (imageList1.Images[0].Width + 5), nodes.Count * (imageList1.Images[0].Height + 3));
            nodes.Add(new NodeView(curnode,pic));
            baseControl.Invalidate();
        }
        
        public void addEdgeView(NodeEvent curnodev)
        {
            EdgeView curent=new EdgeView(nodes[curnodev.parentNode.index],nodes[curnodev.associatedNode.index]);
            edges.Add(curent);
        }
        public void loadEdgeView(NodeEvent[] nodeEvents)
        {
            edges.Clear();
            for (int i = 0; i < nodeEvents.Length; i++)
            {
                addEdgeView((NodeEvent)nodeEvents[i]);
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
                baseControl.Invalidate();
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

        private void drawnodes()
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                nodes[i].Draw(selectedNode == i,imageList1);                
            }

        }

        private void DrawNodeEvent()
        {
            Bitmap btm = new Bitmap(baseControl.Width, baseControl.Height);
            Graphics gr = Graphics.FromImage(btm);
            for (int i = 0; i < edges.Count; i++)
            {
                edges[i].Draw(gr, i == selectedEdge);            
            }
            gr.Dispose();
            PictureBox pictureBox1 = (PictureBox)baseControl;
            //baseControl.BackgroundImage = btm;
            pictureBox1.Image = btm;
           // baseControl.Invalidate();
        }

    }



    public class NodeView 
    {
        public PictureBox pictureBox;
        public AnimationNode baseNode;
        public NodeView(AnimationNode _node, PictureBox _pictureBox)
        {
            baseNode = _node;
            pictureBox = _pictureBox;
        }
        public void Draw(bool selected, ImageList imglst)
        {
            Bitmap btm;     
            if (selected)
            {
                btm = (Bitmap)imglst.Images[1].Clone();
            }
            else
            {
                btm = (Bitmap)imglst.Images[0].Clone();
            }
            Graphics gr = Graphics.FromImage(btm);
            gr.DrawString(baseNode.ToString(), new System.Drawing.Font("Times New Roman", 8), Brushes.Black, 5, btm.Height / 2 - 4);
            gr.Dispose();
            pictureBox.Image = btm;
        }
    }

    public class EdgeView
    {
        public NodeView startView;
        public NodeView endView;
        public EdgeView(NodeView _start, NodeView _end)
        {
            startView = _start;
            endView = _end;
        }
        public void Draw(Graphics gr, bool selected)
        {
            PictureBox n1 = startView.pictureBox;//nodesAnim[ne.parentNode.index];
            PictureBox n2 = endView.pictureBox;// nodesAnim[ne.associatedNode.index];

            int x1 = n1.Location.X + (n1.Width / 2);
            int y1 = n1.Location.Y + (n1.Height / 2);
            int x2 = n2.Location.X + (n2.Width / 4);
            int y2 = n2.Location.Y + (n2.Height / 2);

            int x3 = x2 - ((x2 - x1) / 5);
            int y3 = y2 - ((y2 - y1) / 5);


            if (selected)
            {
                gr.DrawLine(new Pen(Brushes.Red, 4), new Point(x1, y1), new Point(x2, y2));
            }
            else
            {
                gr.DrawLine(new Pen(Brushes.Green, 3), new Point(x1, y1), new Point(x2, y2));
            }
            gr.DrawLine(new Pen(Brushes.Black, 6), new Point(x3, y3), new Point(x2, y2));

        }
    }
}

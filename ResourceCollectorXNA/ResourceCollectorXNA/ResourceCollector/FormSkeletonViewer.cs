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
    public partial class SkeletonViewer : Form
    {
        private System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        Skeleton skelet;
        SkeletDrawInfo sdi; Graphics g;
        public SkeletonViewer(Skeleton _skel)
        {
            InitializeComponent();
            skelet = _skel;
            
            sdi = new SkeletDrawInfo(skelet, ClientRectangle);

        }
        public static int NodeSelectionRadius = 30;
        [Serializable]
        class SkeletDrawInfo
        {
           
            
            public static int NodeFullradius = 20;
            public static int NodeBorderradius = 2;
            public static int Relationwidth = 2;
            public static Color NodeFillcolor = Color.OliveDrab;
            public static Color NodeBordercolor = Color.DarkGray;
            public static Brush NodeFillBrush = Brushes.FloralWhite;
            public static Brush NodeBorderBrush = Brushes.Fuchsia;
            public static Color Relationcolor = Color.DarkGray;

            [Serializable]
            public class Nodeinfo
            {
                public Point Locaton;
                [NonSerialized]
                public  Bone bone;

                public int index;
                
                public int[] childrelations;
                public Nodeinfo(int _index, Bone b)
                {
                    index = _index;
                    bone = b;
                    childrelations = new int[b.Childrens.Count];
                }

            }
            [Serializable]
            public class Relationinfo
            {
                public int from
                {
                    get;
                    private set;
                }
                public int to
                {
                    get;
                    private set;
                }
                public bool LimitedLength
                {
                    get;
                    private set;
                }
                public int MaxLength
                {
                    get;
                    private set;
                }
                public Relationinfo(int f, int t)
                {
                    from = f;
                    to = t;
                    LimitedLength = false;
                }

            }
            [NonSerialized]
            public Skeleton sk;
            
            public Relationinfo[] relations;
            public Nodeinfo[] nodes;
            private int width;
            private int heigth;
            private int maxlevel;
            private Rectangle ClientRectangle;
            private List<int> levelygriks = new List<int>();

            public SkeletDrawInfo(Skeleton s, Rectangle _ClientRectangle)
            {
                sk = s;
                ClientRectangle = _ClientRectangle;
                nodes = new Nodeinfo[s.bones.Length];
                List<Relationinfo> relations = new List<Relationinfo>();
                maxlevel = 0;
                for (int i = 0; i < s.bones.Length; i++)
                {
                    if (maxlevel < s.bones[i].level)
                        maxlevel = s.bones[i].level;
                }

                levelygriks.Add(10);
                createnode(sk.Root, relations);
                this.relations = relations.ToArray();

                getdrawdata();
            }
            void createnode(Bone bone, List<Relationinfo> relations)
            {
                if (levelygriks.Count < bone.level + 1)
                {
                    levelygriks.Add(10);
                }
                Nodeinfo n = new Nodeinfo(bone.index, bone);

                n.Locaton = new Point(bone.level * 50, levelygriks[bone.level]);
                levelygriks[bone.level] = levelygriks[bone.level] + 50;
                nodes[n.index] = n;
                for (int i = 0; i < bone.Childrens.Count; i++)
                {
                    Relationinfo ri = new Relationinfo(n.index, bone.Childrens[i].index);
                    relations.Add(ri);
                    createnode(bone.Childrens[i], relations);
                }


            }
            void getdrawdata()
            {
                width = ClientRectangle.Width;
                heigth = ClientRectangle.Height;
            }
        }

        Point lastpos;
        Font f = new Font("consolas", 10);

        private void SkeletonViewer_Paint(object sender, PaintEventArgs e)
        {
            g = CreateGraphics();
            g.Clear(this.BackColor);
            for (int i = 0; i < sdi.relations.Length; i++)
            {
                g.DrawLine(new Pen(SkeletDrawInfo.Relationcolor, SkeletDrawInfo.Relationwidth),
                    sdi.nodes[sdi.relations[i].from].Locaton.X + SkeletDrawInfo.NodeFullradius / 2,
                    sdi.nodes[sdi.relations[i].from].Locaton.Y + SkeletDrawInfo.NodeFullradius / 2,
                    sdi.nodes[sdi.relations[i].to].Locaton.X + SkeletDrawInfo.NodeFullradius / 2,
                    sdi.nodes[sdi.relations[i].to].Locaton.Y + SkeletDrawInfo.NodeFullradius / 2);
            }
            for (int i = 0; i < sdi.nodes.Length; i++)
            {
                g.FillEllipse(SkeletDrawInfo.NodeFillBrush, sdi.nodes[i].Locaton.X, sdi.nodes[i].Locaton.Y, SkeletDrawInfo.NodeFullradius, SkeletDrawInfo.NodeFullradius);
                g.DrawEllipse(new Pen(SkeletDrawInfo.NodeBordercolor, SkeletDrawInfo.NodeBorderradius), sdi.nodes[i].Locaton.X, sdi.nodes[i].Locaton.Y, SkeletDrawInfo.NodeFullradius, SkeletDrawInfo.NodeFullradius);

                g.DrawString(string.Format("[{0}] - {1}", i.ToString(), sdi.nodes[i].bone.Name), f, Brushes.Black, sdi.nodes[i].Locaton.X + 10, sdi.nodes[i].Locaton.Y);
            }
            if(drawednode!=-1)
                g.DrawEllipse(new Pen(Color.LightGreen, SkeletDrawInfo.NodeBorderradius), sdi.nodes[drawednode].Locaton.X-5, sdi.nodes[drawednode].Locaton.Y-5, NodeSelectionRadius, NodeSelectionRadius);

        }
        int captirednode = -1;
        int drawednode = -1;
        private void SkeletonViewer_MouseDown(object sender, MouseEventArgs e)
        {
            captirednode = -1;
            for (int i = 0; i < sdi.nodes.Length; i++)
            {
                PointF d = new PointF(e.Location.X - sdi.nodes[i].Locaton.X, e.Location.Y - sdi.nodes[i].Locaton.Y);
                int delta =Convert.ToInt32( Math.Sqrt(Convert.ToDouble(d.X * d.X + d.Y * d.Y)));
                if (delta < SkeletonViewer.NodeSelectionRadius)
                {
                    captirednode = i;
                    break;
                }
            }
        }

        private void SkeletonViewer_MouseUp(object sender, MouseEventArgs e)
        {
            captirednode = -1;
        }
        
        private void SkeletonViewer_MouseMove(object sender, MouseEventArgs e)
        {
            if (captirednode != -1)
            {
                int deltax = lastpos.X - e.X;
                int deltay = lastpos.Y - e.Y;

                sdi.nodes[captirednode].Locaton.X -= deltax;
                sdi.nodes[captirednode].Locaton.Y -= deltay;

                Invalidate();
            }
            else
            {
                bool wasfound = false;
                bool needredraw = false;
                int lastindex = drawednode;
                for (int i = 0; i < sdi.nodes.Length; i++)
                {
                    PointF d = new PointF(e.Location.X - sdi.nodes[i].Locaton.X - SkeletonViewer.NodeSelectionRadius/4, e.Location.Y - sdi.nodes[i].Locaton.Y - SkeletonViewer.NodeSelectionRadius/4);
                    int delta = Convert.ToInt32(Math.Sqrt(Convert.ToDouble(d.X * d.X + d.Y * d.Y)));
                    if (delta < SkeletonViewer.NodeSelectionRadius/2)
                    {
                        wasfound = true;
                        if (i != lastindex)
                        {
                            drawednode = i;
                            needredraw = true;
                        }
                        break;
                    }
                }
                if (!wasfound)
                {
                    drawednode = -1;
                    needredraw = lastindex != -1;
                }
                
                if (needredraw)
                    Invalidate();
            }
            lastpos = new Point(e.X, e.Y);
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.Stream outputstream = new System.IO.FileStream(sfd.FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);
                formatter.Serialize(outputstream, sdi);
                outputstream.Close();
            }
        }

        private void SkeletonViewer_MouseLeave(object sender, EventArgs e)
        {
            captirednode = -1;
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SkeletDrawInfo _sdi;
                System.IO.Stream stream = new System.IO.FileStream(ofd.FileName, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                try
                {
                    _sdi = (SkeletDrawInfo)formatter.Deserialize(stream);
                    if (_sdi.nodes.Length == skelet.bones.Length && _sdi.relations.Length == sdi.relations.Length)
                    {
                        _sdi.sk = skelet;
                        for (int o = 0; o < _sdi.nodes.Length; o++)
                            _sdi.nodes[o].bone = skelet.bones[_sdi.nodes[o].index];
                        sdi = _sdi;
                        Invalidate();
                    }

                }
                catch (System.Runtime.Serialization.SerializationException)
                {
                    System.Windows.Forms.MessageBox.Show("Wrong file format");
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show(ex.ToString());
                }
                finally
                {
                    stream.Close();
                }
            }
        }
    }
}

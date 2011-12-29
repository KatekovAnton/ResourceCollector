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
    public partial class TexturePropertyWindow : Form
    {
        PictureBox pictureBox;
        Button b;
        Button save;
        Bitmap bt;
        Timer t;
        Button replace;
        public static System.Drawing.Font font = new System.Drawing.Font("Consolas", 10.0f);

        ImageContent _thisobject;
        public TexturePropertyWindow(ImageContent __image)
        {
            _thisobject = __image;
            Bitmap image = null;
            using (var stream = new System.IO.MemoryStream(_thisobject.data))
            {
                image = new Bitmap(stream);
            }

            InitializeComponent();

            if (image != null)
            {
                pictureBox = new PictureBox();
                bt = new Bitmap(image);
                b = new Button();
                b.Location = new Point(10, 10);
                b.Parent = this;
                b.Text = "Redraw";
                
                save = new Button();
                save.Location = new Point(10, 40);
                save.Parent = this;
                save.Text = "Save image";

                replace = new Button();
                replace.Location = new Point(100, 10);
                replace.Parent = this;
                replace.Text = "Replace image";
                replace.Click += new EventHandler(replace_Click);

                pictureBox.Parent = this;
                b.Click += new EventHandler(b_Click);
                save.Click += new EventHandler(save_Click);


             

                pictureBox.Dock = DockStyle.Fill;
                pictureBox.Resize += new EventHandler(pictureBox_Resize);
                pictureBox.MouseClick += new MouseEventHandler(pictureBox_MouseClick);
               // this.Paint += new PaintEventHandler(TexturePropertyWindow_Paint);
                t = new Timer();
                t.Interval = 10;
                t.Tick += new EventHandler(t_Tick);
                t.Start();
            }
        }

        void replace_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filename = ofd.FileName;
                Bitmap bt1 = null;
                try
                {
                    bt1 = new Bitmap(filename);
                }
                catch (Exception)
                {
                    MessageBox.Show("error loading file!");
                    return;
                }
                if(bt == null)
                {
                    MessageBox.Show("error loading file!");
                    return;
                }

                _thisobject.SetImage(bt1);

                using (var stream = new System.IO.MemoryStream(_thisobject.data))
                {
                    bt = new Bitmap(stream);
                }

                draw();

            }

        }

        void save_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string name = sfd.FileName;
                if (!name.EndsWith(".png"))
                    name += ".png";
                bt.Save(name, System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        void pictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                contextMenuStrip1.Show(new Point(e.Location.X+Location.X,e.Location.Y+Location.Y));
            }
        }

        void pictureBox_Resize(object sender, EventArgs e)
        {
            draw();
        }



        void b_Click(object sender, EventArgs e)
        {
            draw();
        }

        void t_Tick(object sender, EventArgs e)
        {
            draw();
            t.Stop();
            t = null;
        }
       
        void draw()
        {
            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(pictureBox.ClientRectangle.Location, pictureBox.ClientRectangle.Size);
            System.Drawing.Graphics g = pictureBox.CreateGraphics();
            g.Clear(System.Drawing.Color.DarkGray);
            ///как нам подогнать картинку? во высоте или по ширине?
            float widthmult = Convert.ToSingle(rect.Width) / Convert.ToSingle(bt.Width);
            float heightmult = Convert.ToSingle(rect.Height) / Convert.ToSingle(bt.Height);
            float more = widthmult / heightmult;
            int h, w; string procentes = null;

            if (more > 1.0f)
            {
                procentes = (Convert.ToInt32(heightmult * 100.0f)).ToString() + "%";
                h = Convert.ToInt32(heightmult * Convert.ToSingle(bt.Height));
                w = Convert.ToInt32(heightmult * Convert.ToSingle(bt.Width));
            }
            else
            {
                procentes = (Convert.ToInt32(widthmult * 100.0f)).ToString() + "%";
                h = Convert.ToInt32(widthmult * Convert.ToSingle(bt.Height));
                w = Convert.ToInt32(widthmult * Convert.ToSingle(bt.Width));
            }

            //на ссколько сместить каритнку что бы была посередине 
            int wd = (rect.Width - w) / 2;
            int hd = (rect.Height - h) / 2;

            g.DrawImage(bt, new System.Drawing.Rectangle(rect.X + wd, rect.Y + hd, w, h));
            g.DrawString("Zoom = " + procentes + "\nSource size = " + bt.Width.ToString() + "x" + bt.Height.ToString(), font, System.Drawing.Brushes.Azure, new System.Drawing.PointF(10.0f, 70.0f));
        }
    }
}

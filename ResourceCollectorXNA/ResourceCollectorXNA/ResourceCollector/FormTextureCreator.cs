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
    public partial class FormTextureCreator : Form
    {
        Pack p;
        public FormTextureCreator(Pack pack)
        {
            InitializeComponent();
            p = pack;
        }
        public ImageContent newimage;
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        Image im = Bitmap.FromStream(new System.IO.FileStream(ofd.FileName, System.IO.FileMode.Open));
                        newimage = new ImageContent(textBox1.Text,im);
                        textBox1.Text = ofd.FileName;
                        label1.Text = "sucessfull loaded";
                        button1.Enabled = button2.Enabled = false;
                    }
                    catch (Exception ex)
                    {
                        newimage = null;
                        MessageBox.Show("bad file. try again");
                    }
                }
            }
            else
            {
                MessageBox.Show("Name must be not empty!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (this.newimage != null)
            {
                DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        bool checkpow2(int number)
        {
            int d = Convert.ToInt32(Math.Log(Convert.ToDouble(number), 2.0));
            return Math.Pow(2.0, Convert.ToDouble(d)) - number == 0;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (checkpow2(Convert.ToInt32(numericUpDown1.Value)))
            {
                if (textBox1.Text != "")
                {
                    //generate lod from picked texture;
                    FormObjectPicker fop = new FormObjectPicker(p, ElementType.PNGTexture);
                    if (fop.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        ImageContent ic = p.getobject( fop.PickedContent[0]) as ImageContent;
                        Image bbb = Image.FromStream(new System.IO.MemoryStream(ic.data));


                        newimage = new ImageContent(
                            textBox1.Text + "\0", new Bitmap(bbb,
                                new Size(bbb.Width / Convert.ToInt32(numericUpDown1.Value),
                                    bbb.Height / Convert.ToInt32(numericUpDown1.Value))));
                        button1.Enabled = button2.Enabled = false;
                        label1.Text = "sucessfull generated";
                    }
                }
                else
                    MessageBox.Show("Name must be not empty!");
            }
            else
            {
                MessageBox.Show("Number must me a power of 2.");
            }
        }
    }
}

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
    public partial class AddAnim : Form
    {
        public OpenFileDialog dlg;
        public AddAnim()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dlg = new OpenFileDialog();
            dlg.Filter = "Animation|*.anim|All Files|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dlg.FileName;
                textBox2.Text = dlg.SafeFileName;
            }


        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}

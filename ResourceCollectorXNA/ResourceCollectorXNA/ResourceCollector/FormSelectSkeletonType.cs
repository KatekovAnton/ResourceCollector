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
    public partial class FormSelectSkeletonType : Form
    {
        public FormSelectSkeletonType()
        {
            InitializeComponent();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
        public int type; 
        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                type = ElementType.Skeleton;
                DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
                return;
            }
            if (radioButton2.Checked)
            {
                type = ElementType.SkeletonWithAddInfo;
                DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
                return;
            }
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }


    }
}

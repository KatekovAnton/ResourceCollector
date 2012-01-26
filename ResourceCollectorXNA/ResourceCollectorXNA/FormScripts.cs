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
    public partial class FormScripts : Form
    {
        public FormScripts(List<dynamic> list)
        {
            InitializeComponent();
            if (list.Count == 0)
            {
                MessageBox.Show("Sorry, NO SCRIPTS!!!\n\nPlease try something other...");
                Close();
            }

            foreach (dynamic val in list)
                        Addscr(val);
        }

        void Addscr(dynamic scr)
        {
            lb.Items.Add(scr);
        }

        private void FormScripts_Load(object sender, EventArgs e)
        {
            label1.Text = "\nIt's only scripts to ResourceCollector\n\nSelect scripts, what you want to execute and press \"RUN\"\n\nThe order of executing you can change by other buttons";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lb.SelectedItems != null && lb.SelectedItems.Count > 0)
            foreach (dynamic val in lb.SelectedItems)
            {
                ResourceCollectorXNA.SE.Instance.ExScript(val.ToString());
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            Eggs.MoveListboxItem(-1, lb);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Eggs.MoveListboxItem(1, lb);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (lb.SelectedItems != null && lb.SelectedItems.Count > 0)
            {
                dynamic val = lb.SelectedItems[0];
                ResourceCollectorXNA.SE.Instance.ExScript(val.ToString());
                lb.SelectedItems.Remove(val);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Eggs.SelectAll(lb, true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Eggs.SelectAll(lb, false);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

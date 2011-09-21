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
    public partial class NodeEventEditor : Form
    {
        CharEvents chev;
        public int CharEventIndex;
        public NodeEventEditor(CharEvents _chev, Object[] obj)
        {
            InitializeComponent();
            chev = _chev;
            comboBox1.Items.AddRange(obj);
            comboBox2.Items.AddRange(obj);
        }
        public NodeEventEditor(CharEvents _chev, Object[] obj, NodeEvent ne)
        {
            InitializeComponent();
            chev = _chev;
            comboBox1.Items.AddRange(obj);
            comboBox2.Items.AddRange(obj);
            comboBox1.SelectedIndex =ne.parentNode.index;
            comboBox2.SelectedIndex = ne.associatedNode.index;
            textBox2.Text =ne.neededEvent;
            textBox1.Text = ne.description;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (chev.ShowDialog() == DialogResult.OK && chev.selectedIndex >= 0 && chev.listBox1.Items.Count>0)
            {
                CharEventIndex = chev.selectedIndex;
                textBox2.Text = chev.listBox1.Items[chev.selectedIndex].ToString();
                textBox2.Tag = chev.listBox1.Items[chev.selectedIndex];                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
   
        }
    }
}

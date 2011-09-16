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
    public partial class CharEvents : Form
    {
        public int selectedIndex;
        public CharEvents()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Replace(" ", "").Length > 0)
            {
                CharacterEvent che = new CharacterEvent(textBox1.Text);
                if (!IsExist(che))
                {
                    listBox1.Items.Add(che);
                }
                else
                {
                    MessageBox.Show("Try Another Event");
                }
                listBox1.SelectedIndex = 0;

            }
            
        }
        private bool IsExist(CharacterEvent chev)
        {
            for (int i = 0; i < listBox1.Items.Count;i++ )
            {
                if (chev.CompareTo((CharacterEvent)listBox1.Items[i]))
                {
                    return true;
                }
            }
            return false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                selectedIndex = listBox1.SelectedIndex;
            }
            else
            {
                MessageBox.Show("You don't select Char Event");
                this.Close();
            }
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ResourceCollectorXNA
{
    public partial class Question : Form
    {
       public string answer="";
        public Question(string qq)
        {
            InitializeComponent();
            label1.Text = qq;

        }

        private void Question_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            answer = richTextBox1.Text;
        }
    }
}

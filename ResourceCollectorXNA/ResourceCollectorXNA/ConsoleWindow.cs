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
    public partial class ConsoleWindow : Form
    {
        public RichTextBox OUT;
        public ConsoleWindow()
        {
            InitializeComponent();
            if (instance == null)
                instance = this;
        }


        private static ConsoleWindow instance;

        public static void SetOUT(RichTextBox rtb)
        {
           instance.OUT = rtb;
        }

        public static void TraceMessage(string message)
        {
            if (instance == null)   return;

            
            string str = DateTime.Now.ToString();
            instance.listBox1.Items.Add(str.Substring(11,str.Length-11) + "->  " + message);
            instance.listBox1.SelectedIndex = instance.listBox1.Items.Count-1;
            if (instance.OUT != null) instance.OUT.Text += "\n"+(string)instance.listBox1.Items[0];

            if (instance.listBox1.Items.Count > 500)
                instance.listBox1.Items.RemoveAt(500);
          
             
        }
        private void ConsoleWindow_Resize(object sender, EventArgs e)
        {
            this.Width = 500;
        }

        private void ConsoleWindow_Load(object sender, EventArgs e)
        {

        }
    }
}

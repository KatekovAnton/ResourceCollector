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
        public Dictionary<string, string> properties;
        AnimationNode node;
        public AddAnim()
        {
            InitializeComponent();
        }

        public AddAnim(AnimationNode _node)
        {
            InitializeComponent();
            node = _node;
            properties = node.properties;
            textBox2.Text = _node.name;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dlg = new OpenFileDialog();
            dlg.Filter = "Animation|*.anim|All Files|*.*";
            dlg.InitialDirectory = AppConfiguration.AddAnimFolder;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dlg.FileName;
                if(textBox1.Text == "")
                textBox2.Text = dlg.SafeFileName;
                AppConfiguration.AddAnimFolder = System.IO.Path.GetDirectoryName(dlg.FileName);
            }
            else
                dlg = null;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (properties == null)
                properties = new Dictionary<string, string>();
            Addition.DictionaryEditor de = new Addition.DictionaryEditor(properties);
            if (de.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                properties = de.editedDict;
            
        }
    }
}

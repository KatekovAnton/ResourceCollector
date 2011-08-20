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
    public partial class FormXNAMonitor : Form
    {
        public FormXNAMonitor()
        {
            InitializeComponent();
            label1.Text = ResourceCollectorXNA.Engine.ContentLoader.ContentLoader.XNAObjectsLoadedCount.ToString();
            for (int i = 0; i < ResourceCollectorXNA.Engine.ContentLoader.ContentLoader.XNAevents.Count; i++)
                listBox1.Items.Insert(0,ResourceCollectorXNA.Engine.ContentLoader.ContentLoader.XNAevents[i]);
        }
    }
}

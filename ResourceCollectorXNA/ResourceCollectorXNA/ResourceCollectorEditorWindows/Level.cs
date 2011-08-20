using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ResourceCollectorXNA.ResourceCollectorEditorWindows
{
    public partial class Level : Form
    {
        public Level()
        {
            InitializeComponent();
        }

        private void Level_Resize(object sender, EventArgs e)
        {
            Width = 470;
        }
    }
}

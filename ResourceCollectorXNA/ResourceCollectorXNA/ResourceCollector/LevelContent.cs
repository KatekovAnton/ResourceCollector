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
    public class LevelContent:PackContent
    {
        public Pack pack;
        public override int loadbody(System.IO.BinaryReader br, ToolStripProgressBar toolStripProgressBar)
        {
            return 0;
        }
        public override void saveheader(System.IO.BinaryWriter br)
        {
           
        }
        public override void savebody(System.IO.BinaryWriter br, ToolStripProgressBar toolStripProgressBar)
        {
            
        }
        public override void calcheadersize()
        {
           
        }
        public override void calcbodysize(ToolStripProgressBar targetbar)
        {
           
        }
        public override System.Windows.Forms.DialogResult createpropertieswindow(Pack p, System.Windows.Forms.TreeView outputtreeview)
        {
            return DialogResult.OK;
        }
        public override void loadobjectheader(HeaderInfo hi, System.IO.BinaryReader br)
        {
            loadedformat = hi.loadedformat;
            name = hi.name;
            forsavingformat = hi.forsavingformat;
            offset = hi.offset;
            size = hi.size;
            headersize = hi.headersize;

            size = br.ReadInt32();
        }
        public override void ViewBasicInfo(
            ComboBox comboBox1, ComboBox comboBox2, Label label1, Label label2, Label label3,
            Label label4, GroupBox groupBox1, TextBox tb, Button button2, Button button1)
        {
            
        }
    }
}

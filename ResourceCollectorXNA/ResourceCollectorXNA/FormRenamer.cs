using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ResourceCollector;
namespace ResourceCollectorXNA
{
    public partial class FormRenamer : Form
    {
        public static string search_="";
        public static string replace_="";
        List<RenamerClassOfObject> rcoo = new List<RenamerClassOfObject>();
        public FormRenamer()
        {
            InitializeComponent();
            rcoo = new List<RenamerClassOfObject>();
            textBox3_TextChanged_1(null, null);
            lb1.DataSource = rcoo;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public new static void Show()
        {
            new FormRenamer().ShowDialog();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            try 
            {
               var f1 = Regex.IsMatch(" ", textBox1.Text);
               var f2 = Regex.IsMatch(" ", textBox2.Text);

               search_ = textBox1.Text;
               replace_ = textBox2.Text;

               Upd_List(false);
            }
            catch {}
        }

        private void FormRenamer_Load(object sender, EventArgs e)
        {
            l_help.Text = Program.help_regex;
        }

        private void textBox3_TextChanged_1(object sender, EventArgs e)
        {
             Upd_List(true);
        }


        public void Upd_List(bool refill)
        {
            if (refill)
            try
            {
                var f1 = Regex.IsMatch(" ", textBox3.Text);
                List<PackContent> objects = Eggs.GetObjects(textBox3.Text, -1, true);
                rcoo.Clear();
                foreach (PackContent pc in objects)
                {
                    rcoo.Add(new RenamerClassOfObject(pc));
                }
            }
            catch { }

            lb1.DataBindings.DefaultDataSourceUpdateMode = DataSourceUpdateMode.OnPropertyChanged;
            lb1.DataSource = null;
            lb1.DataSource = rcoo;
            lb1.Update();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (RenamerClassOfObject rc in rcoo)
                rc.Rename();
        }

        private void button1_Click(object sender, EventArgs e)
        {
             button2_Click( sender,  e);
             button3_Click( sender,  e);
        }

    }
   

    public class RenamerClassOfObject 
    {
        PackContent obj;
        string new_name;
        public RenamerClassOfObject(PackContent obj)
        {
            this.obj = obj;
        }

        public void Rename()
        {
            obj.name = new_name + "\0";
        }

        public override string ToString()
        {
            string tmp_name = obj.name.Trim('\0');
            if (Regex.IsMatch(tmp_name, FormRenamer.search_))
                new_name = Regex.Replace(tmp_name, FormRenamer.search_, FormRenamer.replace_);
            else
                new_name = tmp_name;
            return String.Format("{0} \t{1}  -->  {2}", ElementType.ReturnString(obj.loadedformat), tmp_name, new_name);
        }
    }
}

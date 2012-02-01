using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
namespace ResourceCollector
{
    public partial class FormObjectPicker : Form
    {

        public List<string> PickedContent     {     get;    private set;     }

        bool multiselect;
        public FormObjectPicker(Pack p, int filter, bool multiselect = false, string label = "", string found_regex = "", bool auto = false)
        {

            this.multiselect = multiselect;
            PickedContent = new List<string>();
            InitializeComponent();
            checkedListBox1.Items.AddRange(
                p.Objects.FindAll(o => o.loadedformat == filter || o.forsavingformat == filter).ConvertAll(o => o.name).ToArray()); ;

            if (label != "") { label1.Text = label; label1.Visible = true; }

            if (found_regex != "")
            {
                int i =0;
                foreach (dynamic val in checkedListBox1.Items)
                {
                    
                    try
                    {
                        if (Regex.IsMatch(val, found_regex))
                        {
                            checkedListBox1.SetItemChecked(i, true);
                            if (multiselect)
                            {
                                checkedListBox1.SelectedItems.Add(val);
                            }
                            else
                            {
                                checkedListBox1.SelectedItem = val;
                                button1.Focus();
                                break;
                            }

                        }
                    }
                    catch { }
                    i++;
                }
            }

            if (auto)
            { button1_Click(null, null); }

            checkedListBox2.Items.AddRange(
                Eggs.Buffer.FindAll(o => o.loadedformat == filter || o.forsavingformat == filter).ConvertAll(o => o.name).ToArray()); ;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CheckedListBox lb = splitContainer1.Panel2Collapsed ? checkedListBox1 : checkedListBox2;
                if (lb.CheckedItems.Count > 0)
                {
                    if (lb.SelectedIndex != -1)
                    {
                        if (multiselect)
                        {
                            PickedContent.Clear();
                            for (int i = 0; i < lb.CheckedItems.Count; i++)
                            {
                                PickedContent.Add(lb.CheckedItems[i].ToString());
                                Eggs.Bufferize(PackList.Instance.GetObject(lb.CheckedItems[i].ToString()));
                            }

                        }
                        else
                        {
                            PickedContent.Clear();
                            PickedContent.Add(lb.CheckedItems[0].ToString());
                            Eggs.Bufferize(PackList.Instance.GetObject(PickedContent[0]));
                        }
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    Eggs.Message("Select somethig!!!");
                }
        }

       /* private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (checkedListBox1.SelectedIndex != -1)
            {
                if (!multiselect)
                {
                   // PickedContent.Clear();
                    if (checkedListBox1.CheckedItems.Count > 1)
                    {
                        MessageBox.Show("You can to pick only 1 element!");
                        checkedListBox1.SetItemChecked(checkedListBox1.CheckedIndices[1], false);
                     //   PickedContent.Add(checkedListBox1.CheckedItems[0].ToString());
                    }
                }
            }
        }*/

        private void checkedListBox1_MouseClick(object sender, MouseEventArgs e)
        {
            CheckedListBox lb = (CheckedListBox)sender;
                if (lb.SelectedIndex != -1)
                    if (!multiselect)
                        if (lb.CheckedItems.Count > 1)
                        {
                            MessageBox.Show("You can to pick only 1 element!");
                            lb.SetItemChecked(lb.CheckedIndices[1], false);
                        }
        }
        /*
        private void checkedListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (splitContainer1.Panel2Collapsed)
            {
                if (checkedListBox1.SelectedIndex != -1)
                    if (!multiselect)
                        if (checkedListBox1.CheckedItems.Count > 1)
                        {
                            MessageBox.Show("You can to pick only 1 element!");
                            checkedListBox1.SetItemChecked(checkedListBox1.CheckedIndices[1], false);
                        }
            }
            else
            {
                if (checkedListBox2.SelectedIndex != -1)
                    if (!multiselect)
                        if (checkedListBox2.CheckedItems.Count > 1)
                        {
                            MessageBox.Show("You can to pick only 1 element!");
                            checkedListBox1.SetItemChecked(checkedListBox1.CheckedIndices[1], false);
                        }
            }
        }*/

        private void FormObjectPicker_Load(object sender, EventArgs e)
        {
            button1.Focus();
        }

        private void FormObjectPicker_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter && checkedListBox1.CheckedItems.Count > 0)
                button1_Click(null, null);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text == "Buffer")
            {
                button4.Text = "All Objects";
                splitContainer1.Panel1Collapsed = true;
                Update();
            }
            else
            {
                button4.Text = "Buffer";
                splitContainer1.Panel2Collapsed = true;
                Update();
            }
        }

       

    }
}

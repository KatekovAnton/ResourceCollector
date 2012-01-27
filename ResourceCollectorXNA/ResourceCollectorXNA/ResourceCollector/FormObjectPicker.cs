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

        public List<string> PickedContent
        {
            get;
            private set;
        }
        bool multiselect;
        public FormObjectPicker(Pack p, int filter, bool multiselect = false, string label = "", string found_regex = "")
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
                                button1.Focus();
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
            if (checkedListBox1.CheckedItems.Count > 0)
            {
                if (checkedListBox1.SelectedIndex != -1)
                {
                    if (multiselect)
                    {
                        PickedContent.Clear();
                        for (int i = 0; i < checkedListBox1.CheckedItems.Count; i++)
                        {
                            PickedContent.Add(checkedListBox1.CheckedItems[i].ToString());
                        }

                    }
                    else
                    {
                        PickedContent.Clear();
                        PickedContent.Add(checkedListBox1.CheckedItems[0].ToString());
                    }
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();
                }
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
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
        }

        private void checkedListBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (checkedListBox1.SelectedIndex != -1)
            {
                if (!multiselect)
                {
                    //PickedContent.Clear();
                    if (checkedListBox1.CheckedItems.Count > 1)
                    {
                        MessageBox.Show("You can to pick only 1 element!");
                        checkedListBox1.SetItemChecked(checkedListBox1.CheckedIndices[1], false);
                    }
                }
            }
        }

        private void checkedListBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (checkedListBox1.SelectedIndex != -1)
            {
                if (!multiselect)
                {
                    if (checkedListBox1.CheckedItems.Count > 1)
                    {
                        MessageBox.Show("You can to pick only 1 element!");
                        checkedListBox1.SetItemChecked(checkedListBox1.CheckedIndices[1], false);
                    }
                }
            }
        }

        private void FormObjectPicker_Load(object sender, EventArgs e)
        {

        }

       

    }
}

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
    public partial class FormObjectPicker : Form
    {

        public List<string> PickedContent
        {
            get;
            private set;
        }
        bool multiselect;
        public FormObjectPicker(Pack p, int filter, bool multiselect = false)
        {
            this.multiselect = multiselect;
            PickedContent = new List<string>();
            InitializeComponent();
            checkedListBox1.Items.AddRange(
                p.Objects.FindAll(o => o.loadedformat == filter || o.forsavingformat == filter).ConvertAll(o => o.name).ToArray()); ;
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

       

    }
}

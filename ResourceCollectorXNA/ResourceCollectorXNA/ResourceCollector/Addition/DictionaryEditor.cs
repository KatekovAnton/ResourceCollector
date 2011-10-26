using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ResourceCollector.Addition
{
    public partial class DictionaryEditor : Form
    {
        public Dictionary<string, string> editedDict;

        public DictionaryEditor()
        {
            InitializeComponent();
            editedDict = new Dictionary<string, string>();
            outDict();
        }



        public DictionaryEditor(Dictionary<string, string> parameter)
        {
            InitializeComponent();
            editedDict = DictionaryMethods.CreateCopy(parameter);
            outDict();
        }

        public void outDict()
        {
            dataGridView1.Rows.Clear();
            foreach(string s in editedDict.Keys)
            {
                DataGridViewRow newrow = new DataGridViewRow();
                newrow.CreateCells(dataGridView1, new object[] { s, editedDict[s] });
                dataGridView1.Rows.Add(newrow);
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                selectedrow.Cells[0].Value = key;
            }
            else
            {
                string key1 = dataGridView1[0, e.RowIndex].Value.ToString();
                editedDict[key1] = dataGridView1[e.ColumnIndex, e.RowIndex].Value.ToString();
            }
        }

        private string createString(string input)
        {
            return input.Trim('\0', ' ');
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string textkey = createString(textBox1.Text);
            string textvalue = createString(textBox2.Text);
            if (textkey != "" && !editedDict.ContainsKey(textkey) && textvalue != "")
            {
                editedDict.Add(textkey, textvalue);
                DataGridViewRow newrow = new DataGridViewRow();
                newrow.CreateCells(dataGridView1, new object[] { textkey, textvalue });
                dataGridView1.Rows.Add(newrow);
            }
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            if (key != null)
            {
                editedDict.Remove(key);
                selectedrow = null;
                key = null;
            }
        }

        DataGridViewRow selectedrow;
        string key;

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 0)
            {
                selectedrow = dataGridView1.SelectedRows[0];
                key = selectedrow.Cells[0].Value.ToString();
            }
            else
            {
                selectedrow = null;
                key = null;
            }
        }
    }
}

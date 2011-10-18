using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;



namespace ResourceCollector.Content
{
    public partial class FormRenderObjectProperties : Form
    {
        RenderObjectDescription thisobject;
        System.Windows.Forms.TreeView outputtreeview;
        public FormRenderObjectProperties(RenderObjectDescription _ro,  System.Windows.Forms.TreeView _outputtreeview)
        {
            InitializeComponent();
            createdContent = new List<PackContent>();
            thisobject = _ro;
           // textBox4.Text = thisobject.matname;
            outlods();
            outputtreeview = _outputtreeview;
            checkBoxROShadowCaster.Checked = thisobject.IsShadowCaster;
            checkBoxROShadowReceiver.Checked = thisobject.IsShadowReceiver;
            checkBoxRONeedRotate.Checked = thisobject.NeedRotate;
            textBox1.Text = thisobject.name.Length > 0 ? thisobject.name : "New world object";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < createdContent.Count; i++)
            {
                thisobject.pack.Attach(this.createdContent[i], outputtreeview);
            }
            if (textBox1.Text != "")
                thisobject.name = textBox1.Text.TrimEnd('\0') + "\0";
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
        private void outlods()
        {
            listBox1.Items.Clear();
            for (int i = 0; i < thisobject.LODs.Count; i++)
            {
                listBox1.Items.Add("Lod number " + i.ToString() + "; Subset count = " + thisobject.LODs[i].subsets.Count.ToString());
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            thisobject.addlod();
            CurrentLod = thisobject.LODs[thisobject.LODs.Count - 1];
            outlods();
            // CurrentSubSet = null;
            OutSelectedLod();
            OutSubSet();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1 && listBox1.SelectedIndex < thisobject.LODs.Count)
            {
                thisobject.LODs.RemoveAt(listBox1.SelectedIndex);
                outlods();
                CurrentLod = null;
                OutSelectedLod();
                CurrentSubSet = null;
                OutSubSet();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            thisobject.LODs.Clear();
            outlods();
            CurrentLod = null;
            OutSelectedLod();
            CurrentSubSet = null;
            OutSubSet();
        }
        RenderObjectDescription.Model CurrentLod;
        RenderObjectDescription.SubSet CurrentSubSet;
        private void OutSelectedLod()
        {
            listBox2.Items.Clear();
            if (CurrentLod != null)
                for (int i = 0; i < CurrentLod.subsets.Count; i++)
                    listBox2.Items.Add(CurrentLod.subsets[i].ToString());
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index >= 0 && index < thisobject.LODs.Count)
            {


                CurrentLod = thisobject.LODs[index];
                OutSelectedLod();
                OutSubSet();

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (CurrentLod != null)
            {
                CurrentSubSet = new RenderObjectDescription.SubSet(new string[] { });
                CurrentLod.subsets.Add(CurrentSubSet);
                OutSelectedLod();
                outlods();
                OutSubSet();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1 && listBox2.SelectedIndex < CurrentLod.subsets.Count)
            {
                CurrentLod.subsets.RemoveAt(listBox2.SelectedIndex);
                OutSelectedLod();
                outlods();
                CurrentSubSet = null;
                OutSubSet();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (CurrentLod != null)
                CurrentLod.subsets.Clear();
            OutSelectedLod();
            outlods();
            CurrentSubSet = null;
            OutSubSet();
        }
        private void OutSubSet()
        {
            listBox3.Items.Clear();
            if (CurrentSubSet != null)
            {
                

                listBox3.Items.AddRange(CurrentSubSet.MeshNames);
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = listBox2.SelectedIndex;
            if (index >= 0 && index < CurrentLod.subsets.Count)
            {
                CurrentSubSet = CurrentLod.subsets[index];
                OutSubSet();
            }
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void buttonSelectMeshList_Click(object sender, EventArgs e)
        {
            if (CurrentSubSet != null)
            {
                FormObjectPicker fop = new FormObjectPicker(thisobject.pack, ElementType.MeshSkinnedOptimazedForLoading, true);
                if (fop.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    CurrentSubSet.MeshNames = fop.PickedContent.ToArray();
                    OutSubSet();
                    OutSelectedLod();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CurrentSubSet.MeshNames = new string[0];
            OutSubSet();
            OutSelectedLod();
        }

        private void checkBoxROShadowCaster_CheckedChanged(object sender, EventArgs e)
        {
            thisobject.IsShadowCaster = checkBoxROShadowCaster.Checked;
        }

        private void checkBoxROShadowReceiver_CheckedChanged(object sender, EventArgs e)
        {
            thisobject.IsShadowReceiver = checkBoxROShadowReceiver.Checked;
        }

        private void checkBoxRONeedRotate_CheckedChanged(object sender, EventArgs e)
        {
            thisobject.NeedRotate = checkBoxRONeedRotate.Checked;
        }

       
       
        public List<PackContent> createdContent;

        

        private void button11_Click(object sender, EventArgs e)
        {
          /*  FormObjectPicker fop = new FormObjectPicker(thisobject.pack, ElementType.Material);
            if (fop.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //  RenderObject wod = thisobject.Pack.getobject(fop.PickedContent[0]) as RenderObject;
                thisobject.matname = fop.PickedContent[0];
                textBox4.Text = thisobject.matname;
            }*/
        }
   
    }
}

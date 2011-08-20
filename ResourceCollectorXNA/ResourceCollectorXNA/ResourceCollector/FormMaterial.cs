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
    public partial class FormMaterial : Form
    {
        Material thisekement;
        public FormMaterial(Material m)
        {
            InitializeComponent();
            thisekement = m;
            textBoxName.Text = m.name;
            outMatLods();
        }

        Material.Lod currentLod;
        Material.SubsetMaterial currentSubSet;

        //select lod
        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                currentLod = thisekement.lodMats[listBox1.SelectedIndex];
                currentSubSet = null;
                outLod();
                outSubSet();
                
            }
        }
        public void outSubSet()
        {
            if (currentSubSet != null)
            {
                textBox1.Text = currentSubSet.DiffuseTextureName;
            }
            else
                textBox1.Text = "";
        }
        public void outLod()
        {
            listBox2.Items.Clear();
            if (currentLod != null)
            {
                for (int i = 0; i < currentLod.mats.Count; i++)
                {
                    listBox2.Items.Add(currentLod.mats[i]);
                }
            }
        }
        public void outMatLods()
        {

            listBox1.Items.Clear();
            for (int i = 0; i < thisekement.lodMats.Count; i++)
            {
                listBox1.Items.Add(thisekement.lodMats[i]);
            }
        }
        //select subset
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1 && currentLod != null)
            {
                currentSubSet = currentLod.mats[listBox2.SelectedIndex];
                outSubSet();
            }
        }
        //select texture
        private void button3_Click(object sender, EventArgs e)
        {
            if (currentSubSet != null)
            {
                FormObjectPicker fop = new FormObjectPicker(thisekement.pack, ElementType.PNGTexture);
                if (fop.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textBox1.Text = currentSubSet.DiffuseTextureName = fop.PickedContent[0];
                    
                    outLod();
                    
                }
            }
        }
        //add subset
        private void button2_Click(object sender, EventArgs e)
        {
            currentLod.mats.Add(new Material.SubsetMaterial());
            currentSubSet = currentLod.mats[currentLod.mats.Count - 1];
            outMatLods();
            outLod();
            outSubSet();
        }
        //add lod
        private void button1_Click(object sender, EventArgs e)
        {
            thisekement.lodMats.Add(new Material.Lod());
            currentLod = thisekement.lodMats[thisekement.lodMats.Count - 1];
            currentSubSet = null;
            outMatLods();
            outLod();
            outSubSet();
        }
        //del ss
        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex != -1 && currentLod != null)
            {
                currentLod.mats.RemoveAt(listBox2.SelectedIndex);
                currentSubSet = null;
                outMatLods();
                outLod();
                outSubSet();
            }
        }
        //del lod
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                thisekement.lodMats.RemoveAt(listBox1.SelectedIndex);
                currentSubSet = null;
                currentLod = null;
                outMatLods();
                outLod();
                outSubSet();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            thisekement.name = textBoxName.Text + "\0";
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}

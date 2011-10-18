using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ResourceCollector
{
    public partial class LODInterface : Form
    {
        public List<MeshSkinned> meshes = new List<MeshSkinned>();
        string texname;
        MeshSkinned bvasemesh;
        public LODInterface(MeshSkinned msh, string texnm)
        {
            texname = texnm;
            bvasemesh = msh;
            InitializeComponent();
            textBox1.Text = msh.name; ;
            label6.Text = msh.num_faces.ToString();
            label5.Text = msh.num_verts.ToString();
            label4.Text = msh.num_tverts.ToString();
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            button1.Text = "Generate (" + (Convert.ToSingle(trackBar1.Value)*100.0f / Convert.ToSingle(trackBar1.Maximum)).ToString() + "%)";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            LODBuilder lb = new LODBuilder(bvasemesh);
            try
            {
                int tocollapce = lb.BaseMesh.num_verts - lb.BaseMesh.num_verts * trackBar1.Value / trackBar1.Maximum;
                if (lb.BaseMesh.num_verts - tocollapce > 5)
                {
                    lb.CollapceEdges(tocollapce);
                    meshes.Add(lb.ProcessedMesh);
                    meshes[meshes.Count - 1].name = lb.ProcessedMesh.name.TrimEnd('\0') + "_lod" + (meshes.Count - 1).ToString() + "\0";

                    /*meshviever mv = new meshviever(lb.ProcessedMesh);
                    mv.ShowDialog();

                   
                    mv.device.Dispose();*/
                    lb.ProcessedMesh.GenerateOptForLoading();
                    listBox1.Items.Add(meshes[meshes.Count - 1].name.TrimEnd('\0') + " (facecount =" + (meshes[meshes.Count - 1].BufferIndex.Length / 3).ToString() + ")");
                }
                else
                    MessageBox.Show("Need more vertices");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            button1.Text = "Generate (" + (trackBar1.Value / trackBar1.Maximum).ToString() + "%)";
    
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                meshes.RemoveAt(listBox1.SelectedIndex);
                listBox1.Items.Clear();
                for (int i = 0; i < meshes.Count; i++)
                    listBox1.Items.Add(meshes[i].name.TrimEnd('\0') + " (facecount =" + (meshes[i].BufferIndex.Length / 3).ToString() + ")");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            meshes = new List<MeshSkinned>();

        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                /*meshviever ms = new meshviever(meshes[listBox1.SelectedIndex]);
                ms.ShowDialog();
                if (ms.generated)
                {
                    meshes[listBox1.SelectedIndex] = ms.ffffff;
                }
           
                ms.device.Dispose();*/
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*meshviever mv = new meshviever(bvasemesh);
            mv.ShowDialog();
            if (mv.generated)
            {
                meshes[listBox1.SelectedIndex] = mv.ffffff;
            }
        
            mv.device.Dispose();*/
        }
    }
}

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
    public partial class FormMeshCombiner : Form
    {
        MeshSkinned _createdMesh;
        public bool _removemeshes;

        public List<MeshSkinned> _meshesForCombine;

        public FormMeshCombiner()
        {
            InitializeComponent();
            _createdMesh = new MeshSkinned();
            _createdMesh.lods = new string[0];
            _createdMesh.loadedformat = _createdMesh.forsavingformat = ElementType.MeshSkinnedOptimazedForLoading;
            _meshesForCombine = new List<MeshSkinned>();
            textBox1.Text = "NewCombinedMesh" + DateTime.Now.Millisecond.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.OK;
            if (textBox1.Text != "")
                _createdMesh.name = textBox1.Text + "\0";
            else
                _createdMesh.name = "NewCombinedMesh" + DateTime.Now.Millisecond.ToString();


            int vertcount = 0;
            int facecount = 0;
            for (int i = 0; i < _meshesForCombine.Count; i++)
            {
                vertcount += _meshesForCombine[i].BufferVertex.Length;
                facecount += _meshesForCombine[i].BufferIndex.Length;
            }
            _createdMesh.BufferIndex = new int[facecount];
            _createdMesh.BufferVertex = new CSkinnedMeshVertex[vertcount];
            int currentPlaceIndex = 0;
            int currentPlaceVertex = 0;
            for (int i = 0; i < _meshesForCombine.Count; i++)
            {
                MeshSkinned currentMesh = _meshesForCombine[i];
                for (int j = 0; j < currentMesh.BufferVertex.Length; j++)
                    _createdMesh.BufferVertex[currentPlaceVertex + j] = new CSkinnedMeshVertex(currentMesh.BufferVertex[j]);
                for (int j = 0; j < currentMesh.BufferIndex.Length; j++)
                    _createdMesh.BufferIndex[currentPlaceIndex + j] = currentMesh.BufferIndex[j] + currentPlaceVertex;

                currentPlaceVertex += currentMesh.BufferVertex.Length;
                currentPlaceIndex += currentMesh.BufferIndex.Length;
            }

            PackList.Instance.packs[0].Attach(_createdMesh, FormMainPackExplorer.Instance.treeView1);

            Close();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            _removemeshes = checkBox1.Checked;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FormObjectPicker fop = new FormObjectPicker(PackList.Instance.packs[0], ElementType.MeshSkinnedOptimazedForLoading, true);
            if(fop.ShowDialog()!= System.Windows.Forms.DialogResult.OK)
                return;
            foreach (string name in fop.PickedContent)
            {
                MeshSkinned mesh = PackList.Instance.GetObject(name) as MeshSkinned;
                if (mesh == null)
                    throw new Exception("unable to find mesh with name " + name);
                _meshesForCombine.Add(mesh);
                listBox1.Items.Add(name);
            }
            int vertcount = 0;
            for (int i = 0; i < _meshesForCombine.Count; i++)
                vertcount += _meshesForCombine[i].BufferVertex.Length;
            label3.Text = vertcount.ToString();


            int facecount = 0;
            for (int i = 0; i < _meshesForCombine.Count; i++)
                facecount += _meshesForCombine[i].BufferIndex.Length / 3;
            label4.Text = facecount.ToString();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            _meshesForCombine.Clear();
            listBox1.Items.Clear();
            label3.Text = "0";
            label4.Text = "0";
        }

    }
}

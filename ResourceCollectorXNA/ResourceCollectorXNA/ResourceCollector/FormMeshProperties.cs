using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace ResourceCollector
{
    partial class MeshProperties : Form
    {
        MeshSkinned mesh;
        public MeshSkinned[] lods;
        public MeshProperties(MeshSkinned msh)
        {
            InitializeComponent();
           /* System.IO.StreamWriter sw = new System.IO.StreamWriter(msh.name.Substring(0,msh.name.Length-1) + ".txt");
            sw.WriteLine("VertexPositionColor[] vertices = new VertexPositionColor[]{");
            for (int i = 0; i < msh.BufferVertex.Length; i++)
            {
               // ResourceCollectorXNA.Content.Vertex v = new ResourceCollectorXNA.Content.Vertex(new Microsoft.Xna.Framework.Vector3(
                sw.WriteLine(
                    "new VertexPositionColor(new Vector3(" +
                    msh.BufferVertex[i].pos.X.ToString().Replace(',', '.') + "f," + msh.BufferVertex[i].pos.Y.ToString().Replace(',', '.') + "f," + msh.BufferVertex[i].pos.Z.ToString().Replace(',', '.') + "f)," +
                    " Color.White),");
            }
            sw.WriteLine("};");
            sw.WriteLine("short[] indices = {");
            string indi = "";
            for (int i = 0; i < msh.BufferIndex.Length; i++)
            {
                // ResourceCollectorXNA.Content.Vertex v = new ResourceCollectorXNA.Content.Vertex(new Microsoft.Xna.Framework.Vector3(
                indi += msh.BufferIndex[i].ToString() + ", ";
            }
           sw.WriteLine( indi.Substring(0, indi.Length - 2) + "};");
            sw.Close();*/
            mesh = msh;
            groupBox1.Text = mesh.name;
            _labelbodysize.Text = mesh.size.ToString();
            _labelindex.Text = mesh.number.ToString(); 
            _labeloffset.Text = mesh.offset.ToString();
            _labelheadersize.Text = mesh.headersize.ToString();
            comboBox1.Enabled = false;
            comboBox2.Enabled = true;
           
            comboBox1.Text = ElementType.ReturnString(mesh.loadedformat);
            comboBox2.Items.Add(ElementType.ReturnString(ElementType.MeshSkinnedOptimazedForStore));
            comboBox2.Items.Add(ElementType.ReturnString(ElementType.MeshSkinnedOptimazedForLoading));
            comboBox2.Text = ElementType.ReturnString(mesh.forsavingformat);
            label2.Text = mesh.ismaxdetalized.ToString();
            if (mesh.faces != null && mesh.tvertexes!= null)
            {
                _labelfaces.Text = mesh.faces.Length.ToString();
                _labeltcoords.Text = mesh.tvertexes.Length.ToString();
                _labelvertices.Text = mesh.vertexes.Length.ToString();
            }
            else
            {
                _labelib.Text = mesh.BufferIndex.Length.ToString();
                _labelvb.Text = mesh.BufferVertex.Length.ToString();
            }
            if (mesh.ismaxdetalized == 1)
                for (int i = 0; i < mesh.lods.Length; i++)
                    listBox1.Items.Add(mesh.lods[i]);
            else
            {
                button1drop.Enabled = button3dropall.Enabled = button4gennew.Enabled = listBox1.Enabled= false;
            }
        }

        private void button1drop_Click(object sender, EventArgs e)
        {

        }

        private void button3dropall_Click(object sender, EventArgs e)
        {

        }

        private void button4gennew_Click(object sender, EventArgs e)
        {
            mesh.GenerateOptForLoading();
            LODInterface li = new LODInterface(mesh, "");
            li.ShowDialog();
            if (li.meshes.Count != 0)
            {
                lods = li.meshes.ToArray();
                mesh.lods = new string[lods.Length];
                for (int i = 0; i < lods.Length; i++)
                {
                    listBox1.Items.Add(lods[i].name.TrimEnd('\0') + " (facecount =" + (lods[i].BufferIndex.Length / 3).ToString() + ")");
                    mesh.lods[i] = lods[i].name;
                }
            }

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            mesh.forsavingformat = ElementType.ReturnFormat(comboBox2.Text);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                //meshviever mv = new meshviever(mesh);
                //mv.ShowDialog();
            }
            else
            {
                string path = Application.ExecutablePath.Substring(0, Application.ExecutablePath.LastIndexOf('\\'));
                Assembly ass = Assembly.LoadFile(path + "\\tester.dll");
                Type type = ass.GetType("SWRenderForm");
                Form sm = null;
                sm = type.InvokeMember("", BindingFlags.CreateInstance, null, sm, new object[] { mesh }) as Form;

                
                //ConstructorInfo @ctor = type.GetConstructor(new Type[] { typeof(Mesh) });
                //Form sm = @ctor.Invoke(new Object[] { mesh }) as Form;
                sm.ShowDialog();

                //SWRenderForm sm = new SWRenderForm();
                //sm.ShowDialog();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("It's vert important to make a rigth choose. Be carefull with using\n Managed DirectX render in Windows Vista or Windows 7");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            mesh.mult(0.1f);
        }
    }
}

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
    public partial class FormParticleRenderObjectDescription : Form
    {
        ParticleRenderObjectDescription _obj;

        public FormParticleRenderObjectDescription(ParticleRenderObjectDescription __ooo)
        {
            _obj = __ooo;
            InitializeComponent();

            textBox1.Text = "NewParticleRenderObject";
            if (__ooo.name != null)
                textBox1.Text = __ooo.name;
            textBox2.Text = __ooo.MeshName;
            textBox3.Text = __ooo.TextureName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormObjectPicker fo = new FormObjectPicker(_obj.pack, ElementType.MeshSkinnedOptimazedForLoading);
            if (fo.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _obj.MeshName = fo.PickedContent[0];
                textBox2.Text = _obj.MeshName;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormObjectPicker fo = new FormObjectPicker(_obj.pack, ElementType.PNGTexture);
            if (fo.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _obj.TextureName = fo.PickedContent[0];
                textBox3.Text = _obj.TextureName;

            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ResourceCollector.Content;
namespace ResourceCollector
{
    public partial class FormNewElement : Form
    {
        PackList packs; TreeView tv;
        public FormNewElement(PackList _packs, TreeView _tv)
        {
            InitializeComponent();
            packs = _packs;
            tv = _tv;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = true;
            
                if (fileDialog.ShowDialog() != DialogResult.Cancel)
                {
                   foreach (string f_name in fileDialog.FileNames)
                    {
                       try
                       {
                            var path = f_name.Split('\\', '/');
                            var image = new ImageContent(path[path.Length - 1], new Bitmap(f_name));

                            packs.packs[0].Attach(image);
                            FormMainPackExplorer.Instance.UpdateData();
                       }
                       catch 
                       {
                           MessageBox.Show(string.Format("File «{0}» contains no image data!", f_name));
                       }
                    }
                }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TextureListContent tl = new TextureListContent();
            tl.Pack = packs.packs[0];
            if (tl.createpropertieswindow(packs.packs[0], tv) == System.Windows.Forms.DialogResult.OK)
            { 
                packs.packs[0].Attach(tl);
                FormMainPackExplorer.Instance.UpdateData();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FormCollisionMesh ccc = new FormCollisionMesh(tv, packs.packs[0]);
            if(ccc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                packs.packs[0].Attach(ccc.cm);
                FormMainPackExplorer.Instance.UpdateData();
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            LevelObjectDescription rod = new LevelObjectDescription();
            rod.Pack = packs.packs[0];
            if (rod.createpropertieswindow(rod.Pack, tv) == System.Windows.Forms.DialogResult.OK)
            {
                rod.Pack.Attach(rod);
                FormMainPackExplorer.Instance.UpdateData();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (packs.packs.Count > 0)
            {
                LevelObjectDescription wod = packs.packs[0].getobject("TestSideWorldObject\0") as LevelObjectDescription;
                ResourceCollectorXNA.Engine.Logic.LevelObject testsidelevelobject = ResourceCollectorXNA.Engine.ContentLoader.ContentLoader.LevelObjectFromDescription(wod, packs.packs[0]);
                testsidelevelobject.SetGlobalPose(Microsoft.Xna.Framework.Matrix.CreateFromAxisAngle(new Microsoft.Xna.Framework.Vector3(1, 0, 0), -Microsoft.Xna.Framework.MathHelper.PiOver2) * Microsoft.Xna.Framework.Matrix.CreateTranslation(0, 15, 0));
                ResourceCollectorXNA.MyGame.AddOject(testsidelevelobject);


                LevelObjectDescription wod1 = packs.packs[0].getobject("WoodenCrate10WorldObject\0") as LevelObjectDescription;
                ResourceCollectorXNA.Engine.Logic.LevelObject testsidelevelobject1 = ResourceCollectorXNA.Engine.ContentLoader.ContentLoader.LevelObjectFromDescription(wod1, packs.packs[0]);
                testsidelevelobject1.SetGlobalPose(Microsoft.Xna.Framework.Matrix.CreateTranslation(3, 20, 0));
                ResourceCollectorXNA.MyGame.AddOject(testsidelevelobject1);
                Close();
            }
            else
                MessageBox.Show("Load some packs first");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResourceCollectorXNA.Engine.Logic.LevelTerrain lt = new ResourceCollectorXNA.Engine.Logic.LevelTerrain(new  Microsoft.Xna.Framework.Vector2(1000,1000));
            foreach (ResourceCollectorXNA.Engine.Logic.TerrainObject obj in lt.terrain)
            {
                ResourceCollectorXNA.MyGame.AddOject(obj);
            }
            ResourceCollectorXNA.MyGame.AddOject(lt.backterrain);
            Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            RenderObjectDescription rod = new RenderObjectDescription();
            rod.pack = packs.packs[0];
         
            if (rod.createpropertieswindow(rod.pack, tv) == System.Windows.Forms.DialogResult.OK)
            {
                rod.pack.Attach(rod);
                FormMainPackExplorer.Instance.UpdateData();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (packs.packs.Count > 0)
            {

                FormObjectPicker fop = new FormObjectPicker(packs.packs[0], ElementType.LevelObjectDescription);
                if (fop.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string str = fop.PickedContent[0];
                    LevelObjectDescription wod1 = packs.packs[0].getobject(str) as LevelObjectDescription;
                    ResourceCollectorXNA.Engine.Logic.LevelObject testsidelevelobject1 = ResourceCollectorXNA.Engine.ContentLoader.ContentLoader.LevelObjectFromDescription(wod1, packs.packs[0]);
                    testsidelevelobject1.SetGlobalPose(Microsoft.Xna.Framework.Matrix.CreateTranslation(3, 20, 0));
                    ResourceCollectorXNA.MyGame.AddOject(testsidelevelobject1);
                    Close();
                }
            }
            else
                MessageBox.Show("Load some packs first");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            Material rod = new Material();
            rod.pack = packs.packs[0];
            if (rod.createpropertieswindow(rod.pack, tv) == System.Windows.Forms.DialogResult.OK)
            {
                rod.pack.Attach(rod);
                FormMainPackExplorer.Instance.UpdateData();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ParticleRenderObjectDescription prod = new ParticleRenderObjectDescription();
            prod.pack = packs.packs[0];
            if (prod.createpropertieswindow(prod.pack, tv) == System.Windows.Forms.DialogResult.OK)
            {
                prod.pack.Attach(prod);
                FormMainPackExplorer.Instance.UpdateData();
            }
        }

    }
}

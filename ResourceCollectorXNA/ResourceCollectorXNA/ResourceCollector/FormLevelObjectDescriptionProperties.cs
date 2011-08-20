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
    public partial class FormLevelObjectDescriptionProperties : Form
    {
        LevelObjectDescription thisobject;
        TreeView outputtreeview;
        public FormLevelObjectDescriptionProperties(LevelObjectDescription rod, TreeView outputtreeview)
        {
            
            createdContent = new List<PackContent>();
            InitializeComponent();
            thisobject = rod;
            textBox2.Text = thisobject.RODName;
            textBox4.Text = thisobject.matname;
            this.outputtreeview = outputtreeview;
           // outlods();
            textBox1.Text = thisobject.name.Length>0?thisobject.name:"New world object";

            bool IsRCCMEnabled = rod.IsRCCMEnabled;


            if (thisobject.IsRCCMEnabled)
            {
               
                textBoxRCCMName.Text = thisobject.RCCMName;
                groupBoxProcedureShapeProperties.Enabled = false;
                groupBoxRCCM.Enabled = true;

            }
            else
            {
                radioButtonRCShapeTypeCM.Checked = false;
                radioButtonRCShapeTypeProcedureShape.Checked = true;
                groupBoxProcedureShapeProperties.Enabled = true;
                groupBoxRCCM.Enabled = false;
                thisobject.IsRCCMEnabled = false;
                comboBoxProcedureShapeType.SelectedIndex = thisobject.RCShapeType;
                textBoxProcedureShapeSizeX.Text = thisobject.RCShapeSize.X.ToString();
                textBoxProcedureShapeSizeY.Text = thisobject.RCShapeSize.Y.ToString();
                textBoxProcedureShapeSizeZ.Text = thisobject.RCShapeSize.Z.ToString();
            }
           // checkBoxRCCMEnabled.Enabled = true;
            checkBoxIsAnimated.Checked = thisobject.IsAnimated;
            //   checkBoxIsAnimated.Enabled = true;
            checkBoxIsAnimatedRCCM.Checked = thisobject.IsRCCMAnimated;
           
            comboBoxBehaviourType.Items.Add(LevelObjectDescription.GetName(0));
            comboBoxBehaviourType.Items.Add(LevelObjectDescription.GetName(1));
            comboBoxBehaviourType.Items.Add(LevelObjectDescription.GetName(2));
            comboBoxBehaviourType.Items.Add(LevelObjectDescription.GetName(3));

            comboBoxBehaviourType.SelectedIndex = thisobject.BehaviourType;

            comboBoxPhysXShapeType.SelectedIndex = 0;
            switch (thisobject.BehaviourType)
            {
                case LevelObjectDescription.objectmovingbehaviourmodel:
                    {
                        groupBoxPhysicParams.Enabled = false;
                
                    } break;
                case LevelObjectDescription.objectphysicbehaviourmodel:
                    {
                        groupBoxPhysicParams.Enabled = true;
                        if (thisobject.ShapeType == 0)
                        {
                            radioButtonPhysActrTypeShape.Checked = true;
                            radioButtonPhysActrTypeCM.Checked = false;
                            groupBoxPhysXActrCM.Enabled = false;
                            groupBoxPhysXActrShape.Enabled = true;
                            comboBoxPhysXShapeType.SelectedIndex = thisobject.PhysXShapeType;
                            textBoxPhysXShapeSizeX.Text = thisobject.ShapeSize.X.ToString();
                            textBoxPhysXShapeSizeY.Text = thisobject.ShapeSize.Y.ToString();
                            textBoxPhysXShapeSizeZ.Text = thisobject.ShapeSize.Z.ToString();

                        }
                        else if (thisobject.ShapeType == 1)
                        {
                            radioButtonPhysActrTypeShape.Checked = false;
                            radioButtonPhysActrTypeCM.Checked = true;
                            groupBoxPhysXActrCM.Enabled = true;
                            groupBoxPhysXActrShape.Enabled = false;
                            textBoxPhysCMname.Text = thisobject.PhysicCollisionName;
                        }

                        textBoxPhysXMass.Text = thisobject.Mass.ToString();
                        checkBoxPhysMassOK.Checked = true;
                        textBoxPhysXCenterOfMassX.Text = thisobject.CenterOfMass.X.ToString();
                        textBoxPhysXCenterOfMassY.Text = thisobject.CenterOfMass.Y.ToString();
                        textBoxPhysXCenterOfMassZ.Text = thisobject.CenterOfMass.Z.ToString();
                        checkBoxPhysLCoMOK.Checked = true;
                        checkBoxIsStatic.Checked = thisobject.IsStatic;

                    } break;
                case LevelObjectDescription.objectphysiccharcontrollerbehaviourmodel:
                    {
                        groupBoxPhysicParams.Enabled = true;
                        checkBoxIsStatic.Enabled = false;
                        checkBoxIsStatic.Checked = false;

                        if (thisobject.ShapeType == 0)
                        {
                            radioButtonPhysActrTypeShape.Checked = true;
                            radioButtonPhysActrTypeCM.Checked = false;
                            groupBoxPhysXActrCM.Enabled = false;
                            groupBoxPhysXActrShape.Enabled = true;
                            comboBoxPhysXShapeType.SelectedIndex = thisobject.PhysXShapeType;
                            textBoxPhysXShapeSizeX.Text = thisobject.ShapeSize.X.ToString();
                            textBoxPhysXShapeSizeY.Text = thisobject.ShapeSize.Y.ToString();
                            textBoxPhysXShapeSizeZ.Text = thisobject.ShapeSize.Z.ToString();

                        }
                        else if (thisobject.ShapeType == 1)
                        {
                            radioButtonPhysActrTypeShape.Checked = false;
                            radioButtonPhysActrTypeCM.Checked = true;
                            groupBoxPhysXActrCM.Enabled = true;
                            groupBoxPhysXActrShape.Enabled = false;
                            textBoxPhysCMname.Text = thisobject.PhysicCollisionName;
                        }


                        textBoxPhysXMass.Text = thisobject.Mass.ToString();
                        checkBoxPhysMassOK.Checked = true;
                        textBoxPhysXCenterOfMassX.Text = thisobject.CenterOfMass.X.ToString();
                        textBoxPhysXCenterOfMassY.Text = thisobject.CenterOfMass.Y.ToString();
                        textBoxPhysXCenterOfMassZ.Text = thisobject.CenterOfMass.Z.ToString();
                        checkBoxPhysLCoMOK.Checked = true;
                  
                    } break;
                case LevelObjectDescription.objectstaticbehaviourmodel:
                    {
                        groupBoxPhysicParams.Enabled = false;
                    
                    } break;
                default: break;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text != "")
            {
                thisobject.name = textBox1.Text;
                if (!thisobject.name.EndsWith("\0"))
                    thisobject.name += "\0";
            }
        }
      
        public List<PackContent> createdContent
        {
            get;
            private set;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (radioButtonRCShapeTypeProcedureShape.Checked)
            {
                thisobject.IsRCCMEnabled = false;
                thisobject.RCShapeType = comboBoxProcedureShapeType.SelectedIndex;
                float x1, y1, z1;
                try
                {
                    string str = textBoxProcedureShapeSizeX.Text.Replace('.', ',');
                    x1 = Convert.ToSingle(str);
                    //  thisobject.CenterOfMass.X = x;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Wrong X");
                    return;
                }

                try
                {
                    string str = textBoxProcedureShapeSizeY.Text.Replace('.', ',');
                    y1 = Convert.ToSingle(str);
                    // thisobject.CenterOfMass.Y = y;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Wrong Y");
                    return;
                }

                try
                {
                    string str = textBoxProcedureShapeSizeZ.Text.Replace('.', ',');
                    z1 = Convert.ToSingle(str);
                    // thisobject.CenterOfMass.Y = y;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Wrong Z");
                    return;
                }
                thisobject.RCShapeSize = new Microsoft.Xna.Framework.Vector3(x1, y1, z1);
            }
            try
            {
                string str = textBoxPhysXMass.Text.Replace('.', ',');
                float mas = Convert.ToSingle(str);
                thisobject.Mass = mas;
                checkBoxPhysMassOK.Checked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wrong  mass value");
                textBoxPhysXMass.Text = thisobject.Mass.ToString();
                return;
            }

            if (this.radioButtonPhysActrTypeCM.Checked)
            {
                thisobject.ShapeType = 1;

                if (!textBoxPhysCMname.Text.EndsWith("\0"))
                    textBoxPhysCMname.Text += "\0";
                thisobject.PhysicCollisionName = textBoxPhysCMname.Text;
            }
            else if (this.radioButtonPhysActrTypeShape.Checked)
            {
                thisobject.ShapeType = 0;
                if(comboBoxPhysXShapeType.SelectedIndex >=0)
                    thisobject.PhysXShapeType = comboBoxPhysXShapeType.SelectedIndex;
                
                thisobject.ShapeSize = new Microsoft.Xna.Framework.Vector3(
                    Convert.ToSingle(textBoxPhysXShapeSizeX.Text.Replace('.', ',')),
                    Convert.ToSingle(textBoxPhysXShapeSizeY.Text.Replace('.', ',')),
                    Convert.ToSingle(textBoxPhysXShapeSizeZ.Text.Replace('.', ',')));
  
                
            }

            float x, y, z;
            try
            {
                string str = textBoxPhysXCenterOfMassX.Text.Replace('.', ',');
                x = Convert.ToSingle(str);
                //  thisobject.CenterOfMass.X = x;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wrong X");
                return;
            }

            try
            {
                string str = textBoxPhysXCenterOfMassY.Text.Replace('.', ',');
                y = Convert.ToSingle(str);
                // thisobject.CenterOfMass.Y = y;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wrong Y");
                return;
            }

            try
            {
                string str = textBoxPhysXCenterOfMassZ.Text.Replace('.', ',');
                z = Convert.ToSingle(str);
                // thisobject.CenterOfMass.Y = y;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wrong Z");
                return;
            }

            thisobject.CenterOfMass.X = x;
            thisobject.CenterOfMass.Y = y;
            thisobject.CenterOfMass.Z = z;

            


            for (int i = 0; i < createdContent.Count; i++)
            {
                thisobject.Pack.Attach(this.createdContent[i], outputtreeview);
            }

            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            Close();
        }

        private void checkBoxIsAnimated_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxIsAnimated.Enabled = checkBoxIsAnimated.Checked;
        }

        private void radioButtonPhysActrTypeCM_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxPhysXActrCM.Enabled = radioButtonPhysActrTypeCM.Checked;
            thisobject.ShapeType = 1;
        }

        private void radioButtonPhysActrTypeShape_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxPhysXActrShape.Enabled = radioButtonPhysActrTypeShape.Checked;
            groupBoxPhysXActrCM.Enabled =  !radioButtonPhysActrTypeShape.Checked;
            thisobject.ShapeType = 0;

        }

        private void buttonRCCMNameSet_Click(object sender, EventArgs e)
        {
            if (this.textBoxRCCMName.Text != "")
            {
                if (!textBoxRCCMName.Text.EndsWith("\0"))
                    textBoxRCCMName.Text += "\0";
                thisobject.RCCMName = textBoxRCCMName.Text;
            }
            else
            {
                MessageBox.Show("Name must be not empty!");
            }
        }

   
        private void buttonRCCMNameSelect_Click(object sender, EventArgs e)
        {
            FormObjectPicker fop = new FormObjectPicker(thisobject.Pack, ElementType.CollisionMesh);
            if (fop.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                thisobject.RCCMName = this.textBoxRCCMName.Text = fop.PickedContent[0];
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                string str = textBoxPhysXMass.Text.Replace('.', ',');
                float mas = Convert.ToSingle(str);
                thisobject.Mass = mas;
                checkBoxPhysMassOK.Checked = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wrong value");
                textBoxPhysXMass.Text = thisobject.Mass.ToString();
            }
        }

        private void buttonPhysCMNameSet_Click(object sender, EventArgs e)
        {
            if (this.textBoxPhysCMname.Text != "")
            {
                if (!textBoxPhysCMname.Text.EndsWith("\0"))
                    textBoxPhysCMname.Text += "\0";
                thisobject.PhysicCollisionName = textBoxPhysCMname.Text;
            }
            else
            {
                MessageBox.Show("Name must be not empty!");
            }
        }

        private void buttonPhysCMSel_Click(object sender, EventArgs e)
        {
            FormObjectPicker fop = new FormObjectPicker(thisobject.Pack, ElementType.CollisionMesh);
            if (fop.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                thisobject.PhysicCollisionName = this.textBoxPhysCMname.Text = fop.PickedContent[0];
            }
        }

        private void buttonPhysCMCreat_Click(object sender, EventArgs e)
        {
          /*  FormCollisionMesh ccc = new FormCollisionMesh(this.outputtreeview, thisobject.Pack);
            ccc.ShowDialog();
            if (ccc.cm != null)
            {
                createdContent.Add(ccc.cm);
                textBoxPhysCMname.Text = ccc.cm.name;
            }*/
        }

        private void buttonRCCMNameCreate_Click(object sender, EventArgs e)
        {
            /*FormCollisionMesh ccc = new FormCollisionMesh(this.outputtreeview,thisobject.Pack);
            ccc.ShowDialog();
            if (ccc.cm != null)
            {
                createdContent.Add(ccc.cm);
                textBoxRCCMName.Text = ccc.cm.name;
            }*/
        }

        private void comboBoxBehaviourType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxBehaviourType.SelectedIndex != -1)
            {
                int type = LevelObjectDescription.GetHash(comboBoxBehaviourType.Text);
                switch (type)
                {
                    case LevelObjectDescription.objectmovingbehaviourmodel:
                        {
                            groupBoxPhysicParams.Enabled = false;
                            thisobject.BehaviourType = type;
                            radioButtonPhysActrTypeCM.Enabled = true;
                        }break;
                    case LevelObjectDescription.objectphysicbehaviourmodel:
                        {
                            groupBoxPhysicParams.Enabled = true;
                            thisobject.BehaviourType = type;
                            checkBoxIsStatic.Checked = false;
                            checkBoxIsStatic.Enabled = true;
                            radioButtonPhysActrTypeCM.Enabled = true;
                        }break;
                    case LevelObjectDescription.objectphysiccharcontrollerbehaviourmodel:
                        {
                            groupBoxPhysicParams.Enabled = true;
                            thisobject.BehaviourType = type;
                            checkBoxIsStatic.Checked = false;
                            checkBoxIsStatic.Enabled = false;
                            radioButtonPhysActrTypeCM.Enabled = false;
                            radioButtonPhysActrTypeShape.Checked = true;
                        }break;
                    case LevelObjectDescription.objectstaticbehaviourmodel:
                        {
                            groupBoxPhysicParams.Enabled = false;
                            thisobject.BehaviourType = type;
                            radioButtonPhysActrTypeCM.Enabled = true;
                        }break;
                    default: break;
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            float x, y, z;
            try
            {
                string str = textBoxPhysXCenterOfMassX.Text.Replace('.', ',');
                x = Convert.ToSingle(str);
              //  thisobject.CenterOfMass.X = x;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wrong X");
                return;
            }

            try
            {
                string str = textBoxPhysXCenterOfMassY.Text.Replace('.', ',');
                y = Convert.ToSingle(str);
               // thisobject.CenterOfMass.Y = y;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wrong Y");
                return;
            }

            try
            {
                string str = textBoxPhysXCenterOfMassZ.Text.Replace('.', ',');
                z = Convert.ToSingle(str);
                // thisobject.CenterOfMass.Y = y;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Wrong Z");
                return;
            }

            thisobject.CenterOfMass.X = x;
            thisobject.CenterOfMass.Y = y;
            thisobject.CenterOfMass.Z = z;

            checkBoxPhysLCoMOK.Checked = true;
        }

        public void validate()
        {
         /*   bool first = thisobject.LODs.Count > 0 && thisobject.LODs[0] != null && thisobject.LODs[0].subsets != null && thisobject.LODs[0].subsets[0] != null
                && thisobject.LODs[0].subsets[0].MeshNames.Length > 1;
            */
        }

        private void checkBoxIsStatic_CheckedChanged(object sender, EventArgs e)
        {
            thisobject.IsStatic = checkBoxIsStatic.Checked;
            groupBoxMass.Enabled = groupBoxPhysXCenterOfMass.Enabled = !thisobject.IsStatic;
        }

       

        private void radioButtonRCShapeTypeCM_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxProcedureShapeProperties.Enabled = !radioButtonRCShapeTypeCM.Checked;
            groupBoxRCCM.Enabled = radioButtonRCShapeTypeCM.Checked;
            thisobject.IsRCCMEnabled = radioButtonRCShapeTypeCM.Checked;
        }

        private void radioButtonRCShapeTypeProcedureShape_CheckedChanged(object sender, EventArgs e)
        {
            thisobject.IsRCCMEnabled = radioButtonRCShapeTypeProcedureShape.Checked;
            groupBoxProcedureShapeProperties.Enabled = !radioButtonRCShapeTypeCM.Checked;
            groupBoxRCCM.Enabled = radioButtonRCShapeTypeCM.Checked;
            if(groupBoxProcedureShapeProperties.Enabled)
                comboBoxProcedureShapeType.SelectedIndex = 0;
        
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            FormObjectPicker fop = new FormObjectPicker(thisobject.Pack, ElementType.RenderObjectDescription);
            if (fop.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
              //  RenderObject wod = thisobject.Pack.getobject(fop.PickedContent[0]) as RenderObject;
                thisobject.RODName = fop.PickedContent[0];
                textBox2.Text = thisobject.RODName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormObjectPicker fop = new FormObjectPicker(thisobject.Pack, ElementType.Material);
            if (fop.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //  RenderObject wod = thisobject.Pack.getobject(fop.PickedContent[0]) as RenderObject;
                thisobject.matname = fop.PickedContent[0];
                textBox4.Text = thisobject.matname;
            }
        }

        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Microsoft.Xna.Framework;

namespace ResourceCollectorXNA
{
    public partial class RenderWindow : Form
    {
        public static RenderWindow Instance;

        class mosecapturer : Engine.Interface.IMouseUserInterface
        {
            public bool havemose;
            public bool captureMouse()
            {
                if (!havemose)
                {
                    if (!Engine.Interface.MouseManager.IsMouseCaptured)
                        havemose = true;
                    return havemose;
                }
                else
                    return true;
            }
            public void FreeMouse()
            {
                havemose = false;
            }
            public mosecapturer()
            {

            }
            public override bool IsMouseCaptured()
            {
                return havemose;
            }
        }
        mosecapturer capturer;
        private void contextMenuStrip1_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            capturer.FreeMouse();
        }

        Engine.GameEngine engine;
        public static bool islocal = true;
        public IntPtr PanelHandle;

        public RenderWindow()
        {
            InitializeComponent();
            PanelHandle = xnaPanel1.Handle;

            engine = ResourceCollectorXNA.Engine.GameEngine.Instance; ;
            Engine.GameEngine.renderController = new RCViewControllers.RenderWindowVC(this);
            capturer = new mosecapturer();
            Instance = this;
        }

        private void xnaPanel1_SizeChanged(object sender, EventArgs e)
        {
            engine.ResetDevice(ClientSize);
        }


        Engine.Interface.TransformManagerState state;
        

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (toolStripMenuItem2.Checked)
            {
                toolStripMenuItem3.Checked = selectToolStripMenuItem.Checked = false;
                
                setMove(null);
                engine.editor.transformator.SwitchState(Engine.Interface.TransformManagerState.move);
                samePointToolStripMenuItem.Checked = false;
                localPointToolStripMenuItem.Checked = false;
               
            }
            else
                toolStripMenuItem2.Checked = true;
            
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (toolStripMenuItem3.Checked)
            {
                toolStripMenuItem2.Checked = selectToolStripMenuItem.Checked = false;
                engine.editor.transformator.SwitchState(islocal ? Engine.Interface.TransformManagerState.rotatelocal : Engine.Interface.TransformManagerState.rotatesame);
                samePointToolStripMenuItem.Checked = !islocal;
                
                if(islocal)
                    setRotationLocal(null);
                else
                    setRotationSame(null);
                localPointToolStripMenuItem.Checked = islocal;
                
            }
            else
                toolStripMenuItem3.Checked = true;
            contextMenuStrip1.Close();
        }
        
        private void selectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectToolStripMenuItem.Checked)
            {
                toolStripMenuItem2.Checked = toolStripMenuItem3.Checked = false;
                
                setSelect();
                engine.editor.transformator.SwitchState(Engine.Interface.TransformManagerState.select);
                samePointToolStripMenuItem.Checked = false;
                localPointToolStripMenuItem.Checked = false;
                
            }
            else
                selectToolStripMenuItem.Checked = true;
        }

        private void samePointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (samePointToolStripMenuItem.Checked)
            {
                toolStripMenuItem3.Checked = true;
                localPointToolStripMenuItem.Checked = false;
                toolStripMenuItem2.Checked = selectToolStripMenuItem.Checked = false;
                islocal = false;
                
                if(islocal)
                    setRotationLocal(null);
                else
                    setRotationSame(null);
                engine.editor.transformator.SwitchState(islocal ? Engine.Interface.TransformManagerState.rotatelocal : Engine.Interface.TransformManagerState.rotatesame); 
                
            }
            else
                samePointToolStripMenuItem.Checked = true;
        }

        private void localPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (localPointToolStripMenuItem.Checked)
            {
                toolStripMenuItem3.Checked = true;
                samePointToolStripMenuItem.Checked = false;
                toolStripMenuItem2.Checked = selectToolStripMenuItem.Checked = false;
                islocal = true;
                if(islocal)
                    setRotationLocal(null);
                else
                    setRotationSame(null);


                engine.editor.transformator.SwitchState(islocal ? Engine.Interface.TransformManagerState.rotatelocal : Engine.Interface.TransformManagerState.rotatesame);
                
            }
            else
                localPointToolStripMenuItem.Checked = true;
        }


        public void setMove(string[] args)
        {
            if (state != Engine.Interface.TransformManagerState.move)
            {
                state = Engine.Interface.TransformManagerState.move;
                textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = true;
                xnaRadioButton1.Enabled = xnaRadioButton2.Enabled = xnaRadioButton3.Enabled = false;
                textBox4.Enabled = false;
                textBox4.Text = "";
            }
            if (args != null)
            {
                textBox1.Text = args[0];
                textBox2.Text = args[1];
                textBox3.Text = args[2];
            }
            else
                textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = "";
        }
        public void setRotationSame(string[] args)
        {
            if (state != Engine.Interface.TransformManagerState.rotatesame)
            {
                state = Engine.Interface.TransformManagerState.rotatesame;
                textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = true;
                xnaRadioButton1.Enabled = xnaRadioButton2.Enabled = xnaRadioButton3.Enabled = true;
            }
            if (args != null)
            {
                textBox1.Text = args[0];
                textBox2.Text = args[1];
                textBox3.Text = args[2];
                textBox4.Text = args[3];
            }
            else
                textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = "";
        }
        public void setRotationLocal(string[] args)
        {
            if (state != Engine.Interface.TransformManagerState.rotatelocal)
            {
                state = Engine.Interface.TransformManagerState.rotatelocal;
                textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = true;
                xnaRadioButton1.Enabled = xnaRadioButton2.Enabled = xnaRadioButton3.Enabled = true;
            }
            if (args != null)
            {
                textBox1.Text = args[0];
                textBox2.Text = args[1];
                textBox3.Text = args[2];
                textBox4.Text = args[3];
            }
            else
                textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = "";
        }
        public void setRelative(string[] args)
        {
            switch (state)
            {
                case Engine.Interface.TransformManagerState.move:
                    {
                        textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = true;
                        textBox4.Enabled = false; textBox4.Text = "";
                        textBox1.Text = textBox2.Text = textBox3.Text = "0";
                        xnaRadioButton1.Enabled = xnaRadioButton2.Enabled = xnaRadioButton3.Enabled = false;
                    }break;
                case Engine.Interface.TransformManagerState.select:
                    {
                        textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = false;
                        textBox1.Text = ""; textBox2.Text = ""; textBox3.Text = ""; textBox4.Text = "";
                        xnaRadioButton1.Enabled = xnaRadioButton2.Enabled = xnaRadioButton3.Enabled = false;
                    } break;
                case Engine.Interface.TransformManagerState.rotatelocal:
                case Engine.Interface.TransformManagerState.rotatesame:
                    {
                        textBox1.Text = textBox2.Text = textBox4.Text = "0";
                        textBox3.Text = "1";
                        xnaRadioButton1.Enabled = xnaRadioButton2.Enabled = xnaRadioButton3.Enabled = true;
                    } break;
                default: break;
            }
            if (args != null)
            {
                textBox1.Text = args[0];
                textBox2.Text = args[1];
                textBox3.Text = args[2];
                if (args.Length > 3)
                    textBox4.Text = args[3];
            }
            else
                textBox1.Text = textBox2.Text = textBox3.Text = textBox4.Text = "";
            
        }
        public void setSelect()
        {
            if (state != Engine.Interface.TransformManagerState.select)
            {
                textBox1.Enabled = textBox2.Enabled = textBox3.Enabled = textBox4.Enabled = false;
                textBox1.Text = ""; textBox2.Text = ""; textBox3.Text = ""; textBox4.Text = "";
                state = Engine.Interface.TransformManagerState.select;
                xnaRadioButton1.Enabled = xnaRadioButton2.Enabled = xnaRadioButton3.Enabled = false;
            }
        }

        private void xnaPanel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right && !Engine.Interface.MouseManager.IsMouseCaptured)
            {
                contextMenuStrip1.Close();
                if (capturer.captureMouse())
                {
                    contextMenuStrip1.Show(new System.Drawing.Point(e.Location.X + this.DesktopLocation.X, e.Location.Y + contextMenuStrip1.Height / 2 + this.DesktopLocation.Y));
                }
            }
        }

        private void xnaRadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            ResourceCollectorXNA.Engine.Actions.DragPivotObject.currentstate = Engine.Actions.state.cont;
        }

        private void xnaRadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            ResourceCollectorXNA.Engine.Actions.DragPivotObject.currentstate = Engine.Actions.state.one;
        }

        private void xnaRadioButton3_CheckedChanged(object sender, EventArgs e)
        {
            ResourceCollectorXNA.Engine.Actions.DragPivotObject.currentstate = Engine.Actions.state.five;
        }

        private void xnaButton3_Click(object sender, EventArgs e)
        {
            engine.clear();
        }
    }
}

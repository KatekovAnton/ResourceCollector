using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ResourceCollectorXNA
{
    public class XnaPanel : Control
    {
        public float aspectRatio;
        public XnaPanel()
        {
            MinimumSize = new Size(1, 1);
           // ContextMenuStripChanged += new EventHandler(XnaPanel_ContextMenuStripChanged);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Focus();
            base.OnMouseDown(e);
        }
        public new void SetStyle(ControlStyles flag, bool value)
        {
            base.SetStyle(flag, value);
        }

    }
    public class XNARadioButton : RadioButton
    {
        protected override void OnMouseEnter(EventArgs e)
        {
            Engine.GameEngine.actionToInterface = true;
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            Engine.GameEngine.actionToInterface = false;
            base.OnMouseLeave(e);
        }
    }
    public class XNAGroupBox : GroupBox
    {
        public XNAGroupBox()
        {
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            Engine.GameEngine.actionToInterface = true;
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            Engine.GameEngine.actionToInterface = false;
            base.OnMouseLeave(e);
        }
    }
    public class XnaButton : Button
    {
        public XnaButton()
        {

        }
        public new void SetStyle(ControlStyles flag, bool value)
        {
            base.SetStyle(flag, value);
        }
        protected override void OnMouseEnter(EventArgs e)
        {
            Engine.GameEngine.actionToInterface = true;
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            Engine.GameEngine.actionToInterface = false;
            base.OnMouseLeave(e);
        }
    }
    public class XnaTextBox : TextBox
    {
        protected override void OnMouseEnter(EventArgs e)
        {
            Engine.GameEngine.actionToInterface = true;
            base.OnMouseEnter(e);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            Engine.GameEngine.actionToInterface = false;
            base.OnMouseLeave(e);
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {

            base.OnMouseClick(e);
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            Engine.GameEngine.actionToInterface = true;
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            Engine.GameEngine.actionToInterface = false;
            base.OnMouseUp(mevent);
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            Engine.GameEngine.actionToInterface = true;
            base.OnKeyDown(e);
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            Engine.GameEngine.actionToInterface = false;
            base.OnKeyUp(e);
        }
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            Engine.GameEngine.actionToInterface = true;
            base.OnKeyPress(e);
        }
    }
}

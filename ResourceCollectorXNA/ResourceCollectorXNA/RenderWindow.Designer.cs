namespace ResourceCollectorXNA
{
    partial class RenderWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripMenuItem();
            this.samePointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.localPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.xnaButton3 = new ResourceCollectorXNA.XnaButton();
            this.xnaGroupBox1 = new ResourceCollectorXNA.XNAGroupBox();
            this.textBox4 = new ResourceCollectorXNA.XnaTextBox();
            this.textBox3 = new ResourceCollectorXNA.XnaTextBox();
            this.textBox2 = new ResourceCollectorXNA.XnaTextBox();
            this.textBox1 = new ResourceCollectorXNA.XnaTextBox();
            this.xnaGroupBox2 = new ResourceCollectorXNA.XNAGroupBox();
            this.xnaRadioButton3 = new ResourceCollectorXNA.XNARadioButton();
            this.xnaRadioButton2 = new ResourceCollectorXNA.XNARadioButton();
            this.xnaRadioButton1 = new ResourceCollectorXNA.XNARadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.xnaPanel1 = new ResourceCollectorXNA.XnaPanel();
            this.contextMenuStrip1.SuspendLayout();
            this.xnaGroupBox1.SuspendLayout();
            this.xnaGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectToolStripMenuItem,
            this.toolStripMenuItem2,
            this.toolStripMenuItem3,
            this.toolStripSeparator1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(109, 76);
            this.contextMenuStrip1.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.contextMenuStrip1_Closing);
            // 
            // selectToolStripMenuItem
            // 
            this.selectToolStripMenuItem.Checked = true;
            this.selectToolStripMenuItem.CheckOnClick = true;
            this.selectToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.selectToolStripMenuItem.Name = "selectToolStripMenuItem";
            this.selectToolStripMenuItem.Size = new System.Drawing.Size(108, 22);
            this.selectToolStripMenuItem.Text = "Select";
            this.selectToolStripMenuItem.Click += new System.EventHandler(this.selectToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.CheckOnClick = true;
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(108, 22);
            this.toolStripMenuItem2.Text = "Move";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.CheckOnClick = true;
            this.toolStripMenuItem3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.samePointToolStripMenuItem,
            this.localPointToolStripMenuItem});
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(108, 22);
            this.toolStripMenuItem3.Text = "Rotate";
            this.toolStripMenuItem3.Click += new System.EventHandler(this.toolStripMenuItem3_Click);
            // 
            // samePointToolStripMenuItem
            // 
            this.samePointToolStripMenuItem.CheckOnClick = true;
            this.samePointToolStripMenuItem.Name = "samePointToolStripMenuItem";
            this.samePointToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.samePointToolStripMenuItem.Text = "Same point";
            this.samePointToolStripMenuItem.Click += new System.EventHandler(this.samePointToolStripMenuItem_Click);
            // 
            // localPointToolStripMenuItem
            // 
            this.localPointToolStripMenuItem.CheckOnClick = true;
            this.localPointToolStripMenuItem.Name = "localPointToolStripMenuItem";
            this.localPointToolStripMenuItem.Size = new System.Drawing.Size(134, 22);
            this.localPointToolStripMenuItem.Text = "Local point";
            this.localPointToolStripMenuItem.Click += new System.EventHandler(this.localPointToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(105, 6);
            // 
            // xnaButton3
            // 
            this.xnaButton3.Location = new System.Drawing.Point(991, 166);
            this.xnaButton3.Name = "xnaButton3";
            this.xnaButton3.Size = new System.Drawing.Size(155, 30);
            this.xnaButton3.TabIndex = 6;
            this.xnaButton3.Text = "Clear";
            this.xnaButton3.UseVisualStyleBackColor = true;
            this.xnaButton3.Click += new System.EventHandler(this.xnaButton3_Click);
            // 
            // xnaGroupBox1
            // 
            this.xnaGroupBox1.Controls.Add(this.textBox4);
            this.xnaGroupBox1.Controls.Add(this.textBox3);
            this.xnaGroupBox1.Controls.Add(this.textBox2);
            this.xnaGroupBox1.Controls.Add(this.textBox1);
            this.xnaGroupBox1.Controls.Add(this.xnaGroupBox2);
            this.xnaGroupBox1.Controls.Add(this.label4);
            this.xnaGroupBox1.Controls.Add(this.label3);
            this.xnaGroupBox1.Controls.Add(this.label2);
            this.xnaGroupBox1.Controls.Add(this.label1);
            this.xnaGroupBox1.Location = new System.Drawing.Point(287, 668);
            this.xnaGroupBox1.Name = "xnaGroupBox1";
            this.xnaGroupBox1.Size = new System.Drawing.Size(578, 35);
            this.xnaGroupBox1.TabIndex = 5;
            this.xnaGroupBox1.TabStop = false;
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(345, 11);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new System.Drawing.Size(75, 20);
            this.textBox4.TabIndex = 7;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(228, 11);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(75, 20);
            this.textBox3.TabIndex = 7;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(133, 11);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(75, 20);
            this.textBox2.TabIndex = 7;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(29, 11);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(75, 20);
            this.textBox1.TabIndex = 7;
            // 
            // xnaGroupBox2
            // 
            this.xnaGroupBox2.Controls.Add(this.xnaRadioButton3);
            this.xnaGroupBox2.Controls.Add(this.xnaRadioButton2);
            this.xnaGroupBox2.Controls.Add(this.xnaRadioButton1);
            this.xnaGroupBox2.Location = new System.Drawing.Point(440, 0);
            this.xnaGroupBox2.Name = "xnaGroupBox2";
            this.xnaGroupBox2.Size = new System.Drawing.Size(138, 35);
            this.xnaGroupBox2.TabIndex = 8;
            this.xnaGroupBox2.TabStop = false;
            this.xnaGroupBox2.Text = "Swich";
            // 
            // xnaRadioButton3
            // 
            this.xnaRadioButton3.AutoSize = true;
            this.xnaRadioButton3.Enabled = false;
            this.xnaRadioButton3.Location = new System.Drawing.Point(98, 13);
            this.xnaRadioButton3.Name = "xnaRadioButton3";
            this.xnaRadioButton3.Size = new System.Drawing.Size(31, 17);
            this.xnaRadioButton3.TabIndex = 2;
            this.xnaRadioButton3.Text = "5";
            this.xnaRadioButton3.UseVisualStyleBackColor = true;
            this.xnaRadioButton3.CheckedChanged += new System.EventHandler(this.xnaRadioButton3_CheckedChanged);
            // 
            // xnaRadioButton2
            // 
            this.xnaRadioButton2.AutoSize = true;
            this.xnaRadioButton2.Enabled = false;
            this.xnaRadioButton2.Location = new System.Drawing.Point(61, 13);
            this.xnaRadioButton2.Name = "xnaRadioButton2";
            this.xnaRadioButton2.Size = new System.Drawing.Size(31, 17);
            this.xnaRadioButton2.TabIndex = 1;
            this.xnaRadioButton2.Text = "1";
            this.xnaRadioButton2.UseVisualStyleBackColor = true;
            this.xnaRadioButton2.CheckedChanged += new System.EventHandler(this.xnaRadioButton2_CheckedChanged);
            // 
            // xnaRadioButton1
            // 
            this.xnaRadioButton1.AutoSize = true;
            this.xnaRadioButton1.Checked = true;
            this.xnaRadioButton1.Enabled = false;
            this.xnaRadioButton1.Location = new System.Drawing.Point(9, 13);
            this.xnaRadioButton1.Name = "xnaRadioButton1";
            this.xnaRadioButton1.Size = new System.Drawing.Size(46, 17);
            this.xnaRadioButton1.TabIndex = 0;
            this.xnaRadioButton1.TabStop = true;
            this.xnaRadioButton1.Text = "cont";
            this.xnaRadioButton1.UseVisualStyleBackColor = true;
            this.xnaRadioButton1.CheckedChanged += new System.EventHandler(this.xnaRadioButton1_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(318, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "W:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(214, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Z:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(110, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Y:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "X:";
            // 
            // xnaPanel1
            // 
            this.xnaPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xnaPanel1.Location = new System.Drawing.Point(0, 0);
            this.xnaPanel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.xnaPanel1.Name = "xnaPanel1";
            this.xnaPanel1.Size = new System.Drawing.Size(1158, 708);
            this.xnaPanel1.TabIndex = 0;
            this.xnaPanel1.Text = "xnaPanel1";
            this.xnaPanel1.SizeChanged += new System.EventHandler(this.xnaPanel1_SizeChanged);
            this.xnaPanel1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.xnaPanel1_MouseClick);
            // 
            // RenderWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1158, 708);
            this.ControlBox = false;
            this.Controls.Add(this.xnaButton3);
            this.Controls.Add(this.xnaGroupBox1);
            this.Controls.Add(this.xnaPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RenderWindow";
            this.Text = "RenderWindow";
            this.contextMenuStrip1.ResumeLayout(false);
            this.xnaGroupBox1.ResumeLayout(false);
            this.xnaGroupBox1.PerformLayout();
            this.xnaGroupBox2.ResumeLayout(false);
            this.xnaGroupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public XnaPanel xnaPanel1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem selectToolStripMenuItem;
        private XNAGroupBox xnaGroupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem samePointToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem localPointToolStripMenuItem;
        private XNAGroupBox xnaGroupBox2;
        private XNARadioButton xnaRadioButton3;
        private XNARadioButton xnaRadioButton2;
        private XNARadioButton xnaRadioButton1;
        private XnaButton xnaButton3;
        private XnaTextBox textBox4;
        private XnaTextBox textBox3;
        private XnaTextBox textBox2;
        private XnaTextBox textBox1;

    }
}
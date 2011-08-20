using System;
using System.Collections.Generic;
using System.ComponentModel;

/*using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

using System.Text;
using System.Windows.Forms;

namespace ResourceCollector
{
    
    public partial class meshviever : Form
    {

        public Mesh ffffff;
        public bool generated;
        public static string texture;
        private bool lmousing;
        private bool rmousing;
        private bool mmousing;
        private System.Timers.Timer aTimer = new System.Timers.Timer();
        public Texture meshTexture;
        public Material meshMaterial;
        public bool usetexture = false;
        public static Color backcolor = Color.FromName("ActiveCaption");
        bool multi;
        class MyPanel : Panel
        {
            public MyPanel() 
            { 
                
            }
            public void MyStyle(ControlStyles flags, bool value)
            {
                SetStyle(flags,value);
            }
            
        }

        public meshviever(Mesh mes)
        {
            multi = false;
            ffffff = mes;
            InitializeComponent();


            panel1.MyStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);


            PresentParameters presentParams = new PresentParameters();
            presentParams.SwapEffect = SwapEffect.Discard;

            Format current = Manager.Adapters[0].CurrentDisplayMode.Format;
            presentParams.BackBufferFormat = current;

            presentParams.BackBufferCount = 1;
            presentParams.AutoDepthStencilFormat = DepthFormat.D16;
            presentParams.EnableAutoDepthStencil = true;

            presentParams.BackBufferHeight = panel1.Height;
            presentParams.BackBufferWidth = panel1.Width;
            presentParams.Windowed = true;

            device = new Device(0, DeviceType.Hardware, panel1, CreateFlags.HardwareVertexProcessing, presentParams);
            font = new Microsoft.DirectX.Direct3D.Font(device, new System.Drawing.Font("Comic Sans MS", 8.0f, FontStyle.Strikeout));
            if (meshviever.texture != null)
            {

                try
                {
                    meshMaterial = new Material();
                    meshTexture = TextureLoader.FromFile(device, meshviever.texture);
                    usetexture = true;
                    button2.Text = meshviever.texture.Substring(meshviever.texture.LastIndexOf('\\') + 1, meshviever.texture.Length - meshviever.texture.LastIndexOf('\\') - 1);
                    checkBox3.Enabled = true;
                    checkBox3.Checked = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("WrongTexture");
                }
            }
            this.MouseWheel += new MouseEventHandler(meshviever_MouseWheel);


            initbuffers();
            this.Text = mes.name;
            SetUpCamera();
            NewCamPos();
            panel1.Invalidate();
            aTimer.Interval = 33;
            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(aTimer_Elapsed);
            aTimer.Start();

        }
        Mesh[] meshes;
        public meshviever(Mesh[] mes, string Cname)
        {
            multi = true;
            meshes = mes;
            InitializeComponent();


            panel1.MyStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.Opaque, true);


            PresentParameters presentParams = new PresentParameters();
            presentParams.SwapEffect = SwapEffect.Discard;

            Format current = Manager.Adapters[0].CurrentDisplayMode.Format;
            presentParams.BackBufferFormat = current;

            presentParams.BackBufferCount = 1;
            presentParams.AutoDepthStencilFormat = DepthFormat.D16;
            presentParams.EnableAutoDepthStencil = true;

            presentParams.BackBufferHeight = panel1.Height;
            presentParams.BackBufferWidth = panel1.Width;
            presentParams.Windowed = true;

            device = new Device(0, DeviceType.Hardware, panel1,
                CreateFlags.HardwareVertexProcessing, presentParams);
            font = new Microsoft.DirectX.Direct3D.Font(device,
                new System.Drawing.Font("Comic Sans MS", 8.0f, FontStyle.Strikeout));
            if (meshviever.texture != null)
            {

                try
                {
                    meshMaterial = new Material();
                    meshTexture = TextureLoader.FromFile(device, meshviever.texture);
                    usetexture = true;
                    button2.Text = meshviever.texture.Substring(meshviever.texture.LastIndexOf('\\') + 1, meshviever.texture.Length - meshviever.texture.LastIndexOf('\\') - 1);
                    checkBox3.Enabled = true;
                    checkBox3.Checked = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("WrongTexture");
                }
            }
            this.MouseWheel += new MouseEventHandler(meshviever_MouseWheel);


            initbuffers();
            this.Text = Cname;
            SetUpCamera();
            NewCamPos();
            panel1.Invalidate();
            aTimer.Interval = 33;
            aTimer.Elapsed += new System.Timers.ElapsedEventHandler(aTimer_Elapsed);
            aTimer.Start();

        }
        ~meshviever()
        {
            try
            {
                if (multi)
                {
                    for (int i = 0; i < vbs.Length; i++)
                    {
                        vbs[i].Dispose();
                        ibs[i].Dispose();
                    }
                }
                else
                {
                    ib.Dispose();
                    vb.Dispose();
                }
                device.Dispose();
            }
            catch (Exception ex)
            { }
        }
        void aTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            panel1.Invalidate();
        }
        void initbuffers()
        {
            if (!multi)
            {
                if (ffffff.BufferIndex == null || ffffff.BufferVertex == null)
                {
                    ToolStripProgressBar tspb = new ToolStripProgressBar();
                    ffffff.GenerateOptForLoading(tspb);
                    generated = true;
                }
                ib = new IndexBuffer(typeof(int), ffffff.BufferIndex.Length, device, Usage.WriteOnly, Pool.Default);
                ib.SetData(ffffff.BufferIndex, 0, LockFlags.None);
                vb = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), ffffff.BufferVertex.Length, device, Usage.Dynamic, CustomVertex.PositionNormalTextured.Format, Pool.Default);
                CustomVertex.PositionNormalTextured[] verts = new CustomVertex.PositionNormalTextured[ffffff.BufferVertex.Length];
                targetposition = new Vector3(0, 0, 0);
                for (int i = 0; i < ffffff.BufferVertex.Length; i++)
                {
                    targetposition += new Vector3(ffffff.BufferVertex[i].pos.x, ffffff.BufferVertex[i].pos.y, ffffff.BufferVertex[i].pos.z);
                    verts[i] = new CustomVertex.PositionNormalTextured(ffffff.BufferVertex[i].pos.x, ffffff.BufferVertex[i].pos.y, ffffff.BufferVertex[i].pos.z,
                        ffffff.BufferVertex[i].normal.x, ffffff.BufferVertex[i].normal.y, ffffff.BufferVertex[i].normal.z,
                         ffffff.BufferVertex[i].tcoord.x, 1.0f - ffffff.BufferVertex[i].tcoord.y);
                }
                targetposition.X /= ffffff.BufferVertex.Length;
                targetposition.Y /= ffffff.BufferVertex.Length;
                targetposition.Z /= ffffff.BufferVertex.Length;

                vb.SetData(verts, 0, LockFlags.None);

            }
            else
            {
                vbs = new VertexBuffer[meshes.Length];
                ibs = new IndexBuffer[meshes.Length];
                for (int k = 0; k < meshes.Length; k++)
                {
                    if (meshes[k].BufferIndex == null || meshes[k].BufferVertex == null)
                    {
                        ToolStripProgressBar tspb = new ToolStripProgressBar();
                        meshes[k].GenerateOptForLoading(tspb);
                        generated = true;
                    }

                    ibs[k] = new IndexBuffer(typeof(int), meshes[k].BufferIndex.Length, device, Usage.WriteOnly, Pool.Default);
                    ibs[k].SetData(meshes[k].BufferIndex, 0, LockFlags.None);
                    vbs[k] = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), meshes[k].BufferVertex.Length, device, Usage.Dynamic, CustomVertex.PositionNormalTextured.Format, Pool.Default);
                    CustomVertex.PositionNormalTextured[] verts = new CustomVertex.PositionNormalTextured[meshes[k].BufferVertex.Length];
                    targetposition = new Vector3(0, 0, 0);
                    for (int i = 0; i < meshes[k].BufferVertex.Length; i++)
                    {
                    
                        verts[i] = new CustomVertex.PositionNormalTextured(meshes[k].BufferVertex[i].pos.x, meshes[k].BufferVertex[i].pos.y, meshes[k].BufferVertex[i].pos.z,
                            meshes[k].BufferVertex[i].normal.x, meshes[k].BufferVertex[i].normal.y, meshes[k].BufferVertex[i].normal.z,
                             meshes[k].BufferVertex[i].tcoord.x, 1.0f - meshes[k].BufferVertex[i].tcoord.y);
                    }
                  

                    vbs[k].SetData(verts, 0, LockFlags.None);
                }
            }
        }

        void meshviever_MouseWheel(object sender, MouseEventArgs e)
        {

        }
        public Device device = null;
        public VertexBuffer vb;
        public IndexBuffer ib;
        public VertexBuffer[] vbs;
        public IndexBuffer[] ibs;
        private Microsoft.DirectX.Direct3D.Font font;
        public double theta = 1.201d;
        public double fi = 1.201d;
        public float radius = 1;
        public Vector3 cameraposition;
        public Vector3 targetposition;
        private void meshviever_Load(object sender, EventArgs e)
        {

        }
        private void NewCamPos()
        {
            cameraposition.X = radius * Convert.ToSingle(Math.Sin(theta) * Math.Cos(fi)) + targetposition.X;
            cameraposition.Y = radius * Convert.ToSingle(Math.Sin(theta) * Math.Sin(fi)) + targetposition.Y;
            cameraposition.Z = radius * Convert.ToSingle(Math.Cos(theta)) + targetposition.Z;
            device.Transform.View = Matrix.LookAtRH(cameraposition, targetposition, new Vector3(0, 0, 1));
        }
        float Fieldofview = 0.6138f;
        private void SetUpCamera()
        {

            device.Transform.Projection = Matrix.PerspectiveFovRH(Fieldofview, Convert.ToSingle(panel1.Width) / Convert.ToSingle(panel1.Height),
                0.1f, 100.0f);
            cameraposition.X = radius * Convert.ToSingle(Math.Sin(theta) * Math.Cos(fi)) + targetposition.X;
            cameraposition.Y = radius * Convert.ToSingle(Math.Sin(theta) * Math.Sin(fi)) + targetposition.Y;
            cameraposition.Z = radius * Convert.ToSingle(Math.Cos(theta)) + targetposition.Z;
            device.Transform.View = Matrix.LookAtRH(cameraposition, targetposition, new Vector3(0, 0, 1));
            device.Lights[0].Type = LightType.Point;
            device.Lights[0].Attenuation0 = 1.0f;
            device.Lights[0].Enabled = true;
            device.Lights[0].Position = new Vector3(12.0f, -3.0f, 5.0f);
            device.Lights[0].Range = 100;
            device.Lights[0].Diffuse = Color.White;
            device.Lights[0].Update();
            device.RenderState.Ambient = Color.White;
            device.RenderState.Lighting = false;
            device.Transform.World = Matrix.RotationZ((float)Math.PI / 4.0f);
            device.RenderState.CullMode = Cull.None;

        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, backcolor, 1.0f, 0);
                device.Lights[0].Type = LightType.Point;


                device.BeginScene();
                if (usetexture)
                {
                    device.SetTexture(0, meshTexture);
                }
                else
                {
                    device.SetTexture(0, null);
                }

                device.VertexFormat = CustomVertex.PositionNormalTextured.Format;
                if (multi)
                {
                    for (int i = 0; i < ibs.Length; i++)
                    {
                        device.SetStreamSource(0, vbs[i], 0, VertexInformation.GetFormatSize(CustomVertex.PositionNormalTextured.Format));
                        device.Indices = ibs[i];
                        device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, meshes[i].BufferVertex.Length, 0, meshes[i].BufferIndex.Length / 3);
                    }
                   
                }
                else
                {
                    device.SetStreamSource(0, vb, 0, VertexInformation.GetFormatSize(CustomVertex.PositionNormalTextured.Format));
                    device.Indices = ib;
                    device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, ffffff.BufferVertex.Length, 0, ffffff.BufferIndex.Length / 3);
                }
                device.EndScene();
                device.Present();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error occuared in OnPaint");
            }


        }
        
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (lmousing)
                {
                    if ((theta >= 0.1f) && (theta <= Math.PI / 2))
                    {
                        theta += Convert.ToDouble(MousePosition.Y - e.Y) / 150;
                    }
                    if ((theta < 0.1f))
                    {
                        theta = 0.1f;
                    }
                    if (theta > Math.PI / 2)
                    {
                        theta = Math.PI / 2;
                    }
                    fi -= Convert.ToDouble(e.X - MousePosition.X) / 150;
                }
                if (rmousing)
                {
                    targetposition.Z += Convert.ToSingle((MousePosition.Y - e.Y)) / 15.0f;
                }
                if (mmousing)
                {

                    Fieldofview += Convert.ToSingle((MousePosition.Y - e.Y)) / 100.0f;
                    if (Fieldofview < 0.2f)
                        Fieldofview = 0.2f;
                    device.Transform.Projection = Matrix.PerspectiveFovRH(Fieldofview, Convert.ToSingle(panel1.Width) / Convert.ToSingle(panel1.Height),
                   0.1f, 100.0f);
                }
                MousePosition.X = e.X;
                MousePosition.Y = e.Y;
                NewCamPos();
                panel1.Invalidate();
            }
            catch (Exception ex)
            { }
        }
        private Point MousePosition;

        private void meshviever_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    Close();
                }
                if (e.KeyCode == Keys.W)
                {
                    if (device.RenderState.FillMode != FillMode.WireFrame)
                        device.RenderState.FillMode = FillMode.WireFrame;
                    else
                        device.RenderState.FillMode = FillMode.Solid;
                }
                if (e.KeyCode == Keys.L)
                {
                    device.RenderState.Lighting = !device.RenderState.Lighting;
                }
            }
            catch (Exception ex)
            { }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            lmousing = mmousing = rmousing = false;
        }
        bool shift = false;
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                lmousing = true;
            if (e.Button == MouseButtons.Middle)
                mmousing = true;
            if (e.Button == MouseButtons.Right)
                rmousing = true;
            
        }
       
        private void meshviever_MouseMove(object sender, MouseEventArgs e)
        {
          
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                radius = 1.0f + ((float)trackBar1.Value / (float)trackBar1.Maximum) * 10.0f;
                NewCamPos();
            }
            catch (Exception ex)
            { }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                device.RenderState.Lighting = !device.RenderState.Lighting;
            }
            catch (Exception ex)
            { }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (device.RenderState.FillMode != FillMode.WireFrame)
                    device.RenderState.FillMode = FillMode.WireFrame;
                else
                    device.RenderState.FillMode = FillMode.Solid;
            }
            catch (Exception ex)
            { }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            if (cd.ShowDialog() == DialogResult.OK)
            {
                backcolor = cd.Color;
                button1.BackColor = cd.Color;
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    meshMaterial = new Material();
                    meshTexture = TextureLoader.FromFile(device, ofd.FileName);
                    usetexture = true;
                    button2.Text = ofd.FileName.Substring(ofd.FileName.LastIndexOf('\\')+1,ofd.FileName.Length -ofd.FileName.LastIndexOf('\\')-1) ;
                    checkBox3.Enabled = true;
                    checkBox3.Checked = true;
                    texture = ofd.FileName;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("WrongTexture");
                }
                
            }
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            usetexture = checkBox3.Checked;
        }
     
        private void meshviever_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Shift)
            {
                shift = false;
            }
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ColorDialog cd = new ColorDialog();
            cd.AllowFullOpen = true;
            if (cd.ShowDialog() == DialogResult.OK)
            {
                backcolor = cd.Color;
                button1.BackColor = cd.Color;
            }
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            try
            {
                if (device.RenderState.FillMode != FillMode.WireFrame)
                    device.RenderState.FillMode = FillMode.WireFrame;
                else
                    device.RenderState.FillMode = FillMode.Solid;
            }
            catch (Exception ex)
            { }
        }
    }
    class other
    {
        public static readonly string perevodstroki = "\r\n";
        public static string floattosting(float x)
        {
            string tmop = x.ToString();
            if (tmop.Length > 6)
                return tmop.Substring(0, 6);
            return tmop;
            
        }
    }
}
*/
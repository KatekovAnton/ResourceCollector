using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ResourceCollector
{
    public class ImageContent : PackContent
    {

        public int usercount = 0;
        public byte[] data
        {
            get;
            private set;
        }
        public ImageContent(string _name, Image image)
        {
            this.loadedformat = this.forsavingformat = ElementType.PNGTexture;
            using (var stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                data = stream.ToArray();
            }
            name = _name;
            if (!name.EndsWith("\0"))
                name += "\0";
            calcbodysize();
            calcheadersize();

        }

        public ImageContent()
        {
            this.loadedformat = this.forsavingformat = ElementType.PNGTexture;
        }

        public override void calcbodysize()
        {
            size = data.Length;
        }

        public override System.Windows.Forms.DialogResult createpropertieswindow(Pack p, System.Windows.Forms.TreeView outputtreeview)
        {
            Bitmap bitmap;
            using (var stream = new MemoryStream(data))
            {
                bitmap = new Bitmap(stream);
            }
            var window = new ResourceCollector.TexturePropertyWindow(bitmap);
            window.ShowDialog();
            return System.Windows.Forms.DialogResult.OK;
        }

        public override void calcheadersize()
        {
            headersize = 16 + OtherFunctions.GetPackStringLengthForWrite(name);
        }

        public override int loadbody(BinaryReader br)
        {
            data = br.ReadBytes(size);
            return size;
        }

        public override void loadobjectheader(HeaderInfo hi, BinaryReader br)
        {
            loadedformat = hi.loadedformat;
            name = hi.name;
            forsavingformat = hi.forsavingformat;
            offset = hi.offset;
            size = hi.size;
            headersize = hi.headersize;
            size = br.ReadInt32();
        }

        public override void savebody(BinaryWriter br)
        {
            br.Write(data);
        }

        public override void saveheader(BinaryWriter bw)
        {
            bw.WritePackString(name);
            bw.Write(offset);
            bw.Write(ElementType.PNGTexture);

            calcheadersize();
            bw.Write(headersize);
            bw.Write(data.Length);
        }

        public override void ViewBasicInfo(System.Windows.Forms.ComboBox comboBox1, System.Windows.Forms.ComboBox comboBox2, System.Windows.Forms.Label label1, System.Windows.Forms.Label label2, System.Windows.Forms.Label label3, System.Windows.Forms.Label label4, System.Windows.Forms.GroupBox groupBox1, System.Windows.Forms.TextBox tb, System.Windows.Forms.Button button2, System.Windows.Forms.Button button1)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox1.Text = ElementType.ReturnString(this.loadedformat);
            comboBox2.Text = ElementType.ReturnString(this.forsavingformat);
            groupBox1.Text = tb.Text = name;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            tb.Enabled = true;
            button2.Enabled = true;
            button1.Enabled = true;
            label1.Text = this.number.ToString();
            label2.Text = this.offset.ToString();
            label3.Text = this.headersize.ToString();
            label4.Text = this.size.ToString();
        }
    }
}

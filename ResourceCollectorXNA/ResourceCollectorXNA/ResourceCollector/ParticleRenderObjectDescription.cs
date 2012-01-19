using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceCollector
{
    public class ParticleRenderObjectDescription:PackContent
    {
        public string MeshName;
        public string TextureName;

        public bool ShadowCaster;
        public bool ShadowReceiver;
        public bool Transparent;
        public bool SelfIlmn;

        public Pack pack;

        public ParticleRenderObjectDescription()
        {
            MeshName = "\0";
            TextureName = "\0";

            loadedformat = forsavingformat = ElementType.ParticelRenderObjectDescription;
        }

        public override void calcbodysize()
        {
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(new System.IO.MemoryStream());
            ToStream(bw);
            size = Convert.ToInt32(bw.BaseStream.Length);
            bw.Close();
            bw.Dispose();

        }

        public void ToStream(System.IO.BinaryWriter bw)
        {
            bw.WritePackString(MeshName);
            bw.WritePackString(TextureName);

            bw.Write(ShadowCaster);
            bw.Write(ShadowReceiver);
            bw.Write(Transparent);
            bw.Write(SelfIlmn);
        }

        public override void calcheadersize()
        {
            headersize = 16 + OtherFunctions.GetPackStringLengthForWrite(name);
        }

        public override System.Windows.Forms.DialogResult createpropertieswindow(Pack p, System.Windows.Forms.TreeView outputtreeview)
        {
            FormParticleRenderObjectDescription dd = new FormParticleRenderObjectDescription(this);
            dd.ShowDialog();
            name = dd.textBox1.Text;
            if (!name.EndsWith("\0"))
                name += "\0";
            return System.Windows.Forms.DialogResult.OK;
        }

        public override int loadbody(System.IO.BinaryReader br)
        {
            long pos = br.BaseStream.Position;
            MeshName = br.ReadPackString();
            TextureName = br.ReadPackString();

            ShadowCaster = br.ReadBoolean();;
            ShadowReceiver = br.ReadBoolean();
           Transparent = br.ReadBoolean();
            SelfIlmn = br.ReadBoolean();

            return Convert.ToInt32(br.BaseStream.Position - pos);
        }

        public override void saveheader(System.IO.BinaryWriter br)
        {
            br.WritePackString(name);
            br.Write(offset);
            br.Write(forsavingformat);

            calcheadersize();
            br.Write(headersize);
            calcbodysize();
            br.Write(size);
        }

        public override void savebody(System.IO.BinaryWriter bw)
        {
            ToStream(bw);
        }

        public override void loadobjectheader(HeaderInfo hi, System.IO.BinaryReader br)
        {
            loadedformat = hi.loadedformat;
            name = hi.name;
            forsavingformat = hi.forsavingformat;
            offset = hi.offset;
            size = hi.size;
            headersize = hi.headersize;

            size = br.ReadInt32();
        }
    }
}

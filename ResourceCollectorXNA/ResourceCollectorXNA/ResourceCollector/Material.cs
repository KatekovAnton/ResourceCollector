using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.IO;

namespace ResourceCollector.Content
{
    public class Material : PackContent
    {
        public class SubsetMaterial
        {
            public string DiffuseTextureName;
            public override string ToString()
            {
                return "DT: " + DiffuseTextureName;
            }
        }

        public class Lod
        {
            public List<SubsetMaterial> mats;
            public Lod()
            {
                mats = new List<SubsetMaterial>();
            }
            public override string ToString()
            {
                return "Subset count = " + mats.Count();
            }
        }

        public List<Lod> lodMats;
        public Pack pack;
        public System.Windows.Forms.TreeNode TreeNode;

        public Material()
        {
            loadedformat = forsavingformat = ElementType.Material;
            name = "New Material " + DateTime.Now.Millisecond.ToString();
            lodMats = new List<Lod>();
        }

        public override void calcbodysize()
        {
            MemoryStream m = new MemoryStream();
            BinaryWriter br = new BinaryWriter(m);
            br.Write(lodMats.Count);
            for (int i = 0; i < lodMats.Count; i++)
            {
                Lod texturenames = lodMats[i];
                br.Write(texturenames.mats.Count);
                for (int j = 0; j < texturenames.mats.Count; j++)
                    br.WritePackString(texturenames.mats[j].DiffuseTextureName);
            }
            size = Convert.ToInt32( br.BaseStream.Position);
            br.Close();
        }

        public override void calcheadersize()
        {
            headersize = 16 + OtherFunctions.GetPackStringLengthForWrite(name);
        }

        public override System.Windows.Forms.DialogResult createpropertieswindow(Pack p, System.Windows.Forms.TreeView outputtreeview)
        {
            if (Enginereadedobject.Count == 0)
            {
                FormMaterial form = new FormMaterial(this);

                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //  name = form.TextBoxName.Text + "\0";
                    if (TreeNode != null)
                        TreeNode.Text = name;
                }
                return form.DialogResult;
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("object are loaded to scene. cannot to edit");
                return System.Windows.Forms.DialogResult.Cancel;
            }
        }

        public override int loadbody(BinaryReader br)
        {
            long pos = br.BaseStream.Position;
            int count = br.ReadInt32();
            lodMats = new List<Lod>();// Lod[count];
            for (int i = 0; i < count; i++)
            {
                Lod l = new Lod();
                lodMats.Add(l);
                int matc = br.ReadInt32();
                lodMats[i].mats = new List<SubsetMaterial>();// SubsetMaterial[br.ReadInt32()];
                for (int j = 0; j < matc; j++)
                {
                    SubsetMaterial sm = new SubsetMaterial();
                    sm.DiffuseTextureName = br.ReadPackString();
                    lodMats[i].mats.Add(sm);
                }
            }
            return Convert.ToInt32(br.BaseStream.Position - pos);
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
            br.Write(lodMats.Count);
            for (int i = 0; i < lodMats.Count; i++)
            {
                Lod texturenames = lodMats[i];
                br.Write(texturenames.mats.Count);
                for (int j = 0; j < texturenames.mats.Count; j++)
                {
                    br.WritePackString(texturenames.mats[j].DiffuseTextureName);
                }
            }
        }

        public override void saveheader(BinaryWriter br)
        {
            br.WritePackString(name);
            br.Write(offset);
            br.Write(forsavingformat);

            calcheadersize();
            br.Write(headersize);
            calcbodysize();
            br.Write(size);
        }
    }
}

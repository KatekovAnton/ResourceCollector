using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceCollector
{
    public class SkeletonWithAddInfo : PackContent//int SkeletonWithAddInfo = 31;
    {
        #region packcontent methods
        public override void calcbodysize(System.Windows.Forms.ToolStripProgressBar targetbar)
        {
            size = 4;//bone count

            for(int i =0;i<baseskelet.bones.Length;i++)
            {
                size += OtherFunctions.GetPackStringLengthForWrite(baseskelet.bones[i].Name)+5;
                if (baseskelet.bones[i].index != this.RootIndex)
                    size += OtherFunctions.GetPackStringLengthForWrite(baseskelet.bones[i].Parent.Name)+5;
                else
                    size += OtherFunctions.GetPackStringLengthForWrite("-\0")+5;
                
            }
            size += 0;
            for (int i = 0; i < baseskelet.bones.Length; i++)
            {
                size += 48;
            }

            size += 20;//headindex, weaponindex, rootindex, toprootindes, bottomrootindex
            size += 8;//length of botomindexes, topindexes
            size += 4 * botomindexes.Length;
            size += 4 * topindexes.Length;

        }
        public override System.Windows.Forms.DialogResult createpropertieswindow(Pack p, System.Windows.Forms.TreeView outputtreeview)
        {
            FormAnimationTools fat = new FormAnimationTools(this,p,outputtreeview);
            return fat.ShowDialog();
        }
        public override void calcheadersize()
        {
            headersize = 16 + name.Length;
        }
        public override int loadbody(BinaryReader br, System.Windows.Forms.ToolStripProgressBar toolStripProgressBar)
        {
            long a = br.BaseStream.Position;
            baseskelet = Skeleton.FromStream(br);
            headindex = br.ReadInt32();
            weaponindex = br.ReadInt32();
            rootindex = br.ReadInt32();
            toprootindex = br.ReadInt32();
            bottomrootindex = br.ReadInt32();
            botomindexes = new int[br.ReadInt32()];
            for (int i = 0; i < botomindexes.Length; i++)
                botomindexes[i] = br.ReadInt32();

            topindexes = new int[br.ReadInt32()];
            for (int i = 0; i < topindexes.Length; i++)
                topindexes[i] = br.ReadInt32();
            return Convert.ToInt32(br.BaseStream.Position - a);
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
        public override void savebody(BinaryWriter br, System.Windows.Forms.ToolStripProgressBar toolStripProgressBar)
        {
            baseskelet.ToStream(br);
            br.Write(headindex);
            br.Write(weaponindex);
            br.Write(rootindex);
            br.Write(toprootindex);
            br.Write(bottomrootindex);
            br.Write(botomindexes.Length);
            for (int i = 0; i < botomindexes.Length; i++)
                br.Write(botomindexes[i]);

            br.Write(topindexes.Length);
            for (int i = 0; i < topindexes.Length; i++)
                br.Write(topindexes[i]);
        }
        public override void saveheader(BinaryWriter br)
        {
            br.WritePackString(name);
            br.Write(offset);
            br.Write(ElementType.SkeletonWithAddInfo);
            calcheadersize();
            br.Write(headersize);
            br.Write(this.size);
        }
        public override void ViewBasicInfo(System.Windows.Forms.ComboBox loadedcombobox, System.Windows.Forms.ComboBox comboBox2, System.Windows.Forms.Label label1, System.Windows.Forms.Label label2, System.Windows.Forms.Label label3, System.Windows.Forms.Label label4, System.Windows.Forms.GroupBox groupBox1, System.Windows.Forms.TextBox tb, System.Windows.Forms.Button button2, System.Windows.Forms.Button button1)
        {
            loadedcombobox.Items.Clear();
            comboBox2.Items.Clear();
            loadedcombobox.Text = ElementType.ReturnString(this.loadedformat);
            comboBox2.Text = ElementType.ReturnString(this.forsavingformat);
            groupBox1.Text = tb.Text = name;
            loadedcombobox.Enabled = false;
            comboBox2.Enabled = false;
            tb.Enabled = true;
            button2.Enabled = true;
            button1.Enabled = true;
            label1.Text = this.number.ToString();
            label2.Text = this.offset.ToString();
            label3.Text = this.headersize.ToString();
            label4.Text = this.size.ToString();
        }
        #endregion

        #region data
        private int[] botomindexes;

        public int[] BottomIndexes
        {
            get
            {
                return botomindexes;
            }
            set
            {
                bool needsort = false;

                for (int i = 0; i < value.Length - 1; i++)
                    if (value[i] > value[i + 1])
                    {
                        needsort = true;
                        break;
                    }

                if (needsort)
                    Array.Sort(value);

                botomindexes = value;
            }
        }
        private int[] topindexes;
        public int[] TopIndexes
        {
            get
            {
                return topindexes;
            }
            set
            {
                bool needsort = false;

                for (int i = 0; i < value.Length - 1; i++)
                    if (value[i] > value[i + 1])
                    {
                        needsort = true;
                        break;
                    }

                if (needsort)
                    Array.Sort(value);

                topindexes = value;
            }
        }
        private int headindex;
        public int HeadIndex
        {
            get
            {
                return headindex;
            }
            set
            {
                headindex = value;
            }
        }
        private int weaponindex;
        public int WeaponIndex
        {
            get
            {
                return weaponindex;
            }
            set
            {
                weaponindex = value;
            }
        }
        private int rootindex;
        public int RootIndex
        {
            get
            {
                return rootindex;
            }
            set
            {
                rootindex = value;
            }
        }
        private int toprootindex;
        public int TopRootIndex
        {
            get
            {
                return toprootindex;
            }
            set
            {
                toprootindex = value;
            }
        }
        private int bottomrootindex;
        public int BottomRootIndex
        {
            get
            {
                return bottomrootindex;
            }
            set
            {
                bottomrootindex = value;
            }
        }
        public Skeleton baseskelet
        {
            get;
            private set;
        }
        #endregion



        public SkeletonWithAddInfo(Skeleton s)
        {
            baseskelet = s;
            this.loadedformat = this.forsavingformat = ElementType.SkeletonWithAddInfo;
        }
        public SkeletonWithAddInfo()
        {
            this.loadedformat = this.forsavingformat = ElementType.SkeletonWithAddInfo;
        }
       

    }
}

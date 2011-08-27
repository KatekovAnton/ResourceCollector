using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using Microsoft.Xna.Framework;

namespace ResourceCollector
{
    public class AnimationBase : PackContent//int BaseAnimation = 40;
    {
        public bool needtosave;

        public override void calcbodysize(System.Windows.Forms.ToolStripProgressBar targetbar)
        {
            throw new NotImplementedException();
        }
        public override System.Windows.Forms.DialogResult createpropertieswindow(Pack p, System.Windows.Forms.TreeView outputtreeview)
        {
            return System.Windows.Forms.DialogResult.OK;
        }
        public override void calcheadersize()
        {
            throw new NotImplementedException();
        }
        public override int loadbody(BinaryReader br, System.Windows.Forms.ToolStripProgressBar toolStripProgressBar)
        {
            throw new NotImplementedException();
        }
        public override void loadobjectheader(HeaderInfo hi, BinaryReader br)
        {
            throw new NotImplementedException();
        }
        public override void savebody(BinaryWriter br, System.Windows.Forms.ToolStripProgressBar toolStripProgressBar)
        {
            throw new NotImplementedException();
        }
        public override void saveheader(BinaryWriter br)
        {
            throw new NotImplementedException();
        }
        public override void ViewBasicInfo(System.Windows.Forms.ComboBox comboBox1, System.Windows.Forms.ComboBox comboBox2, System.Windows.Forms.Label label1, System.Windows.Forms.Label label2, System.Windows.Forms.Label label3, System.Windows.Forms.Label label4, System.Windows.Forms.GroupBox groupBox1, System.Windows.Forms.TextBox tb, System.Windows.Forms.Button button2, System.Windows.Forms.Button button1)
        {
            throw new NotImplementedException();
        }
        public int BonesCount
        {
            get;
            protected set;
        }

        public Matrix[][] Frames
        {
            get;
            protected set;
        }

        public int Length
        {
            get { return Frames.Length; }
        }

        public Skeleton Skeleton
        {
            get;
            protected set;
        }

        public AnimationBase()
        {
            loadedformat = forsavingformat = ElementType.BaseAnimation;
        }

        public static AnimationBase FromStream(System.IO.Stream stream, Skeleton skeleton)
        {

            var clip = new AnimationBase();
            var reader = new System.IO.BinaryReader(stream);

            var start = reader.ReadInt32();
            var end = reader.ReadInt32();
            var length = end - start + 1;

            clip.BonesCount = reader.ReadInt32();
            var counter = reader.ReadInt32();

            clip.Frames = new Matrix[length][];
            for (int i = 0; i < length; i++)
            {
                clip.Frames[i] = new Matrix[clip.BonesCount];
            }

            for (int i = 0; i < clip.BonesCount; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    clip.Frames[j][i] = reader.ReadMatrix();
                }
            }

            clip.Skeleton = skeleton;

            return clip;
        }

    }
}

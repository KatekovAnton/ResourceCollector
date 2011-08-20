using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BoneMap = System.Collections.Generic.Dictionary<string, int>;
namespace ResourceCollector
{
    class SkeletonInformation
    {
        public int rootindex
        {
            get;
            private set;
        }
        public int weaponindex
        {
            get;
            private set;
        }
        public int headindex
        {
            get;
            private set;
        }

        public int topindex
        {
            get;
            private set;
        }
        public int bottomindex
        {
            get;
            private set;
        }

        public SkeletonInformation(Skeleton sk)
        { }

    }
    public class Bone
    {
        public int level;
        public string Name;
        public int index;
        public Matrix BaseMatrix;
        public List<Bone> Childrens;
        public float Length
        {
            get;
            private set;
        }

        public Bone Parent;
        public Bone()
        {
            Childrens = new List<Bone>();
        }
        public override string ToString()
        {
            return index.ToString() + " " + Name;
        }
    }

    public class Skeleton : PackContent//int Skeleton = 30;
    {
        public override void calcbodysize(System.Windows.Forms.ToolStripProgressBar targetbar)
        {
            size = 4;//bone count

            foreach (var bone in bones)
            {
                size += OtherFunctions.GetPackStringLengthForWrite(bone.Name);
                if (bone.index != this.RootIndex)
                    size += OtherFunctions.GetPackStringLengthForWrite(bone.Parent.Name);
                else
                    size += OtherFunctions.GetPackStringLengthForWrite("-\0");
                size += 48;
            }
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

        public Bone[] bones
        {
            get;
           private set;
        }
        public BoneMap map
        {
            get;
            private set;
        }
        public Skeleton()
        { }

        public void Init(Bone root, Bone[] bones)
        {
            Root = root;
            this.bones = bones;

            map = new BoneMap(bones.Length);
            for (int i = 0; i < bones.Length; i++)
                map.Add(bones[i].Name, i);
        }
        public Bone Root
        {
            get;
            private set;
        }
        public int IndexOf(string name)
        {
            return map[name];
        }

        public int RootIndex
        {
            get { return map[Root.Name]; }
        }
        public void ToStream(BinaryWriter stream)
        {
            stream.Write(bones.Length);
            for (int i = 0; i < bones.Length; i++)
            {
                OtherFunctions.WritePackString(stream, bones[i].Name);
                if(i!=RootIndex)
                    OtherFunctions.WritePackString(stream, bones[i].Parent.Name);
                else
                    OtherFunctions.WritePackString(stream, "-\0");
            }
            for (int i = 0; i < bones.Length; i++)
            {
                stream.WriteMatrix(bones[i].BaseMatrix);
            }


        }
        public static Skeleton FromStream(BinaryReader stream)
        {
          //  var self = new BinaryReader(stream);
            var bones = new Bone[stream.ReadInt32()];
            var parentNames = new string[bones.Length];
            Bone root = null;

            for (int i = 0; i < bones.Length; i++)
            {
                bones[i] = new Bone();
                bones[i].index = i;
                bones[i].Name = stream.ReadPackString();
                parentNames[i] = stream.ReadPackString();
                if (parentNames[i] == "-\0")
                    root = bones[i];
            }

            if (root == null)
                throw new Exception("Root bone can not be null");

            var skeleton = new Skeleton();
            skeleton.Init(root, bones);
            for (int i = 0; i < bones.Length; i++)
            {
                bones[i].BaseMatrix = stream.ReadMatrix();
                if (bones[i] != root)
                    bones[i].Parent = bones[skeleton.IndexOf(parentNames[i])];
            }
            foreach (Bone b in bones)
                for (int i = 0; i < bones.Length; i++)
                    if (bones[i].Parent == b)
                        b.Childrens.Add(bones[i]);


            return skeleton;
        }
    }
}

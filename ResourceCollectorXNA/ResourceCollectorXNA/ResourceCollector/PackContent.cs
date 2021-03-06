﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace ResourceCollector
{
   /* public class ElementClass
    {
        public int type;
        public string name;
        public int imgindex;
        
    }*/

    public class ElementType
    {
        static Hashtable formats = new Hashtable();
        public const int MissingObject = -1;
        public const int MeshSkinnedOptimazedForStore = 0;
        public const int MeshSkinnedOptimazedForLoading = 1;
        public const int PNGTexture = 2;
        
        
        public const int MeshList = 10;
        public const int StringList = 11;
        public const int LevelObjectDescription = 12;
        public const int GameObjectDescription = 13;
        public const int RenderObjectDescription = 14;
        public const int ParticelRenderObjectDescription = 15;


        public const int CollisionMesh = 20;
        public const int ConvexMesh = 21;

        public const int Skeleton = 30;
        public const int SkeletonWithAddInfo = 31;
        public const int BaseAnimation = 40;
        public const int Character = 50;
        public const int TextureList = 60;
        public const int Material = 61;

        public const int LevelContent = 100;

        static public void Init()
        { 
                formats.Add( ElementType.MeshSkinnedOptimazedForLoading     ,"Mesh Skinned New format (recomendated)");
                formats.Add( ElementType.MeshSkinnedOptimazedForStore       ,"Mesh Skinned Old format (not recomendated)");
                formats.Add( ElementType.PNGTexture                         , "Texture (PNG)");
                formats.Add( ElementType.Skeleton                           , "Skeleton");
                formats.Add( ElementType.BaseAnimation                      , "Base Animation");
                formats.Add( ElementType.Character                          , "Character description");
                formats.Add( ElementType.MeshList                           , "Mesh List");
                formats.Add( ElementType.SkeletonWithAddInfo                , "Skeleton with additional information");
                formats.Add( ElementType.TextureList                        , "Texture list");
                formats.Add( ElementType.CollisionMesh                      , "Collision mesh");
                formats.Add( ElementType.ConvexMesh                         , "Convex mesh");
                formats.Add( ElementType.StringList                         , "String list");
                formats.Add( ElementType.LevelObjectDescription             , "Level object description");
                formats.Add( ElementType.GameObjectDescription              , "Game object description");
                formats.Add( ElementType.RenderObjectDescription            , "Render object description");
                formats.Add( ElementType.Material                           , "Material");
                formats.Add( ElementType.LevelContent                       , "Level");
                formats.Add( ElementType.ParticelRenderObjectDescription    , "Particle Object Description");
                Eggs.Mirror(formats);
        }

        public static string ReturnString(int format)
        {
            if (formats.ContainsKey(format))
                return (string)formats[format];
            else return "wrong format";
        }

        public static int ReturnFormat(string format)
        {
            if (formats.ContainsKey(format))
                return (int)formats[format];
            else return -1;
        }
    }
    public enum NodeType
    {
        element,
        folder,
        packname
    }
    public struct infopair
    {
        public int index;
        public int number;
    };
    public struct Mypair
    {
        public int frst, scnd;
        public override string ToString()
        {
            return "frst = " + frst.ToString() + "; scnd = " + scnd.ToString();
        }
    }
    public class HeaderInfo
    {
        public int offset;
        public int size;
        public int headersize;
        public int loadedformat, forsavingformat;
        public string name;
    }
    

    public abstract class PackContent
    {
        public ContextMenu contextmenu;
        public bool SuccessedReadBody;
        public bool SuccessedReadHead;
        public int number;
        public int offset;
        public int size;
        public int headersize;
        public int loadedformat, forsavingformat;
        public string name;
        public List<object> Enginereadedobject;
        public PackContent()
        {
            name = "";
            Enginereadedobject = new List<object>();
        }
        public static void loadbaseheader(System.IO.BinaryReader br, HeaderInfo hi)//16+имя
        {
            int length = br.ReadInt32();
            hi.name = new string(br.ReadChars(length + 1));

            hi.offset = br.ReadInt32();
            hi.loadedformat = br.ReadInt32();
            hi.headersize = br.ReadInt32();
            hi.forsavingformat = hi.loadedformat;

        }
        /// <summary>
        /// loading body
        /// </summary>
        /// <param name="br">stream for loading</param>
        /// <param name="toolStripProgressBar">for progress output</param>
        /// <returns>returns size</returns>
        public abstract int loadbody(System.IO.BinaryReader br);
        public abstract void saveheader(System.IO.BinaryWriter bw);
        public abstract void savebody(System.IO.BinaryWriter bw);

        public abstract void calcheadersize();
        public abstract System.Windows.Forms.DialogResult createpropertieswindow(Pack p, TreeView outputtreeview);
        public abstract void loadobjectheader(HeaderInfo hi, System.IO.BinaryReader br);
        public abstract void calcbodysize();
        public virtual void ViewBasicInfo(
            ComboBox comboBox1, ComboBox comboBox2, Label label1, Label label2, Label label3,
            Label label4, GroupBox groupBox1, TextBox tb, Button button2, Button button1)
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

    

    public class MissingObject : PackContent
    {
        byte[] data;
        public Pack pack;
        public MissingObject(int num)
        {
            loadedformat = forsavingformat = ElementType.MissingObject;
            name = "Missing object";
            number = num;
            offset = headersize = size = 0;
            SuccessedReadBody = SuccessedReadHead = false;
        }
        public override int loadbody(System.IO.BinaryReader br)
        {
            data =  br.ReadBytes(this.size);
            return this.size;
        }
        public override void saveheader(System.IO.BinaryWriter bw)
        {
            bw.WritePackString(name);
            bw.Write(offset);
            bw.Write(loadedformat);

            bw.Write(headersize);
            bw.Write(data.Length);
        }
        public override void savebody(System.IO.BinaryWriter br)
        {
            br.Write(data);
        }
        public override void calcheadersize()
        {
            headersize = 16 + OtherFunctions.GetPackStringLengthForWrite(name);
        }
        public override void calcbodysize( )
        {
            size = data.Length;
        }
        public override System.Windows.Forms.DialogResult createpropertieswindow(Pack p, System.Windows.Forms.TreeView outputtreeview)
        {
            return DialogResult.OK;
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
        public override void ViewBasicInfo(
            ComboBox comboBox1, ComboBox comboBox2, Label label1, Label label2, Label label3,
            Label label4, GroupBox groupBox1, TextBox tb, Button button2, Button button1)
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox1.Text = ElementType.ReturnString(this.loadedformat);
            comboBox2.Text = ElementType.ReturnString(this.forsavingformat);
            groupBox1.Text = tb.Text = name + "- missing format";
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            tb.Enabled = false;
            button2.Enabled = true;
            button1.Enabled = false;
            label1.Text = this.number.ToString();
            label2.Text = this.offset.ToString();
            label3.Text = this.headersize.ToString();
            label4.Text = this.size.ToString();
        }
    }
}
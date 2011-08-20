using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ResourceCollector.Content
{
    /// <summary>
    /// описание видимого объекта
    /// </summary>
    public class GameObjectDescription : PackContent
    {
        //эти параметры поведение объекта:

       




        //отбрасывает ли тени
        bool shadowcaster = false;

        //принимает ли тени(затеняется или нет)
        bool shadowreceiver = false;

        //является ли крышей(не будет рисоваться если крыши спрятаны)
        bool isroof = false;

        //проверять ли этот объект на пересечение с курсором мыши
        bool intersectcheckobject = true;

        //подсвечивать ли при наведении на него мышкой
        bool glowonintersect = false;

        //колижонмеш, который будет использоваться для проверки на пересечение (при наведении курсора, при просчёте полёта пуль).
        //он может быть анимированным, если сам объект анимирован (к примеру паукообразный робот) 
        CollisionMesh RayCastCollisionMesh;

        //описание соответствующего рендеробъекта
        string RenderObjectDescriptionName;

        //ну пока пусть будет неаним....
        bool animated = false;

        //анимирован ли RCCM - рей каст солижн меш...
        bool animatedRCCM = false;
        //для анимированных объектов отдельно указатель на чарактера и на стартовую анимацию.
        //string charactername;


        //тип поведения объекта(один из констинтов сверху)
        int ObjectBehaviourType;

        //колижнмеш для физ модели. неаним
        CollisionMesh PhysicCollisionMesh;

        //масса объекта
        float Mass;

        /// <summary>
        /// загрузка всего из массива байт
        /// </summary>
        /// <param name="array">байт эррэй с данными</param>
        public override void calcbodysize(System.Windows.Forms.ToolStripProgressBar targetbar)
        {
            throw new NotImplementedException();
        }
        public override System.Windows.Forms.DialogResult createpropertieswindow(Pack p, System.Windows.Forms.TreeView outputtreeview)
        {
            throw new NotImplementedException();
        }
        public override void calcheadersize()
        {
            headersize = 20 + name.Length;
        }
        public override int loadbody(System.IO.BinaryReader br, System.Windows.Forms.ToolStripProgressBar toolStripProgressBar)
        {
            throw new NotImplementedException();
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
        public override void savebody(System.IO.BinaryWriter br, System.Windows.Forms.ToolStripProgressBar toolStripProgressBar)
        {
            throw new NotImplementedException();
        }
        public override void saveheader(System.IO.BinaryWriter bw)
        {
            bw.WritePackString(name);
            bw.Write(offset);
            bw.Write(forsavingformat);
            bw.Write(headersize);
            calcbodysize(null);
            bw.Write(size);
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

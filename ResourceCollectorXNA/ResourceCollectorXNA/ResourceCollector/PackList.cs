using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ResourceCollector
{
    public class PackList
    {
        public List<Pack> packs = new List<Pack>();
        private bool lastsuccess;
        public static PackList Instance;
        public PackList()
        {
            if (Instance == null)
                Instance = this;
        }

        public void AddPack(string filename,
            System.IO.BinaryReader br)
        {
            Pack p = new Pack();
            p.Init(filename, br);
            if (p.fullsucces)
                packs.Add(p);
            for (int i = 0; i< p.Objects.Count;i++)
                if (!p.Objects[i].name.EndsWith("\0")) p.rename(i, p.Objects[i].name + "\0");
        }

        public PackContent GetObject(string name)
        {
            for (int j = 0; j < packs.Count; j++)
                if (packs[j].Objects != null)
                    for (int i = 0; i < packs[j].Objects.Count; i++)
                        if (packs[j].Objects[i].name == name)
                            return packs[j].Objects[i];

            return null;
        }

        public PackContent[] GetObjects(string[] names)
        {
            PackContent[] contents = new PackContent[names.Length];
            int number = 0;
            for (int n = 0; n < names.Length; n++)
                for (int j = 0; j < packs.Count; j++)
                    if (packs[j].Objects != null)
                        for (int i = 0; i < packs[j].Objects.Count; i++)
                            if (packs[j].Objects[i].name == names[n])
                            {
                                contents[number] = packs[j].Objects[i];
                                number++;
                            }

            return contents;
        }

        public PackContent findobject(string name, ref System.Drawing.Point cc)
        {
            for (int j = 0; j < packs.Count; j++)
                if (packs[j].Objects != null)
                    for (int i = 0; i < packs[j].Objects.Count; i++)
                        if (packs[j].Objects[i].name == name)
                        {
                            cc.X = j;
                            cc.Y = i;
                            return packs[j].Objects[i];
                        }
            cc.Y = cc.X = -1;
            return null;
        }
        public void Save(int number, string filename)
        {
            if (Eggs.CheckPack(true, "Для того чтоб все равно сохранить пак нажми ОК"))
            if (packs != null && packs.Count > number && packs[number] != null && packs[number].Objects != null)
            {
                packs[number].Save(filename);
            }
        }
        public void Drop(int packnumber, int contentnumber)
        {
            if(packnumber<packs.Count)
                packs[packnumber].DropElement(contentnumber);
        }
        public void SaveAllInOne(int number)
        {
            
        }
        public bool SuccessLast
        {
            get 
            {
                return lastsuccess;
            }
        }
        void Clear()
        {
            packs.Clear();
        }

        public void AddEmptyPack(ToolStripProgressBar toolStripProgressBar1,
            ToolStripProgressBar toolStripProgressBar2,
            TreeView treeView1)
        {
            Pack p = new Pack();
            p.Init();
            packs.Add(p);
        }
    }
}

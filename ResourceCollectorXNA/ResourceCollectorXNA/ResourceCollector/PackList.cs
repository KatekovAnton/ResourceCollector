﻿using System;
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
            System.IO.BinaryReader br,
            ToolStripProgressBar toolStripProgressBar1,
            ToolStripProgressBar toolStripProgressBar2,
            TreeView treeView1)
        {
            Pack p = new Pack();
            p.Init(filename, br, toolStripProgressBar1, toolStripProgressBar2, treeView1);
            if (p.fullsucces)
                packs.Add(p);
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
        public void Save(int number, ToolStripProgressBar toolStripProgressBar, string filename)
        {
            if (packs != null && packs.Count > number && packs[number] != null && packs[number].Objects != null)
            {
                packs[number].Save(filename, toolStripProgressBar);
            }
        }
        public void Drop(int packnumber, int contentnumber)
        {
            if(packnumber<packs.Count)
                packs[packnumber].DropElement(contentnumber);
        }
        public void SaveAllInOne(int number, ToolStripProgressBar toolStripProgressBar)
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
            p.Init(toolStripProgressBar1, toolStripProgressBar2, treeView1);
            packs.Add(p);
        }
    }
}

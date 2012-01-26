using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ResourceCollectorXNA;
using ResourceCollectorXNA.Content;
using ResourceCollector.Content;

namespace ResourceCollector
{
   public static class Eggs
    {
       public static List<string> Filter(List<string> list, string search_pattern = "", bool invert = false)
        {
            List<string> new_list = new List<string>();
            Regex regex = new Regex(search_pattern);
            foreach (string name in list)
                if (regex.IsMatch(name)^invert)
                    new_list.Add(name);

            return new_list;
        }


       public static void Filter(List<string> list, void_obj Action, string search_pattern = "", bool invert = false)
       {
           Regex regex = new Regex(search_pattern);
           foreach (string name in list)
               if (regex.IsMatch(name) ^ invert)
                           Action(name);
       }


       public static List<dynamic> Filter(Dictionary<string, dynamic> list, string search_pattern = "", bool invert = false)
       {
           List<dynamic> new_list = new List<dynamic>();
           Regex regex = new Regex(search_pattern);
           foreach (string name in list.Keys)
           {
               if (regex.IsMatch(name) ^ invert)
                   new_list.Add(list[name]);
           }
           return new_list;
       }



       public static void Filter(Dictionary<string, dynamic> list, void_obj Action, string search_pattern = "", bool invert = false)
       {
           Regex regex = new Regex(search_pattern);
           foreach (string name in list.Keys)
           {
               if (regex.IsMatch(name) ^ invert)
                   Action(list[name]);
           }

       }


       public static string ShortFileName(string filename)
       { 
            string _filename = "";
            int i1 = filename.LastIndexOf("\\");
            if (i1 > 0) _filename += filename.Substring(i1+1);
            i1 = _filename.LastIndexOf(".");
            if (i1 > 0) _filename = _filename.Substring(0,i1);
            return _filename;
       }


      public delegate void void_obj(dynamic obj);



      public static string ofd()
      {
          var ofd = new OpenFileDialog();
          if (ofd.ShowDialog() == DialogResult.OK)
              return ofd.FileName;
          return "";

      }

      public static void SelectAll(ListBox listBox, bool val)
      {
          for (int i = 0; i < listBox.Items.Count; i++)
          {
              listBox.SetSelected(i, val);
          }
      }


      public static void MoveListboxItem(int index, ListBox listBox)
      {
          if (listBox.SelectedItems != null) //is there an item selected?
          {
             //if it's moving up, the loop moves from first to last, otherwise, it moves from last to first
              for (int i = (index < 0 ? 0 : listBox.Items.Count - 1); index < 0 ? i < listBox.Items.Count : i > -1; i -= index)
              {
                  if (listBox.SelectedIndices.Contains(i))
                  {
                      //if it's moving up, it should not be the first item, or, if it's moving down, it should not be the last
                      if ((index < 0 && i > 0) || (index > 0 && i < listBox.Items.Count - 1))
                      {
                          //if it's moving up, the previous item should not be selected, or, if it's moving down, the following item should not be selected
                          if ((index < 0 && !listBox.SelectedIndices.Contains(i - 1)) || (index > 0 && !listBox.SelectedIndices.Contains(i + 1)))
                          {
                              dynamic itemA = listBox.Items[i]; //the selected item

                              listBox.Items.Remove(itemA); //is removed

                              listBox.Items.Insert(i + index, itemA);//and swapped
                              listBox.SelectedItems.Add(itemA);
                          }
                          else
                          { 
                            
                          }
                      }
                  }
              }
          }
      }

      public static List<PackContent> GetObjects(int loadedformat, string search_pattern = "")
      { 
          List<PackContent> objects = new List<PackContent>();
          for (int i = 0; i < PackList.Instance.packs.Count; i++)
              for (int j = 0; j < PackList.Instance.packs[i].Objects.Count; j++)
              {
                  if (!((loadedformat == PackList.Instance.packs[i].Objects[j].loadedformat) || ( loadedformat == PackList.Instance.packs[i].Objects[j].forsavingformat )))
                      continue;
                  string name = PackList.Instance.packs[i].Objects[j].name;
                  if (Regex.Match(name, search_pattern).Success) objects.Add(PackList.Instance.packs[i].Objects[j]);
              }
          return objects;
      }

      public static void Rename(int loadedformat, string search_pattern, string replace_string)
      {
          List<PackContent> objects = GetObjects(loadedformat, search_pattern);
          foreach (PackContent pc in objects)
          {
              pc.name = Regex.Replace(pc.name, search_pattern, replace_string);  
          }
      }

      public static void Mirror(Hashtable hashtable, bool delete = false)
      {
          List<dynamic> keys = new List<dynamic>();
          foreach (dynamic k in hashtable.Keys)
              keys.Add(k);

          foreach (dynamic k in keys)
          {
              hashtable.Add(hashtable[k], k);
              if (delete) hashtable.Remove(k);
          }
      }

      public static List<PackContent> CreateCollisionMeshes(string search_pattern)
      {
          var ccc = new List<PackContent>();

          ccc = Eggs.GetObjects(ElementType.MeshSkinnedOptimazedForLoading, search_pattern);
          ccc.InsertRange(0, Eggs.GetObjects(ElementType.MeshSkinnedOptimazedForStore, search_pattern));

          var cccr = new List<PackContent>();
          foreach (PackContent pc in ccc)
          {
              MeshSkinned m = null;
              if (pc != null) m = pc as MeshSkinned;
              if (m != null)
              {
                  CollisionMesh cm = new CollisionMesh(m);
                  cm.name = "cm_" + pc.name;
                  ResourceCollectorXNA.ConsoleWindow.TraceMessage("Created CollisionMesh  " + cm.name);
                  cccr.Add(cm);
              }
          }
          return cccr;
      }

    }
}

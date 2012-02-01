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
       private static StreamWriter _sw;
       public static StreamWriter sw 
       {
           get
           {
               if (_sw == null)
               {
                   _sw = new StreamWriter(AppConfiguration.PackPlaceFolder + "\\eggs.cfg", false);
                   _sw.AutoFlush = true;
               }
               return _sw;
           }
       }
       
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

      /// <summary>
      /// просто просит у пользователя имя файла через OpenFileDialog
      /// </summary>
      public static string ofd()
      {
          var ofd = new OpenFileDialog();
          if (ofd.ShowDialog() == DialogResult.OK)
              return ofd.FileName;
          return Cancel;

      }

      public static void SelectAll(ListBox listBox, bool val)
      {
          for (int i = 0; i < listBox.Items.Count; i++)
          {
              listBox.SetSelected(i, val);
          }
      }

       /// <summary>
      /// двигает выделенные элементы listBox на index позиций
       /// </summary>
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
       /// <summary>
       /// получает объекты из пака(паков) в соответствие с шаблоном поиска по имени и типом
       /// </summary>
       /// <param name="loadedformat">тип, -1</param>
      /// <param name="search_pattern">шаблоном поиска по имени и типом</param>
      /// <param name="in_pack">-1 для всех паков, или номер пака</param>
       /// <returns></returns>
      public static List<PackContent> GetObjects(int loadedformat, string search_pattern = "" , int in_pack = -1)
      { 
          List<PackContent> objects = new List<PackContent>();

          if (in_pack < 0)
          for (int i = 0; i < PackList.Instance.packs.Count; i++)
            objects.AddRange(PackList.Instance.packs[i].Objects.FindAll(o => (Regex.Match(o.name , search_pattern).Success) && (o.loadedformat == loadedformat || o.forsavingformat == loadedformat)));
          else
              objects.AddRange(PackList.Instance.packs[in_pack].Objects.FindAll(o => (Regex.Match(o.name, search_pattern).Success) && (o.loadedformat == loadedformat || o.forsavingformat == loadedformat)));
             
          return objects;
      }
       /// <summary>
       /// получает объекты из пака(паков) в соответствие с шаблоном поиска по имени
       /// </summary>
       /// <param name="search_pattern">шаблоном поиска</param>
       /// <param name="in_pack">-1 для всех паков, или номер пака</param>
       /// <param name="use_types">использовать для поиска имена типов?</param>
       /// <returns></returns>
      public static List<PackContent> GetObjects(string search_pattern = "", int in_pack = -1, bool use_types = false)
      {
          List<PackContent> objects = new List<PackContent>();

          if (in_pack < 0)
          for (int i = 0; i < PackList.Instance.packs.Count; i++)
              objects.AddRange(PackList.Instance.packs[i].Objects.FindAll(o => (Regex.Match((use_types ? ElementType.ReturnString(o.loadedformat) + " " : "") + o.name, search_pattern).Success)));
          else
              objects.AddRange(PackList.Instance.packs[in_pack].Objects.FindAll(o => (Regex.Match((use_types ? ElementType.ReturnString(o.loadedformat) + " " : "") + o.name, search_pattern).Success)));
          return objects;
      }

       /// <summary>
       /// Групповое переименование объектов
       /// </summary>
       /// <param name="loadedformat">фильтр по loadedformat, -1 == для всех типов</param>
       /// <param name="search_pattern">шаблон поиска</param>
       /// <param name="replace_string">шаблон замены</param>
       /// <param name="incfg">true - записать лог в файл, который можно использовать потом в кач-ве cfg, false - не записывать</param>
      public static void Rename(int loadedformat, string search_pattern, string replace_string, bool incfg = false)
      {
          List<PackContent> objects = GetObjects(loadedformat, search_pattern);
          int i = 0;
          foreach (PackContent pc in objects)
          {
              string new_name = Regex.Replace(pc.name, search_pattern, replace_string.Replace(@"\c",i.ToString()));
              i++;

              if (!replace_string.EndsWith("\0")) new_name += "\0";
              if (incfg)
              sw.WriteLine("{0} = \"{1}\"", new_name, pc.name.Substring(0, pc.name.Length - 1));

              ResourceCollectorXNA.ConsoleWindow.TraceMessage(pc.name.Substring(0, pc.name.Length-1) + " \t  was renamed to: \t " + new_name);
              pc.name = new_name;
          }
      }

       /// <summary>
       /// Отражает Hashtable наоборот
       /// </summary>
       /// <param name="hashtable">какая таблица</param>
       /// <param name="delete">true - удалить предыдущие элементы, false - оставить</param>
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

      public static List<PackContent> CreateCollisionMeshes(string search_pattern = "")
      {
          var ccc = Eggs.GetObjects(ElementType.MeshSkinnedOptimazedForLoading, search_pattern);
         // ccc.AddRange(Eggs.GetObjects(ElementType.MeshSkinnedOptimazedForStore, search_pattern));

          var cccr = new List<PackContent>();
          ResourceCollectorXNA.ConsoleWindow.TraceMessage("Created CollisionMeshes:");
          foreach (PackContent pc in ccc)
          {
              MeshSkinned m = null;
              if (pc != null) m = pc as MeshSkinned;
              if (m != null)
              {
                  CollisionMesh cm = new CollisionMesh(m);
                  cm.name = "cm_" + pc.name;
                  ResourceCollectorXNA.ConsoleWindow.TraceMessage("\t\t\t" + cm.name);
                  cccr.Add(cm);
              }
          }
          ResourceCollectorXNA.ConsoleWindow.TraceMessage("");
          return cccr;
      }


      public static List<PackContent> CreateRenderObjectDescriptions(string search_pattern = "")
      {
         var ccc = Eggs.GetObjects(ElementType.MeshSkinnedOptimazedForLoading, search_pattern);
          // ccc.InsertRange(0, Eggs.GetObjects(ElementType.MeshSkinnedOptimazedForStore, search_pattern)); // why only MeshSkinnedOptimazedForLoading???

          var cccr = new List<PackContent>();
             ResourceCollectorXNA.ConsoleWindow.TraceMessage("Created RenderObjectDescriptions:");
          foreach (PackContent pc in ccc)
          {
              RenderObjectDescription rod = new RenderObjectDescription();
              rod.pack = PackList.Instance.packs[0];
              //3 раза
              // add lod, subset, meshes
              string m_name = GetObjectName(ElementType.MeshSkinnedOptimazedForLoading, pc.name);

              rod.addlod().AddSubSet(new string[] { m_name });
              rod.addlod().AddSubSet(new string[] { m_name });
              rod.addlod().AddSubSet(new string[] { m_name });

              rod.name = "rod_" + pc.name;
              rod.IsShadowCaster = true;
              rod.IsShadowReceiver = true;

              cccr.Add(rod);
              ResourceCollectorXNA.ConsoleWindow.TraceMessage("\t\t\t" + rod.name);
          }
          ResourceCollectorXNA.ConsoleWindow.TraceMessage("");
          return cccr;
       
      }

      public static List<PackContent> CreateDiffuseMaterials(string search_pattern = "")
      {
          var ccc = Eggs.GetObjects(ElementType.PNGTexture, search_pattern);
          List<PackContent> cccr = new List<PackContent>();
          ResourceCollectorXNA.ConsoleWindow.TraceMessage("Created DiffuseMaterials:");
          foreach (PackContent pc in ccc)
          {
              Material mat = new Material("mat_" + pc.name, PackList.Instance.packs[0]);
              string diff_name = GetObjectName(ElementType.PNGTexture, pc.name);

              mat.AddLod().Addmat().DiffuseTextureName = diff_name;
              mat.AddLod().Addmat().DiffuseTextureName = diff_name;
              mat.AddLod().Addmat().DiffuseTextureName = diff_name;

              cccr.Add(mat);
              ResourceCollectorXNA.ConsoleWindow.TraceMessage("\t\t\t" + mat.name);
          }
          ResourceCollectorXNA.ConsoleWindow.TraceMessage("");
          return cccr;
      }

      public static List<PackContent> CreateLevelObjectDescriptions(string search_pattern = "")
      {
          var ccc = Eggs.GetObjects(ElementType.MeshSkinnedOptimazedForLoading, search_pattern);
      //    ccc.AddRange(Eggs.GetObjects(ElementType.MeshSkinnedOptimazedForStore, search_pattern));

          var cccr = new List<PackContent>();

          ResourceCollectorXNA.ConsoleWindow.TraceMessage("Created LevelObjectDescriptions:");
          foreach (PackContent pc in ccc)
          {
              LevelObjectDescription lod = new LevelObjectDescription("lo_" + pc.name, PackList.Instance.packs[0]);
              
              lod.RODName = GetObjectName(ElementType.RenderObjectDescription, "rod_"+pc.name);
              lod.matname = GetObjectName(ElementType.Material, "mat_tex_"+pc.name);
         //   lod.PhysicCollisionName = "cm_"+pc.name;
              lod.BehaviourType = LevelObjectDescription.objectstaticbehaviourmodel;
              lod.RCCMName =  GetObjectName(ElementType.CollisionMesh, "cm_" + pc.name);
              lod.IsAnimated = false;
              lod.IsRCCMAnimated = false;
              lod.IsRCCMEnabled = true;
              
              ResourceCollectorXNA.ConsoleWindow.TraceMessage("\t\t\t" + lod.name);
              cccr.Add(lod);
          }
           
          ResourceCollectorXNA.ConsoleWindow.TraceMessage("");
          return cccr;
      }

       
       /// <summary>
       /// ф-ция автоматически подбирает нужный элемент из пака 0. в случае, если такого элемента нет - спрашивает у юзера, какой элемент подставить вместо него. в случае если есть элемент с похожим именем - пердлагает его
       /// </summary>
       /// <param name="format">ElementType.____</param>
       /// <param name="name">имя нужного объекта</param>
       /// <returns></returns>
      public static string GetObjectName(int format, string name) // советую элементы назначать через эту ф-цию.
      {
          string nn = name;
          if (PackList.Instance.packs[0].getobject(format, name) == null)
          {
              FormObjectPicker f = new FormObjectPicker(PackList.Instance.packs[0], format, false, ElementType.ReturnString(format) + " not found:  " + name, "^"+ name.TrimEnd("1234567890_\0".ToCharArray()) ,true);
              if (f.PickedContent.Count>0)
              {
                  nn = f.PickedContent[0];
              }
              else
              if (f.ShowDialog() == DialogResult.OK)
              {
                  nn = f.PickedContent[0];
              }
              f.Close();
          }
          return nn;
      }

      public static PackContent NULL = null;
       
       /// <summary>
       /// Очистка пака (паков). Удаляет элементы в соответствии с шаблоном и типом
       /// </summary>
       /// <param name="search_pattern">шаблон имени для объектов</param>
       /// <param name="format">loadedformat фильтр, -1 для всех объектов</param>
       /// <param name="pack">в каком паке выполнить очистку. null - во всех</param>
       public static void ClearPack(string search_pattern = "", int format = -1, Pack pack = null)
       {
           if (pack == null)
           {
               foreach (Pack pack_ in PackList.Instance.packs)   ClearPack(search_pattern, format, pack_);
           }
           else
           {
                   for (int i = 0; i < pack.Objects.Count; i++)
                       if (((format < 0) || (pack.Objects[i].loadedformat == format) || (pack.Objects[i].forsavingformat == format)) && Regex.IsMatch(pack.Objects[i].name, search_pattern))
                           pack.DropElement(i--);
           }
       }

       public static string Question(string message)
       {
           ResourceCollectorXNA.Question q = new Question(message);
           if (q.ShowDialog() == DialogResult.OK)
           {
               return q.answer;
           }
           return Cancel;
       }
       public static string Cancel = "Cancel";
       /// <summary>
       /// создает новый MessageBox
       /// </summary>
       public static void Message(string message)
       {
           MessageBox.Show(message);
       }

       /// <summary>
       /// Сообщение в ConsoleWindow
       /// </summary>
       public static void wr(string message)
       {
           ResourceCollectorXNA.ConsoleWindow.TraceMessage(message);
       }

       //TODO:
       /// <summary>
       ///  получает инфу об объекте
       /// </summary>
      public static string GetInfo(dynamic val)
      {
          if (val != null)
          {
              string str = val.ToString() + " : \n";
              try
              {
              
              }
              catch (Exception ee) 
              {
                  return str + "\n" + "ERROR: " + ee.Message;
              }
              return str;
          }
          else
          return "NULL";
      }

       /// <summary>
       /// выполняет различные проверки на корректность пака. возвращает тру если пак хорош
       /// </summary>
      public static bool CheckPack(bool need_view, string question = "")
      {
         if (need_view) wr("Проверка пака на корректность...");
          bool correct = true;
       //TODO : выполнять различные проверки на корректность пака 
          string hash = "";
          foreach (Pack pack in PackList.Instance.packs)
          {
              foreach (var pc in pack.Objects)
              {
                  string name = pc.name.TrimEnd('\0');
                  if (Regex.IsMatch(hash, "\\b" + name + "\\b" ))
                  {
                      wr("Сопадение имен: " + name);
                      correct = false;
                  }
                  hash += name+ " ";               
              }
          }

          if (!correct)
          {
              Message("Pack is not correct. See ConsoleWindow for details..");
              if (question != "")
              correct = Eggs.Question(question) != Eggs.Cancel;
          }
          else if (need_view) wr("Пак нормальный :)");
          return correct;
      }

    }
}

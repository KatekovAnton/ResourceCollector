using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceCollector
{
    public class AppConfiguration
    {
        private static Dictionary<string,string> properties = new Dictionary<string,string>();
        private static string _addAnimFolder = "_addAnimFolder";
        private static string _packPlaceFolder = "_packPlaceFolder";

        public static string AppPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).Substring(6);

        public static string AddAnimFolder
        {
            get
            {
                if (properties.Keys.Contains(_addAnimFolder))
                    return properties[_addAnimFolder];
                else
                    return "C:\\";
            }
            set
            {
                properties[_addAnimFolder] = value;
                SaveProperties();
            }
        }

        public static string PackPlaceFolder
        {
            get
            {
                if (properties.Keys.Contains(_packPlaceFolder))
                    return properties[_packPlaceFolder];
                else
                    return "C:\\";
            }
            set
            {
                properties[_packPlaceFolder] = value; 
                SaveProperties();
            }
        }

        public static void SaveProperties()
        { 
            string path = AppPath + "\\config.bin";
            System.IO.BinaryWriter br = new System.IO.BinaryWriter(new System.IO.FileStream(path, System.IO.FileMode.Create, System.IO.FileAccess.ReadWrite));
            DictionaryMethods.ToStream(properties, br);
            br.BaseStream.Flush();
            br.Close();
        }

        public static void ReadProperties()
        {
            string path = AppPath + "\\config.bin";
            if (!System.IO.File.Exists(path))
                return;

            System.IO.BinaryReader br = new System.IO.BinaryReader(new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.ReadWrite));

            try
            {
                Dictionary<string, string> _properties = DictionaryMethods.FromStream(br);
                properties = _properties;
            }
            catch (Exception)
            { }
            finally
            {
                br.BaseStream.Flush();
                br.Close();
            }
        }
    }
}

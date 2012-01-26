using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System.IO;

namespace ResourceCollectorXNA
{
    
    public abstract class AutoLoadingContent : Hashtable
    {
        public dynamic Empty;
        public string path = "";
        public List<string> Names = new List<string>();
        public AutoLoadingContent(string Path)
            : base()
        {
            path = "";
            if (!Path.Contains(":")) path = ResourceCollector.AppConfiguration.AppPath + "\\";
            if (Path.LastIndexOf("\\")>0) path += Path.Substring(0, Path.LastIndexOf("\\")+1);

            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public dynamic GetDirectly(string _name) { dynamic f = this[_name]; return this[_name]; }

        public dynamic this[string key]
        {
            get
            {
                if (base[key] == null)
                { Add(key, Load(key)); Names.Add(key); }
                return base[key];
            }
            set
            {
                base[key] = value;
            }
        }

        public abstract dynamic Load(string _name);
    }

}
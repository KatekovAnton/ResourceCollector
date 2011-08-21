using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ResourceCollectorXNA.Engine.Logic
{
    public enum ObjectEditorType
    {
        SolidObject,
        TerrainObject
    };
    public class EditorData
    {
        public string DescriptionName;
        public bool isActive;
        public int id;
        public ObjectEditorType objtype;
        public EditorData(string name, ObjectEditorType type)
        {
            objtype = type;
            DescriptionName = name;
        }

        ~EditorData()
        {
 
        }
    }
}

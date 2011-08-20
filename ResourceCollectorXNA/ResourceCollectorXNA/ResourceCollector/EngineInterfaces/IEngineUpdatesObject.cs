using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ResourceCollector;

namespace ResourceCollector.EngineInterfaces
{
    interface IEngineUpdatesObject
    {
        PackContent duplicateObject();
        void setDataFromObject(PackContent pc);
    }
}

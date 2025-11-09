using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelContentImportScript
{
    public class MapCollection : ConfigurationElementCollection
    {
        public MapElement this[int index]
        {
            get
            {
                return BaseGet(index) as MapElement;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new MapElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((MapElement)element).Client;
        }
    }
}

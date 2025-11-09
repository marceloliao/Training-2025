using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelContentImportScript
{
    public class MapSettingsConfig : ConfigurationSection
    {
        [ConfigurationProperty("map")]
        [ConfigurationCollection(typeof(MapCollection))]
        public MapCollection Map
        {
            get
            {
                return this["map"] as MapCollection;
            }
        }
    }
}

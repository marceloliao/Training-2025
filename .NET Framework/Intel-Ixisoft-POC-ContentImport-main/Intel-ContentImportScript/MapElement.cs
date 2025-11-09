using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntelContentImportScript
{
    public class MapElement : ConfigurationElement
    {
        [ConfigurationProperty ("client", IsKey = true, IsRequired = true)]
        public string Client
        {
            get
            {
                return base["client"] as string;
            }

            set
            {
                base["client"] = value;
            }
        }

        [ConfigurationProperty("rws", IsRequired = true)]
        public string Rws
        {
            get
            {
                return base["rws"] as string;
            }

            set
            {
                base["rws"] = value;
            }
        }
    }
}

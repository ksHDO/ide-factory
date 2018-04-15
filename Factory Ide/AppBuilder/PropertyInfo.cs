using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBuilder
{
    public struct PropertyInfo
    {
        public string name;
        public string formatRep;
        public PropertyInfo(string name, string formatRep)
        {
            this.name = name;
            this.formatRep = formatRep;
        }
    }
}

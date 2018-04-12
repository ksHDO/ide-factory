using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBuilder
{
    public class ElementTemplate
    {
        public string name = "";
        public string formatString = "";
        private int elementID = 0;

        public ElementTemplate(string name, string formatString)
        {
            this.name = name;
            this.formatString = formatString;
        }

        public string Format(ElementInfo eInfo)
        {
            
            string res = String.Copy(formatString);
            res = res.Replace("%content%", eInfo.content);
            res = res.Replace("%w%", eInfo.w.ToString());
            res = res.Replace("%h%", eInfo.h.ToString());
            res = res.Replace("%l%", eInfo.l.ToString());
            res = res.Replace("%t%", eInfo.t.ToString());
            res = res.Replace("%id%", elementID.ToString());
            elementID++;
            return res;
        }

    }
}

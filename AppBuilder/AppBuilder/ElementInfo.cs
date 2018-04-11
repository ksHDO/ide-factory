using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBuilder
{
    public struct ElementInfo
    {
        public string type;
        public string content;
        public int t, l, w, h;

        public ElementInfo(string type, string content, int t, int l, int w, int h)
        {
            this.type = type;
            this.content = content;
            this.t = t;
            this.l = l;
            this.w = w;
            this.h = h;
        }
    }

}

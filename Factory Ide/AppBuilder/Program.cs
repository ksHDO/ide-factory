using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBuilder
{

    class Program
    {
        static void Main(string[] args)
        {
            LanguageFactory a = new LanguageFactory();
            List<ElementInfo> test = new List<ElementInfo>();
            test.Add(new ElementInfo("textbox", "Reee", 100, 20, 10, 10));
            a.BuildApplication("html",test, "no");
        }
    }
}

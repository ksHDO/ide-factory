using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBuilder
{
    class LanguageList
    {

        private List<Type> supportedLanguages = new List<Type>();
        public void SetTypes(Type[] types)
        {
            supportedLanguages.AddRange(types);
        }

        public void Add(Type type)
        {
            supportedLanguages.Add(type);
        }
        

        public Type[] SupportedLanguages()
        {
            return supportedLanguages.ToArray();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBuilder
{
    public class LanguageFactory
    {
        static LanguageList languages;
        
        static LanguageFactory()
        {
            languages = new LanguageList();
            var properLangs = typeof(Language).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Language))).ToArray();
            languages.SetTypes(properLangs); 
        }

        public List<string> GetSupportedLanguages()
        {
            List<string> langs = new List<string>();
            var l = languages.SupportedLanguages();
            for (int j = 0; j < l.Length; j++)
            {
                langs.Add(l[j].GetType().Name);
            }
            return langs;
        }

        //public List<string> GetSupportedElementsFor(string s)
        //{
            
        //}


        public void BuildApplication(string languageName, List<ElementInfo> elementList, string path)
        {
            Language l = null;
            var supportedLanguages = languages.SupportedLanguages();
            int ct = supportedLanguages.Length;
            for (int j = 0; j < ct; j++)
            {
                var t = supportedLanguages[j];

                if (t.Name.ToLower() == languageName.ToLower())
                {
                    l = (Language)Activator.CreateInstance(t);
                    break;
                }
            }

            l.BuildApp(path, elementList);
        }

    }
}

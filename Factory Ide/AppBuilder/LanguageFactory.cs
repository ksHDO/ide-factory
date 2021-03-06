﻿using System;
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
            var properLangs = typeof(object).Assembly.GetTypes().Where(type => type.IsSubclassOf(typeof(Language))).ToArray();
            languages.SetTypes(properLangs); 
        }

        public static void AddLanguage<T>() where T : Language
        {
            languages.Add(typeof(T));
        }

        public List<string> GetSupportedLanguages()
        {
            List<string> langs = new List<string>();
            var l = languages.SupportedLanguages();
            for (int j = 0; j < l.Length; j++)
            {
                langs.Add(l[j].Name);
            }
            return langs;
        }

        public List<string> GetSupportedComponentsFor(string languageName)
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

            if (l == null) return new List<string>() { "" };
            List<string> ret = new List<string>();

            for (int j = 0; j < l.languageElements.Length; j++)
            {
                ret.Add(String.Copy(l.languageElements[j].name));
            }
            return ret;
        }


        //public List<string> GetSupportedElementsFor(string s)
        //{
                 
        //}


        public void BuildApplication(string languageName, List<ElementInfo> elementList, string folder, string outputName)
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

            l?.BuildApp(outputName, folder, elementList);
            
        }

    }
}

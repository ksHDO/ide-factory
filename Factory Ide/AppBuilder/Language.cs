﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBuilder
{
    abstract class Language
    {
        protected string topText = "";
        protected string bottomText = "";
        private string body = "";

        protected ElementTemplate[] languageElements = new ElementTemplate[0];

        private void AppendTopText()
        {
            body += topText;
        }
        private void AppendBottomText()
        {
            body += bottomText;
        }

        private ElementTemplate FindTemplate(string name)
        {
            var ct = languageElements.Length;
            for (int j = 0; j < ct; ++j)
            {
                if(languageElements[j].name == name)
                {
                    return languageElements[j];
                }
            }
            return null;
        }


        private void ApplyBody(List<ElementInfo> elementList)
        {
            var ct = elementList.Count;
            for (int j = 0; j < ct; j++)
            {
                var f = FindTemplate(elementList[j].type);
                if (f == null)
                {
                    //Log
                    Console.WriteLine("No template found for type");
                    break;
                }

                string s = f.Format(elementList[j]);
                body += s;

            }
        }


        public void BuildApp(string path, List<ElementInfo> elementList)
        {
            AppendTopText();
            ApplyBody(elementList);
            AppendBottomText();

            Console.WriteLine(body);
            
        }
    }
}

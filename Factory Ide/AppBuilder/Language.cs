using System;
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
        protected string body = "";

        protected ElementTemplate[] languageElements = new ElementTemplate[0];

        protected void AppendTopText()
        {
            body += topText;
        }
        protected void AppendBottomText()
        {
            body += bottomText;
        }

        protected ElementTemplate FindTemplate(string name)
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


        protected void ApplyBody(List<ElementInfo> elementList)
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

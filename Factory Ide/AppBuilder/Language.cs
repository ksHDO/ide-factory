using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBuilder
{
    public abstract class Language
    {
        protected string topText = "";
        protected string bottomText = "";
        protected string body = "";
        protected string fileExtension = "";
        protected string executeableExtension = "";

        public ElementTemplate[] languageElements { get; protected set; } = new ElementTemplate[0];

         

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

        protected abstract void Compile(string folder, string name);

        public void BuildApp(string outputName, string folder, List<ElementInfo> elementList)
        {
            AppendTopText();
            ApplyBody(elementList);
            AppendBottomText();
            Compile(folder, outputName);
            //Console.WriteLine(body);
            //CmdProcess.RunCommand()

            
        }
    }
}

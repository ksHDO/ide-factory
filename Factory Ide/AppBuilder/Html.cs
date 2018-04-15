using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.IO;

namespace AppBuilder
{
    public class Html : Language
    {

        protected override void Compile(string folder, string name)
        {
            string newPath = folder + name + "." + fileExtension;
            File.WriteAllText(newPath, body);
        }

        public Html()
        {
            topText = "<!DOCTYPE HTML>\r\n<html>\r\n<head></head>\r\n<body>\r\n";
            bottomText = "\r\n</body>\r\n</html>";
            fileExtension = "html";
            languageElements = new ElementTemplate[]
            {
                new ElementTemplate("label",
                    "<div style=\"position: absolute; left: %l%px; top: %t%px; width: %w%px; height: %h%px;\">" +
                    "%content%" +
                    "</div>\r\n"
                ),
                new ElementTemplate("textbox",
                    "<input type=\"text\" value=\"%content%\" style=\"position: absolute; left: %l%px; top: %t%px; width: %w%px; height: %h%px;\">" +
                    "</input>\r\n"
                ),
                new ElementTemplate("button",
                    "<button type=\"button\" style=\"position: absolute; left: %l%px; top: %t%px; width: %w%px; height: %h%px;\">" +
                    "%content%" +
                    "</button>\r\n"
                )
            };
        }


    }
}

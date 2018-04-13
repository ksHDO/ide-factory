using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppBuilder
{
    class Wpf : Language
    {
        public Wpf()
        {
            topText = "using System;\r\n"
                    + "using System.Windows;\r\n"
                    + "using System.Windows.Controls;\r\n"
                    + "namespace FactoryApp {\r\n"
                    + "public class MainWindow : Window\r\n"
                    + "    {\r\n"
                    + "        [STAThread]\r\n"
                    + "        public static void Main() {\r\n"
                    + "            Application app = new Application();\r\n"
                    + "            app.Run(new MainWindow());\r\n"
                    + "        }\r\n"
                    + "        public MainWindow()\r\n"
                    + "        {\r\n"
                    + "            Width = 300;\r\n"
                    + "            Height = 300;\r\n"
                    + "            Canvas canvas = new Canvas();\r\n"
                    + "            Content = canvas;\r\n";

            bottomText = "}\r\n}\r\n}";

            languageElements = new ElementTemplate[]
            {
                new ElementTemplate("textbox",
                    "var textBlock%id% = new TextBlock();\r\n"+
                    "textBlock%id%.Text = \"%content%\";"+
                    "canvas.Children.Add(textBlock%id%);"
                ),
                new ElementTemplate("button",
                    "var button%id% = new Button();\r\n"+
                    "button%id%.Text = \"%content%\";"+
                    "canvas.Children.Add(button%id%);"
                ),
                new ElementTemplate("label",
                    "var label%id% = new Label();\r\n"+
                    "label%id%.Text = \"%content%\";"+
                    "canvas.Children.Add(label%id%);"
                ),
            };
        }

    }
}

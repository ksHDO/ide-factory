using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AppBuilder
{
    public class Wpf : Language
    {
        protected override void Compile(string folder, string name)
        {
            
            string path = Path.Combine(folder, name);
            File.WriteAllText(path + "." + fileExtension, body);


            CSharpCodeProvider provider = new CSharpCodeProvider();
            CompilerParameters parameters = new CompilerParameters();

            parameters.ReferencedAssemblies.Add("PresentationFramework.dll");
            parameters.ReferencedAssemblies.Add("WindowsBase.dll");
            parameters.ReferencedAssemblies.Add("PresentationCore.dll");
            parameters.ReferencedAssemblies.Add("System.dll");
            parameters.ReferencedAssemblies.Add("System.Xaml.dll");
            parameters.OutputAssembly = $"{path}.{executeableExtension}";
            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = true;

            CompilerResults results = provider.CompileAssemblyFromSource(parameters, body);
            if (results.Errors.HasErrors)
            {
                var errorBuilder = new StringBuilder();
                foreach (CompilerError error in results.Errors)
                {
                    errorBuilder.AppendLine(String.Format("Error ({0}): {1}", error.ErrorNumber, error.ErrorText));
                }

                throw new InvalidOperationException(errorBuilder.ToString());
            }

            //string frameworkPath = RuntimeEnvironment.GetRuntimeDirectory();
            //string cSharpCompilerPath = Path.Combine(frameworkPath, newPath);
            //string command = $"C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\csc.exe /out:{folder}{name}.exe /target:winexe {newPath} /reference:\"%path%presentationframework.dll\" /reference:\"%path%windowsbase.dll\" /reference:\"%path%presentationcore.dll\" /reference:\"%path%System.Xaml.dll";
            ////string command = $"C:\\Windows\\Microsoft.NET\\Framework\\v4.0.30319\\csc.exe /out:{folder}{name}.exe /target:winexe {newPath} /reference:\"C:\\Program Files(x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.6.2\\presentationframework.dll\" /reference:\"C:\\Program Files(x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.6.2\\windowsbase.dll\" /reference:\"C:\\Program Files(x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.6.2\\presentationcore.dll\" /reference:\"C:\\Program Files(x86)\\Reference Assemblies\\Microsoft\\Framework\\.NETFramework\\v4.6.2\\System.Xaml.dll";

            ////C:\\Program Files(x86)\\Reference Assemblies\\Microsoft\Framework\\.NETFramework\\v4.6.2\\
            //command = command.Replace("%path%", frameworkPath);
            //CmdProcess.RunCommand(command);

        }


        public Wpf()
        {
            fileExtension = "cs";
            executeableExtension = "exe";
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
                    "var textBox%id% = new TextBox();\r\n"+
                    "textBox%id%.Text = \"%content%\";"+
                    "Canvas.SetLeft(textBox%id%, %l%);" +
                    "Canvas.SetTop(textBox%id%, %t%);" +
                    "textBox%id%.Width = %w%;" +
                    "textBox%id%.Height = %h%;" +
                    "canvas.Children.Add(textBox%id%);"
                ),
                new ElementTemplate("button",
                    "var button%id% = new Button();\r\n"+
                    "button%id%.Content = \"%content%\";"+
                    "Canvas.SetLeft(button%id%, %l%);" +
                    "Canvas.SetTop(button%id%, %t%);" +
                    "button%id%.Width = %w%;" +
                    "button%id%.Height = %h%;" +
                    "canvas.Children.Add(button%id%);"
                ),
                new ElementTemplate("label",
                    "var label%id% = new Label();\r\n"+
                    "label%id%.Content = \"%content%\";"+
                     "Canvas.SetLeft(label%id%, %l%);" +
                    "Canvas.SetTop(label%id%, %t%);" +
                    "label%id%.Width = %w%;" +
                    "label%id%.Height = %h%;" +
                    "canvas.Children.Add(label%id%);"
                ),
                new ElementTemplate("checkbox",
                    "var checkbox%id% = new CheckBox();\r\n"+
                    "checkbox%id%.Content = \"%content%\";"+
                    "Canvas.SetLeft(checkbox%id%, %l%);" +
                    "Canvas.SetTop(checkbox%id%, %t%);" +
                    "checkbox%id%.Width = %w%;" +
                    "checkbox%id%.Height = %h%;" +
                    "canvas.Children.Add(checkbox%id%);"
                )
            };
        }

    }
}

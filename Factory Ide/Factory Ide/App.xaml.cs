using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Factory_Ide
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Startup += StartApp;
        }

        private void StartApp(object sender, StartupEventArgs e)
        {
            Factory_Ide.MainWindow.Instance.Show();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Factory_Ide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private struct Component
        {
            public string Name;
            public Type Type;
        }
        private List<Type> m_components;

        public MainWindow()
        {
            InitializeComponent();
            LoadSupportedComponents();

            AddCanvasComponents();

            
        }

        private void LoadSupportedComponents()
        {
            m_components = new List<Type>();

            m_components.Add(typeof(Button));

            m_components.Add(typeof(Label));

            LbxComponents.ItemsSource = m_components.Select(s => s.Name);
            
        }

        private void AddCanvasComponents()
        {
            AddComponent(new Button()
            {
                Content = "Button"
            });
            
        }

        private void AddComponent(Control control)
        {
            CvsInterface.Children.Add(control);

            control.GotFocus += OnCanvasComponentSelected;
            
        }

        private void UpdatePropertiesEvent(object sender, SelectionChangedEventArgs args)
        {

        }

        private void OnCanvasComponentSelected(object sender, RoutedEventArgs args)
        {
            Control c = (Control) sender;
            List<PropertyControl> properties = new List<PropertyControl>();

            if (c is ContentControl control)
            {
                var content = new PropertyControl("Content", control.Content.ToString());
                content.OnTextboxDataChanged += (o, eventArgs) => control.Content = (o as TextBox)?.Text;
                properties.Add(content);
            }

            

            LbxProperties.ItemsSource = properties;

        }
    }
}

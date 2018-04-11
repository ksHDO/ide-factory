using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for PropertyControl.xaml
    /// </summary>
    public partial class PropertyControl : UserControl
    {
        private string m_propertyName, m_propertyValue;

        public string PropertyName
        {
            get => m_propertyName;
            set
            {
                m_propertyName = value;
                LblName.Content = value;
            }
        }

        public string PropertyValue
        {
            get => m_propertyValue;
            set
            {
                m_propertyValue = value;
                TbxValue.Text = value;
            }
        }

        public event TextChangedEventHandler OnTextboxDataChanged
        {
            add => TbxValue.TextChanged += value;
            remove => TbxValue.TextChanged -= value;
        }

        public PropertyControl(string name, string value)
        {
            InitializeComponent();

            PropertyName = name;
            PropertyValue = value;

        }


    }
}

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
        protected string propertyName, propertyValue;

        public string PropertyName
        {
            get => propertyName;
            set
            {
                propertyName = value;
                LblName.Content = value;
            }
        }

        public string PropertyValue
        {
            get => propertyValue;
            set
            {
                propertyValue = value;
                TbxValue.Text = value;
            }
        }

        public event TextChangedEventHandler OnTextboxDataChanged
        {
            add => TbxValue.TextChanged += value;
            remove => TbxValue.TextChanged -= value;
        }

        public PropertyControl(string name, string value) : this()
        {
            PropertyName = name;
            PropertyValue = value;

        }

        public PropertyControl()
        {
            InitializeComponent();
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory_Ide.Controls
{
    public class PropertyNumberControl : PropertyControl
    {
        protected new double propertyValue;

        public new double PropertyValue
        {
            get => propertyValue;
            set
            {
                propertyValue = value;
                TbxValue.Text = value.ToString();
            }

        }

        public void SetPropertyValue(string value)
        {
            double num;
            if (double.TryParse(value, out num))
            {
                PropertyValue = num;
            }
            else if (string.IsNullOrEmpty(value))
            {
                PropertyValue = 0;
            }
        }

        public PropertyNumberControl(string name, double value) : base()
        {
            PropertyName = name;
            PropertyValue = value;
        }
    }
}

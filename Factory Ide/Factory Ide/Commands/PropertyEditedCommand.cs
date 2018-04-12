using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Factory_Ide.Commands
{
    public class PropertyEditedCommand : IFactoryIdeCommand
    {
        private Control m_control;
        private PropertyInfo m_property;
        private TextBox m_textBox;
        private object m_previousValue;
        private object m_setValue;

        public PropertyEditedCommand(Control control, string propertyName, TextBox textBox, object value)
        {
            m_control = control;
            m_property = control.GetType().GetProperty(propertyName);
            m_textBox = textBox;
            m_previousValue = m_property.GetValue(control);
            m_setValue = value;
        }
        public void Do()
        {
            m_property.SetValue(m_control, m_setValue);
            m_textBox.Text = m_setValue.ToString();
        }

        public void Undo()
        {
            m_property.SetValue(m_control, m_previousValue);
            m_textBox.Text = m_previousValue.ToString();
        }
    }
}

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Factory_Ide.Controls
{
    public class LabelTextbox : TextBox
    {
        public LabelTextbox()
        {
            IsReadOnly = true;
            BorderThickness = new Thickness(0);
            Cursor = Cursors.Arrow;
        }
    }
}
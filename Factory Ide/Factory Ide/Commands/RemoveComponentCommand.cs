using AppBuilder.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Factory_Ide.Commands
{
    public class RemoveComponentCommand : IFactoryIdeCommand
    {
        private Canvas m_canvas;
        private Control m_control;

        public RemoveComponentCommand(Canvas canvas, Control control)
        {
            m_canvas = canvas;
            m_control = control;
        }

        public void Do()
        {
            m_canvas.Children.Remove(m_control);

            // ReSharper disable once PossibleUnintendedReferenceComparison
            if (MainWindow.Instance.SelectedControl == m_control)
                MainWindow.Instance.SelectedControl = null;
        }

        public void Undo()
        {
            m_canvas.Children.Add(m_control);
            MainWindow.Instance.SelectedControl = m_control;
        }
    }
}
